using System;
using System.Text;

namespace D3Sharp.Net.Game
{
    public class GameSetupMessage : GameMessage
    {
        public int Field0;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("GameSetupMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class ConnectionEstablishedMessage : GameMessage
    {
        public int Field0;
        public int Field1;
        public int Field2;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(3);
            Field1 = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(3, Field0);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(32, Field2);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("ConnectionEstablishedMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class QuitGameMessage : GameMessage
    {
        public int Field0;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(3);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(3, Field0);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("QuitGameMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class DWordDataMessage : GameMessage
    {
        public int Field0;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("DWordDataMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class BroadcastTextMessage : GameMessage
    {
        public string Field0;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadCharArray(512);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteCharArray(512, Field0);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("BroadcastTextMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: \"" + Field0 + "\"");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class GenericBlobMessage : GameMessage
    {
        public byte[] Data;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Data = buffer.ReadBlob(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteBlob(32, Data);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("GenericBlobMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Length: 0x" + Data.Length.ToString("X8") + " (" + Data.Length + ")");
            for (int i = 0; i < Data.Length; i += 16)
            {

                b.Append(' ', pad);
                b.Append(i.ToString("X4"));
                b.Append(' ');

                int off = i;
                for (int j = 0; j < 8; j++, off++)
                {
                    if (off < Data.Length)
                    {
                        b.Append(Data[off].ToString("X2"));
                        b.Append(' ');
                    }
                    else
                    {
                        b.Append(' '); b.Append(' '); b.Append(' ');
                    }
                }
                b.Append(' ');
                off = i + 8;
                for (int j = 0; j < 8; j++, off++)
                {
                    if (off < Data.Length)
                    {
                        b.Append(Data[off].ToString("X2"));
                        b.Append(' ');
                    }
                    else
                    {
                        b.Append(' '); b.Append(' '); b.Append(' ');
                    }
                }

                b.Append(' ');

                off = i;
                for (int j = 0; j < 8; j++, off++)
                {
                    if (off < Data.Length)
                    {
                        if (Data[off] >= 20 && Data[off] < 128)
                            b.Append((char)Data[off]);
                        else
                            b.Append('.');
                    }
                    else
                        b.Append(' ');
                }
                b.Append(' ');
                off = i + 8;
                for (int j = 0; j < 8; j++, off++)
                {
                    if (off < Data.Length)
                    {
                        if (Data[off] >= 20 && Data[off] < 128)
                            b.Append((char)Data[off]);
                        else
                            b.Append('.');
                    }
                    else
                        b.Append(' ');
                }
                b.AppendLine();
            }
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class UInt64DataMessage : GameMessage
    {
        public long Field0;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt64(64);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt64(64, Field0);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("UInt64DataMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X16"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class VersionsMessage : GameMessage
    {
        public int SNOPackHash;
        public int ProtocolHash;
        public string Version;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            SNOPackHash = buffer.ReadInt(32);
            ProtocolHash = buffer.ReadInt(32);
            Version = buffer.ReadCharArray(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, SNOPackHash);
            buffer.WriteInt(32, ProtocolHash);
            buffer.WriteCharArray(32, Version);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("VersionsMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("SNOPackHash: 0x" + SNOPackHash.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("ProtocolHash: 0x" + ProtocolHash.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Version: \"" + Version + "\"");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class PlayerIndexMessage : GameMessage
    {
        public int Field0;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(4) + (-1);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(4, Field0 - (-1));
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("PlayerIndexMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class NewPlayerMessage : GameMessage
    {
        public int Field0;
        public string Field1;
        public string Field2;
        public int Field3;
        public int Field4;
        public int /* sno */ snoActorPortrait;
        public int Field6;
        public HeroStateData Field7;
        public bool Field8;
        public int Field9;
        public int Field10;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(3);
            Field1 = buffer.ReadCharArray(128);
            Field2 = buffer.ReadCharArray(101);
            Field3 = buffer.ReadInt(5) + (-1);
            Field4 = buffer.ReadInt(3) + (-1);
            snoActorPortrait = buffer.ReadInt(32);
            Field6 = buffer.ReadInt(7);
            Field7 = new HeroStateData();
            Field7.Parse(buffer);
            Field8 = buffer.ReadBool();
            Field9 = buffer.ReadInt(32);
            Field10 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(3, Field0);
            buffer.WriteCharArray(128, Field1);
            buffer.WriteCharArray(101, Field2);
            buffer.WriteInt(5, Field3 - (-1));
            buffer.WriteInt(3, Field4 - (-1));
            buffer.WriteInt(32, snoActorPortrait);
            buffer.WriteInt(7, Field6);
            Field7.Encode(buffer);
            buffer.WriteBool(Field8);
            buffer.WriteInt(32, Field9);
            buffer.WriteInt(32, Field10);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("NewPlayerMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: \"" + Field1 + "\"");
            b.Append(' ', pad); b.AppendLine("Field2: \"" + Field2 + "\"");
            b.Append(' ', pad); b.AppendLine("Field3: 0x" + Field3.ToString("X8") + " (" + Field3 + ")");
            b.Append(' ', pad); b.AppendLine("Field4: 0x" + Field4.ToString("X8") + " (" + Field4 + ")");
            b.Append(' ', pad); b.AppendLine("snoActorPortrait: 0x" + snoActorPortrait.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field6: 0x" + Field6.ToString("X8") + " (" + Field6 + ")");
            Field7.AsText(b, pad);
            b.Append(' ', pad); b.AppendLine("Field8: " + (Field8 ? "true" : "false"));
            b.Append(' ', pad); b.AppendLine("Field9: 0x" + Field9.ToString("X8") + " (" + Field9 + ")");
            b.Append(' ', pad); b.AppendLine("Field10: 0x" + Field10.ToString("X8") + " (" + Field10 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class EnterWorldMessage : GameMessage
    {
        public Vector3D Field0;
        public int Field1;
        public int /* sno */ Field2;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = new Vector3D();
            Field0.Parse(buffer);
            Field1 = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            Field0.Encode(buffer);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(32, Field2);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("EnterWorldMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            Field0.AsText(b, pad);
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class RevealWorldMessage : GameMessage
    {
        public int Field0;
        public int /* sno */ Field1;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(32, Field1);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("RevealWorldMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class RevealSceneMessage : GameMessage
    {
        public int Field0;
        public SceneSpecification Field1;
        public int Field2;
        public int /* sno */ snoScene;
        public PRTransform Field4;
        public int Field5;
        public int /* sno */ snoSceneGroup;
        // MaxLength = 256
        public int /* gbid */[] arAppliedLabels;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = new SceneSpecification();
            Field1.Parse(buffer);
            Field2 = buffer.ReadInt(32);
            snoScene = buffer.ReadInt(32);
            Field4 = new PRTransform();
            Field4.Parse(buffer);
            Field5 = buffer.ReadInt(32);
            snoSceneGroup = buffer.ReadInt(32);
            arAppliedLabels = new int /* gbid */[buffer.ReadInt(9)];
            for (int i = 0; i < arAppliedLabels.Length; i++) arAppliedLabels[i] = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            Field1.Encode(buffer);
            buffer.WriteInt(32, Field2);
            buffer.WriteInt(32, snoScene);
            Field4.Encode(buffer);
            buffer.WriteInt(32, Field5);
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
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            Field1.AsText(b, pad);
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', pad); b.AppendLine("snoScene: 0x" + snoScene.ToString("X8"));
            Field4.AsText(b, pad);
            b.Append(' ', pad); b.AppendLine("Field5: 0x" + Field5.ToString("X8") + " (" + Field5 + ")");
            b.Append(' ', pad); b.AppendLine("snoSceneGroup: 0x" + snoSceneGroup.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("arAppliedLabels:");
            b.Append(' ', pad); b.AppendLine("{");
            for (int i = 0; i < arAppliedLabels.Length; ) { b.Append(' ', pad + 1); for (int j = 0; j < 8 && i < arAppliedLabels.Length; j++, i++) { b.Append("0x" + arAppliedLabels[i].ToString("X8") + ", "); } b.AppendLine(); }
            b.Append(' ', pad); b.AppendLine("}"); b.AppendLine();
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class DestroySceneMessage : GameMessage
    {
        public int Field0;
        public int Field1;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(32, Field1);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("DestroySceneMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class SwapSceneMessage : GameMessage
    {
        public int Field0;
        public int Field1;
        public int Field2;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(32, Field2);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("SwapSceneMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class RevealTeamMessage : GameMessage
    {
        public int Field0;
        public int Field1;
        public int Field2;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(5) + (-1);
            Field1 = buffer.ReadInt(2);
            Field2 = buffer.ReadInt(2) + (-1);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(5, Field0 - (-1));
            buffer.WriteInt(2, Field1);
            buffer.WriteInt(2, Field2 - (-1));
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("RevealTeamMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class HeroStateMessage : GameMessage
    {
        public HeroStateData Field0;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = new HeroStateData();
            Field0.Parse(buffer);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            Field0.Encode(buffer);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("HeroStateMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            Field0.AsText(b, pad);
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class ACDEnterKnownMessage : GameMessage
    {
        public int Field0;
        public int /* sno */ Field1;
        public int Field2;
        public int Field3;
        public WorldLocationMessageData Field4;
        public InventoryLocationMessageData Field5;
        public GBHandle Field6;
        public int Field7;
        public int Field8;
        public int Field9;
        public byte Field10;
        public int /* sno */? Field11;
        public int? Field12;
        public int? Field13;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(5);
            Field3 = buffer.ReadInt(2) + (-1);
            if (buffer.ReadBool())
            {
                Field4 = new WorldLocationMessageData();
                Field4.Parse(buffer);
            }
            if (buffer.ReadBool())
            {
                Field5 = new InventoryLocationMessageData();
                Field5.Parse(buffer);
            }
            Field6 = new GBHandle();
            Field6.Parse(buffer);
            Field7 = buffer.ReadInt(32);
            Field8 = buffer.ReadInt(32);
            Field9 = buffer.ReadInt(4) + (-1);
            Field10 = (byte)buffer.ReadInt(8);
            if (buffer.ReadBool())
            {
                Field11 = buffer.ReadInt(32);
            }
            if (buffer.ReadBool())
            {
                Field12 = buffer.ReadInt(32);
            }
            if (buffer.ReadBool())
            {
                Field13 = buffer.ReadInt(32);
            }
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(5, Field2);
            buffer.WriteInt(2, Field3 - (-1));
            buffer.WriteBool(Field4 != null);
            if (Field4 != null)
            {
                Field4.Encode(buffer);
            }
            buffer.WriteBool(Field5 != null);
            if (Field5 != null)
            {
                Field5.Encode(buffer);
            }
            Field6.Encode(buffer);
            buffer.WriteInt(32, Field7);
            buffer.WriteInt(32, Field8);
            buffer.WriteInt(4, Field9 - (-1));
            buffer.WriteInt(8, Field10);
            buffer.WriteBool(Field11.HasValue);
            if (Field11.HasValue)
            {
                buffer.WriteInt(32, Field11.Value);
            }
            buffer.WriteBool(Field12.HasValue);
            if (Field12.HasValue)
            {
                buffer.WriteInt(32, Field12.Value);
            }
            buffer.WriteBool(Field13.HasValue);
            if (Field13.HasValue)
            {
                buffer.WriteInt(32, Field13.Value);
            }
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("ACDEnterKnownMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', pad); b.AppendLine("Field3: 0x" + Field3.ToString("X8") + " (" + Field3 + ")");
            if (Field4 != null)
            {
                Field4.AsText(b, pad);
            }
            if (Field5 != null)
            {
                Field5.AsText(b, pad);
            }
            Field6.AsText(b, pad);
            b.Append(' ', pad); b.AppendLine("Field7: 0x" + Field7.ToString("X8") + " (" + Field7 + ")");
            b.Append(' ', pad); b.AppendLine("Field8: 0x" + Field8.ToString("X8") + " (" + Field8 + ")");
            b.Append(' ', pad); b.AppendLine("Field9: 0x" + Field9.ToString("X8") + " (" + Field9 + ")");
            b.Append(' ', pad); b.AppendLine("Field10: 0x" + Field10.ToString("X2"));
            if (Field11.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("Field11.Value: 0x" + Field11.Value.ToString("X8"));
            }
            if (Field12.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("Field12.Value: 0x" + Field12.Value.ToString("X8") + " (" + Field12.Value + ")");
            }
            if (Field13.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("Field13.Value: 0x" + Field13.Value.ToString("X8") + " (" + Field13.Value + ")");
            }
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class ANNDataMessage : GameMessage
    {
        public int Field0;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("ANNDataMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class PlayerEnterKnownMessage : GameMessage
    {
        public int Field0;
        public int Field1;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(3);
            Field1 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(3, Field0);
            buffer.WriteInt(32, Field1);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("PlayerEnterKnownMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class ACDWorldPositionMessage : GameMessage
    {
        public int Field0;
        public WorldLocationMessageData Field1;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = new WorldLocationMessageData();
            Field1.Parse(buffer);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            Field1.Encode(buffer);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("ACDWorldPositionMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            Field1.AsText(b, pad);
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class ACDInventoryPositionMessage : GameMessage
    {
        public int Field0;
        public InventoryLocationMessageData Field1;
        public int Field2;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = new InventoryLocationMessageData();
            Field1.Parse(buffer);
            Field2 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            Field1.Encode(buffer);
            buffer.WriteInt(32, Field2);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("ACDInventoryPositionMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            Field1.AsText(b, pad);
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class ACDInventoryUpdateActorSNO : GameMessage
    {
        public int Field0;
        public int /* sno */ Field1;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(32, Field1);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("ACDInventoryUpdateActorSNO:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class PlayerActorSetInitialMessage : GameMessage
    {
        public int Field0;
        public int Field1;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(3);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(3, Field1);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("PlayerActorSetInitialMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class VisualInventoryMessage : GameMessage
    {
        public int Field0;
        public VisualEquipment Field1;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = new VisualEquipment();
            Field1.Parse(buffer);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            Field1.Encode(buffer);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("VisualInventoryMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            Field1.AsText(b, pad);
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class ACDChangeGBHandleMessage : GameMessage
    {
        public int Field0;
        public GBHandle Field1;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = new GBHandle();
            Field1.Parse(buffer);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            Field1.Encode(buffer);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("ACDChangeGBHandleMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            Field1.AsText(b, pad);
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class AffixMessage : GameMessage
    {
        public int Field0;
        public int Field1;
        // MaxLength = 32
        public int /* gbid */[] aAffixGBIDs;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(2);
            aAffixGBIDs = new int /* gbid */[buffer.ReadInt(6)];
            for (int i = 0; i < aAffixGBIDs.Length; i++) aAffixGBIDs[i] = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(2, Field1);
            buffer.WriteInt(6, aAffixGBIDs.Length);
            for (int i = 0; i < aAffixGBIDs.Length; i++) buffer.WriteInt(32, aAffixGBIDs[i]);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("AffixMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad); b.AppendLine("aAffixGBIDs:");
            b.Append(' ', pad); b.AppendLine("{");
            for (int i = 0; i < aAffixGBIDs.Length; ) { b.Append(' ', pad + 1); for (int j = 0; j < 8 && i < aAffixGBIDs.Length; j++, i++) { b.Append("0x" + aAffixGBIDs[i].ToString("X8") + ", "); } b.AppendLine(); }
            b.Append(' ', pad); b.AppendLine("}"); b.AppendLine();
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class LearnedSkillMessage : GameMessage
    {
        // MaxLength = 128
        public int /* sno */[] aSkillSNOs;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            aSkillSNOs = new int /* sno */[buffer.ReadInt(8)];
            for (int i = 0; i < aSkillSNOs.Length; i++) aSkillSNOs[i] = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(8, aSkillSNOs.Length);
            for (int i = 0; i < aSkillSNOs.Length; i++) buffer.WriteInt(32, aSkillSNOs[i]);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("LearnedSkillMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("aSkillSNOs:");
            b.Append(' ', pad); b.AppendLine("{");
            for (int i = 0; i < aSkillSNOs.Length; ) { b.Append(' ', pad + 1); for (int j = 0; j < 8 && i < aSkillSNOs.Length; j++, i++) { b.Append("0x" + aSkillSNOs[i].ToString("X8") + ", "); } b.AppendLine(); }
            b.Append(' ', pad); b.AppendLine("}"); b.AppendLine();
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class PortalSpecifierMessage : GameMessage
    {
        public int Field0;
        public ResolvedPortalDestination Field1;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = new ResolvedPortalDestination();
            Field1.Parse(buffer);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            Field1.Encode(buffer);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("PortalSpecifierMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            Field1.AsText(b, pad);
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class RareMonsterNamesMessage : GameMessage
    {
        public int Field0;
        // MaxLength = 2
        public int /* gbid */[] Field1;
        // MaxLength = 8
        public int /* gbid */[] Field2;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = new int /* gbid */[2];
            for (int i = 0; i < Field1.Length; i++) Field1[i] = buffer.ReadInt(32);
            Field2 = new int /* gbid */[8];
            for (int i = 0; i < Field2.Length; i++) Field2[i] = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            for (int i = 0; i < Field1.Length; i++) buffer.WriteInt(32, Field1[i]);
            for (int i = 0; i < Field2.Length; i++) buffer.WriteInt(32, Field2[i]);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("RareMonsterNamesMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1:");
            b.Append(' ', pad); b.AppendLine("{");
            for (int i = 0; i < Field1.Length; ) { b.Append(' ', pad + 1); for (int j = 0; j < 8 && i < Field1.Length; j++, i++) { b.Append("0x" + Field1[i].ToString("X8") + ", "); } b.AppendLine(); }
            b.Append(' ', pad); b.AppendLine("}"); b.AppendLine();
            b.Append(' ', pad); b.AppendLine("Field2:");
            b.Append(' ', pad); b.AppendLine("{");
            for (int i = 0; i < Field2.Length; ) { b.Append(' ', pad + 1); for (int j = 0; j < 8 && i < Field2.Length; j++, i++) { b.Append("0x" + Field2[i].ToString("X8") + ", "); } b.AppendLine(); }
            b.Append(' ', pad); b.AppendLine("}"); b.AppendLine();
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class RareItemNameMessage : GameMessage
    {
        public int Field0;
        public RareItemName Field1;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = new RareItemName();
            Field1.Parse(buffer);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            Field1.Encode(buffer);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("RareItemNameMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            Field1.AsText(b, pad);
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class AttributeSetValueMessage : GameMessage
    {
        public int Field0;
        public NetAttributeKeyValue Field1;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = new NetAttributeKeyValue();
            Field1.Parse(buffer);
            Field1.ParseValue(buffer);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            Field1.Encode(buffer);
            Field1.EncodeValue(buffer);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("AttributeSetValueMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            Field1.AsText(b, pad);
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class ProjectileStickMessage : GameMessage
    {
        public Vector3D Field0;
        public int Field1;
        public int /* sno */ Field2;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = new Vector3D();
            Field0.Parse(buffer);
            Field1 = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            Field0.Encode(buffer);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(32, Field2);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("ProjectileStickMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            Field0.AsText(b, pad);
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class AttributesSetValuesMessage : GameMessage
    {
        public int Field0;
        // MaxLength = 15
        public NetAttributeKeyValue[] atKeyVals;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            atKeyVals = new NetAttributeKeyValue[buffer.ReadInt(4)];
            for (int i = 0; i < atKeyVals.Length; i++) { atKeyVals[i] = new NetAttributeKeyValue(); atKeyVals[i].Parse(buffer); }
            for (int i = 0; i < atKeyVals.Length; i++) { atKeyVals[i].ParseValue(buffer); }
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(4, atKeyVals.Length);
            for (int i = 0; i < atKeyVals.Length; i++) { atKeyVals[i].Encode(buffer); }
            for (int i = 0; i < atKeyVals.Length; i++) { atKeyVals[i].EncodeValue(buffer); }
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("AttributesSetValuesMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("atKeyVals:");
            b.Append(' ', pad); b.AppendLine("{");
            for (int i = 0; i < atKeyVals.Length; i++) { atKeyVals[i].AsText(b, pad + 1); b.AppendLine(); }
            b.Append(' ', pad); b.AppendLine("}"); b.AppendLine();
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class ChatMessage : GameMessage
    {
        public int Field0;
        public int Field1;
        public string Field2;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(2);
            Field1 = buffer.ReadInt(4) + (-1);
            Field2 = buffer.ReadCharArray(512);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(2, Field0);
            buffer.WriteInt(4, Field1 - (-1));
            buffer.WriteCharArray(512, Field2);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("ChatMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad); b.AppendLine("Field2: \"" + Field2 + "\"");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class VictimMessage : GameMessage
    {
        public int Field0;
        public int Field1;
        public int Field2;
        public int Field3;
        public int /* sno */ snoKillerMonster;
        public int /* sno */ snoKillerActor;
        public int Field6;
        // MaxLength = 2
        public int /* gbid */[] Field7;
        public int /* sno */ snoPowerDmgSource;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(3);
            Field1 = buffer.ReadInt(7);
            Field2 = buffer.ReadInt(4) + (-1);
            Field3 = buffer.ReadInt(4) + (-1);
            snoKillerMonster = buffer.ReadInt(32);
            snoKillerActor = buffer.ReadInt(32);
            Field6 = buffer.ReadInt(5) + (-1);
            Field7 = new int /* gbid */[2];
            for (int i = 0; i < Field7.Length; i++) Field7[i] = buffer.ReadInt(32);
            snoPowerDmgSource = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(3, Field0);
            buffer.WriteInt(7, Field1);
            buffer.WriteInt(4, Field2 - (-1));
            buffer.WriteInt(4, Field3 - (-1));
            buffer.WriteInt(32, snoKillerMonster);
            buffer.WriteInt(32, snoKillerActor);
            buffer.WriteInt(5, Field6 - (-1));
            for (int i = 0; i < Field7.Length; i++) buffer.WriteInt(32, Field7[i]);
            buffer.WriteInt(32, snoPowerDmgSource);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("VictimMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', pad); b.AppendLine("Field3: 0x" + Field3.ToString("X8") + " (" + Field3 + ")");
            b.Append(' ', pad); b.AppendLine("snoKillerMonster: 0x" + snoKillerMonster.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("snoKillerActor: 0x" + snoKillerActor.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field6: 0x" + Field6.ToString("X8") + " (" + Field6 + ")");
            b.Append(' ', pad); b.AppendLine("Field7:");
            b.Append(' ', pad); b.AppendLine("{");
            for (int i = 0; i < Field7.Length; ) { b.Append(' ', pad + 1); for (int j = 0; j < 8 && i < Field7.Length; j++, i++) { b.Append("0x" + Field7[i].ToString("X8") + ", "); } b.AppendLine(); }
            b.Append(' ', pad); b.AppendLine("}"); b.AppendLine();
            b.Append(' ', pad); b.AppendLine("snoPowerDmgSource: 0x" + snoPowerDmgSource.ToString("X8"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class KillCountMessage : GameMessage
    {
        public int Field0;
        public int Field1;
        public int Field2;
        public int Field3;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(3);
            Field1 = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(32);
            Field3 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(3, Field0);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(32, Field2);
            buffer.WriteInt(32, Field3);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("KillCountMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', pad); b.AppendLine("Field3: 0x" + Field3.ToString("X8") + " (" + Field3 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class PlayAnimationMessage : GameMessage
    {
        public int Field0;
        public int Field1;
        public float Field2;
        // MaxLength = 3
        public PlayAnimationMessageSpec[] tAnim;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(4);
            Field2 = buffer.ReadFloat32();
            tAnim = new PlayAnimationMessageSpec[buffer.ReadInt(2)];
            for (int i = 0; i < tAnim.Length; i++) { tAnim[i] = new PlayAnimationMessageSpec(); tAnim[i].Parse(buffer); }
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(4, Field1);
            buffer.WriteFloat32(Field2);
            buffer.WriteInt(2, tAnim.Length);
            for (int i = 0; i < tAnim.Length; i++) { tAnim[i].Encode(buffer); }
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("PlayAnimationMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad); b.AppendLine("Field2: " + Field2.ToString("G"));
            b.Append(' ', pad); b.AppendLine("tAnim:");
            b.Append(' ', pad); b.AppendLine("{");
            for (int i = 0; i < tAnim.Length; i++) { tAnim[i].AsText(b, pad + 1); b.AppendLine(); }
            b.Append(' ', pad); b.AppendLine("}"); b.AppendLine();
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class ACDTranslateNormalMessage : GameMessage
    {
        public int Field0;
        public Vector3D Field1;
        public float /* angle */? Field2;
        public bool? Field3;
        public float? Field4;
        public int? Field5;
        public int? Field6;
        public int? Field7;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            if (buffer.ReadBool())
            {
                Field1 = new Vector3D();
                Field1.Parse(buffer);
            }
            if (buffer.ReadBool())
            {
                Field2 = buffer.ReadFloat32();
            }
            if (buffer.ReadBool())
            {
                Field3 = buffer.ReadBool();
            }
            if (buffer.ReadBool())
            {
                Field4 = buffer.ReadFloat32();
            }
            if (buffer.ReadBool())
            {
                Field5 = buffer.ReadInt(24);
            }
            if (buffer.ReadBool())
            {
                Field6 = buffer.ReadInt(21) + (-1);
            }
            if (buffer.ReadBool())
            {
                Field7 = buffer.ReadInt(32);
            }
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteBool(Field1 != null);
            if (Field1 != null)
            {
                Field1.Encode(buffer);
            }
            buffer.WriteBool(Field2.HasValue);
            if (Field2.HasValue)
            {
                buffer.WriteFloat32(Field2.Value);
            }
            buffer.WriteBool(Field3.HasValue);
            if (Field3.HasValue)
            {
                buffer.WriteBool(Field3.Value);
            }
            buffer.WriteBool(Field4.HasValue);
            if (Field4.HasValue)
            {
                buffer.WriteFloat32(Field4.Value);
            }
            buffer.WriteBool(Field5.HasValue);
            if (Field5.HasValue)
            {
                buffer.WriteInt(24, Field5.Value);
            }
            buffer.WriteBool(Field6.HasValue);
            if (Field6.HasValue)
            {
                buffer.WriteInt(21, Field6.Value - (-1));
            }
            buffer.WriteBool(Field7.HasValue);
            if (Field7.HasValue)
            {
                buffer.WriteInt(32, Field7.Value);
            }
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("ACDTranslateNormalMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8"));
            if (Field1 != null)
            {
                Field1.AsText(b, pad);
            }
            if (Field2.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("Field2.Value: " + Field2.Value.ToString("G"));
            }
            if (Field3.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("Field3.Value: " + (Field3.Value ? "true" : "false"));
            }
            if (Field4.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("Field4.Value: " + Field4.Value.ToString("G"));
            }
            if (Field5.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("Field5.Value: 0x" + Field5.Value.ToString("X8") + " (" + Field5.Value + ")");
            }
            if (Field6.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("Field6.Value: 0x" + Field6.Value.ToString("X8") + " (" + Field6.Value + ")");
            }
            if (Field7.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("Field7.Value: 0x" + Field7.Value.ToString("X8") + " (" + Field7.Value + ")");
            }
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class ACDTranslateSnappedMessage : GameMessage
    {
        public int Field0;
        public Vector3D Field1;
        public float /* angle */ Field2;
        public bool Field3;
        public int Field4;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = new Vector3D();
            Field1.Parse(buffer);
            Field2 = buffer.ReadFloat32();
            Field3 = buffer.ReadBool();
            Field4 = buffer.ReadInt(24);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            Field1.Encode(buffer);
            buffer.WriteFloat32(Field2);
            buffer.WriteBool(Field3);
            buffer.WriteInt(24, Field4);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("ACDTranslateSnappedMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8"));
            Field1.AsText(b, pad);
            b.Append(' ', pad); b.AppendLine("Field2: " + Field2.ToString("G"));
            b.Append(' ', pad); b.AppendLine("Field3: " + (Field3 ? "true" : "false"));
            b.Append(' ', pad); b.AppendLine("Field4: 0x" + Field4.ToString("X8") + " (" + Field4 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class ACDTranslateFacingMessage : GameMessage
    {
        public int Field0;
        public float /* angle */ Field1;
        public bool Field2;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadFloat32();
            Field2 = buffer.ReadBool();
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteFloat32(Field1);
            buffer.WriteBool(Field2);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("ACDTranslateFacingMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field1: " + Field1.ToString("G"));
            b.Append(' ', pad); b.AppendLine("Field2: " + (Field2 ? "true" : "false"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class ACDTranslateFixedMessage : GameMessage
    {
        public int Field0;
        public Vector3D Field1;
        public int Field2;
        public int Field3;
        public int /* sno */ Field4;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = new Vector3D();
            Field1.Parse(buffer);
            Field2 = buffer.ReadInt(24);
            Field3 = buffer.ReadInt(21) + (-1);
            Field4 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            Field1.Encode(buffer);
            buffer.WriteInt(24, Field2);
            buffer.WriteInt(21, Field3 - (-1));
            buffer.WriteInt(32, Field4);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("ACDTranslateFixedMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8"));
            Field1.AsText(b, pad);
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', pad); b.AppendLine("Field3: 0x" + Field3.ToString("X8") + " (" + Field3 + ")");
            b.Append(' ', pad); b.AppendLine("Field4: 0x" + Field4.ToString("X8"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class ACDTranslateArcMessage : GameMessage
    {
        public int Field0;
        public Vector3D Field1;
        public Vector3D Field2;
        public int Field3;
        public int Field4;
        public int Field5;
        public float Field6;
        public int /* sno */ Field7;
        public float Field8;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = new Vector3D();
            Field1.Parse(buffer);
            Field2 = new Vector3D();
            Field2.Parse(buffer);
            Field3 = buffer.ReadInt(24);
            Field4 = buffer.ReadInt(21) + (-1);
            Field5 = buffer.ReadInt(21) + (-1);
            Field6 = buffer.ReadFloat32();
            Field7 = buffer.ReadInt(32);
            Field8 = buffer.ReadFloat32();
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            Field1.Encode(buffer);
            Field2.Encode(buffer);
            buffer.WriteInt(24, Field3);
            buffer.WriteInt(21, Field4 - (-1));
            buffer.WriteInt(21, Field5 - (-1));
            buffer.WriteFloat32(Field6);
            buffer.WriteInt(32, Field7);
            buffer.WriteFloat32(Field8);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("ACDTranslateArcMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8"));
            Field1.AsText(b, pad);
            Field2.AsText(b, pad);
            b.Append(' ', pad); b.AppendLine("Field3: 0x" + Field3.ToString("X8") + " (" + Field3 + ")");
            b.Append(' ', pad); b.AppendLine("Field4: 0x" + Field4.ToString("X8") + " (" + Field4 + ")");
            b.Append(' ', pad); b.AppendLine("Field5: 0x" + Field5.ToString("X8") + " (" + Field5 + ")");
            b.Append(' ', pad); b.AppendLine("Field6: " + Field6.ToString("G"));
            b.Append(' ', pad); b.AppendLine("Field7: 0x" + Field7.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field8: " + Field8.ToString("G"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class ACDTranslateDetPathMessage : GameMessage
    {
        public int Field0;
        public int Field1;
        public int Field2;
        public int Field3;
        public Vector3D Field4;
        public float /* angle */ Field5;
        public Vector3D Field6;
        public int Field7;
        public int Field8;
        public int Field9;
        public int /* sno */ Field10;
        public int Field11;
        public float Field12;
        public float Field13;
        public float Field14;
        public float Field15;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(4);
            Field2 = buffer.ReadInt(32);
            Field3 = buffer.ReadInt(32);
            Field4 = new Vector3D();
            Field4.Parse(buffer);
            Field5 = buffer.ReadFloat32();
            Field6 = new Vector3D();
            Field6.Parse(buffer);
            Field7 = buffer.ReadInt(32);
            Field8 = buffer.ReadInt(32);
            Field9 = buffer.ReadInt(32);
            Field10 = buffer.ReadInt(32);
            Field11 = buffer.ReadInt(32);
            Field12 = buffer.ReadFloat32();
            Field13 = buffer.ReadFloat32();
            Field14 = buffer.ReadFloat32();
            Field15 = buffer.ReadFloat32();
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(4, Field1);
            buffer.WriteInt(32, Field2);
            buffer.WriteInt(32, Field3);
            Field4.Encode(buffer);
            buffer.WriteFloat32(Field5);
            Field6.Encode(buffer);
            buffer.WriteInt(32, Field7);
            buffer.WriteInt(32, Field8);
            buffer.WriteInt(32, Field9);
            buffer.WriteInt(32, Field10);
            buffer.WriteInt(32, Field11);
            buffer.WriteFloat32(Field12);
            buffer.WriteFloat32(Field13);
            buffer.WriteFloat32(Field14);
            buffer.WriteFloat32(Field15);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("ACDTranslateDetPathMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', pad); b.AppendLine("Field3: 0x" + Field3.ToString("X8") + " (" + Field3 + ")");
            Field4.AsText(b, pad);
            b.Append(' ', pad); b.AppendLine("Field5: " + Field5.ToString("G"));
            Field6.AsText(b, pad);
            b.Append(' ', pad); b.AppendLine("Field7: 0x" + Field7.ToString("X8") + " (" + Field7 + ")");
            b.Append(' ', pad); b.AppendLine("Field8: 0x" + Field8.ToString("X8") + " (" + Field8 + ")");
            b.Append(' ', pad); b.AppendLine("Field9: 0x" + Field9.ToString("X8") + " (" + Field9 + ")");
            b.Append(' ', pad); b.AppendLine("Field10: 0x" + Field10.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field11: 0x" + Field11.ToString("X8") + " (" + Field11 + ")");
            b.Append(' ', pad); b.AppendLine("Field12: " + Field12.ToString("G"));
            b.Append(' ', pad); b.AppendLine("Field13: " + Field13.ToString("G"));
            b.Append(' ', pad); b.AppendLine("Field14: " + Field14.ToString("G"));
            b.Append(' ', pad); b.AppendLine("Field15: " + Field15.ToString("G"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class ACDTranslateDetPathSinMessage : GameMessage
    {
        public int Field0;
        public int Field1;
        public int Field2;
        public int Field3;
        public Vector3D Field4;
        public float /* angle */ Field5;
        public Vector3D Field6;
        public int Field7;
        public int Field8;
        public int Field9;
        public int /* sno */ Field10;
        public int Field11;
        public float Field12;
        public float Field13;
        public DPathSinData Field14;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(4);
            Field2 = buffer.ReadInt(32);
            Field3 = buffer.ReadInt(32);
            Field4 = new Vector3D();
            Field4.Parse(buffer);
            Field5 = buffer.ReadFloat32();
            Field6 = new Vector3D();
            Field6.Parse(buffer);
            Field7 = buffer.ReadInt(32);
            Field8 = buffer.ReadInt(32);
            Field9 = buffer.ReadInt(32);
            Field10 = buffer.ReadInt(32);
            Field11 = buffer.ReadInt(32);
            Field12 = buffer.ReadFloat32();
            Field13 = buffer.ReadFloat32();
            Field14 = new DPathSinData();
            Field14.Parse(buffer);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(4, Field1);
            buffer.WriteInt(32, Field2);
            buffer.WriteInt(32, Field3);
            Field4.Encode(buffer);
            buffer.WriteFloat32(Field5);
            Field6.Encode(buffer);
            buffer.WriteInt(32, Field7);
            buffer.WriteInt(32, Field8);
            buffer.WriteInt(32, Field9);
            buffer.WriteInt(32, Field10);
            buffer.WriteInt(32, Field11);
            buffer.WriteFloat32(Field12);
            buffer.WriteFloat32(Field13);
            Field14.Encode(buffer);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("ACDTranslateDetPathSinMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', pad); b.AppendLine("Field3: 0x" + Field3.ToString("X8") + " (" + Field3 + ")");
            Field4.AsText(b, pad);
            b.Append(' ', pad); b.AppendLine("Field5: " + Field5.ToString("G"));
            Field6.AsText(b, pad);
            b.Append(' ', pad); b.AppendLine("Field7: 0x" + Field7.ToString("X8") + " (" + Field7 + ")");
            b.Append(' ', pad); b.AppendLine("Field8: 0x" + Field8.ToString("X8") + " (" + Field8 + ")");
            b.Append(' ', pad); b.AppendLine("Field9: 0x" + Field9.ToString("X8") + " (" + Field9 + ")");
            b.Append(' ', pad); b.AppendLine("Field10: 0x" + Field10.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field11: 0x" + Field11.ToString("X8") + " (" + Field11 + ")");
            b.Append(' ', pad); b.AppendLine("Field12: " + Field12.ToString("G"));
            b.Append(' ', pad); b.AppendLine("Field13: " + Field13.ToString("G"));
            Field14.AsText(b, pad);
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class ACDTranslateDetPathSpiralMessage : GameMessage
    {
        public int Field0;
        public Vector3D Field1;
        public Vector3D Field2;
        public int Field3;
        public int Field4;
        public int Field5;
        public DPathSinData Field6;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = new Vector3D();
            Field1.Parse(buffer);
            Field2 = new Vector3D();
            Field2.Parse(buffer);
            Field3 = buffer.ReadInt(32);
            Field4 = buffer.ReadInt(32);
            Field5 = buffer.ReadInt(32);
            Field6 = new DPathSinData();
            Field6.Parse(buffer);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            Field1.Encode(buffer);
            Field2.Encode(buffer);
            buffer.WriteInt(32, Field3);
            buffer.WriteInt(32, Field4);
            buffer.WriteInt(32, Field5);
            Field6.Encode(buffer);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("ACDTranslateDetPathSpiralMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8"));
            Field1.AsText(b, pad);
            Field2.AsText(b, pad);
            b.Append(' ', pad); b.AppendLine("Field3: 0x" + Field3.ToString("X8") + " (" + Field3 + ")");
            b.Append(' ', pad); b.AppendLine("Field4: 0x" + Field4.ToString("X8") + " (" + Field4 + ")");
            b.Append(' ', pad); b.AppendLine("Field5: 0x" + Field5.ToString("X8") + " (" + Field5 + ")");
            Field6.AsText(b, pad);
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class ACDTranslateSyncMessage : GameMessage
    {
        public int Field0;
        public Vector3D Field1;
        public bool? Field2;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = new Vector3D();
            Field1.Parse(buffer);
            if (buffer.ReadBool())
            {
                Field2 = buffer.ReadBool();
            }
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            Field1.Encode(buffer);
            buffer.WriteBool(Field2.HasValue);
            if (Field2.HasValue)
            {
                buffer.WriteBool(Field2.Value);
            }
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("ACDTranslateSyncMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8"));
            Field1.AsText(b, pad);
            if (Field2.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("Field2.Value: " + (Field2.Value ? "true" : "false"));
            }
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class ACDTranslateFixedUpdateMessage : GameMessage
    {
        public int Field0;
        public Vector3D Field1;
        public Vector3D Field2;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = new Vector3D();
            Field1.Parse(buffer);
            Field2 = new Vector3D();
            Field2.Parse(buffer);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            Field1.Encode(buffer);
            Field2.Encode(buffer);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("ACDTranslateFixedUpdateMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8"));
            Field1.AsText(b, pad);
            Field2.AsText(b, pad);
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class ACDCollFlagsMessage : GameMessage
    {
        public int Field0;
        public int Field1;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(12);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(12, Field1);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("ACDCollFlagsMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class GoldModifiedMessage : GameMessage
    {
        public bool Field0;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadBool();
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteBool(Field0);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("GoldModifiedMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: " + (Field0 ? "true" : "false"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class PlayEffectMessage : GameMessage
    {
        public int Field0;
        public int Field1;
        public int? Field2;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(7) + (-1);
            if (buffer.ReadBool())
            {
                Field2 = buffer.ReadInt(32);
            }
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(7, Field1 - (-1));
            buffer.WriteBool(Field2.HasValue);
            if (Field2.HasValue)
            {
                buffer.WriteInt(32, Field2.Value);
            }
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("PlayEffectMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            if (Field2.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("Field2.Value: 0x" + Field2.Value.ToString("X8") + " (" + Field2.Value + ")");
            }
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class PlayHitEffectMessage : GameMessage
    {
        public int Field0;
        public int Field1;
        public int Field2;
        public bool Field3;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(3) + (-1);
            Field3 = buffer.ReadBool();
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(3, Field2 - (-1));
            buffer.WriteBool(Field3);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("PlayHitEffectMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', pad); b.AppendLine("Field3: " + (Field3 ? "true" : "false"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class PlayHitEffectOverrideMessage : GameMessage
    {
        public int Field0;
        public int Field1;
        public int /* sno */ Field2;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(32, Field2);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("PlayHitEffectOverrideMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class PlayNonPositionalSoundMessage : GameMessage
    {
        public int /* sno */ Field0;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("PlayNonPositionalSoundMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class PlayErrorSoundMessage : GameMessage
    {
        public int Field0;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("PlayErrorSoundMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class PlayMusicMessage : GameMessage
    {
        public int /* sno */ snoMusic;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            snoMusic = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, snoMusic);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("PlayMusicMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("snoMusic: 0x" + snoMusic.ToString("X8"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class PlayCutsceneMessage : GameMessage
    {
        public int Field0;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("PlayCutsceneMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class FlippyMessage : GameMessage
    {
        public int Field0;
        public int /* sno */ Field1;
        public int /* sno */ Field2;
        public Vector3D Field3;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(32);
            Field3 = new Vector3D();
            Field3.Parse(buffer);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(32, Field2);
            Field3.Encode(buffer);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("FlippyMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8"));
            Field3.AsText(b, pad);
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class NPCInteractOptionsMessage : GameMessage
    {
        public int Field0;
        // MaxLength = 20
        public NPCInteraction[] tNPCInteraction;
        public int Field2;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            tNPCInteraction = new NPCInteraction[buffer.ReadInt(5)];
            for (int i = 0; i < tNPCInteraction.Length; i++) { tNPCInteraction[i] = new NPCInteraction(); tNPCInteraction[i].Parse(buffer); }
            Field2 = buffer.ReadInt(2);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(5, tNPCInteraction.Length);
            for (int i = 0; i < tNPCInteraction.Length; i++) { tNPCInteraction[i].Encode(buffer); }
            buffer.WriteInt(2, Field2);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("NPCInteractOptionsMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("tNPCInteraction:");
            b.Append(' ', pad); b.AppendLine("{");
            for (int i = 0; i < tNPCInteraction.Length; i++) { tNPCInteraction[i].AsText(b, pad + 1); b.AppendLine(); }
            b.Append(' ', pad); b.AppendLine("}"); b.AppendLine();
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class PetMessage : GameMessage
    {
        public int Field0;
        public int Field1;
        public int Field2;
        public int Field3;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(3);
            Field1 = buffer.ReadInt(5);
            Field2 = buffer.ReadInt(32);
            Field3 = buffer.ReadInt(5) + (-1);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(3, Field0);
            buffer.WriteInt(5, Field1);
            buffer.WriteInt(32, Field2);
            buffer.WriteInt(5, Field3 - (-1));
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("PetMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', pad); b.AppendLine("Field3: 0x" + Field3.ToString("X8") + " (" + Field3 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class HirelingInfoUpdateMessage : GameMessage
    {
        public int Field0;
        public bool Field1;
        public int Field2;
        public int Field3;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(2);
            Field1 = buffer.ReadBool();
            Field2 = buffer.ReadInt(32);
            Field3 = buffer.ReadInt(7);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(2, Field0);
            buffer.WriteBool(Field1);
            buffer.WriteInt(32, Field2);
            buffer.WriteInt(7, Field3);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("HirelingInfoUpdateMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: " + (Field1 ? "true" : "false"));
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', pad); b.AppendLine("Field3: 0x" + Field3.ToString("X8") + " (" + Field3 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class QuestUpdateMessage : GameMessage
    {
        public int /* sno */ snoQuest;
        public int /* sno */ snoLevelArea;
        public int Field2;
        public bool Field3;
        public bool Field4;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            snoQuest = buffer.ReadInt(32);
            snoLevelArea = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(32);
            Field3 = buffer.ReadBool();
            Field4 = buffer.ReadBool();
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, snoQuest);
            buffer.WriteInt(32, snoLevelArea);
            buffer.WriteInt(32, Field2);
            buffer.WriteBool(Field3);
            buffer.WriteBool(Field4);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("QuestUpdateMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("snoQuest: 0x" + snoQuest.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("snoLevelArea: 0x" + snoLevelArea.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', pad); b.AppendLine("Field3: " + (Field3 ? "true" : "false"));
            b.Append(' ', pad); b.AppendLine("Field4: " + (Field4 ? "true" : "false"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class QuestMeterMessage : GameMessage
    {
        public int /* sno */ snoQuest;
        public int Field1;
        public float Field2;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            snoQuest = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
            Field2 = buffer.ReadFloat32();
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, snoQuest);
            buffer.WriteInt(32, Field1);
            buffer.WriteFloat32(Field2);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("QuestMeterMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("snoQuest: 0x" + snoQuest.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad); b.AppendLine("Field2: " + Field2.ToString("G"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class QuestCounterMessage : GameMessage
    {
        public int /* sno */ snoQuest;
        public int /* sno */ snoLevelArea;
        public int Field2;
        public int Field3;
        public int Field4;
        public int Field5;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            snoQuest = buffer.ReadInt(32);
            snoLevelArea = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(32);
            Field3 = buffer.ReadInt(32);
            Field4 = buffer.ReadInt(32);
            Field5 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, snoQuest);
            buffer.WriteInt(32, snoLevelArea);
            buffer.WriteInt(32, Field2);
            buffer.WriteInt(32, Field3);
            buffer.WriteInt(32, Field4);
            buffer.WriteInt(32, Field5);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("QuestCounterMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("snoQuest: 0x" + snoQuest.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("snoLevelArea: 0x" + snoLevelArea.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', pad); b.AppendLine("Field3: 0x" + Field3.ToString("X8") + " (" + Field3 + ")");
            b.Append(' ', pad); b.AppendLine("Field4: 0x" + Field4.ToString("X8") + " (" + Field4 + ")");
            b.Append(' ', pad); b.AppendLine("Field5: 0x" + Field5.ToString("X8") + " (" + Field5 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class PlayerLevel : GameMessage
    {
        public int Field0;
        public int Field1;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(3);
            Field1 = buffer.ReadInt(7);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(3, Field0);
            buffer.WriteInt(7, Field1);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("PlayerLevel:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class WaypointActivatedMessage : GameMessage
    {
        public int Field0;
        public int Field1;
        public int /* sno */ Field2;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(32, Field2);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("WaypointActivatedMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class AimTargetMessage : GameMessage
    {
        public int Field0;
        public int Field1;
        public int Field2;
        public WorldPlace Field3;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(2) + (-1);
            Field2 = buffer.ReadInt(32);
            Field3 = new WorldPlace();
            Field3.Parse(buffer);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(2, Field1 - (-1));
            buffer.WriteInt(32, Field2);
            Field3.Encode(buffer);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("AimTargetMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            Field3.AsText(b, pad);
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class SetIdleAnimationMessage : GameMessage
    {
        public int Field0;
        public int Field1;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(32, Field1);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("SetIdleAnimationMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class ACDPickupFailedMessage : GameMessage
    {
        public int Field0;
        public int Field1;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(3);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(3, Field1);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("ACDPickupFailedMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class TrickleMessage : GameMessage
    {
        public int Field0;
        public int /* sno */ Field1;
        public WorldPlace Field2;
        public int? Field3;
        public int /* sno */ Field4;
        public float? Field5;
        public int Field6;
        public int Field7;
        public int? Field8;
        public int? Field9;
        public int? Field10;
        public int? Field11;
        public int? Field12;
        public float? Field13;
        public float? Field14;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
            Field2 = new WorldPlace();
            Field2.Parse(buffer);
            if (buffer.ReadBool())
            {
                Field3 = buffer.ReadInt(4) + (-1);
            }
            Field4 = buffer.ReadInt(32);
            if (buffer.ReadBool())
            {
                Field5 = buffer.ReadFloat32();
            }
            Field6 = buffer.ReadInt(4);
            Field7 = buffer.ReadInt(6);
            if (buffer.ReadBool())
            {
                Field8 = buffer.ReadInt(32);
            }
            if (buffer.ReadBool())
            {
                Field9 = buffer.ReadInt(32);
            }
            if (buffer.ReadBool())
            {
                Field10 = buffer.ReadInt(32);
            }
            if (buffer.ReadBool())
            {
                Field11 = buffer.ReadInt(32);
            }
            if (buffer.ReadBool())
            {
                Field12 = buffer.ReadInt(32);
            }
            if (buffer.ReadBool())
            {
                Field13 = buffer.ReadFloat32();
            }
            if (buffer.ReadBool())
            {
                Field14 = buffer.ReadFloat32();
            }
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(32, Field1);
            Field2.Encode(buffer);
            buffer.WriteBool(Field3.HasValue);
            if (Field3.HasValue)
            {
                buffer.WriteInt(4, Field3.Value - (-1));
            }
            buffer.WriteInt(32, Field4);
            buffer.WriteBool(Field5.HasValue);
            if (Field5.HasValue)
            {
                buffer.WriteFloat32(Field5.Value);
            }
            buffer.WriteInt(4, Field6);
            buffer.WriteInt(6, Field7);
            buffer.WriteBool(Field8.HasValue);
            if (Field8.HasValue)
            {
                buffer.WriteInt(32, Field8.Value);
            }
            buffer.WriteBool(Field9.HasValue);
            if (Field9.HasValue)
            {
                buffer.WriteInt(32, Field9.Value);
            }
            buffer.WriteBool(Field10.HasValue);
            if (Field10.HasValue)
            {
                buffer.WriteInt(32, Field10.Value);
            }
            buffer.WriteBool(Field11.HasValue);
            if (Field11.HasValue)
            {
                buffer.WriteInt(32, Field11.Value);
            }
            buffer.WriteBool(Field12.HasValue);
            if (Field12.HasValue)
            {
                buffer.WriteInt(32, Field12.Value);
            }
            buffer.WriteBool(Field13.HasValue);
            if (Field13.HasValue)
            {
                buffer.WriteFloat32(Field13.Value);
            }
            buffer.WriteBool(Field14.HasValue);
            if (Field14.HasValue)
            {
                buffer.WriteFloat32(Field14.Value);
            }
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("TrickleMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8"));
            Field2.AsText(b, pad);
            if (Field3.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("Field3.Value: 0x" + Field3.Value.ToString("X8") + " (" + Field3.Value + ")");
            }
            b.Append(' ', pad); b.AppendLine("Field4: 0x" + Field4.ToString("X8"));
            if (Field5.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("Field5.Value: " + Field5.Value.ToString("G"));
            }
            b.Append(' ', pad); b.AppendLine("Field6: 0x" + Field6.ToString("X8") + " (" + Field6 + ")");
            b.Append(' ', pad); b.AppendLine("Field7: 0x" + Field7.ToString("X8") + " (" + Field7 + ")");
            if (Field8.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("Field8.Value: 0x" + Field8.Value.ToString("X8") + " (" + Field8.Value + ")");
            }
            if (Field9.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("Field9.Value: 0x" + Field9.Value.ToString("X8") + " (" + Field9.Value + ")");
            }
            if (Field10.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("Field10.Value: 0x" + Field10.Value.ToString("X8") + " (" + Field10.Value + ")");
            }
            if (Field11.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("Field11.Value: 0x" + Field11.Value.ToString("X8") + " (" + Field11.Value + ")");
            }
            if (Field12.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("Field12.Value: 0x" + Field12.Value.ToString("X8") + " (" + Field12.Value + ")");
            }
            if (Field13.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("Field13.Value: " + Field13.Value.ToString("G"));
            }
            if (Field14.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("Field14.Value: " + Field14.Value.ToString("G"));
            }
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class MapRevealSceneMessage : GameMessage
    {
        public int Field0;
        public int /* sno */ snoScene;
        public PRTransform Field2;
        public int Field3;
        public int Field4;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            snoScene = buffer.ReadInt(32);
            Field2 = new PRTransform();
            Field2.Parse(buffer);
            Field3 = buffer.ReadInt(32);
            Field4 = buffer.ReadInt(3);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(32, snoScene);
            Field2.Encode(buffer);
            buffer.WriteInt(32, Field3);
            buffer.WriteInt(3, Field4);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("MapRevealSceneMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("snoScene: 0x" + snoScene.ToString("X8"));
            Field2.AsText(b, pad);
            b.Append(' ', pad); b.AppendLine("Field3: 0x" + Field3.ToString("X8") + " (" + Field3 + ")");
            b.Append(' ', pad); b.AppendLine("Field4: 0x" + Field4.ToString("X8") + " (" + Field4 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class SavePointInfoMessage : GameMessage
    {
        public int /* sno */ snoLevelArea;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            snoLevelArea = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, snoLevelArea);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("SavePointInfoMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("snoLevelArea: 0x" + snoLevelArea.ToString("X8"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class HearthPortalInfoMessage : GameMessage
    {
        public int /* sno */ snoLevelArea;
        public int Field1;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            snoLevelArea = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, snoLevelArea);
            buffer.WriteInt(32, Field1);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("HearthPortalInfoMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("snoLevelArea: 0x" + snoLevelArea.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class ReturnPointInfoMessage : GameMessage
    {
        public int /* sno */ snoLevelArea;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            snoLevelArea = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, snoLevelArea);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("ReturnPointInfoMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("snoLevelArea: 0x" + snoLevelArea.ToString("X8"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class DataIDDataMessage : GameMessage
    {
        public int Field0;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("DataIDDataMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class PlayerInteractMessage : GameMessage
    {
        public int Field0;
        public int Field1;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(3);
            Field1 = buffer.ReadInt(1) + (-1);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(3, Field0);
            buffer.WriteInt(1, Field1 - (-1));
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("PlayerInteractMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class TradeMessage : GameMessage
    {
        public int Field0;
        public int Field1;
        public int Field2;
        public long Field3;
        public int Field4;
        // MaxLength = 5
        public int[] Field5;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(4);
            Field1 = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(32);
            Field3 = buffer.ReadInt64(64);
            Field4 = buffer.ReadInt(32);
            Field5 = new int[5];
            for (int i = 0; i < Field5.Length; i++) Field5[i] = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(4, Field0);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(32, Field2);
            buffer.WriteInt64(64, Field3);
            buffer.WriteInt(32, Field4);
            for (int i = 0; i < Field5.Length; i++) buffer.WriteInt(32, Field5[i]);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("TradeMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', pad); b.AppendLine("Field3: 0x" + Field3.ToString("X16"));
            b.Append(' ', pad); b.AppendLine("Field4: 0x" + Field4.ToString("X8") + " (" + Field4 + ")");
            b.Append(' ', pad); b.AppendLine("Field5:");
            b.Append(' ', pad); b.AppendLine("{");
            for (int i = 0; i < Field5.Length; ) { b.Append(' ', pad + 1); for (int j = 0; j < 8 && i < Field5.Length; j++, i++) { b.Append("0x" + Field5[i].ToString("X8") + ", "); } b.AppendLine(); }
            b.Append(' ', pad); b.AppendLine("}"); b.AppendLine();
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class ActTransitionMessage : GameMessage
    {
        public int Field0;
        public bool Field1;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(10) + (-1);
            Field1 = buffer.ReadBool();
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(10, Field0 - (-1));
            buffer.WriteBool(Field1);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("ActTransitionMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: " + (Field1 ? "true" : "false"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class InterstitialMessage : GameMessage
    {
        public int Field0;
        public bool Field1;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(3) + (-1);
            Field1 = buffer.ReadBool();
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(3, Field0 - (-1));
            buffer.WriteBool(Field1);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("InterstitialMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: " + (Field1 ? "true" : "false"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class RopeEffectMessageACDToACD : GameMessage
    {
        public int /* sno */ Field0;
        public int Field1;
        public int Field2;
        public int Field3;
        public int Field4;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(5);
            Field3 = buffer.ReadInt(32);
            Field4 = buffer.ReadInt(5);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(5, Field2);
            buffer.WriteInt(32, Field3);
            buffer.WriteInt(5, Field4);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("RopeEffectMessageACDToACD:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', pad); b.AppendLine("Field3: 0x" + Field3.ToString("X8") + " (" + Field3 + ")");
            b.Append(' ', pad); b.AppendLine("Field4: 0x" + Field4.ToString("X8") + " (" + Field4 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class RopeEffectMessageACDToPlace : GameMessage
    {
        public int /* sno */ Field0;
        public int Field1;
        public int Field2;
        public WorldPlace Field3;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(5);
            Field3 = new WorldPlace();
            Field3.Parse(buffer);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(5, Field2);
            Field3.Encode(buffer);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("RopeEffectMessageACDToPlace:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            Field3.AsText(b, pad);
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class UIElementMessage : GameMessage
    {
        public int Field0;
        public bool Field1;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(4) + (-1);
            Field1 = buffer.ReadBool();
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(4, Field0 - (-1));
            buffer.WriteBool(Field1);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("UIElementMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: " + (Field1 ? "true" : "false"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class ACDChangeActorMessage : GameMessage
    {
        public int Field0;
        public int /* sno */ Field1;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(32, Field1);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("ACDChangeActorMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class PlayerWarpedMessage : GameMessage
    {
        public int Field0;
        public float Field1;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(4);
            Field1 = buffer.ReadFloat32();
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(4, Field0);
            buffer.WriteFloat32(Field1);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("PlayerWarpedMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: " + Field1.ToString("G"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class GameSyncedDataMessage : GameMessage
    {
        public GameSyncedData Field0;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = new GameSyncedData();
            Field0.Parse(buffer);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            Field0.Encode(buffer);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("GameSyncedDataMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            Field0.AsText(b, pad);
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class EndOfTickMessage : GameMessage
    {
        public int Field0;
        public int Field1;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(32, Field1);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("EndOfTickMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class CreateBNetGameMessage : GameMessage
    {
        public string Field0;
        public int Field1;
        public int Field2;
        public int /* sno */ Field3;
        public int Field4;
        public bool Field5;
        public int /* sno */ Field6;
        public int Field7;
        public int Field8;
        public int Field9;
        public int Field10;
        public short Field11;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadCharArray(33);
            Field1 = buffer.ReadInt(3) + (-1);
            Field2 = buffer.ReadInt(32);
            Field3 = buffer.ReadInt(32);
            Field4 = buffer.ReadInt(32);
            Field5 = buffer.ReadBool();
            Field6 = buffer.ReadInt(32);
            Field7 = buffer.ReadInt(16);
            Field8 = buffer.ReadInt(3) + (1);
            Field9 = buffer.ReadInt(32);
            Field10 = buffer.ReadInt(32);
            Field11 = (short)buffer.ReadInt(16);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteCharArray(33, Field0);
            buffer.WriteInt(3, Field1 - (-1));
            buffer.WriteInt(32, Field2);
            buffer.WriteInt(32, Field3);
            buffer.WriteInt(32, Field4);
            buffer.WriteBool(Field5);
            buffer.WriteInt(32, Field6);
            buffer.WriteInt(16, Field7);
            buffer.WriteInt(3, Field8 - (1));
            buffer.WriteInt(32, Field9);
            buffer.WriteInt(32, Field10);
            buffer.WriteInt(16, Field11);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("CreateBNetGameMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: \"" + Field0 + "\"");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', pad); b.AppendLine("Field3: 0x" + Field3.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field4: 0x" + Field4.ToString("X8") + " (" + Field4 + ")");
            b.Append(' ', pad); b.AppendLine("Field5: " + (Field5 ? "true" : "false"));
            b.Append(' ', pad); b.AppendLine("Field6: 0x" + Field6.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field7: 0x" + Field7.ToString("X8") + " (" + Field7 + ")");
            b.Append(' ', pad); b.AppendLine("Field8: 0x" + Field8.ToString("X8") + " (" + Field8 + ")");
            b.Append(' ', pad); b.AppendLine("Field9: 0x" + Field9.ToString("X8") + " (" + Field9 + ")");
            b.Append(' ', pad); b.AppendLine("Field10: 0x" + Field10.ToString("X8") + " (" + Field10 + ")");
            b.Append(' ', pad); b.AppendLine("Field11: 0x" + Field11.ToString("X4"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class CreateBNetGameResultMessage : GameMessage
    {
        public int Field0;
        public GameId Field1;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(2);
            Field1 = new GameId();
            Field1.Parse(buffer);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(2, Field0);
            Field1.Encode(buffer);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("CreateBNetGameResultMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            Field1.AsText(b, pad);
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class RequestJoinBNetGameMessage : GameMessage
    {
        public GameId Field0;
        public EntityId Field1;
        public int Field2;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = new GameId();
            Field0.Parse(buffer);
            Field1 = new EntityId();
            Field1.Parse(buffer);
            Field2 = buffer.ReadInt(5) + (-1);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            Field0.Encode(buffer);
            Field1.Encode(buffer);
            buffer.WriteInt(5, Field2 - (-1));
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("RequestJoinBNetGameMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            Field0.AsText(b, pad);
            Field1.AsText(b, pad);
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class BNetJoinGameRequestResultMessage : GameMessage
    {
        public int Field0;
        public GameId Field1;
        public long Field2;
        public int Field3;
        public int /* sno */ Field4;
        public int Field5;
        public int Field6;
        public short Field7;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(3) + (-1);
            Field1 = new GameId();
            Field1.Parse(buffer);
            Field2 = buffer.ReadInt64(64);
            Field3 = buffer.ReadInt(3) + (-1);
            Field4 = buffer.ReadInt(32);
            Field5 = buffer.ReadInt(16);
            Field6 = buffer.ReadInt(32);
            Field7 = (short)buffer.ReadInt(16);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(3, Field0 - (-1));
            Field1.Encode(buffer);
            buffer.WriteInt64(64, Field2);
            buffer.WriteInt(3, Field3 - (-1));
            buffer.WriteInt(32, Field4);
            buffer.WriteInt(16, Field5);
            buffer.WriteInt(32, Field6);
            buffer.WriteInt(16, Field7);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("BNetJoinGameRequestResultMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            Field1.AsText(b, pad);
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X16"));
            b.Append(' ', pad); b.AppendLine("Field3: 0x" + Field3.ToString("X8") + " (" + Field3 + ")");
            b.Append(' ', pad); b.AppendLine("Field4: 0x" + Field4.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field5: 0x" + Field5.ToString("X8") + " (" + Field5 + ")");
            b.Append(' ', pad); b.AppendLine("Field6: 0x" + Field6.ToString("X8") + " (" + Field6 + ")");
            b.Append(' ', pad); b.AppendLine("Field7: 0x" + Field7.ToString("X4"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class JoinBNetGameMessage : GameMessage
    {
        public EntityId Field0;  // this *is* the toon id /raist.
        public GameId Field1;
        public int Field2; // and this is the SGameId there we set in D3Sharp.Core.Games.Game.cs when we send the connection info to client /raist.
        public long Field3;
        public int Field4;
        public int ProtocolHash;
        public int SNOPackHash;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = new EntityId();
            Field0.Parse(buffer);
            Field1 = new GameId();
            Field1.Parse(buffer);
            Field2 = buffer.ReadInt(32);
            Field3 = buffer.ReadInt64(64);
            Field4 = buffer.ReadInt(4) + (2);
            ProtocolHash = buffer.ReadInt(32);
            SNOPackHash = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            Field0.Encode(buffer);
            Field1.Encode(buffer);
            buffer.WriteInt(32, Field2);
            buffer.WriteInt64(64, Field3);
            buffer.WriteInt(4, Field4 - (2));
            buffer.WriteInt(32, ProtocolHash);
            buffer.WriteInt(32, SNOPackHash);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("JoinBNetGameMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            Field0.AsText(b, pad);
            Field1.AsText(b, pad);
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', pad); b.AppendLine("Field3: 0x" + Field3.ToString("X16"));
            b.Append(' ', pad); b.AppendLine("Field4: 0x" + Field4.ToString("X8") + " (" + Field4 + ")");
            b.Append(' ', pad); b.AppendLine("ProtocolHash: 0x" + ProtocolHash.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("SNOPackHash: 0x" + SNOPackHash.ToString("X8"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class JoinLANGameMessage : GameMessage
    {
        public int Field0;
        public string Field1;
        public string Field2;
        public int Field3;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadCharArray(128);
            Field2 = buffer.ReadCharArray(49);
            Field3 = buffer.ReadInt(4) + (2);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteCharArray(128, Field1);
            buffer.WriteCharArray(49, Field2);
            buffer.WriteInt(4, Field3 - (2));
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("JoinLANGameMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: \"" + Field1 + "\"");
            b.Append(' ', pad); b.AppendLine("Field2: \"" + Field2 + "\"");
            b.Append(' ', pad); b.AppendLine("Field3: 0x" + Field3.ToString("X8") + " (" + Field3 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class NetworkAddressMessage : GameMessage
    {
        public int Field0;
        public short Field1;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = (short)buffer.ReadInt(16);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(16, Field1);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("NetworkAddressMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X4"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class GameIdMessage : GameMessage
    {
        public GameId Field0;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = new GameId();
            Field0.Parse(buffer);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            Field0.Encode(buffer);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("GameIdMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            Field0.AsText(b, pad);
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class IntDataMessage : GameMessage
    {
        public int Field0;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("IntDataMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class EntityIdMessage : GameMessage
    {
        public EntityId Field0;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = new EntityId();
            Field0.Parse(buffer);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            Field0.Encode(buffer);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("EntityIdMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            Field0.AsText(b, pad);
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class CreateHeroMessage : GameMessage
    {
        public string Field0;
        public int /* gbid */ Field1;
        public int Field2;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadCharArray(49);
            Field1 = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(29);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteCharArray(49, Field0);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(29, Field2);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("CreateHeroMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: \"" + Field0 + "\"");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class CreateHeroResultMessage : GameMessage
    {
        public int Field0;
        public EntityId Field1;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(2) + (-1);
            Field1 = new EntityId();
            Field1.Parse(buffer);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(2, Field0 - (-1));
            Field1.Encode(buffer);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("CreateHeroResultMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            Field1.AsText(b, pad);
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class BlizzconCVarsMessage : GameMessage
    {
        public bool Field0;
        public bool Field1;
        public bool Field2;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadBool();
            Field1 = buffer.ReadBool();
            Field2 = buffer.ReadBool();
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteBool(Field0);
            buffer.WriteBool(Field1);
            buffer.WriteBool(Field2);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("BlizzconCVarsMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: " + (Field0 ? "true" : "false"));
            b.Append(' ', pad); b.AppendLine("Field1: " + (Field1 ? "true" : "false"));
            b.Append(' ', pad); b.AppendLine("Field2: " + (Field2 ? "true" : "false"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class LogoutContextMessage : GameMessage
    {
        public bool Field0;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadBool();
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteBool(Field0);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("LogoutContextMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: " + (Field0 ? "true" : "false"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class LogoutTickTimeMessage : GameMessage
    {
        public bool Field0;
        public int Field1;
        public int Field2;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadBool();
            Field1 = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteBool(Field0);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(32, Field2);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("LogoutTickTimeMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: " + (Field0 ? "true" : "false"));
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class TargetMessage : GameMessage
    {
        public int Field0;
        public int Field1;
        public WorldPlace Field2;
        public int /* sno */ snoPower;
        public int Field4;
        public int Field5;
        public AnimPreplayData Field6;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(2) + (-1);
            Field1 = buffer.ReadInt(32);
            Field2 = new WorldPlace();
            Field2.Parse(buffer);
            snoPower = buffer.ReadInt(32);
            Field4 = buffer.ReadInt(32);
            Field5 = buffer.ReadInt(2);
            if (buffer.ReadBool())
            {
                Field6 = new AnimPreplayData();
                Field6.Parse(buffer);
            }
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(2, Field0 - (-1));
            buffer.WriteInt(32, Field1);
            Field2.Encode(buffer);
            buffer.WriteInt(32, snoPower);
            buffer.WriteInt(32, Field4);
            buffer.WriteInt(2, Field5);
            buffer.WriteBool(Field6 != null);
            if (Field6 != null)
            {
                Field6.Encode(buffer);
            }
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("TargetMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            Field2.AsText(b, pad);
            b.Append(' ', pad); b.AppendLine("snoPower: 0x" + snoPower.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field4: 0x" + Field4.ToString("X8") + " (" + Field4 + ")");
            b.Append(' ', pad); b.AppendLine("Field5: 0x" + Field5.ToString("X8") + " (" + Field5 + ")");
            if (Field6 != null)
            {
                Field6.AsText(b, pad);
            }
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class SecondaryAnimationPowerMessage : GameMessage
    {
        public int /* sno */ snoPower;
        public AnimPreplayData Field1;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            snoPower = buffer.ReadInt(32);
            if (buffer.ReadBool())
            {
                Field1 = new AnimPreplayData();
                Field1.Parse(buffer);
            }
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, snoPower);
            buffer.WriteBool(Field1 != null);
            if (Field1 != null)
            {
                Field1.Encode(buffer);
            }
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("SecondaryAnimationPowerMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("snoPower: 0x" + snoPower.ToString("X8"));
            if (Field1 != null)
            {
                Field1.AsText(b, pad);
            }
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class SNODataMessage : GameMessage
    {
        public int /* sno */ Field0;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("SNODataMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class TryConsoleCommand : GameMessage
    {
        public string Field0;
        public int Field1;
        public WorldPlace Field2;
        public int Field3;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadCharArray(512);
            Field1 = buffer.ReadInt(4) + (-1);
            Field2 = new WorldPlace();
            Field2.Parse(buffer);
            Field3 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteCharArray(512, Field0);
            buffer.WriteInt(4, Field1 - (-1));
            Field2.Encode(buffer);
            buffer.WriteInt(32, Field3);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("TryConsoleCommand:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: \"" + Field0 + "\"");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            Field2.AsText(b, pad);
            b.Append(' ', pad); b.AppendLine("Field3: 0x" + Field3.ToString("X8") + " (" + Field3 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class TryChatMessage : GameMessage
    {
        public int Field0;
        public int Field1;
        public string Field2;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(3);
            Field1 = buffer.ReadInt(4) + (-1);
            Field2 = buffer.ReadCharArray(512);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(3, Field0);
            buffer.WriteInt(4, Field1 - (-1));
            buffer.WriteCharArray(512, Field2);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("TryChatMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad); b.AppendLine("Field2: \"" + Field2 + "\"");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class TryWaypointMessage : GameMessage
    {
        public int Field0;
        public int Field1;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(5) + (-1);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(5, Field1 - (-1));
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("TryWaypointMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class InventoryRequestMoveMessage : GameMessage
    {
        public int Field0;
        public InvLoc Field1;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = new InvLoc();
            Field1.Parse(buffer);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            Field1.Encode(buffer);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("InventoryRequestMoveMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            Field1.AsText(b, pad);
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class InventorySplitStackMessage : GameMessage
    {
        public int Field0;
        public long Field1;
        public InvLoc Field2;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt64(64);
            Field2 = new InvLoc();
            Field2.Parse(buffer);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt64(64, Field1);
            Field2.Encode(buffer);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("InventorySplitStackMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X16"));
            Field2.AsText(b, pad);
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class InventoryStackTransferMessage : GameMessage
    {
        public int Field0;
        public int Field1;
        public long Field2;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
            Field2 = buffer.ReadInt64(64);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt64(64, Field2);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("InventoryStackTransferMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X16"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class InventoryRequestSocketMessage : GameMessage
    {
        public int Field0;
        public int Field1;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(32, Field1);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("InventoryRequestSocketMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class InventoryRequestUseMessage : GameMessage
    {
        public int Field0;
        public int Field1;
        public int Field2;
        public WorldPlace Field3;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(2) + (-1);
            Field2 = buffer.ReadInt(32);
            Field3 = new WorldPlace();
            Field3.Parse(buffer);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(2, Field1 - (-1));
            buffer.WriteInt(32, Field2);
            Field3.Encode(buffer);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("InventoryRequestUseMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            Field3.AsText(b, pad);
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class SocketSpellMessage : GameMessage
    {
        public int Field0;
        public int Field1;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(32, Field1);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("SocketSpellMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class HelperDetachMessage : GameMessage
    {
        public int Field0;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("HelperDetachMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class AssignSkillMessage : GameMessage
    {
        public int /* sno */ snoPower;
        public int Field1;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            snoPower = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(5);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, snoPower);
            buffer.WriteInt(5, Field1);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("AssignSkillMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("snoPower: 0x" + snoPower.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class HirelingRequestLearnSkillMessage : GameMessage
    {
        public int Field0;
        public int /* sno */ Field1;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(32, Field1);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("HirelingRequestLearnSkillMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class PlayerChangeHotbarButtonMessage : GameMessage
    {
        public int Field0;
        public HotbarButtonData Field1;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(4) + (-1);
            Field1 = new HotbarButtonData();
            Field1.Parse(buffer);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(4, Field0 - (-1));
            Field1.Encode(buffer);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("PlayerChangeHotbarButtonMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            Field1.AsText(b, pad);
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class WorldStatusMessage : GameMessage
    {
        public int Field0;
        public bool Field1;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadBool();
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteBool(Field1);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("WorldStatusMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: " + (Field1 ? "true" : "false"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class WeatherOverrideMessage : GameMessage
    {
        public float Field0;
        public float Field1;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadFloat32();
            Field1 = buffer.ReadFloat32();
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteFloat32(Field0);
            buffer.WriteFloat32(Field1);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("WeatherOverrideMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: " + Field0.ToString("G"));
            b.Append(' ', pad); b.AppendLine("Field1: " + Field1.ToString("G"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class ComplexEffectAddMessage : GameMessage
    {
        public int Field0;
        public int Field1;
        public int /* sno */ Field2;
        public int Field3;
        public int Field4;
        public int Field5;
        public int Field6;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(32);
            Field3 = buffer.ReadInt(32);
            Field4 = buffer.ReadInt(32);
            Field5 = buffer.ReadInt(32);
            Field6 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(32, Field2);
            buffer.WriteInt(32, Field3);
            buffer.WriteInt(32, Field4);
            buffer.WriteInt(32, Field5);
            buffer.WriteInt(32, Field6);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("ComplexEffectAddMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field3: 0x" + Field3.ToString("X8") + " (" + Field3 + ")");
            b.Append(' ', pad); b.AppendLine("Field4: 0x" + Field4.ToString("X8") + " (" + Field4 + ")");
            b.Append(' ', pad); b.AppendLine("Field5: 0x" + Field5.ToString("X8") + " (" + Field5 + ")");
            b.Append(' ', pad); b.AppendLine("Field6: 0x" + Field6.ToString("X8") + " (" + Field6 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class EffectGroupACDToACDMessage : GameMessage
    {
        public int /* sno */ Field0;
        public int Field1;
        public int Field2;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(32, Field2);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("EffectGroupACDToACDMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class ACDShearMessage : GameMessage
    {
        public int Field0;
        public float Field1;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadFloat32();
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteFloat32(Field1);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("ACDShearMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: " + Field1.ToString("G"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class ACDGroupMessage : GameMessage
    {
        public int Field0;
        public int Field1;
        public int Field2;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(32, Field2);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("ACDGroupMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class PlayConvLineMessage : GameMessage
    {
        public int Field0;
        // MaxLength = 9
        public int[] Field1;
        public PlayLineParams Field2;
        public int Field3;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = new int[9];
            for (int i = 0; i < Field1.Length; i++) Field1[i] = buffer.ReadInt(32);
            Field2 = new PlayLineParams();
            Field2.Parse(buffer);
            Field3 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            for (int i = 0; i < Field1.Length; i++) buffer.WriteInt(32, Field1[i]);
            Field2.Encode(buffer);
            buffer.WriteInt(32, Field3);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("PlayConvLineMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1:");
            b.Append(' ', pad); b.AppendLine("{");
            for (int i = 0; i < Field1.Length; ) { b.Append(' ', pad + 1); for (int j = 0; j < 8 && i < Field1.Length; j++, i++) { b.Append("0x" + Field1[i].ToString("X8") + ", "); } b.AppendLine(); }
            b.Append(' ', pad); b.AppendLine("}"); b.AppendLine();
            Field2.AsText(b, pad);
            b.Append(' ', pad); b.AppendLine("Field3: 0x" + Field3.ToString("X8") + " (" + Field3 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class StopConvLineMessage : GameMessage
    {
        public int Field0;
        public bool Field1;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadBool();
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteBool(Field1);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("StopConvLineMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: " + (Field1 ? "true" : "false"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class EndConversationMessage : GameMessage
    {
        public int Field0;
        public int Field1;
        public int Field2;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(32, Field2);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("EndConversationMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class HirelingSwapMessage : GameMessage
    {
        public int Field0;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("HirelingSwapMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class DeathFadeTimeMessage : GameMessage
    {
        public int Field0;
        public int Field1;
        public int Field2;
        public bool Field3;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(11) + (-1);
            Field2 = buffer.ReadInt(11);
            Field3 = buffer.ReadBool();
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(11, Field1 - (-1));
            buffer.WriteInt(11, Field2);
            buffer.WriteBool(Field3);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("DeathFadeTimeMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', pad); b.AppendLine("Field3: " + (Field3 ? "true" : "false"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class DisplayGameTextMessage : GameMessage
    {
        public string Field0;
        public int? Field1;
        public int? Field2;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadCharArray(512);
            if (buffer.ReadBool())
            {
                Field1 = buffer.ReadInt(32);
            }
            if (buffer.ReadBool())
            {
                Field2 = buffer.ReadInt(32);
            }
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteCharArray(512, Field0);
            buffer.WriteBool(Field1.HasValue);
            if (Field1.HasValue)
            {
                buffer.WriteInt(32, Field1.Value);
            }
            buffer.WriteBool(Field2.HasValue);
            if (Field2.HasValue)
            {
                buffer.WriteInt(32, Field2.Value);
            }
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("DisplayGameTextMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: \"" + Field0 + "\"");
            if (Field1.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("Field1.Value: 0x" + Field1.Value.ToString("X8") + " (" + Field1.Value + ")");
            }
            if (Field2.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("Field2.Value: 0x" + Field2.Value.ToString("X8") + " (" + Field2.Value + ")");
            }
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class GBIDDataMessage : GameMessage
    {
        public int /* gbid */ Field0;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("GBIDDataMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class ACDLookAtMessage : GameMessage
    {
        public int Field0;
        public int Field1;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(32, Field1);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("ACDLookAtMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class KillCounterUpdateMessage : GameMessage
    {
        public int Field0;
        public int Field1;
        public int Field2;
        public bool Field3;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(2);
            Field1 = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(32);
            Field3 = buffer.ReadBool();
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(2, Field0);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(32, Field2);
            buffer.WriteBool(Field3);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("KillCounterUpdateMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', pad); b.AppendLine("Field3: " + (Field3 ? "true" : "false"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class LowHealthCombatMessage : GameMessage
    {
        public float Field0;
        public int Field1;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadFloat32();
            Field1 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteFloat32(Field0);
            buffer.WriteInt(32, Field1);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("LowHealthCombatMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: " + Field0.ToString("G"));
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class SaviorMessage : GameMessage
    {
        public int Field0;
        public int Field1;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(32, Field1);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("SaviorMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class FloatingNumberMessage : GameMessage
    {
        public int Field0;
        public float Field1;
        public int Field2;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadFloat32();
            Field2 = buffer.ReadInt(6);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteFloat32(Field1);
            buffer.WriteInt(6, Field2);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("FloatingNumberMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: " + Field1.ToString("G"));
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class FloatingAmountMessage : GameMessage
    {
        public WorldPlace Field0;
        public int Field1;
        public int? Field2;
        public int Field3;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = new WorldPlace();
            Field0.Parse(buffer);
            Field1 = buffer.ReadInt(32);
            if (buffer.ReadBool())
            {
                Field2 = buffer.ReadInt(32);
            }
            Field3 = buffer.ReadInt(6);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            Field0.Encode(buffer);
            buffer.WriteInt(32, Field1);
            buffer.WriteBool(Field2.HasValue);
            if (Field2.HasValue)
            {
                buffer.WriteInt(32, Field2.Value);
            }
            buffer.WriteInt(6, Field3);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("FloatingAmountMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            Field0.AsText(b, pad);
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            if (Field2.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("Field2.Value: 0x" + Field2.Value.ToString("X8") + " (" + Field2.Value + ")");
            }
            b.Append(' ', pad); b.AppendLine("Field3: 0x" + Field3.ToString("X8") + " (" + Field3 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class RemoveRagdollMessage : GameMessage
    {
        public int Field0;
        public int /* sno */ Field1;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(32, Field1);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("RemoveRagdollMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class SNONameDataMessage : GameMessage
    {
        public SNOName Field0;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = new SNOName();
            Field0.Parse(buffer);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            Field0.Encode(buffer);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("SNONameDataMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            Field0.AsText(b, pad);
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class LoreMessage : GameMessage
    {
        public int /* sno */ snoLore;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            snoLore = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, snoLore);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("LoreMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("snoLore: 0x" + snoLore.ToString("X8"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class WorldDeletedMessage : GameMessage
    {
        public int Field0;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("WorldDeletedMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class TimedEventStartedMessage : GameMessage
    {
        public ActiveEvent Field0;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = new ActiveEvent();
            Field0.Parse(buffer);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            Field0.Encode(buffer);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("TimedEventStartedMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            Field0.AsText(b, pad);
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class ActTransitionStartedMessage : GameMessage
    {
        public int Field0;
        public int Field1;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(32, Field1);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("ActTransitionStartedMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class PlayerQuestMessage : GameMessage
    {
        public int Field0;
        public int /* sno */ Field1;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(4) + (-1);
            Field1 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(4, Field0 - (-1));
            buffer.WriteInt(32, Field1);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("PlayerQuestMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class PlayerDeSyncSnapMessage : GameMessage
    {
        public WorldPlace Field0;
        public int /* sno */ Field1;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = new WorldPlace();
            Field0.Parse(buffer);
            Field1 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            Field0.Encode(buffer);
            buffer.WriteInt(32, Field1);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("PlayerDeSyncSnapMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            Field0.AsText(b, pad);
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class SalvageResultsMessage : GameMessage
    {
        public int /* gbid */ gbidOriginalItem;
        public int Field1;
        public int Field2;
        // MaxLength = 10
        public int /* gbid */[] gbidNewItems;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            gbidOriginalItem = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(4) + (-1);
            Field2 = buffer.ReadInt(32);
            gbidNewItems = new int /* gbid */[10];
            for (int i = 0; i < gbidNewItems.Length; i++) gbidNewItems[i] = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, gbidOriginalItem);
            buffer.WriteInt(4, Field1 - (-1));
            buffer.WriteInt(32, Field2);
            for (int i = 0; i < gbidNewItems.Length; i++) buffer.WriteInt(32, gbidNewItems[i]);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("SalvageResultsMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("gbidOriginalItem: 0x" + gbidOriginalItem.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', pad); b.AppendLine("gbidNewItems:");
            b.Append(' ', pad); b.AppendLine("{");
            for (int i = 0; i < gbidNewItems.Length; ) { b.Append(' ', pad + 1); for (int j = 0; j < 8 && i < gbidNewItems.Length; j++, i++) { b.Append("0x" + gbidNewItems[i].ToString("X8") + ", "); } b.AppendLine(); }
            b.Append(' ', pad); b.AppendLine("}"); b.AppendLine();
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class MapMarkerInfoMessage : GameMessage
    {
        public int Field0;
        public WorldPlace Field1;
        public int Field2;
        public int /* sno */ m_snoStringList;
        public int Field4;
        public float Field5;
        public float Field6;
        public float Field7;
        public int Field8;
        public bool Field9;
        public bool Field10;
        public bool Field11;
        public int Field12;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = new WorldPlace();
            Field1.Parse(buffer);
            Field2 = buffer.ReadInt(32);
            m_snoStringList = buffer.ReadInt(32);
            Field4 = buffer.ReadInt(32);
            Field5 = buffer.ReadFloat32();
            Field6 = buffer.ReadFloat32();
            Field7 = buffer.ReadFloat32();
            Field8 = buffer.ReadInt(32);
            Field9 = buffer.ReadBool();
            Field10 = buffer.ReadBool();
            Field11 = buffer.ReadBool();
            Field12 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            Field1.Encode(buffer);
            buffer.WriteInt(32, Field2);
            buffer.WriteInt(32, m_snoStringList);
            buffer.WriteInt(32, Field4);
            buffer.WriteFloat32(Field5);
            buffer.WriteFloat32(Field6);
            buffer.WriteFloat32(Field7);
            buffer.WriteInt(32, Field8);
            buffer.WriteBool(Field9);
            buffer.WriteBool(Field10);
            buffer.WriteBool(Field11);
            buffer.WriteInt(32, Field12);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("MapMarkerInfoMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            Field1.AsText(b, pad);
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', pad); b.AppendLine("m_snoStringList: 0x" + m_snoStringList.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field4: 0x" + Field4.ToString("X8") + " (" + Field4 + ")");
            b.Append(' ', pad); b.AppendLine("Field5: " + Field5.ToString("G"));
            b.Append(' ', pad); b.AppendLine("Field6: " + Field6.ToString("G"));
            b.Append(' ', pad); b.AppendLine("Field7: " + Field7.ToString("G"));
            b.Append(' ', pad); b.AppendLine("Field8: 0x" + Field8.ToString("X8") + " (" + Field8 + ")");
            b.Append(' ', pad); b.AppendLine("Field9: " + (Field9 ? "true" : "false"));
            b.Append(' ', pad); b.AppendLine("Field10: " + (Field10 ? "true" : "false"));
            b.Append(' ', pad); b.AppendLine("Field11: " + (Field11 ? "true" : "false"));
            b.Append(' ', pad); b.AppendLine("Field12: 0x" + Field12.ToString("X8") + " (" + Field12 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class DebugActorTooltipMessage : GameMessage
    {
        public int Field0;
        public string Field1;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadCharArray(512);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteCharArray(512, Field1);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("DebugActorTooltipMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: \"" + Field1 + "\"");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class BossEncounterMessage : GameMessage
    {
        public int Field0;
        public int /* sno */ snoEncounter;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            snoEncounter = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(32, snoEncounter);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("BossEncounterMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("snoEncounter: 0x" + snoEncounter.ToString("X8"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class EncounterInviteStateMessage : GameMessage
    {
        public int Field0;
        public int Field1;
        public int Field2;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(32, Field2);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("EncounterInviteStateMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class BoolDataMessage : GameMessage
    {
        public bool Field0;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadBool();
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteBool(Field0);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("BoolDataMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: " + (Field0 ? "true" : "false"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class CameraFocusMessage : GameMessage
    {
        public int Field0;
        public bool Field1;
        public float Field2;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadBool();
            Field2 = buffer.ReadFloat32();
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteBool(Field1);
            buffer.WriteFloat32(Field2);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("CameraFocusMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: " + (Field1 ? "true" : "false"));
            b.Append(' ', pad); b.AppendLine("Field2: " + Field2.ToString("G"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class CameraZoomMessage : GameMessage
    {
        public float Field0;
        public bool Field1;
        public float Field2;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadFloat32();
            Field1 = buffer.ReadBool();
            Field2 = buffer.ReadFloat32();
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteFloat32(Field0);
            buffer.WriteBool(Field1);
            buffer.WriteFloat32(Field2);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("CameraZoomMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: " + Field0.ToString("G"));
            b.Append(' ', pad); b.AppendLine("Field1: " + (Field1 ? "true" : "false"));
            b.Append(' ', pad); b.AppendLine("Field2: " + Field2.ToString("G"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class CameraYawMessage : GameMessage
    {
        public float /* angle */ Field0;
        public bool Field1;
        public float Field2;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadFloat32();
            Field1 = buffer.ReadBool();
            Field2 = buffer.ReadFloat32();
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteFloat32(Field0);
            buffer.WriteBool(Field1);
            buffer.WriteFloat32(Field2);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("CameraYawMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: " + Field0.ToString("G"));
            b.Append(' ', pad); b.AppendLine("Field1: " + (Field1 ? "true" : "false"));
            b.Append(' ', pad); b.AppendLine("Field2: " + Field2.ToString("G"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class BossZoomMessage : GameMessage
    {
        public float Field0;
        public float Field1;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadFloat32();
            Field1 = buffer.ReadFloat32();
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteFloat32(Field0);
            buffer.WriteFloat32(Field1);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("BossZoomMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: " + Field0.ToString("G"));
            b.Append(' ', pad); b.AppendLine("Field1: " + Field1.ToString("G"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class EnchantItemMessage : GameMessage
    {
        public int Field0;
        public int /* gbid */ Field1;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(32, Field1);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("EnchantItemMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class CraftingResultsMessage : GameMessage
    {
        public int Field0;
        public int /* gbid */ Field1;
        public int Field2;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(32, Field2);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("CraftingResultsMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class DebugDrawPrimMessage : GameMessage
    {
        public int Field0;
        public int Field1;
        public int Field2;
        public Vector3D Field3;
        public Vector3D Field4;
        public float Field5;
        public float Field6;
        public int Field7;
        public RGBAColor Field8;
        public string Field9;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(32);
            Field3 = new Vector3D();
            Field3.Parse(buffer);
            Field4 = new Vector3D();
            Field4.Parse(buffer);
            Field5 = buffer.ReadFloat32();
            Field6 = buffer.ReadFloat32();
            Field7 = buffer.ReadInt(32);
            Field8 = new RGBAColor();
            Field8.Parse(buffer);
            Field9 = buffer.ReadCharArray(128);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(32, Field2);
            Field3.Encode(buffer);
            Field4.Encode(buffer);
            buffer.WriteFloat32(Field5);
            buffer.WriteFloat32(Field6);
            buffer.WriteInt(32, Field7);
            Field8.Encode(buffer);
            buffer.WriteCharArray(128, Field9);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("DebugDrawPrimMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            Field3.AsText(b, pad);
            Field4.AsText(b, pad);
            b.Append(' ', pad); b.AppendLine("Field5: " + Field5.ToString("G"));
            b.Append(' ', pad); b.AppendLine("Field6: " + Field6.ToString("G"));
            b.Append(' ', pad); b.AppendLine("Field7: 0x" + Field7.ToString("X8") + " (" + Field7 + ")");
            Field8.AsText(b, pad);
            b.Append(' ', pad); b.AppendLine("Field9: \"" + Field9 + "\"");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class CrafterLevelUpMessage : GameMessage
    {
        public int Field0;
        public int Field1;
        public int Field2;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(32, Field2);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("CrafterLevelUpMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class GameTestingSamplingStartMessage : GameMessage
    {
        public int /* sno */ Field0;
        public int /* sno */ Field1;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(32, Field1);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("GameTestingSamplingStartMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class RequestBuffCancelMessage : GameMessage
    {
        public int /* sno */ Field0;
        public int Field1;

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(32, Field1);
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("RequestBuffCancelMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class SimpleMessage : GameMessage
    {

        public override void VisitHandler(IGameMessageHandler handler) { handler.OnMessage(this); }

        public override void Parse(GameBitBuffer buffer)
        {
        }

        public override void Encode(GameBitBuffer buffer)
        {
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("SimpleMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class HeroStateData
    {
        public int Field0;
        public int Field1;
        public int Field2;
        public int Field3;
        public PlayerSavedData Field4;
        public int Field5;
        // MaxLength = 100
        public PlayerQuestRewardHistoryEntry[] tQuestRewardHistory;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(32);
            Field3 = buffer.ReadInt(29);
            Field4 = new PlayerSavedData();
            Field4.Parse(buffer);
            Field5 = buffer.ReadInt(32);
            tQuestRewardHistory = new PlayerQuestRewardHistoryEntry[buffer.ReadInt(7)];
            for (int i = 0; i < tQuestRewardHistory.Length; i++) { tQuestRewardHistory[i] = new PlayerQuestRewardHistoryEntry(); tQuestRewardHistory[i].Parse(buffer); }
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(32, Field2);
            buffer.WriteInt(29, Field3);
            Field4.Encode(buffer);
            buffer.WriteInt(32, Field5);
            buffer.WriteInt(7, tQuestRewardHistory.Length);
            for (int i = 0; i < tQuestRewardHistory.Length; i++) { tQuestRewardHistory[i].Encode(buffer); }
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("HeroStateData:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', pad); b.AppendLine("Field3: 0x" + Field3.ToString("X8") + " (" + Field3 + ")");
            Field4.AsText(b, pad);
            b.Append(' ', pad); b.AppendLine("Field5: 0x" + Field5.ToString("X8") + " (" + Field5 + ")");
            b.Append(' ', pad); b.AppendLine("tQuestRewardHistory:");
            b.Append(' ', pad); b.AppendLine("{");
            for (int i = 0; i < tQuestRewardHistory.Length; i++) { tQuestRewardHistory[i].AsText(b, pad + 1); b.AppendLine(); }
            b.Append(' ', pad); b.AppendLine("}"); b.AppendLine();
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class Vector3D
    {
        public float Field0;
        public float Field1;
        public float Field2;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadFloat32();
            Field1 = buffer.ReadFloat32();
            Field2 = buffer.ReadFloat32();
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteFloat32(Field0);
            buffer.WriteFloat32(Field1);
            buffer.WriteFloat32(Field2);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("Vector3D:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: " + Field0.ToString("G"));
            b.Append(' ', pad); b.AppendLine("Field1: " + Field1.ToString("G"));
            b.Append(' ', pad); b.AppendLine("Field2: " + Field2.ToString("G"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class SceneSpecification
    {
        public int Field0;
        public IVector2D Field1;
        // MaxLength = 4
        public int /* sno */[] arSnoLevelAreas;
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
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            Field1.AsText(b, pad);
            b.Append(' ', pad); b.AppendLine("arSnoLevelAreas:");
            b.Append(' ', pad); b.AppendLine("{");
            for (int i = 0; i < arSnoLevelAreas.Length; ) { b.Append(' ', pad + 1); for (int j = 0; j < 8 && i < arSnoLevelAreas.Length; j++, i++) { b.Append("0x" + arSnoLevelAreas[i].ToString("X8") + ", "); } b.AppendLine(); }
            b.Append(' ', pad); b.AppendLine("}"); b.AppendLine();
            b.Append(' ', pad); b.AppendLine("snoPrevWorld: 0x" + snoPrevWorld.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field4: 0x" + Field4.ToString("X8") + " (" + Field4 + ")");
            b.Append(' ', pad); b.AppendLine("snoPrevLevelArea: 0x" + snoPrevLevelArea.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("snoNextWorld: 0x" + snoNextWorld.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field7: 0x" + Field7.ToString("X8") + " (" + Field7 + ")");
            b.Append(' ', pad); b.AppendLine("snoNextLevelArea: 0x" + snoNextLevelArea.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("snoMusic: 0x" + snoMusic.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("snoCombatMusic: 0x" + snoCombatMusic.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("snoAmbient: 0x" + snoAmbient.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("snoReverb: 0x" + snoReverb.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("snoWeather: 0x" + snoWeather.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("snoPresetWorld: 0x" + snoPresetWorld.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field15: 0x" + Field15.ToString("X8") + " (" + Field15 + ")");
            b.Append(' ', pad); b.AppendLine("Field16: 0x" + Field16.ToString("X8") + " (" + Field16 + ")");
            b.Append(' ', pad); b.AppendLine("Field17: 0x" + Field17.ToString("X8") + " (" + Field17 + ")");
            b.Append(' ', pad); b.AppendLine("Field18: 0x" + Field18.ToString("X8") + " (" + Field18 + ")");
            tCachedValues.AsText(b, pad);
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class PRTransform
    {
        public Quaternion Field0;
        public Vector3D Field1;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = new Quaternion();
            Field0.Parse(buffer);
            Field1 = new Vector3D();
            Field1.Parse(buffer);
        }

        public void Encode(GameBitBuffer buffer)
        {
            Field0.Encode(buffer);
            Field1.Encode(buffer);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("PRTransform:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            Field0.AsText(b, pad);
            Field1.AsText(b, pad);
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class WorldLocationMessageData
    {
        public float Field0;
        public PRTransform Field1;
        public int Field2;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadFloat32();
            Field1 = new PRTransform();
            Field1.Parse(buffer);
            Field2 = buffer.ReadInt(32);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteFloat32(Field0);
            Field1.Encode(buffer);
            buffer.WriteInt(32, Field2);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("WorldLocationMessageData:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: " + Field0.ToString("G"));
            Field1.AsText(b, pad);
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class InventoryLocationMessageData
    {
        public int Field0;
        public int Field1;
        public IVector2D Field2;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(5) + (-1);
            Field2 = new IVector2D();
            Field2.Parse(buffer);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(5, Field1 - (-1));
            Field2.Encode(buffer);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("InventoryLocationMessageData:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            Field2.AsText(b, pad);
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class GBHandle
    {
        public int Field0;
        public int Field1;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(6) + (-2);
            Field1 = buffer.ReadInt(32);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(6, Field0 - (-2));
            buffer.WriteInt(32, Field1);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("GBHandle:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class VisualEquipment
    {
        // MaxLength = 8
        public VisualItem[] Field0;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = new VisualItem[8];
            for (int i = 0; i < Field0.Length; i++) { Field0[i] = new VisualItem(); Field0[i].Parse(buffer); }
        }

        public void Encode(GameBitBuffer buffer)
        {
            for (int i = 0; i < Field0.Length; i++) { Field0[i].Encode(buffer); }
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("VisualEquipment:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0:");
            b.Append(' ', pad); b.AppendLine("{");
            for (int i = 0; i < Field0.Length; i++) { Field0[i].AsText(b, pad + 1); b.AppendLine(); }
            b.Append(' ', pad); b.AppendLine("}"); b.AppendLine();
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class ResolvedPortalDestination
    {
        public int /* sno */ snoWorld;
        public int Field1;
        public int /* sno */ snoDestLevelArea;

        public void Parse(GameBitBuffer buffer)
        {
            snoWorld = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
            snoDestLevelArea = buffer.ReadInt(32);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, snoWorld);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(32, snoDestLevelArea);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("ResolvedPortalDestination:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("snoWorld: 0x" + snoWorld.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad); b.AppendLine("snoDestLevelArea: 0x" + snoDestLevelArea.ToString("X8"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class RareItemName
    {
        public bool Field0;
        public int /* sno */ snoAffixStringList;
        public int Field2;
        public int Field3;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadBool();
            snoAffixStringList = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(32);
            Field3 = buffer.ReadInt(32);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteBool(Field0);
            buffer.WriteInt(32, snoAffixStringList);
            buffer.WriteInt(32, Field2);
            buffer.WriteInt(32, Field3);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("RareItemName:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: " + (Field0 ? "true" : "false"));
            b.Append(' ', pad); b.AppendLine("snoAffixStringList: 0x" + snoAffixStringList.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', pad); b.AppendLine("Field3: 0x" + Field3.ToString("X8") + " (" + Field3 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class NetAttributeKeyValue
    {
        public int? Field0;
        //public int Field1;
        public GameAttribute Attribute;
        public int Int;
        public float Float;

        public void Parse(GameBitBuffer buffer)
        {
            if (buffer.ReadBool())
            {
                Field0 = buffer.ReadInt(20);
            }
            int index = buffer.ReadInt(10) & 0xFFF;

            Attribute = GameAttribute.Attributes[index];
        }

        public void ParseValue(GameBitBuffer buffer)
        {
            switch (Attribute.EncodingType)
            {
                case GameAttributeEncoding.Int:
                    Int = buffer.ReadInt(Attribute.BitCount);
                    break;
                case GameAttributeEncoding.IntMinMax:
                    Int = buffer.ReadInt(Attribute.BitCount) + Attribute.Min;
                    break;
                case GameAttributeEncoding.Float16:
                    Float = buffer.ReadFloat16();
                    break;
                case GameAttributeEncoding.Float16Or32:
                    Float = buffer.ReadBool() ? buffer.ReadFloat16() : buffer.ReadFloat32();
                    break;
                default:
                    throw new Exception("bad voodoo");
            }
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteBool(Field0.HasValue);
            if (Field0.HasValue)
            {
                buffer.WriteInt(20, Field0.Value);
            }
            buffer.WriteInt(10, Attribute.Id);
        }

        public void EncodeValue(GameBitBuffer buffer)
        {
            switch (Attribute.EncodingType)
            {
                case GameAttributeEncoding.Int:
                    buffer.WriteInt(Attribute.BitCount, Int);
                    break;
                case GameAttributeEncoding.IntMinMax:
                    buffer.WriteInt(Attribute.BitCount, Int - Attribute.Min);
                    break;
                case GameAttributeEncoding.Float16:
                    buffer.WriteFloat16(Float);
                    break;
                case GameAttributeEncoding.Float16Or32:
                    if (Float >= 65536.0f || -65536.0f >= Float)
                    {
                        buffer.WriteBool(false);
                        buffer.WriteFloat32(Float);
                    }
                    else
                    {
                        buffer.WriteBool(true);
                        buffer.WriteFloat16(Float);
                    }
                    break;
                default:
                    throw new Exception("bad voodoo");
            }
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("NetAttributeKeyValue:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            if (Field0.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("Field0.Value: 0x" + Field0.Value.ToString("X8") + " (" + Field0.Value + ")");
            }
            b.Append(' ', pad);
            b.Append(Attribute.Name);
            b.Append(" (" + Attribute.Id + "): ");

            if (Attribute.IsInteger)
                b.AppendLine("0x" + Int.ToString("X8") + " (" + Int + ")");
            else
                b.AppendLine(Float.ToString("G"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class PlayAnimationMessageSpec
    {
        public int Field0;
        public int /* sno */ Field1;
        public int Field2;
        public float Field3;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(32);
            Field3 = buffer.ReadFloat32();
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(32, Field2);
            buffer.WriteFloat32(Field3);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("PlayAnimationMessageSpec:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', pad); b.AppendLine("Field3: " + Field3.ToString("G"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class DPathSinData
    {
        public float Field0;
        public float Field1;
        public float Field2;
        public float Field3;
        public float Field4;
        public float Field5;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadFloat32();
            Field1 = buffer.ReadFloat32();
            Field2 = buffer.ReadFloat32();
            Field3 = buffer.ReadFloat32();
            Field4 = buffer.ReadFloat32();
            Field5 = buffer.ReadFloat32();
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteFloat32(Field0);
            buffer.WriteFloat32(Field1);
            buffer.WriteFloat32(Field2);
            buffer.WriteFloat32(Field3);
            buffer.WriteFloat32(Field4);
            buffer.WriteFloat32(Field5);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("DPathSinData:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: " + Field0.ToString("G"));
            b.Append(' ', pad); b.AppendLine("Field1: " + Field1.ToString("G"));
            b.Append(' ', pad); b.AppendLine("Field2: " + Field2.ToString("G"));
            b.Append(' ', pad); b.AppendLine("Field3: " + Field3.ToString("G"));
            b.Append(' ', pad); b.AppendLine("Field4: " + Field4.ToString("G"));
            b.Append(' ', pad); b.AppendLine("Field5: " + Field5.ToString("G"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class NPCInteraction
    {
        public int Field0;
        public int Field1;
        public int Field2;
        public int Field3;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(4);
            Field1 = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(32);
            Field3 = buffer.ReadInt(2);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(4, Field0);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(32, Field2);
            buffer.WriteInt(2, Field3);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("NPCInteraction:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', pad); b.AppendLine("Field3: 0x" + Field3.ToString("X8") + " (" + Field3 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class WorldPlace
    {
        public Vector3D Field0;
        public int Field1;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = new Vector3D();
            Field0.Parse(buffer);
            Field1 = buffer.ReadInt(32);
        }

        public void Encode(GameBitBuffer buffer)
        {
            Field0.Encode(buffer);
            buffer.WriteInt(32, Field1);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("WorldPlace:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            Field0.AsText(b, pad);
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class GameSyncedData
    {
        public bool Field0;
        public int Field1;
        public int Field2;
        public int Field3;
        public int Field4;
        public int Field5;
        // MaxLength = 2
        public int[] Field6;
        // MaxLength = 2
        public int[] Field7;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadBool();
            Field1 = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(32);
            Field3 = buffer.ReadInt(32);
            Field4 = buffer.ReadInt(32);
            Field5 = buffer.ReadInt(32);
            Field6 = new int[2];
            for (int i = 0; i < Field6.Length; i++) Field6[i] = buffer.ReadInt(32);
            Field7 = new int[2];
            for (int i = 0; i < Field7.Length; i++) Field7[i] = buffer.ReadInt(32);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteBool(Field0);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(32, Field2);
            buffer.WriteInt(32, Field3);
            buffer.WriteInt(32, Field4);
            buffer.WriteInt(32, Field5);
            for (int i = 0; i < Field6.Length; i++) buffer.WriteInt(32, Field6[i]);
            for (int i = 0; i < Field7.Length; i++) buffer.WriteInt(32, Field7[i]);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("GameSyncedData:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: " + (Field0 ? "true" : "false"));
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', pad); b.AppendLine("Field3: 0x" + Field3.ToString("X8") + " (" + Field3 + ")");
            b.Append(' ', pad); b.AppendLine("Field4: 0x" + Field4.ToString("X8") + " (" + Field4 + ")");
            b.Append(' ', pad); b.AppendLine("Field5: 0x" + Field5.ToString("X8") + " (" + Field5 + ")");
            b.Append(' ', pad); b.AppendLine("Field6:");
            b.Append(' ', pad); b.AppendLine("{");
            for (int i = 0; i < Field6.Length; ) { b.Append(' ', pad + 1); for (int j = 0; j < 8 && i < Field6.Length; j++, i++) { b.Append("0x" + Field6[i].ToString("X8") + ", "); } b.AppendLine(); }
            b.Append(' ', pad); b.AppendLine("}"); b.AppendLine();
            b.Append(' ', pad); b.AppendLine("Field7:");
            b.Append(' ', pad); b.AppendLine("{");
            for (int i = 0; i < Field7.Length; ) { b.Append(' ', pad + 1); for (int j = 0; j < 8 && i < Field7.Length; j++, i++) { b.Append("0x" + Field7[i].ToString("X8") + ", "); } b.AppendLine(); }
            b.Append(' ', pad); b.AppendLine("}"); b.AppendLine();
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class GameId
    {
        public long Field0;
        public long Field1;
        public long Field2;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt64(64);
            Field1 = buffer.ReadInt64(64);
            Field2 = buffer.ReadInt64(64);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt64(64, Field0);
            buffer.WriteInt64(64, Field1);
            buffer.WriteInt64(64, Field2);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("GameId:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X16"));
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X16"));
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X16"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class EntityId
    {
        public long Field0;
        public long Field1;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt64(64);
            Field1 = buffer.ReadInt64(64);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt64(64, Field0);
            buffer.WriteInt64(64, Field1);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("EntityId:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X16"));
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X16"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class AnimPreplayData
    {
        public int Field0;
        public int Field1;
        public int Field2;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(32);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(32, Field2);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("AnimPreplayData:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class InvLoc
    {
        public int Field0;
        public int Field1;
        public int Field2;
        public int Field3;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(5) + (-1);
            Field2 = buffer.ReadInt(32);
            Field3 = buffer.ReadInt(32);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(5, Field1 - (-1));
            buffer.WriteInt(32, Field2);
            buffer.WriteInt(32, Field3);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("InvLoc:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', pad); b.AppendLine("Field3: 0x" + Field3.ToString("X8") + " (" + Field3 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class HotbarButtonData
    {
        public int /* sno */ m_snoPower;
        public int /* gbid */ m_gbidItem;

        public void Parse(GameBitBuffer buffer)
        {
            m_snoPower = buffer.ReadInt(32);
            m_gbidItem = buffer.ReadInt(32);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, m_snoPower);
            buffer.WriteInt(32, m_gbidItem);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("HotbarButtonData:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("m_snoPower: 0x" + m_snoPower.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("m_gbidItem: 0x" + m_gbidItem.ToString("X8"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class PlayLineParams
    {
        public int /* sno */ snoConversation;
        public int Field1;
        public bool Field2;
        public int Field3;
        public int Field4;
        public int Field5;
        public int Field6;
        public int Field7;
        public int Field8;
        public int /* sno */ snoSpeakerActor;
        public string Field10;
        public int Field11;
        public int Field12;
        public int Field13;
        public int Field14;
        public int Field15;

        public void Parse(GameBitBuffer buffer)
        {
            snoConversation = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
            Field2 = buffer.ReadBool();
            Field3 = buffer.ReadInt(32);
            Field4 = buffer.ReadInt(32);
            Field5 = buffer.ReadInt(32);
            Field6 = buffer.ReadInt(32);
            Field7 = buffer.ReadInt(32);
            Field8 = buffer.ReadInt(32);
            snoSpeakerActor = buffer.ReadInt(32);
            Field10 = buffer.ReadCharArray(49);
            Field11 = buffer.ReadInt(32);
            Field12 = buffer.ReadInt(32);
            Field13 = buffer.ReadInt(32);
            Field14 = buffer.ReadInt(32);
            Field15 = buffer.ReadInt(32);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, snoConversation);
            buffer.WriteInt(32, Field1);
            buffer.WriteBool(Field2);
            buffer.WriteInt(32, Field3);
            buffer.WriteInt(32, Field4);
            buffer.WriteInt(32, Field5);
            buffer.WriteInt(32, Field6);
            buffer.WriteInt(32, Field7);
            buffer.WriteInt(32, Field8);
            buffer.WriteInt(32, snoSpeakerActor);
            buffer.WriteCharArray(49, Field10);
            buffer.WriteInt(32, Field11);
            buffer.WriteInt(32, Field12);
            buffer.WriteInt(32, Field13);
            buffer.WriteInt(32, Field14);
            buffer.WriteInt(32, Field15);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("PlayLineParams:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("snoConversation: 0x" + snoConversation.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad); b.AppendLine("Field2: " + (Field2 ? "true" : "false"));
            b.Append(' ', pad); b.AppendLine("Field3: 0x" + Field3.ToString("X8") + " (" + Field3 + ")");
            b.Append(' ', pad); b.AppendLine("Field4: 0x" + Field4.ToString("X8") + " (" + Field4 + ")");
            b.Append(' ', pad); b.AppendLine("Field5: 0x" + Field5.ToString("X8") + " (" + Field5 + ")");
            b.Append(' ', pad); b.AppendLine("Field6: 0x" + Field6.ToString("X8") + " (" + Field6 + ")");
            b.Append(' ', pad); b.AppendLine("Field7: 0x" + Field7.ToString("X8") + " (" + Field7 + ")");
            b.Append(' ', pad); b.AppendLine("Field8: 0x" + Field8.ToString("X8") + " (" + Field8 + ")");
            b.Append(' ', pad); b.AppendLine("snoSpeakerActor: 0x" + snoSpeakerActor.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field10: \"" + Field10 + "\"");
            b.Append(' ', pad); b.AppendLine("Field11: 0x" + Field11.ToString("X8") + " (" + Field11 + ")");
            b.Append(' ', pad); b.AppendLine("Field12: 0x" + Field12.ToString("X8") + " (" + Field12 + ")");
            b.Append(' ', pad); b.AppendLine("Field13: 0x" + Field13.ToString("X8") + " (" + Field13 + ")");
            b.Append(' ', pad); b.AppendLine("Field14: 0x" + Field14.ToString("X8") + " (" + Field14 + ")");
            b.Append(' ', pad); b.AppendLine("Field15: 0x" + Field15.ToString("X8") + " (" + Field15 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class SNOName
    {
        public int /* sno_group */ Field0;
        public int /* snoname_handle */ Field1;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(32, Field1);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("SNOName:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class ActiveEvent
    {
        public int /* sno */ snoTimedEvent;
        public int Field1;
        public int Field2;

        public void Parse(GameBitBuffer buffer)
        {
            snoTimedEvent = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(32);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, snoTimedEvent);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(32, Field2);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("ActiveEvent:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("snoTimedEvent: 0x" + snoTimedEvent.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class RGBAColor
    {
        public byte Field0;
        public byte Field1;
        public byte Field2;
        public byte Field3;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = (byte)buffer.ReadInt(8);
            Field1 = (byte)buffer.ReadInt(8);
            Field2 = (byte)buffer.ReadInt(8);
            Field3 = (byte)buffer.ReadInt(8);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(8, Field0);
            buffer.WriteInt(8, Field1);
            buffer.WriteInt(8, Field2);
            buffer.WriteInt(8, Field3);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("RGBAColor:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X2"));
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X2"));
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X2"));
            b.Append(' ', pad); b.AppendLine("Field3: 0x" + Field3.ToString("X2"));
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class PlayerSavedData
    {
        // MaxLength = 9
        public HotbarButtonData[] Field0;
        // MaxLength = 15
        public SkillKeyMapping[] Field1;
        public int /* time */ Field2;
        public int Field3;
        public HirelingSavedData Field4;
        public int Field5;
        public LearnedLore Field6;
        // MaxLength = 6
        public int /* sno */[] snoActiveSkills;
        // MaxLength = 3
        public int /* sno */[] snoTraits;
        public SavePointData Field9;
        // MaxLength = 64
        public int /* sno */[] m_SeenTutorials;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = new HotbarButtonData[9];
            for (int i = 0; i < Field0.Length; i++) { Field0[i] = new HotbarButtonData(); Field0[i].Parse(buffer); }
            Field1 = new SkillKeyMapping[15];
            for (int i = 0; i < Field1.Length; i++) { Field1[i] = new SkillKeyMapping(); Field1[i].Parse(buffer); }
            Field2 = buffer.ReadInt(32);
            Field3 = buffer.ReadInt(32);
            Field4 = new HirelingSavedData();
            Field4.Parse(buffer);
            Field5 = buffer.ReadInt(32);
            Field6 = new LearnedLore();
            Field6.Parse(buffer);
            snoActiveSkills = new int /* sno */[6];
            for (int i = 0; i < snoActiveSkills.Length; i++) snoActiveSkills[i] = buffer.ReadInt(32);
            snoTraits = new int /* sno */[3];
            for (int i = 0; i < snoTraits.Length; i++) snoTraits[i] = buffer.ReadInt(32);
            Field9 = new SavePointData();
            Field9.Parse(buffer);
            m_SeenTutorials = new int /* sno */[64];
            for (int i = 0; i < m_SeenTutorials.Length; i++) m_SeenTutorials[i] = buffer.ReadInt(32);
        }

        public void Encode(GameBitBuffer buffer)
        {
            for (int i = 0; i < Field0.Length; i++) { Field0[i].Encode(buffer); }
            for (int i = 0; i < Field1.Length; i++) { Field1[i].Encode(buffer); }
            buffer.WriteInt(32, Field2);
            buffer.WriteInt(32, Field3);
            Field4.Encode(buffer);
            buffer.WriteInt(32, Field5);
            Field6.Encode(buffer);
            for (int i = 0; i < snoActiveSkills.Length; i++) buffer.WriteInt(32, snoActiveSkills[i]);
            for (int i = 0; i < snoTraits.Length; i++) buffer.WriteInt(32, snoTraits[i]);
            Field9.Encode(buffer);
            for (int i = 0; i < m_SeenTutorials.Length; i++) buffer.WriteInt(32, m_SeenTutorials[i]);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("PlayerSavedData:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0:");
            b.Append(' ', pad); b.AppendLine("{");
            for (int i = 0; i < Field0.Length; i++) { Field0[i].AsText(b, pad + 1); b.AppendLine(); }
            b.Append(' ', pad); b.AppendLine("}"); b.AppendLine();
            b.Append(' ', pad); b.AppendLine("Field1:");
            b.Append(' ', pad); b.AppendLine("{");
            for (int i = 0; i < Field1.Length; i++) { Field1[i].AsText(b, pad + 1); b.AppendLine(); }
            b.Append(' ', pad); b.AppendLine("}"); b.AppendLine();
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field3: 0x" + Field3.ToString("X8") + " (" + Field3 + ")");
            Field4.AsText(b, pad);
            b.Append(' ', pad); b.AppendLine("Field5: 0x" + Field5.ToString("X8") + " (" + Field5 + ")");
            Field6.AsText(b, pad);
            b.Append(' ', pad); b.AppendLine("snoActiveSkills:");
            b.Append(' ', pad); b.AppendLine("{");
            for (int i = 0; i < snoActiveSkills.Length; ) { b.Append(' ', pad + 1); for (int j = 0; j < 8 && i < snoActiveSkills.Length; j++, i++) { b.Append("0x" + snoActiveSkills[i].ToString("X8") + ", "); } b.AppendLine(); }
            b.Append(' ', pad); b.AppendLine("}"); b.AppendLine();
            b.Append(' ', pad); b.AppendLine("snoTraits:");
            b.Append(' ', pad); b.AppendLine("{");
            for (int i = 0; i < snoTraits.Length; ) { b.Append(' ', pad + 1); for (int j = 0; j < 8 && i < snoTraits.Length; j++, i++) { b.Append("0x" + snoTraits[i].ToString("X8") + ", "); } b.AppendLine(); }
            b.Append(' ', pad); b.AppendLine("}"); b.AppendLine();
            Field9.AsText(b, pad);
            b.Append(' ', pad); b.AppendLine("m_SeenTutorials:");
            b.Append(' ', pad); b.AppendLine("{");
            for (int i = 0; i < m_SeenTutorials.Length; ) { b.Append(' ', pad + 1); for (int j = 0; j < 8 && i < m_SeenTutorials.Length; j++, i++) { b.Append("0x" + m_SeenTutorials[i].ToString("X8") + ", "); } b.AppendLine(); }
            b.Append(' ', pad); b.AppendLine("}"); b.AppendLine();
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class PlayerQuestRewardHistoryEntry
    {
        public int /* sno */ snoQuest;
        public int Field1;
        public enum eField2
        {
            Normal = 0,
            Nightmare = 1,
            Hell = 2,
            Inferno = 3,
        }
        public eField2 Field2;

        public void Parse(GameBitBuffer buffer)
        {
            snoQuest = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
            Field2 = (eField2)buffer.ReadInt(2);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, snoQuest);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(2, (int)Field2);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("PlayerQuestRewardHistoryEntry:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("snoQuest: 0x" + snoQuest.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad); b.AppendLine("Field2: " + Field2.ToString());
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class IVector2D
    {
        public int Field0;
        public int Field1;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(32, Field1);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("IVector2D:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class SceneCachedValues
    {
        public int Field0;
        public int Field1;
        public int Field2;
        public AABB Field3;
        public AABB Field4;
        // MaxLength = 4
        public int[] Field5;
        public int Field6;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(32);
            Field3 = new AABB();
            Field3.Parse(buffer);
            Field4 = new AABB();
            Field4.Parse(buffer);
            Field5 = new int[4];
            for (int i = 0; i < Field5.Length; i++) Field5[i] = buffer.ReadInt(32);
            Field6 = buffer.ReadInt(32);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(32, Field2);
            Field3.Encode(buffer);
            Field4.Encode(buffer);
            for (int i = 0; i < Field5.Length; i++) buffer.WriteInt(32, Field5[i]);
            buffer.WriteInt(32, Field6);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("SceneCachedValues:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            Field3.AsText(b, pad);
            Field4.AsText(b, pad);
            b.Append(' ', pad); b.AppendLine("Field5:");
            b.Append(' ', pad); b.AppendLine("{");
            for (int i = 0; i < Field5.Length; ) { b.Append(' ', pad + 1); for (int j = 0; j < 8 && i < Field5.Length; j++, i++) { b.Append("0x" + Field5[i].ToString("X8") + ", "); } b.AppendLine(); }
            b.Append(' ', pad); b.AppendLine("}"); b.AppendLine();
            b.Append(' ', pad); b.AppendLine("Field6: 0x" + Field6.ToString("X8") + " (" + Field6 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class Quaternion
    {
        public float Field0;
        public Vector3D Field1;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadFloat32();
            Field1 = new Vector3D();
            Field1.Parse(buffer);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteFloat32(Field0);
            Field1.Encode(buffer);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("Quaternion:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: " + Field0.ToString("G"));
            Field1.AsText(b, pad);
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class VisualItem
    {
        public int /* gbid */ Field0;
        public int Field1;
        public int Field2;
        public int Field3;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(5);
            Field2 = buffer.ReadInt(4);
            Field3 = buffer.ReadInt(5) + (-1);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(5, Field1);
            buffer.WriteInt(4, Field2);
            buffer.WriteInt(5, Field3 - (-1));
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("VisualItem:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', pad); b.AppendLine("Field3: 0x" + Field3.ToString("X8") + " (" + Field3 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class SkillKeyMapping
    {
        public int /* sno */ Power;
        public int Field1;
        public int Field2;

        public void Parse(GameBitBuffer buffer)
        {
            Power = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(4);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Power);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(4, Field2);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("SkillKeyMapping:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Power: 0x" + Power.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class HirelingSavedData
    {
        // MaxLength = 4
        public HirelingInfo[] Field0;
        public int Field1;
        public int Field2;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = new HirelingInfo[4];
            for (int i = 0; i < Field0.Length; i++) { Field0[i] = new HirelingInfo(); Field0[i].Parse(buffer); }
            Field1 = buffer.ReadInt(2);
            Field2 = buffer.ReadInt(32);
        }

        public void Encode(GameBitBuffer buffer)
        {
            for (int i = 0; i < Field0.Length; i++) { Field0[i].Encode(buffer); }
            buffer.WriteInt(2, Field1);
            buffer.WriteInt(32, Field2);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("HirelingSavedData:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0:");
            b.Append(' ', pad); b.AppendLine("{");
            for (int i = 0; i < Field0.Length; i++) { Field0[i].AsText(b, pad + 1); b.AppendLine(); }
            b.Append(' ', pad); b.AppendLine("}"); b.AppendLine();
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class LearnedLore
    {
        public int Field0;
        // MaxLength = 256
        public int /* sno */[] m_snoLoreLearned;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            m_snoLoreLearned = new int /* sno */[256];
            for (int i = 0; i < m_snoLoreLearned.Length; i++) m_snoLoreLearned[i] = buffer.ReadInt(32);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            for (int i = 0; i < m_snoLoreLearned.Length; i++) buffer.WriteInt(32, m_snoLoreLearned[i]);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("LearnedLore:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("m_snoLoreLearned:");
            b.Append(' ', pad); b.AppendLine("{");
            for (int i = 0; i < m_snoLoreLearned.Length; ) { b.Append(' ', pad + 1); for (int j = 0; j < 8 && i < m_snoLoreLearned.Length; j++, i++) { b.Append("0x" + m_snoLoreLearned[i].ToString("X8") + ", "); } b.AppendLine(); }
            b.Append(' ', pad); b.AppendLine("}"); b.AppendLine();
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class SavePointData
    {
        public int /* sno */ snoWorld;
        public int Field1;

        public void Parse(GameBitBuffer buffer)
        {
            snoWorld = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, snoWorld);
            buffer.WriteInt(32, Field1);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("SavePointData:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("snoWorld: 0x" + snoWorld.ToString("X8"));
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class AABB
    {
        public Vector3D Field0;
        public Vector3D Field1;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = new Vector3D();
            Field0.Parse(buffer);
            Field1 = new Vector3D();
            Field1.Parse(buffer);
        }

        public void Encode(GameBitBuffer buffer)
        {
            Field0.Encode(buffer);
            Field1.Encode(buffer);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("AABB:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            Field0.AsText(b, pad);
            Field1.AsText(b, pad);
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }

    public class HirelingInfo
    {
        public int Field0;
        public int Field1;
        public int Field2;
        public int Field3;
        public bool Field4;
        public int Field5;
        public int Field6;
        public int Field7;
        public int Field8;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(2);
            Field1 = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(7);
            Field3 = buffer.ReadInt(32);
            Field4 = buffer.ReadBool();
            Field5 = buffer.ReadInt(32);
            Field6 = buffer.ReadInt(32);
            Field7 = buffer.ReadInt(32);
            Field8 = buffer.ReadInt(32);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(2, Field0);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(7, Field2);
            buffer.WriteInt(32, Field3);
            buffer.WriteBool(Field4);
            buffer.WriteInt(32, Field5);
            buffer.WriteInt(32, Field6);
            buffer.WriteInt(32, Field7);
            buffer.WriteInt(32, Field8);
        }

        public void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("HirelingInfo:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("Field0: 0x" + Field0.ToString("X8") + " (" + Field0 + ")");
            b.Append(' ', pad); b.AppendLine("Field1: 0x" + Field1.ToString("X8") + " (" + Field1 + ")");
            b.Append(' ', pad); b.AppendLine("Field2: 0x" + Field2.ToString("X8") + " (" + Field2 + ")");
            b.Append(' ', pad); b.AppendLine("Field3: 0x" + Field3.ToString("X8") + " (" + Field3 + ")");
            b.Append(' ', pad); b.AppendLine("Field4: " + (Field4 ? "true" : "false"));
            b.Append(' ', pad); b.AppendLine("Field5: 0x" + Field5.ToString("X8") + " (" + Field5 + ")");
            b.Append(' ', pad); b.AppendLine("Field6: 0x" + Field6.ToString("X8") + " (" + Field6 + ")");
            b.Append(' ', pad); b.AppendLine("Field7: 0x" + Field7.ToString("X8") + " (" + Field7 + ")");
            b.Append(' ', pad); b.AppendLine("Field8: 0x" + Field8.ToString("X8") + " (" + Field8 + ")");
            b.Append(' ', --pad);
            b.AppendLine("}");
        }


    }


}