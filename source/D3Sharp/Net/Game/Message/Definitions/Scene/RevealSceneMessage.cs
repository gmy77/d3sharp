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

namespace D3Sharp.Net.Game.Message.Definitions.Scene
{
    public class RevealSceneMessage : GameMessage
    {
        public int WorldID;
        public SceneSpecification SceneSpec;
        public int ChunkID;
        public int /* sno */ snoScene;
        public PRTransform Position;
        public int ParentChunkID;
        public int /* sno */ snoSceneGroup;
        // MaxLength = 256
        public int /* gbid */[] arAppliedLabels;

        public override void Parse(GameBitBuffer buffer)
        {
            WorldID = buffer.ReadInt(32);
            SceneSpec = new SceneSpecification();
            SceneSpec.Parse(buffer);
            ChunkID = buffer.ReadInt(32);
            snoScene = buffer.ReadInt(32);
            Position = new PRTransform();
            Position.Parse(buffer);
            ParentChunkID = buffer.ReadInt(32);
            snoSceneGroup = buffer.ReadInt(32);
            arAppliedLabels = new int /* gbid */[buffer.ReadInt(9)];
            for (int i = 0; i < arAppliedLabels.Length; i++) arAppliedLabels[i] = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, WorldID);
            SceneSpec.Encode(buffer);
            buffer.WriteInt(32, ChunkID);
            buffer.WriteInt(32, snoScene);
            Position.Encode(buffer);
            buffer.WriteInt(32, ParentChunkID);
            buffer.WriteInt(32, snoSceneGroup);
            buffer.WriteInt(9, arAppliedLabels.Length);
            for (int i = 0; i < arAppliedLabels.Length; i++) buffer.WriteInt(32, arAppliedLabels[i]);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("RevealSceneMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("WorldID: 0x" + WorldID.ToString("X8") + " (" + WorldID + ")");
            SceneSpec.AsText(b, pad);
            b.Append(' ', pad); b.AppendLine("ChunkID: 0x" + ChunkID.ToString("X8") + " (" + ChunkID + ")");
            b.Append(' ', pad); b.AppendLine("snoScene: 0x" + snoScene.ToString("X8"));
            Position.AsText(b, pad);
            b.Append(' ', pad); b.AppendLine("ParentChunkID: 0x" + ParentChunkID.ToString("X8") + " (" + ParentChunkID + ")");
            b.Append(' ', pad); b.AppendLine("snoSceneGroup: 0x" + snoSceneGroup.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("arAppliedLabels:");
            b.Append(' ', pad); b.AppendLine("{");
            for (int i = 0; i < arAppliedLabels.Length; ) { b.Append(' ', pad + 1); for (int j = 0; j < 8 && i < arAppliedLabels.Length; j++, i++) { b.Append("0x" + arAppliedLabels[i].ToString("X8") + ", "); } b.AppendLine(); }
            b.Append(' ', pad); b.AppendLine("}"); b.AppendLine();
            b.Append(' ', --pad);
            b.AppendLine("}");
        }

        public RevealSceneMessage()
        {

        }

        public RevealSceneMessage(string[] data, int f0)
        {
            Id = 0x0034;
            WorldID = f0;//0x772E0000; //int.Parse(data[0]),
            SceneSpec = new SceneSpecification()
                            {
                                Field0 = int.Parse(data[1]),
                                Field1 = new IVector2D()
                                             {
                                                 Field0 = int.Parse(data[2]),
                                                 Field1 = int.Parse(data[3]),
                                             },
                                arSnoLevelAreas = new int[4] { int.Parse(data[4]), int.Parse(data[5]), int.Parse(data[6]), int.Parse(data[7]), },
                                snoPrevWorld = int.Parse(data[8]),
                                Field4 = int.Parse(data[9]),
                                snoPrevLevelArea = int.Parse(data[10]),
                                snoNextWorld = int.Parse(data[11]),
                                Field7 = int.Parse(data[12]),
                                snoNextLevelArea = int.Parse(data[13]),
                                snoMusic = int.Parse(data[14]),
                                snoCombatMusic = int.Parse(data[15]),
                                snoAmbient = int.Parse(data[16]),
                                snoReverb = int.Parse(data[17]),
                                snoWeather = int.Parse(data[18]),
                                snoPresetWorld = int.Parse(data[19]),
                                Field15 = int.Parse(data[20]),
                                Field16 = int.Parse(data[21]),
                                Field17 = int.Parse(data[22]),
                                Field18 = int.Parse(data[23]),
                                tCachedValues = new SceneCachedValues()
                                                    {
                                                        Field0 = int.Parse(data[24]),
                                                        Field1 = int.Parse(data[25]),
                                                        Field2 = int.Parse(data[26]),
                                                        Field3 = new AABB()
                                                                     {
                                                                         Field0 = new Vector3D()
                                                                                      {
                                                                                          X = float.Parse(data[27], System.Globalization.CultureInfo.InvariantCulture),
                                                                                          Y = float.Parse(data[28], System.Globalization.CultureInfo.InvariantCulture),
                                                                                          Z = float.Parse(data[29], System.Globalization.CultureInfo.InvariantCulture),
                                                                                      },
                                                                         Field1 = new Vector3D()
                                                                                      {
                                                                                          X = float.Parse(data[30], System.Globalization.CultureInfo.InvariantCulture),
                                                                                          Y = float.Parse(data[31], System.Globalization.CultureInfo.InvariantCulture),
                                                                                          Z = float.Parse(data[32], System.Globalization.CultureInfo.InvariantCulture),
                                                                                      },
                                                                     },
                                                        Field4 = new AABB()
                                                                     {
                                                                         Field0 = new Vector3D()
                                                                                      {
                                                                                          X = float.Parse(data[33], System.Globalization.CultureInfo.InvariantCulture),
                                                                                          Y = float.Parse(data[34], System.Globalization.CultureInfo.InvariantCulture),
                                                                                          Z = float.Parse(data[35], System.Globalization.CultureInfo.InvariantCulture),
                                                                                      },
                                                                         Field1 = new Vector3D()
                                                                                      {
                                                                                          X = float.Parse(data[36], System.Globalization.CultureInfo.InvariantCulture),
                                                                                          Y = float.Parse(data[37], System.Globalization.CultureInfo.InvariantCulture),
                                                                                          Z = float.Parse(data[38], System.Globalization.CultureInfo.InvariantCulture),
                                                                                      },
                                                                     },
                                                        Field5 = new int[4] { int.Parse(data[39]), int.Parse(data[40]), int.Parse(data[41]), int.Parse(data[42]), },
                                                        Field6 = int.Parse(data[43]),
                                                    },
                            };
            ChunkID = int.Parse(data[44]);
            snoScene = int.Parse(data[45]);
            Position = new PRTransform()
                           {
                               Field0 = new Quaternion()
                                            {
                                                Amount = float.Parse(data[49], System.Globalization.CultureInfo.InvariantCulture),
                                                Axis = new Vector3D()
                                                             {
                                                                 X = float.Parse(data[46], System.Globalization.CultureInfo.InvariantCulture),
                                                                 Y = float.Parse(data[47], System.Globalization.CultureInfo.InvariantCulture),
                                                                 Z = float.Parse(data[48], System.Globalization.CultureInfo.InvariantCulture),
                                                             },
                                            },
                               ReferencePoint = new Vector3D()
                                            {
                                                X = float.Parse(data[50], System.Globalization.CultureInfo.InvariantCulture),
                                                Y = float.Parse(data[51], System.Globalization.CultureInfo.InvariantCulture),
                                                Z = float.Parse(data[52], System.Globalization.CultureInfo.InvariantCulture),
                                            },
                           };

            ParentChunkID = int.Parse(data[53]);
            snoSceneGroup = int.Parse(data[54]);
            arAppliedLabels = new int[0];
        }

    }
}