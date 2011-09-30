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

using System.Text;

namespace Mooege.Net.GS.Message.Fields
{
    public class SceneSpecification
    {
        public int Field0;
        public IVector2D Field1;
        // MaxLength = 4
        public int /* sno */[] arSnoLevelAreas; //area names
        public int /* sno */ snoPrevWorld;
        public int Field4;
        public int /* sno */ snoPrevLevelArea;
        public int /* sno */ snoNextWorld;
        public int Field7;
        public int /* sno */ snoNextLevelArea;
        public int /* sno */ snoMusic;
        public int /* sno */ snoCombatMusic;
        public int /* sno */ snoAmbient;
        public int /* sno */ snoReverb;
        public int /* sno */ snoWeather;
        public int /* sno */ snoPresetWorld;
        public int Field15;
        public int Field16;
        public int Field17;
        public int Field18;
        public SceneCachedValues tCachedValues;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = new IVector2D();
            Field1.Parse(buffer);
            arSnoLevelAreas = new int /* sno */[4];
            for (int i = 0; i < arSnoLevelAreas.Length; i++) arSnoLevelAreas[i] = buffer.ReadInt(32);
            snoPrevWorld = buffer.ReadInt(32);
            Field4 = buffer.ReadInt(32);
            snoPrevLevelArea = buffer.ReadInt(32);
            snoNextWorld = buffer.ReadInt(32);
            Field7 = buffer.ReadInt(32);
            snoNextLevelArea = buffer.ReadInt(32);
            snoMusic = buffer.ReadInt(32);
            snoCombatMusic = buffer.ReadInt(32);
            snoAmbient = buffer.ReadInt(32);
            snoReverb = buffer.ReadInt(32);
            snoWeather = buffer.ReadInt(32);
            snoPresetWorld = buffer.ReadInt(32);
            Field15 = buffer.ReadInt(32);
            Field16 = buffer.ReadInt(32);
            Field17 = buffer.ReadInt(32);
            Field18 = buffer.ReadInt(32);
            tCachedValues = new SceneCachedValues();
            tCachedValues.Parse(buffer);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            Field1.Encode(buffer);
            for (int i = 0; i < arSnoLevelAreas.Length; i++) buffer.WriteInt(32, arSnoLevelAreas[i]);
            buffer.WriteInt(32, snoPrevWorld);
            buffer.WriteInt(32, Field4);
            buffer.WriteInt(32, snoPrevLevelArea);
            buffer.WriteInt(32, snoNextWorld);
            buffer.WriteInt(32, Field7);
            buffer.WriteInt(32, snoNextLevelArea);
            buffer.WriteInt(32, snoMusic);
            buffer.WriteInt(32, snoCombatMusic);
            buffer.WriteInt(32, snoAmbient);
            buffer.WriteInt(32, snoReverb);
            buffer.WriteInt(32, snoWeather);
            buffer.WriteInt(32, snoPresetWorld);
            buffer.WriteInt(32, Field15);
            buffer.WriteInt(32, Field16);
            buffer.WriteInt(32, Field17);
            buffer.WriteInt(32, Field18);
            tCachedValues.Encode(buffer);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("SceneSpecification:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad);
            b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            Field1.AsText(b, pad);
            b.Append(' ', pad);
            b.AppendLine("arSnoLevelAreas:");
            b.Append(' ', pad);
            b.AppendLine("{");
            for (int i = 0; i < arSnoLevelAreas.Length;)
            {
                b.Append(' ', pad + 1);
                for (int j = 0; j < 8 && i < arSnoLevelAreas.Length; j++, i++)
                {
                    b.Append("0x" + arSnoLevelAreas[i].ToString("X8") + ", ");
                }
                b.AppendLine();
            }
            b.Append(' ', pad);
            b.AppendLine("}");
            b.AppendLine();
            b.Append(' ', pad);
            b.AppendLine("snoPrevWorld: 0x" + snoPrevWorld.ToString("X8"));
            b.Append(' ', pad);
            b.AppendLine("Field4: 0x" + Field4.ToString("X8") + " (" + Field4 + ")");
            b.Append(' ', pad);
            b.AppendLine("snoPrevLevelArea: 0x" + snoPrevLevelArea.ToString("X8"));
            b.Append(' ', pad);
            b.AppendLine("snoNextWorld: 0x" + snoNextWorld.ToString("X8"));
            b.Append(' ', pad);
            b.AppendLine("Field7: 0x" + Field7.ToString("X8") + " (" + Field7 + ")");
            b.Append(' ', pad);
            b.AppendLine("snoNextLevelArea: 0x" + snoNextLevelArea.ToString("X8"));
            b.Append(' ', pad);
            b.AppendLine("snoMusic: 0x" + snoMusic.ToString("X8"));
            b.Append(' ', pad);
            b.AppendLine("snoCombatMusic: 0x" + snoCombatMusic.ToString("X8"));
            b.Append(' ', pad);
            b.AppendLine("snoAmbient: 0x" + snoAmbient.ToString("X8"));
            b.Append(' ', pad);
            b.AppendLine("snoReverb: 0x" + snoReverb.ToString("X8"));
            b.Append(' ', pad);
            b.AppendLine("snoWeather: 0x" + snoWeather.ToString("X8"));
            b.Append(' ', pad);
            b.AppendLine("snoPresetWorld: 0x" + snoPresetWorld.ToString("X8"));
            b.Append(' ', pad);
            b.AppendLine("Field15: 0x" + Field15.ToString("X8") + " (" + Field15 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field16: 0x" + Field16.ToString("X8") + " (" + Field16 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field17: 0x" + Field17.ToString("X8") + " (" + Field17 + ")");
            b.Append(' ', pad);
            b.AppendLine("Field18: 0x" + Field18.ToString("X8") + " (" + Field18 + ")");
            tCachedValues.AsText(b, pad);
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }
}