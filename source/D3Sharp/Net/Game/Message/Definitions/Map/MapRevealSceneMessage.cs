/*
 * Copyright (C) 2011 D3Sharp Project
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
using System.Text;
using D3Sharp.Net.Game.Message.Fields;

namespace D3Sharp.Net.Game.Message.Definitions.Map
{
    public class MapRevealSceneMessage : GameMessage
    {
        public int ChunkID;
        public int /* sno */ snoScene;
        public PRTransform Field2;
        public int Field3;
        public int MiniMapVisibility;

        public override void Parse(GameBitBuffer buffer)
        {
            ChunkID = buffer.ReadInt(32);
            snoScene = buffer.ReadInt(32);
            Field2 = new PRTransform();
            Field2.Parse(buffer);
            Field3 = buffer.ReadInt(32);
            MiniMapVisibility = buffer.ReadInt(3);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, ChunkID);
            buffer.WriteInt(32, snoScene);
            Field2.Encode(buffer);
            buffer.WriteInt(32, Field3);
            buffer.WriteInt(3, MiniMapVisibility);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("MapRevealSceneMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("ChunkID: 0x" + ChunkID.ToString("X8") + " (" + ChunkID + ")");
            b.Append(' ', pad); b.AppendLine("snoScene: 0x" + snoScene.ToString("X8"));
            Field2.AsText(b, pad);
            b.Append(' ', pad); b.AppendLine("Field3: 0x" + Field3.ToString("X8") + " (" + Field3 + ")");
            b.Append(' ', pad); b.AppendLine("MiniMapVisibility: 0x" + MiniMapVisibility.ToString("X8") + " (" + MiniMapVisibility + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }

        public MapRevealSceneMessage()
        {

        }

        public MapRevealSceneMessage(string[] data2, int f3)
        {
            Id = 0x0044;
            ChunkID = int.Parse(data2[0]);
            snoScene = int.Parse(data2[1]);
            Field2 = new PRTransform()
                         {
                             Field0 = new Quaternion()
                                          {
                                              Amount = float.Parse(data2[5], System.Globalization.CultureInfo.InvariantCulture),
                                              Axis = new Vector3D()
                                                           {
                                                               X = float.Parse(data2[2], System.Globalization.CultureInfo.InvariantCulture),
                                                               Y = float.Parse(data2[3], System.Globalization.CultureInfo.InvariantCulture),
                                                               Z = float.Parse(data2[4], System.Globalization.CultureInfo.InvariantCulture),
                                                           },
                                          },
                             ReferencePoint = new Vector3D()
                                          {
                                              X = float.Parse(data2[6], System.Globalization.CultureInfo.InvariantCulture),
                                              Y = float.Parse(data2[7], System.Globalization.CultureInfo.InvariantCulture),
                                              Z = float.Parse(data2[8], System.Globalization.CultureInfo.InvariantCulture),
                                          },
                         };
            Field3 = f3;//int.Parse(data2[9]),
            MiniMapVisibility = int.Parse(data2[10]);
        }


    }
}