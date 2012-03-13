/*
 * Copyright (C) 2011 - 2012 mooege project - http://www.mooege.org
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

using System.Text;

namespace Mooege.Net.GS.Message.Fields
{
    public class PlayerSavedData
    {
        // MaxLength = 6
        public HotbarButtonData[] HotBarButtons;
        public byte Field2;
        public HotbarButtonData HotBarButton;
        public int PlaytimeTotal;
        public int WaypointFlags;
        public HirelingSavedData Field4;
        public int Field5;
        public LearnedLore LearnedLore;
        // MaxLength = 6
        public ActiveSkillSavedData[] ActiveSkills;
        // MaxLength = 3
        public int /* sno */[] snoTraits;
        public SavePointData SavePointData;

        public void Parse(GameBitBuffer buffer)
        {
            HotBarButtons = new HotbarButtonData[6];
            for (int i = 0; i < HotBarButtons.Length; i++)
            {
                HotBarButtons[i] = new HotbarButtonData();
                HotBarButtons[i].Parse(buffer);
            }
            HotBarButton = new HotbarButtonData();
            Field2 = (byte)buffer.ReadInt(8);
            HotBarButton.Parse(buffer);
            PlaytimeTotal = buffer.ReadInt(32);
            WaypointFlags = buffer.ReadInt(32);
            Field4 = new HirelingSavedData();
            Field4.Parse(buffer);
            Field5 = buffer.ReadInt(32);
            LearnedLore = new LearnedLore();
            LearnedLore.Parse(buffer);
            ActiveSkills = new ActiveSkillSavedData[6];
            for (int i = 0; i < ActiveSkills.Length; i++)
            {
                ActiveSkills[i] = new ActiveSkillSavedData();
                ActiveSkills[i].Parse(buffer);
            }
            snoTraits = new int /* sno */[3];
            for (int i = 0; i < snoTraits.Length; i++) snoTraits[i] = buffer.ReadInt(32);
            SavePointData = new SavePointData();
            SavePointData.Parse(buffer);
        }

        public void Encode(GameBitBuffer buffer)
        {
            for (int i = 0; i < HotBarButtons.Length; i++)
            {
                HotBarButtons[i].Encode(buffer);
            }
            buffer.WriteInt(8, Field2);
            HotBarButton.Encode(buffer);
            buffer.WriteInt(32, PlaytimeTotal);
            buffer.WriteInt(32, WaypointFlags);
            Field4.Encode(buffer);
            buffer.WriteInt(32, Field5);
            LearnedLore.Encode(buffer);
            for (int i = 0; i < ActiveSkills.Length; i++)
            {
                ActiveSkills[i].Encode(buffer);
            }
            for (int i = 0; i < snoTraits.Length; i++) buffer.WriteInt(32, snoTraits[i]);
            SavePointData.Encode(buffer);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("PlayerSavedData:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("Field0:");
            b.Append(' ', pad);
            b.AppendLine("{");
            for (int i = 0; i < HotBarButtons.Length; i++)
            {
                HotBarButtons[i].AsText(b, pad + 1);
                b.AppendLine();
            }
            b.Append(' ', pad);
            b.AppendLine("}");
            b.AppendLine();
            b.Append(' ', pad);
            HotBarButton.AsText(b, pad);
            b.Append(' ', pad);
            b.AppendLine("Field2: 0x" + Field2.ToString("X2") + " (" + Field2 + ")");
            b.Append(' ', pad);
            b.AppendLine("PlaytimeTotal: 0x" + PlaytimeTotal.ToString("X8") + " (" + PlaytimeTotal + ")");
            b.Append(' ', pad);
            b.AppendLine("WaypointFlags: 0x" + WaypointFlags.ToString("X8") + " (" + WaypointFlags + ")");
            Field4.AsText(b, pad);
            b.Append(' ', pad);
            b.AppendLine("Field5: 0x" + Field5.ToString("X8") + " (" + Field5 + ")");
            LearnedLore.AsText(b, pad);
            b.Append(' ', pad);
            b.AppendLine("snoActiveSkills:");
            b.Append(' ', pad);
            b.AppendLine("{");
            for (int i = 0; i < ActiveSkills.Length; i++ )
            {
                ActiveSkills[i].AsText(b, pad + 1);
                b.AppendLine();
            }
            b.Append(' ', pad);
            b.AppendLine("}");
            b.AppendLine();
            b.Append(' ', pad);
            b.AppendLine("snoTraits:");
            b.Append(' ', pad);
            b.AppendLine("{");
            for (int i = 0; i < snoTraits.Length;)
            {
                b.Append(' ', pad + 1);
                for (int j = 0; j < 8 && i < snoTraits.Length; j++, i++)
                {
                    b.Append("0x" + snoTraits[i].ToString("X8") + ", ");
                }
                b.AppendLine();
            }
            b.Append(' ', pad);
            b.AppendLine("}");
            b.AppendLine();
            SavePointData.AsText(b, pad);
            b.Append(' ', pad);
            b.AppendLine();
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}