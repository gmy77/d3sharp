using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace D3Sharp.Net.Game.Messages.Map
{
    public class MapRevealSceneMessage : GameMessage
    {
        public int ChunkID;
        public int /* sno */ snoScene;
        public PRTransform Field2;
        public int Field3;
        public int MiniMapVisibility;

        public MapRevealSceneMessage():base(Opcodes.MapRevealSceneMessage){}

        public MapRevealSceneMessage(string[] data2, int f3)
            : base(Opcodes.MapRevealSceneMessage)
        {
            Id = 0x0044;
            ChunkID = int.Parse(data2[0]);
            snoScene = int.Parse(data2[1]);
            Field2 = new PRTransform()
            {
                Field0 = new Quaternion()
                {
                    Field0 = float.Parse(data2[5], System.Globalization.CultureInfo.InvariantCulture),
                    Field1 = new Vector3D()
                    {
                        Field0 = float.Parse(data2[2], System.Globalization.CultureInfo.InvariantCulture),
                        Field1 = float.Parse(data2[3], System.Globalization.CultureInfo.InvariantCulture),
                        Field2 = float.Parse(data2[4], System.Globalization.CultureInfo.InvariantCulture),
                    },
                },
                Field1 = new Vector3D()
                {
                    Field0 = float.Parse(data2[6], System.Globalization.CultureInfo.InvariantCulture),
                    Field1 = float.Parse(data2[7], System.Globalization.CultureInfo.InvariantCulture),
                    Field2 = float.Parse(data2[8], System.Globalization.CultureInfo.InvariantCulture),
                },
            };
            Field3 = f3;//int.Parse(data2[9]),
            MiniMapVisibility = int.Parse(data2[10]);
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
    }
}
