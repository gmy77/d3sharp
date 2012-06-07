/*
 * Copyright (C) 2012 mooege project - http://www.mooege.org
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Mooege.Core.GS.Objects;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;

namespace Mooege.Net.GS.Message
{
    // exception class for initialization errors
    public class ScriptedAttributeInitializerError : Exception
    {
        public ScriptedAttributeInitializerError() { }
        public ScriptedAttributeInitializerError(string message) : base(message) { }
    }

    // Compiles GameAttribute scripts, generates script dependency lists.
    public class ScriptedAttributeInitializer
    {
        #region Pin() implementation for scripts to use.
        public static int Pin(int a, int b, int c)
        {
            if (b > a)
                return b;
            else if (a > c)
                return c;
            else
                return a;
        }

        public static float Pin(float a, float b, float c)
        {
            if (b > a)
                return b;
            else if (a > c)
                return c;
            else
                return a;
        }
        #endregion

        public static void ProcessAttributes(GameAttribute[] attributes)
        {
            // build string -> GameAttribute lookup
            var attributeLookup = attributes.ToDictionary(attr => attr.Name);
            // will contain C# code for the func<> body that represents each attribute's script.
            var csharpScripts = new Dictionary<GameAttribute, string>();

            // generate C#-compatible source lines from scripts and create attribute dependency lists
            foreach (GameAttribute attr in attributes)
            {
                // check for valid script in the attribute and select it
                string script;

                if (attr.ScriptA.Length > 0 && attr.ScriptA != "0")
                    script = attr.ScriptA;
                else if (attr.ScriptB.Length > 0 && attr.ScriptB != "0")
                    script = attr.ScriptB;
                else
                    continue;  // no valid script, done processing this attribute

                // by default all scripts are not settable
                // can be set to true if self-referring identifier is found
                attr.ScriptedAndSettable = false;

                // replace attribute references with GameAttributeMap lookups
                // also record all attributes used by script into each attribute's dependency list
                script = Regex.Replace(script, @"([A-Za-z_]\w*)(\.Agg)?(\#[A-Za-z_]\w*)?(?=[^\(\w]|\z)( \?)?",
                    (match) =>
                    {
                        // lookup attribute object
                        string identifierName = match.Groups[1].Value;
                        if (!attributeLookup.ContainsKey(identifierName))
                            throw new ScriptedAttributeInitializerError("invalid identifer parsed: " + identifierName);

                        GameAttribute identifier = attributeLookup[identifierName];

                        // key selection
                        int? key = null;
                        string keyString = "_key";
                        bool usesExplicitKey = false;

                        if (match.Groups[3].Success)
                        {
                            switch (match.Groups[3].Value.ToUpper())
                            {
                                case "#NONE": key = null; break;
                                case "#PHYSICAL": key = 0; break;
                                case "#FIRE": key = 1; break;
                                case "#LIGHTNING": key = 2; break;
                                case "#COLD": key = 3; break;
                                case "#POISON": key = 4; break;
                                case "#ARCANE": key = 5; break;
                                case "#HOLY": key = 6; break;
                                default:
                                    throw new ScriptedAttributeInitializerError("error processing attribute script, invalid key in identifier: " + match.Groups[3].Value);
                            }

                            if (key == null)
                                keyString = "null";
                            else
                                keyString = key.ToString();

                            usesExplicitKey = true;
                        }

                        // add comparsion for int attributes that are directly used in an ?: expression.
                        string compare = "";
                        if (match.Groups[4].Success)
                            compare = identifier is GameAttributeI ? " > 0 ?" : " ?";

                        // handle self-referring lookup. example: Resource.Agg
                        if (match.Groups[2].Success)
                        {
                            attr.ScriptedAndSettable = true;
                            return "_map._RawGetAttribute(GameAttribute." + identifierName
                                + ", " + keyString + ")" + compare;
                        }

                        // record dependency
                        if (identifier.Dependents == null)
                            identifier.Dependents = new List<GameAttributeDependency>();

                        identifier.Dependents.Add(new GameAttributeDependency(attr, key, usesExplicitKey, false));

                        // generate normal lookup
                        return "_map[GameAttribute." + identifierName + ", " + keyString + "]" + compare;
                    });

                // transform function calls into C# equivalents
                script = Regex.Replace(script, @"floor\(", "(float)Math.Floor(", RegexOptions.IgnoreCase);
                script = Regex.Replace(script, @"max\(", "Math.Max(", RegexOptions.IgnoreCase);
                script = Regex.Replace(script, @"min\(", "Math.Min(", RegexOptions.IgnoreCase);
                script = Regex.Replace(script, @"pin\(", "ScriptedAttributeInitializer.Pin(", RegexOptions.IgnoreCase);

                // add C# single-precision affix to decimal literals. example: 1.25 => 1.25f
                script = Regex.Replace(script, @"\d+\.\d+", "$0f");

                csharpScripts[attr] = script;
            }

            // generate and write final C# code to file
            string sourcePathBase = Path.Combine(Path.GetTempPath(), "MooegeScriptedAttributeFuncs");

            using (StreamWriter fout = new StreamWriter(sourcePathBase + ".cs"))
            {
                fout.Write(
@"// This file was auto-generated by Mooege class ScriptedAttributeInitializer
// It contains Funcs derived from GameAttribute.ScriptA/B scripts.
// Funcs will be assigned to their respective GameAttribute.ScriptFunc member.
using System;
using Mooege.Net.GS.Message;
using Mooege.Core.GS.Objects;

namespace Mooege.Net.GS.Message.GeneratedCode
{
    public class ScriptedAttributeFuncs
    {
");
                foreach (var scriptEntry in csharpScripts)
                {
                    // select output type cast to ensure it matches attribute type
                    string castType = scriptEntry.Key is GameAttributeF ? "float" : "int";

                    // write out full Func static class field
                    fout.WriteLine("        public static Func<GameAttributeMap, int?, GameAttributeValue> {0} = (_map, _key) => new GameAttributeValue(({1})({2}));",
                        scriptEntry.Key.Name,
                        castType,
                        scriptEntry.Value);
                }

                fout.Write(
@"    }
}
");
            }

            // compile code
            var options = new CompilerParameters();
            options.GenerateExecutable = false;
            options.OutputAssembly = sourcePathBase + ".dll";
            options.IncludeDebugInformation = true;
            options.ReferencedAssemblies.Add(Assembly.GetExecutingAssembly().Location);

            var results = new CSharpCodeProvider().CompileAssemblyFromFile(options, sourcePathBase + ".cs");
            if (results.Errors.Count > 0)
            {
                StringBuilder emsg = new StringBuilder();
                emsg.AppendLine("encountered errors compiling attribute funcs:");
                foreach (var e in results.Errors)
                    emsg.AppendLine(e.ToString());

                throw new ScriptedAttributeInitializerError(emsg.ToString());
            }

            // pull funcs from new assembly and assign them to their respective attributes
            Type funcs = results.CompiledAssembly.GetType("Mooege.Net.GS.Message.GeneratedCode.ScriptedAttributeFuncs");

            foreach (var attr in csharpScripts.Keys)
            {
                attr.ScriptFunc = (Func<GameAttributeMap, int?, GameAttributeValue>)funcs
                    .GetField(attr.Name).GetValue(null);
            }
        }
    }
}
