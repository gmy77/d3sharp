﻿﻿/*
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
using Mooege.Core.GS.Objects;
using System.Linq;
using Mooege.Net.GS.Message;
using Mooege.Net.GS;

namespace Mooege.Core.GS.Objects
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
        private WorldObject _parent;

        public GameAttributeMap(WorldObject parent)
        {
            _parent = parent;
        }



        #region message broadcasting

        public void SendMessage(GameClient client)
        {
            var list = GetMessageList();
            foreach (var msg in list)
                client.SendMessage(msg);
            _changedAttributes.Clear();
        }

        public void SendMessage(IEnumerable<GameClient> clients)
        {
            var list = GetMessageList();
            foreach (var msg in list)
            {
                foreach(var client in clients)
                    client.SendMessage(msg);
            }
            _changedAttributes.Clear();
        }

        /// <summary>
        /// Send only the changed attributes. How nice is that?
        /// You should generaly use Broadcast if possible
        /// </summary>
        /// <param name="client">the client we send it to</param>
        public void SendChangedMessage(GameClient client)
        {
            var list = GetChangedMessageList();
            foreach (var msg in list)
                client.SendMessage(msg);
            _changedAttributes.Clear();
        }

        public void SendChangedMessage(IEnumerable<GameClient> clients)
        {
            if (_changedAttributes.Count == 0)
                return;

            var list = GetChangedMessageList();
            foreach (var msg in list)
            {
                foreach (var client in clients)
                    client.SendMessage(msg);
            }
            _changedAttributes.Clear();
        }

        /// <summary>
        /// Broadcasts attribs to players that the parent actor has been revealed to.
        /// </summary>
        public void BroadcastIfRevealed()
        {
            SendMessage(_parent.World.Players.Values
                .Where(@player => @player.RevealedObjects.ContainsKey(_parent.DynamicID))
                .Select(@player => @player.InGameClient));
        }

        /// <summary>
        /// Broadcasts changed attribs to players that the parent actor has been revealed to.
        /// </summary>
        public void BroadcastChangedIfRevealed()
        {
            SendChangedMessage(_parent.World.Players.Values
                .Where(@player => @player.RevealedObjects.ContainsKey(_parent.DynamicID))
                .Select(@player => @player.InGameClient));
        }

        #endregion

        public void ClearChanged()
        {
            _changedAttributes.Clear();
        }

        private List<GameMessage> GetMessageList()
        {
            var e = _attributeValues.Keys.GetEnumerator();
            var level = _attributeValues.ContainsKey(new KeyId() { Id = GameAttribute.Level.Id });
            return GetMessageListFromEnumerator(e, _attributeValues.Count, level);
        }

        private List<GameMessage> GetChangedMessageList()
        {
            var e = _changedAttributes.GetEnumerator();
            var level = _changedAttributes.Contains(new KeyId() { Id = GameAttribute.Level.Id });
            return GetMessageListFromEnumerator(e, _changedAttributes.Count, level);
        }

        private List<GameMessage> GetMessageListFromEnumerator(IEnumerator<KeyId> e, int count, bool level)
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
                msg.ActorID = _parent.DynamicID;
                msg.Field1 = new Mooege.Net.GS.Message.Fields.NetAttributeKeyValue();
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
                        msg.ActorID = _parent.DynamicID;
                        msg.atKeyVals = new Mooege.Net.GS.Message.Fields.NetAttributeKeyValue[15];
                        for (int i = 0; i < 15; i++)
                            msg.atKeyVals[i] = new Mooege.Net.GS.Message.Fields.NetAttributeKeyValue();
                        for (int i = 0; i < 15; i++)
                        {
                            KeyId keyid;
                            if (!e.MoveNext())
                            {
                                if (level)
                                {
                                    keyid = new KeyId { Id = GameAttribute.Level.Id };
                                    level = false;
                                }
                                else
                                {
                                    throw new Exception("Expected values in enumerator.");
                                }
                            }
                            else
                            {
                                keyid = e.Current;
                            }

                            var kv = msg.atKeyVals[i];
                            if (level && keyid.Id == GameAttribute.Level.Id)
                            {
                                i--;
                                continue;
                            }

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
                    msg.ActorID = _parent.DynamicID;
                    msg.atKeyVals = new Mooege.Net.GS.Message.Fields.NetAttributeKeyValue[count];
                    for (int i = 0; i < count; i++)
                    {
                        KeyId keyid;
                        if (!e.MoveNext())
                        {
                            if (level)
                            {
                                keyid = new KeyId { Id = GameAttribute.Level.Id };
                                level = false;
                            }
                            else
                            {
                                throw new Exception("Expected values in enumerator.");
                            }
                        }
                        else
                        {
                            keyid = e.Current;
                        }
                        var kv = new Mooege.Net.GS.Message.Fields.NetAttributeKeyValue();
                        msg.atKeyVals[i] = kv;

                        if (level && keyid.Id == GameAttribute.Level.Id)
                        {
                            i--;
                            continue;
                        }

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
