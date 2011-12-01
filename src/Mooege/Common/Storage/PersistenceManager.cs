/*
 * Copyright (C) 2011 mooege project
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */

using System;
using System.Reflection;
using System.Data.SQLite;
using Mooege.Common.Storage;
using System.Collections.Generic;
using System.Collections;
using Mooege.Common.MPQ.FileFormats.Types;

namespace Mooege.Common.Storage
{
    /// <summary>
    /// This attribute is used to tag properties that are persisted by the persistance manager.
    /// The class is mapped to a table with the same name as the class, and by default, each property is
    /// mapped to a column with the same name as the property (property, not the type of the property...)
    /// unless you override it by setting another name.
    /// </summary>
    public class PersistentPropertyAttribute : Attribute
    {
        public string Name { get; private set; }
        public int Count { get; private set; }

        public PersistentPropertyAttribute(string name) { Name = name; Count = 1; }
        public PersistentPropertyAttribute() { }
        public PersistentPropertyAttribute(string name, int count) { Name = name; Count = count; }
    }


    /// <summary>
    /// Loading classes from and saving classes to the mpq mirror database
    /// </summary>
    public class PersistenceManager
    {

        private static PersistentPropertyAttribute GetPersistentAttribute(PropertyInfo p)
        {
            if (p.GetCustomAttributes(typeof(PersistentPropertyAttribute), false).Length > 0)
            {
                return (PersistentPropertyAttribute)p.GetCustomAttributes(typeof(PersistentPropertyAttribute), false)[0];
            }
            
            return null;
        }


        /// <summary>
        /// Loads data from the mpqmirror into an already existing object
        /// </summary>
        /// <param name="o">Object with tagged properties that are to be loaded from databade</param>
        /// <param name="id">The id of the corresponding entry in the database</param>
        public static void LoadPartial(object o, string id)
        {
            // check if the object type is mapped at all
            using (var cmd = new SQLiteCommand(String.Format("SELECT name FROM sqlite_master WHERE type='table' AND name='{0}'", o.GetType().Name), DBManager.MPQMirror))
            {
                if (cmd.ExecuteScalar() == null) return;
            }

            // load the entry and begn reading
            using (var cmd = new SQLiteCommand(String.Format("SELECT * FROM {0} WHERE Id={1}", o.GetType().Name, id), DBManager.MPQMirror))
            {
                Load(o, cmd.ExecuteReader());
            }
        }

        // TODO first projection, then join... not the other way around
        private static string genericListsql = "SELECT {1}.* FROM {0}_{1} JOIN {1} ON {0}_{1}.{1}Id = {1}.Id WHERE {0}Id = {2}";

        /// <summary>
        /// Loads properties of an object from the passes reader
        /// </summary>
        /// <param name="o">Object to be filled with data</param>
        /// <param name="reader">Reader from which to take the data</param>
        /// <param name="embeddedPrefix">Prefix for 'inlined' properties (complex types)</param>
        public static void Load(object o, SQLiteDataReader reader, string embeddedPrefix = "")
        {
            foreach (var property in o.GetType().GetProperties())
            {
                if (GetPersistentAttribute(property) != null)
                {
                    string columnName = String.Format("{0}{1}", embeddedPrefix, GetPersistentAttribute(property).Name == null ? property.Name : GetPersistentAttribute(property).Name);
                    string entryId = reader["Id"].ToString();

                    // Load generic lists by finding the mn-mapping table and loading every entry recursivly
                    if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                    {
                        using (var cmd = new SQLiteCommand(String.Format(genericListsql, o.GetType().Name, property.PropertyType.GetGenericArguments()[0].Name, entryId), DBManager.MPQMirror))
                        {
                            var itemReader = cmd.ExecuteReader();
                            var list = Activator.CreateInstance(property.PropertyType);

                            if (itemReader.HasRows)
                            {
                                while (itemReader.Read())
                                {
                                    var item = Activator.CreateInstance(property.PropertyType.GetGenericArguments()[0]);
                                    Load(item, itemReader);
                                    (list as IList).Add(item);
                                }
                            }
                            property.SetValue(o, list, null);

                        }
                        continue;
                    }

                    // load scalar types
                    if (property.PropertyType.Namespace == "System")
                    {
                        // load array of scalar types. The column name of the i-th array entry is "columnName_i"
                        if (property.PropertyType.IsArray)
                        {
                            Array vals = (Array)Activator.CreateInstance(property.PropertyType, GetPersistentAttribute(property).Count);
                            for (int i = 0; i < vals.Length; i++)
                            {
                                vals.SetValue(Convert.ChangeType(reader[columnName + "_" + i.ToString()], property.PropertyType.GetElementType()), i);
                            }

                            property.SetValue(o, vals, null);
                        }
                        else
                        {
                            property.SetValue(o, Convert.ChangeType(reader[columnName], property.PropertyType), null);
                        }
                        continue;
                    }

                    // load enums
                    if (property.PropertyType.IsEnum)
                    {
                        property.SetValue(o, Enum.Parse(property.PropertyType, reader[columnName].ToString(), true), null);
                        continue;
                    }

                    // if its none of the earlier types, its a inlined class. class properties
                    var embedded = Activator.CreateInstance(property.PropertyType);
                    Load(embedded, reader, embeddedPrefix + property.Name + "_");
                    property.SetValue(o, embedded, null);
                }
            }
        }

        private static string saveSql = "INSERT INTO {0} ";
        private static string updateValue = "UPDATE {0} SET {1} = {2} WHERE {0}.Id={3}";


        public static void Save(object o, string id)
        {
            Save(o, id, new Dictionary<string, string>());
        }

        private static string Save(object o, string id, Dictionary<string, string> values, string embeddedPrefix = "")
        {
            if (values == null)
                values = new Dictionary<string, string>();

            // Save all scalar and inline types first, so we have the new table id four our mn-table later
            foreach (var property in o.GetType().GetProperties())
            {
                if (GetPersistentAttribute(property) != null)
                {
                    string columnName = String.Format("{0}{1}", embeddedPrefix, GetPersistentAttribute(property).Name == null ? property.Name : GetPersistentAttribute(property).Name);

                    // save scalar types
                    if (property.PropertyType.Namespace == "System" || property.PropertyType.IsEnum)
                    {
                        // save array of scalar types
                        if (property.PropertyType.IsArray)
                        {
                            for (int i = 0; i < GetPersistentAttribute(property).Count; i++)
                            {
                                values.Add(columnName + "_" + i, "'" + (property.GetValue(o, null) as Array).GetValue(i) .ToString() + "'");
                            }
                        }
                        else
                        {
                            values.Add(columnName, "'" + property.GetValue(o, null).ToString() + "'");
                        }
                        continue;
                    }

                    // if its none of the earlier types, its a inlined class. class properties
                    Save(property.GetValue(o, null), id, values, embeddedPrefix + property.Name + "_");
                }
            }

            if (embeddedPrefix == "")
            {
                if (id != null)
                {
                    values.Add("Id", id);
                }

                string cnames = String.Join(",", (new List<string>(values.Keys).ToArray()));
                string cvalues = String.Join(",", (new List<string>(values.Values).ToArray()));

                using (var cmd = new SQLiteCommand(String.Format("INSERT INTO {0} ({1}) VALUES ({2})", o.GetType().Name, cnames, cvalues), DBManager.MPQMirror))
                {
                    cmd.ExecuteNonQuery();

                    using (var last = new SQLiteCommand("SELECT last_insert_rowid()", DBManager.MPQMirror))
                    {
                        id = last.ExecuteScalar().ToString();
                    }
                }
            }


            // save generic lists
            foreach (var property in o.GetType().GetProperties())
            {
                if (GetPersistentAttribute(property) != null)
                {
                    if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                    {
                        IList list = (IList)property.GetValue(o, null);

                        foreach (var item in list)
                        {
                            string newId = Save(item, null, null, "");

                            using (var cmd = new SQLiteCommand(String.Format(
                                "INSERT INTO {0}_{1} ({0}Id, {1}Id) VALUES ({2}, {3})",
                                o.GetType().Name,
                                property.PropertyType.GetGenericArguments()[0].Name,
                                id,
                                newId
                                ), DBManager.MPQMirror))
                            {
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }


            return id;
        }

    }
}