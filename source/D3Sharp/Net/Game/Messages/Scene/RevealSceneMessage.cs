using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace D3Sharp.Net.Game.Messages.Scene
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

        public RevealSceneMessage():base(Opcodes.RevealSceneMessage)
        {  }

        public RevealSceneMessage(string[] data, int f0)
            : base(Opcodes.RevealSceneMessage)
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
                            Field0 = float.Parse(data[27], System.Globalization.CultureInfo.InvariantCulture),
                            Field1 = float.Parse(data[28], System.Globalization.CultureInfo.InvariantCulture),
                            Field2 = float.Parse(data[29], System.Globalization.CultureInfo.InvariantCulture),
                        },
                        Field1 = new Vector3D()
                        {
                            Field0 = float.Parse(data[30], System.Globalization.CultureInfo.InvariantCulture),
                            Field1 = float.Parse(data[31], System.Globalization.CultureInfo.InvariantCulture),
                            Field2 = float.Parse(data[32], System.Globalization.CultureInfo.InvariantCulture),
                        },
                    },
                    Field4 = new AABB()
                    {
                        Field0 = new Vector3D()
                        {
                            Field0 = float.Parse(data[33], System.Globalization.CultureInfo.InvariantCulture),
                            Field1 = float.Parse(data[34], System.Globalization.CultureInfo.InvariantCulture),
                            Field2 = float.Parse(data[35], System.Globalization.CultureInfo.InvariantCulture),
                        },
                        Field1 = new Vector3D()
                        {
                            Field0 = float.Parse(data[36], System.Globalization.CultureInfo.InvariantCulture),
                            Field1 = float.Parse(data[37], System.Globalization.CultureInfo.InvariantCulture),
                            Field2 = float.Parse(data[38], System.Globalization.CultureInfo.InvariantCulture),
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
                    Field0 = float.Parse(data[49], System.Globalization.CultureInfo.InvariantCulture),
                    Field1 = new Vector3D()
                    {
                        Field0 = float.Parse(data[46], System.Globalization.CultureInfo.InvariantCulture),
                        Field1 = float.Parse(data[47], System.Globalization.CultureInfo.InvariantCulture),
                        Field2 = float.Parse(data[48], System.Globalization.CultureInfo.InvariantCulture),
                    },
                },
                Field1 = new Vector3D()
                {
                    Field0 = float.Parse(data[50], System.Globalization.CultureInfo.InvariantCulture),
                    Field1 = float.Parse(data[51], System.Globalization.CultureInfo.InvariantCulture),
                    Field2 = float.Parse(data[52], System.Globalization.CultureInfo.InvariantCulture),
                },
            };

            ParentChunkID = int.Parse(data[53]);
            snoSceneGroup = int.Parse(data[54]);
            arAppliedLabels = new int[0];
        }

        public override void Handle(GameClient client)
        {
            throw new NotImplementedException();
        }

        public override void Parse(GameBitBuffer buffer)
        {
            throw new NotImplementedException();
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
    }
}
