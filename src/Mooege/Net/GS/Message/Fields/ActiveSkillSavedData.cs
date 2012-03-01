using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mooege.Net.GS.Message.Fields
{
    public class ActiveSkillSavedData
    {
        public int snoSkill;
        public int Field1;

        public void Parse(GameBitBuffer buffer)
        {
            snoSkill = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(3) + (-1);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, snoSkill);
            buffer.WriteInt(3, Field1 - (-1));
        }

        public void AsText(StringBuilder b, int pad)
        {
        }
    }
}
