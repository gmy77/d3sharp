
using System.Collections.Generic;
using Mooege.Net.GS.Message.Definitions.Attribute;
using System;
namespace Mooege.Net.GS.Message
{

    class GameAttributeMap
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

        Dictionary<KeyId, GameAttributeValue> _attributeValues = new Dictionary<KeyId, GameAttributeValue>();

        public void SendMessage(GameClient client, int actorId)
        {
            int count = _attributeValues.Count;

            if (_attributeValues.Count == 1)
            {
                AttributeSetValueMessage msg = new AttributeSetValueMessage();
                msg.Id = (int)Opcodes.AttributeSetValueMessage;
                var e = _attributeValues.GetEnumerator();

                if (!e.MoveNext())
                    throw new Exception("Expected value in enumerator.");

                var keyid = e.Current.Key;
                var value = e.Current.Value;

                int id = keyid.Id;
                msg.Field0 = actorId;
                msg.Field1 = new Fields.NetAttributeKeyValue();
                msg.Field1.Field0 = keyid.Key;
                // FIXME: need to rework NetAttributeKeyValue, and maybe rename GameAttribute to NetAttribute?
                msg.Field1.Attribute = GameAttribute.Attributes[id]; // FIXME
                if (msg.Field1.Attribute.IsInteger)
                    msg.Field1.Int = value.Value;
                else
                    msg.Field1.Float = value.ValueF;

                client.SendMessage(msg);
            }
            else
            {
                AttributesSetValuesMessage msg = new AttributesSetValuesMessage();
                msg.Id = (int)Opcodes.AttributesSetValuesMessage;
                msg.Field0 = actorId;

                var e = _attributeValues.GetEnumerator();
                // FIXME: probably need to rework AttributesSetValues as well a bit
                if (count >= 15)
                {
                    msg.atKeyVals = new Fields.NetAttributeKeyValue[15];
                    for (int i = 0; i < 15; i++)
                        msg.atKeyVals[i] = new Fields.NetAttributeKeyValue();

                    for (; count >= 15; count -= 15)
                    {
                        for(int i = 0;i < 15;i++)
                        {
                            if (!e.MoveNext())
                                throw new Exception("Expected values in enumerator.");
                            var kv = msg.atKeyVals[i];

                            var keyid = e.Current.Key;
                            var id = keyid.Id;
                            var value = e.Current.Value;

                            kv.Field0 = keyid.Key;
                            kv.Attribute = GameAttribute.Attributes[id];
                            if (kv.Attribute.IsInteger)
                                kv.Int = value.Value;
                            else
                                kv.Float = value.ValueF;
                        }
                        client.SendMessage(msg);
                    }
                }

                if (count > 0)
                {
                    msg.atKeyVals = new Fields.NetAttributeKeyValue[count];
                    for (int i = 0; i < count; i++)
                    {
                        if (!e.MoveNext())
                            throw new Exception("Expected values in enumerator.");
                        var kv = new Fields.NetAttributeKeyValue();
                        msg.atKeyVals[i] = kv;

                        var keyid = e.Current.Key;
                        var id = keyid.Id;
                        var value = e.Current.Value;
                        kv.Field0 = keyid.Key;
                        kv.Attribute = GameAttribute.Attributes[id];
                        if (kv.Attribute.IsInteger)
                            kv.Int = value.Value;
                        else
                            kv.Float = value.ValueF;
                    }
                    client.SendMessage(msg);
                }
            }


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
            get { return GetAttributeValue(attribute, null).Value; }
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
