using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D3Sharp.Utils;

namespace D3Sharp.Core.Items
{
    public class NetAttribute
    {

        public static readonly Logger Logger = LogManager.CreateLogger();

        public int MessageType { get; set; } // Don't know what this is for
        public int Key { get; set; }
        public float FloatValue { get; set; }
        public int IntValue { get; set; }

        private NetAttribute()
        {
        } 

        public NetAttribute(int gbid, float floatValue)
        {
            Key = gbid;
            FloatValue = floatValue;
            IntValue = 0;
            MessageType = -1;
        }

        public NetAttribute(int gbid, int intValue)
        {
            Key = gbid;
            FloatValue = 0f;
            IntValue = intValue;
            MessageType = -1;
        }


        public NetAttribute(int gbid, float floatValue, int messageType)
        {
            Key = gbid;
            FloatValue = floatValue;
            IntValue = 0;
            MessageType = messageType;
        }

        public NetAttribute(int gbid, int intValue, int messageTyp)
        {
            Key = gbid;
            FloatValue = 0f;
            IntValue = intValue;
            MessageType = messageTyp;
        }

        public String ToString()
        {
            return String.Format("{0}:{1}:{2}:{3}", Key, IntValue,FloatValue, MessageType);
        }

        public static NetAttribute Parse(String attrString)
        {
            try
            {
                String[] parts = attrString.Split(new char[] { ':' });

                String keyStr = parts[0];
                String intValueString = parts[1];
                String floatValueString = parts[1];
            
                NetAttribute attr = new NetAttribute();
                attr.Key = int.Parse(keyStr);
                attr.IntValue = int.Parse(intValueString);
                attr.FloatValue = float.Parse(floatValueString);

                return attr;
            }catch(Exception e)
            {
                throw new Exception("NetAttribute could't parsed of String: " + attrString, e);
            }
        }
    }
}
