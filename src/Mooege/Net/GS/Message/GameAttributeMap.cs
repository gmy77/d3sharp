﻿/*
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
using System.Collections.Generic;
using Mooege.Net.GS.Message.Definitions.Attribute;

namespace Mooege.Net.GS.Message
{
    public class GameAttributeMap
    {
        struct KeyId
        {
            // was using Id | (Key << 12) like Blizz at first but im not 100% sure it will work... /cm
            public int Id;
            public int? Key;

            public override bool Equals(object obj)
            {
                if (obj is KeyId)
                {
                    var other = (KeyId)obj;
                    if (Key.HasValue != other.Key.HasValue)
                        return false;
                    if (Key.HasValue && Key.Value != other.Key.Value)
                        return false;
                    return Id == other.Id;
                }
                return false;
            }

            public override int GetHashCode()
            {
                if (Key.HasValue)
                    return Id | (Key.Value << 12);
                return Id;
            }
        }

        private HashSet<KeyId> _changedAttributes = new HashSet<KeyId>();
        private Dictionary<KeyId, GameAttributeValue> _attributeValues = new Dictionary<KeyId, GameAttributeValue>();

        public void SendMessage(GameClient client, uint actorID)
        {
            var list = GetMessageList(actorID);
            foreach (var msg in list)
                client.SendMessage(msg);
            _changedAttributes.Clear();
        }

        /// <summary>
        /// Send only the changed attributes. How nice is that?
        /// </summary>
        /// <param name="client">the client we send it to</param>
        /// <param name="actorID">the actor this attribs belong to</param>
        public void SendChangedMessage(GameClient client, uint actorID)
        {
            var list = GetChangedMessageList(actorID);
            foreach (var msg in list)
                client.SendMessage(msg);
            _changedAttributes.Clear();
        }

        public void SendChangedMessage(IEnumerable<GameClient> clients, uint actorID)
        {
            if (_changedAttributes.Count == 0)
                return;

            var list = GetChangedMessageList(actorID);
            foreach (var msg in list)
            {
                foreach(var client in clients)
                    client.SendMessage(msg);
            }
            _changedAttributes.Clear();
        }

        public void ClearChanged()
        {
            _changedAttributes.Clear();
        }

        public List<GameMessage> GetMessageList(uint actorID)
        {
            var e = _attributeValues.Keys.GetEnumerator();
            return GetMessageListFromEnumerator(actorID, e, _attributeValues.Count);
        }

        public List<GameMessage> GetChangedMessageList(uint actorID)
        {
            var e = _changedAttributes.GetEnumerator();
            return GetMessageListFromEnumerator(actorID, e, _changedAttributes.Count);
        }

        private List<GameMessage> GetMessageListFromEnumerator(uint actorID, IEnumerator<KeyId> e, int count)
        {
            var messageList = new List<GameMessage>();

            if (count == 0)
                return messageList;

            if (count == 1)
            {
                AttributeSetValueMessage msg = new AttributeSetValueMessage();
                if (!e.MoveNext())
                    throw new Exception("Expected value in enumerator.");

                var keyid = e.Current;
                var value = _attributeValues[keyid];

                int id = keyid.Id;
                msg.ActorID = actorID;
                msg.Field1 = new Fields.NetAttributeKeyValue();
                msg.Field1.Field0 = keyid.Key;
                // FIXME: need to rework NetAttributeKeyValue, and maybe rename GameAttribute to NetAttribute?
                msg.Field1.Attribute = GameAttribute.Attributes[id]; // FIXME
                if (msg.Field1.Attribute.IsInteger)
                    msg.Field1.Int = value.Value;
                else
                    msg.Field1.Float = value.ValueF;

                messageList.Add(msg);
            }
            else
            {
                // FIXME: probably need to rework AttributesSetValues as well a bit
                if (count >= 15)
                {
                    for (; count >= 15; count -= 15)
                    {
                        AttributesSetValuesMessage msg = new AttributesSetValuesMessage();
                        msg.ActorID = actorID;
                        msg.atKeyVals = new Fields.NetAttributeKeyValue[15];
                        for (int i = 0; i < 15; i++)
                            msg.atKeyVals[i] = new Fields.NetAttributeKeyValue();
                        for (int i = 0; i < 15; i++)
                        {
                            if (!e.MoveNext())
                                throw new Exception("Expected values in enumerator.");
                            var kv = msg.atKeyVals[i];

                            var keyid = e.Current;
                            var value = _attributeValues[keyid];
                            var id = keyid.Id;

                            kv.Field0 = keyid.Key;
                            kv.Attribute = GameAttribute.Attributes[id];
                            if (kv.Attribute.IsInteger)
                                kv.Int = value.Value;
                            else
                                kv.Float = value.ValueF;
                        }
                        messageList.Add(msg);
                    }
                }

                if (count > 0)
                {
                    AttributesSetValuesMessage msg = new AttributesSetValuesMessage();
                    msg.ActorID = actorID;
                    msg.atKeyVals = new Fields.NetAttributeKeyValue[count];
                    for (int i = 0; i < count; i++)
                    {
                        if (!e.MoveNext())
                            throw new Exception("Expected values in enumerator.");
                        var kv = new Fields.NetAttributeKeyValue();
                        msg.atKeyVals[i] = kv;

                        var keyid = e.Current;
                        var value = _attributeValues[keyid];
                        var id = keyid.Id;

                        kv.Field0 = keyid.Key;
                        kv.Attribute = GameAttribute.Attributes[id];
                        if (kv.Attribute.IsInteger)
                            kv.Int = value.Value;
                        else
                            kv.Float = value.ValueF;
                    }
                    messageList.Add(msg);
                }
            }
            return messageList;
        }

        GameAttributeValue GetAttributeValue(GameAttribute attribute, int? key)
        {
            KeyId keyid;
            keyid.Id = attribute.Id;
            keyid.Key = key;

            GameAttributeValue gaValue;
            if (_attributeValues.TryGetValue(keyid, out gaValue))
                return gaValue;
            return attribute._DefaultValue;
        }

        void SetAttributeValue(GameAttribute attribute, int? key, GameAttributeValue value)
        {
            KeyId keyid;
            keyid.Id = attribute.Id;
            keyid.Key = key;

            if (!_changedAttributes.Contains(keyid))
                _changedAttributes.Add(keyid);

            if (attribute.EncodingType == GameAttributeEncoding.IntMinMax)
            {
                if (value.Value < attribute.Min.Value || value.Value > attribute.Max.Value)
                    throw new ArgumentOutOfRangeException("GameAttribute." + attribute.Name.Replace(' ', '_'), "Min: " + attribute.Min.Value + " Max: " + attribute.Max.Value + " Tried to set: " + value.Value);
            }
            else if (attribute.EncodingType == GameAttributeEncoding.Float16)
            {
                if (value.ValueF < GameAttribute.Float16Min || value.ValueF > GameAttribute.Float16Max)
                    throw new ArgumentOutOfRangeException("GameAttribute." + attribute.Name.Replace(' ', '_'), "Min: " + GameAttribute.Float16Min  + " Max " + GameAttribute.Float16Max + " Tried to set: " + value.ValueF);
            }
            _attributeValues[keyid] = value;
        }

        public int this[GameAttributeI attribute]
        {
            get { return GetAttributeValue(attribute, null).Value; }
            set { SetAttributeValue(attribute, null, new GameAttributeValue(value)); }
        }

        public int this[GameAttributeI attribute, int key]
        {
            get { return GetAttributeValue(attribute, key).Value; }
            set { SetAttributeValue(attribute, key, new GameAttributeValue(value)); }
        }

        public float this[GameAttributeF attribute]
        {
            get { return GetAttributeValue(attribute, null).ValueF; }
            set { SetAttributeValue(attribute, null, new GameAttributeValue(value)); }
        }

        public float this[GameAttributeF attribute, int key]
        {
            get { return GetAttributeValue(attribute, key).ValueF; }
            set { SetAttributeValue(attribute, key, new GameAttributeValue(value)); }
        }

        public bool this[GameAttributeB attribute]
        {
            get { return GetAttributeValue(attribute, null).Value != 0; }
            set { SetAttributeValue(attribute, null, new GameAttributeValue(value ? 1 : 0)); }
        }

        public bool this[GameAttributeB attribute, int key]
        {
            get { return GetAttributeValue(attribute, key).Value != 0; }
            set { SetAttributeValue(attribute, key, new GameAttributeValue(value ? 1 : 0)); }
        }
    }
}
