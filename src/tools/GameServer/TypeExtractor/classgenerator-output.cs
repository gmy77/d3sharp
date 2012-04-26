    public class EndOfTickMessage : GameMessage
    {
        public int Field0;
        public int Field1;

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

    }

    public class CreateHeroResultMessage : GameMessage
    {
        int _Field0;
        public int Field0 { get { return _Field0; } set { if(value < -1 || value > 2) throw new ArgumentOutOfRangeException(); _Field0 = value; } }
        public EntityId Field1;

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

    }

    public class CreateHeroMessage : GameMessage
    {
        public string _Field0;
        public string Field0 { get { return _Field0; } set { if(value != null && value.Length > 49) throw new ArgumentOutOfRangeException(); _Field0 = value; } }
        public int Field1;
        int _Field2;
        public int Field2 { get { return _Field2; } set { if(value < 0 || value > 0x3FFFFFFF) throw new ArgumentOutOfRangeException(); _Field2 = value; } }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadCharArray(49);
            Field1 = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(30);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteCharArray(49, Field0);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(30, Field2);
        }

    }

    public class JoinBNetGameMessage : GameMessage
    {
        public EntityId Field0;
        public GameId Field1;
        public int Field2;
        public long Field3;
        int _Field4;
        public int Field4 { get { return _Field4; } set { if(value < 2 || value > 17) throw new ArgumentOutOfRangeException(); _Field4 = value; } }
        public int Field5;
        public int Field6;

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = new EntityId();
            Field0.Parse(buffer);
            Field1 = new GameId();
            Field1.Parse(buffer);
            Field2 = buffer.ReadInt(32);
            Field3 = buffer.ReadInt64(64);
            Field4 = buffer.ReadInt(4) + (2);
            Field5 = buffer.ReadInt(32);
            Field6 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            Field0.Encode(buffer);
            Field1.Encode(buffer);
            buffer.WriteInt(32, Field2);
            buffer.WriteInt64(64, Field3);
            buffer.WriteInt(4, Field4 - (2));
            buffer.WriteInt(32, Field5);
            buffer.WriteInt(32, Field6);
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

    }

    public class BNetJoinGameRequestResultMessage : GameMessage
    {
        int _Field0;
        public int Field0 { get { return _Field0; } set { if(value < -1 || value > 5) throw new ArgumentOutOfRangeException(); _Field0 = value; } }
        public GameId Field1;
        public long Field2;
        int _Field3;
        public int Field3 { get { return _Field3; } set { if(value < -1 || value > 5) throw new ArgumentOutOfRangeException(); _Field3 = value; } }
        public int Field4;
        int _Field5;
        public int Field5 { get { return _Field5; } set { if(value < 0 || value > 0xFFFF) throw new ArgumentOutOfRangeException(); _Field5 = value; } }
        public int Field6;
        public ushort Field7;

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
            Field7 = (ushort)buffer.ReadInt(16);
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

    }

    public class RequestJoinBNetGameMessage : GameMessage
    {
        public GameId Field0;
        public EntityId Field1;
        int _Field2;
        public int Field2 { get { return _Field2; } set { if(value < -1 || value > 22) throw new ArgumentOutOfRangeException(); _Field2 = value; } }

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

    }

    public class CreateBNetGameResultMessage : GameMessage
    {
        int _Field0;
        public int Field0 { get { return _Field0; } set { if(value < 0 || value > 2) throw new ArgumentOutOfRangeException(); _Field0 = value; } }
        public GameId Field1;

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

    }

    public class CreateBNetGameMessage : GameMessage
    {
        public string _Field0;
        public string Field0 { get { return _Field0; } set { if(value != null && value.Length > 33) throw new ArgumentOutOfRangeException(); _Field0 = value; } }
        int _Field1;
        public int Field1 { get { return _Field1; } set { if(value < -1 || value > 5) throw new ArgumentOutOfRangeException(); _Field1 = value; } }
        public int Field2;
        public int Field3;
        public int Field4;
        public bool Field5;
        public int Field6;
        int _Field7;
        public int Field7 { get { return _Field7; } set { if(value < 0 || value > 0xFFFF) throw new ArgumentOutOfRangeException(); _Field7 = value; } }
        int _Field8;
        public int Field8 { get { return _Field8; } set { if(value < 1 || value > 8) throw new ArgumentOutOfRangeException(); _Field8 = value; } }
        public int Field9;
        public int Field10;
        public ushort Field11;

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
            Field11 = (ushort)buffer.ReadInt(16);
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

    }

    public class EffectGroupACDToACDMessage : GameMessage
    {
        public int Field0;
        public int Field1;
        public int Field2;

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

    }

    public class RopeEffectMessageACDToPlace : GameMessage
    {
        public int Field0;
        public int Field1;
        int _Field2;
        public int Field2 { get { return _Field2; } set { if(value < 0 || value > 19) throw new ArgumentOutOfRangeException(); _Field2 = value; } }
        public WorldPlace Field3;

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

    }

    public class RopeEffectMessageACDToACD : GameMessage
    {
        public int Field0;
        public int Field1;
        int _Field2;
        public int Field2 { get { return _Field2; } set { if(value < 0 || value > 19) throw new ArgumentOutOfRangeException(); _Field2 = value; } }
        public int Field3;
        int _Field4;
        public int Field4 { get { return _Field4; } set { if(value < 0 || value > 19) throw new ArgumentOutOfRangeException(); _Field4 = value; } }

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

    }

    public class PlayMusicMessage : GameMessage
    {
        public int snoMusic;

        public override void Parse(GameBitBuffer buffer)
        {
            snoMusic = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, snoMusic);
        }

    }

    public class PlayNonPositionalSoundMessage : GameMessage
    {
        public int Field0;

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
        }

    }

    public class PlayHitEffectOverrideMessage : GameMessage
    {
        public int Field0;
        public int Field1;
        public int Field2;

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

    }

    public class PlayHitEffectMessage : GameMessage
    {
        public int Field0;
        public int Field1;
        int _Field2;
        public int Field2 { get { return _Field2; } set { if(value < -1 || value > 6) throw new ArgumentOutOfRangeException(); _Field2 = value; } }
        public bool Field3;

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

    }

    public class PlayEffectMessage : GameMessage
    {
        public int Field0;
        int _Field1;
        public int Field1 { get { return _Field1; } set { if(value < -1 || value > 70) throw new ArgumentOutOfRangeException(); _Field1 = value; } }
        public int? Field2;

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(7) + (-1);
            if(buffer.ReadBool())
                Field2 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(7, Field1 - (-1));
            if(Field2.HasValue)
                buffer.WriteInt(32, Field2.Value);
        }

    }

    public class EndConversationMessage : GameMessage
    {
        public int Field0;
        public int Field1;

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

    }

    public class StopConvLineMessage : GameMessage
    {
        public int Field0;
        public bool Field1;

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

    }

    public class UpdateConvAutoAdvanceMessage : GameMessage
    {
        public int Field0;
        public int Field1;
        public int Field2;

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

    }

    public class AdvanceConvMessage : GameMessage
    {
        public int Field0;
        public int Field1;

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

    }

    public class PlayConvLineMessage : GameMessage
    {
        public int Field0;
        int[] _Field1;
        public int[] Field1 { get { return _Field1; } set { if(value != null && value.Length != 9) throw new ArgumentOutOfRangeException(); _Field1 = value; } }
        public PlayLineParams Field2;
        public int Field3;

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = new int[9];
            for(int i = 0;i < _Field1.Length;i++) _Field1[i] = buffer.ReadInt(32);
            Field2 = new PlayLineParams();
            Field2.Parse(buffer);
            Field3 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            for(int i = 0;i < _Field1.Length;i++) buffer.WriteInt(32, _Field1[i]);
            Field2.Encode(buffer);
            buffer.WriteInt(32, Field3);
        }

    }

    public class PlayLineParams
    {
        public int snoConversation;
        public int Field1;
        public bool Field2;
        public bool Field3;
        public bool Field4;
        public int Field5;
        public int Field6;
        public int Field7;
        public int Field8;
        public int snoSpeakerActor;
        public int Field10;
        public int Field11;
        public string _Field12;
        public string Field12 { get { return _Field12; } set { if(value != null && value.Length > 49) throw new ArgumentOutOfRangeException(); _Field12 = value; } }
        public int Field13;
        public int Field14;
        public int Field15;
        public int Field16;
        public int Field17;

        public void Parse(GameBitBuffer buffer)
        {
            snoConversation = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
            Field2 = buffer.ReadBool();
            Field3 = buffer.ReadBool();
            Field4 = buffer.ReadBool();
            Field5 = buffer.ReadInt(32);
            Field6 = buffer.ReadInt(32);
            Field7 = buffer.ReadInt(32);
            Field8 = buffer.ReadInt(32);
            snoSpeakerActor = buffer.ReadInt(32);
            Field10 = buffer.ReadInt(32);
            Field11 = buffer.ReadInt(32);
            Field12 = buffer.ReadCharArray(49);
            Field13 = buffer.ReadInt(32);
            Field14 = buffer.ReadInt(32);
            Field15 = buffer.ReadInt(32);
            Field16 = buffer.ReadInt(32);
            Field17 = buffer.ReadInt(32);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, snoConversation);
            buffer.WriteInt(32, Field1);
            buffer.WriteBool(Field2);
            buffer.WriteBool(Field3);
            buffer.WriteBool(Field4);
            buffer.WriteInt(32, Field5);
            buffer.WriteInt(32, Field6);
            buffer.WriteInt(32, Field7);
            buffer.WriteInt(32, Field8);
            buffer.WriteInt(32, snoSpeakerActor);
            buffer.WriteInt(32, Field10);
            buffer.WriteInt(32, Field11);
            buffer.WriteCharArray(49, Field12);
            buffer.WriteInt(32, Field13);
            buffer.WriteInt(32, Field14);
            buffer.WriteInt(32, Field15);
            buffer.WriteInt(32, Field16);
            buffer.WriteInt(32, Field17);
        }

    }

    public class TimedEventStartedMessage : GameMessage
    {
        public ActiveEvent Field0;

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = new ActiveEvent();
            Field0.Parse(buffer);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            Field0.Encode(buffer);
        }

    }

    public class ActiveEvent
    {
        public int snoTimedEvent;
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

    }

    public class ActTransitionStartedMessage : GameMessage
    {
        public int Field0;
        public int Field1;

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

    }

    public class PortalSpecifierMessage : GameMessage
    {
        public int Field0;
        public ResolvedPortalDestination Field1;

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

    }

    public class ResolvedPortalDestination
    {
        public int snoWorld;
        public int Field1;
        public int snoDestLevelArea;

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

    }

    public class ACDClientTranslateMessage : GameMessage
    {
        public int Field0;
        int _Field1;
        public int Field1 { get { return _Field1; } set { if(value < 0 || value > 12) throw new ArgumentOutOfRangeException(); _Field1 = value; } }
        public Vector3D Field2;
        public float Field3;
        public float Field4;
        public int Field5;
        int _Field6;
        public int Field6 { get { return _Field6; } set { if(value < -1 || value > 0xFFFFF) throw new ArgumentOutOfRangeException(); _Field6 = value; } }
        public int? Field7;

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(4);
            Field2 = new Vector3D();
            Field2.Parse(buffer);
            Field3 = buffer.ReadFloat32();
            Field4 = buffer.ReadFloat32();
            Field5 = buffer.ReadInt(32);
            Field6 = buffer.ReadInt(21) + (-1);
            if(buffer.ReadBool())
                Field7 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(4, Field1);
            Field2.Encode(buffer);
            buffer.WriteFloat32(Field3);
            buffer.WriteFloat32(Field4);
            buffer.WriteInt(32, Field5);
            buffer.WriteInt(21, Field6 - (-1));
            if(Field7.HasValue)
                buffer.WriteInt(32, Field7.Value);
        }

    }

    public class ACDTranslateFixedUpdateMessage : GameMessage
    {
        public int Field0;
        public Vector3D Field1;
        public Vector3D Field2;

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

    }

    public class ACDTranslateDetPathSinMessage : GameMessage
    {
        public int Field0;
        int _Field1;
        public int Field1 { get { return _Field1; } set { if(value < 0 || value > 8) throw new ArgumentOutOfRangeException(); _Field1 = value; } }
        public int Field2;
        public int Field3;
        public Vector3D Field4;
        public float Field5;
        public Vector3D Field6;
        public int Field7;
        public int Field8;
        public int Field9;
        public int Field10;
        public int Field11;
        public float Field12;
        public float Field13;
        public DPathSinData Field14;

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

    }

    public class ACDTranslateDetPathMessage : GameMessage
    {
        public int Field0;
        int _Field1;
        public int Field1 { get { return _Field1; } set { if(value < 0 || value > 8) throw new ArgumentOutOfRangeException(); _Field1 = value; } }
        public int Field2;
        public int Field3;
        public Vector3D Field4;
        public float Field5;
        public Vector3D Field6;
        public int Field7;
        public int Field8;
        public int Field9;
        public int Field10;
        public int Field11;
        public float Field12;
        public float Field13;
        public float Field14;
        public float Field15;

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

    }

    public class ACDTranslateArcMessage : GameMessage
    {
        public int Field0;
        public Vector3D Field1;
        public Vector3D Field2;
        int _Field3;
        public int Field3 { get { return _Field3; } set { if(value < 0 || value > 0x1FFFFFF) throw new ArgumentOutOfRangeException(); _Field3 = value; } }
        int _Field4;
        public int Field4 { get { return _Field4; } set { if(value < -1 || value > 0xFFFFF) throw new ArgumentOutOfRangeException(); _Field4 = value; } }
        int _Field5;
        public int Field5 { get { return _Field5; } set { if(value < -1 || value > 0xFFFFF) throw new ArgumentOutOfRangeException(); _Field5 = value; } }
        public float Field6;
        public int Field7;
        public float Field8;
        public float Field9;

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = new Vector3D();
            Field1.Parse(buffer);
            Field2 = new Vector3D();
            Field2.Parse(buffer);
            Field3 = buffer.ReadInt(25);
            Field4 = buffer.ReadInt(21) + (-1);
            Field5 = buffer.ReadInt(21) + (-1);
            Field6 = buffer.ReadFloat32();
            Field7 = buffer.ReadInt(32);
            Field8 = buffer.ReadFloat32();
            Field9 = buffer.ReadFloat32();
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            Field1.Encode(buffer);
            Field2.Encode(buffer);
            buffer.WriteInt(25, Field3);
            buffer.WriteInt(21, Field4 - (-1));
            buffer.WriteInt(21, Field5 - (-1));
            buffer.WriteFloat32(Field6);
            buffer.WriteInt(32, Field7);
            buffer.WriteFloat32(Field8);
            buffer.WriteFloat32(Field9);
        }

    }

    public class ACDTranslateFixedMessage : GameMessage
    {
        public int Field0;
        public Vector3D Field1;
        int _Field2;
        public int Field2 { get { return _Field2; } set { if(value < 0 || value > 0x1FFFFFF) throw new ArgumentOutOfRangeException(); _Field2 = value; } }
        int _Field3;
        public int Field3 { get { return _Field3; } set { if(value < -1 || value > 0xFFFFF) throw new ArgumentOutOfRangeException(); _Field3 = value; } }
        public int Field4;

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = new Vector3D();
            Field1.Parse(buffer);
            Field2 = buffer.ReadInt(25);
            Field3 = buffer.ReadInt(21) + (-1);
            Field4 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            Field1.Encode(buffer);
            buffer.WriteInt(25, Field2);
            buffer.WriteInt(21, Field3 - (-1));
            buffer.WriteInt(32, Field4);
        }

    }

    public class ACDTranslateFacingMessage : GameMessage
    {
        public int Field0;
        public float Field1;
        public bool Field2;

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

    }

    public class ACDTranslateAckMessage : GameMessage
    {
        public int Field0;

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
        }

    }

    public class ACDTranslateSyncMessage : GameMessage
    {
        public int Field0;
        public Vector3D Field1;
        public bool? Field2;
        int? _Field3;
        public int? Field3 { get { return _Field3; } set { if(value.HasValue && (value.Value < 0 || value.Value > 0xFFFF)) throw new ArgumentOutOfRangeException(); _Field3 = value; } }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = new Vector3D();
            Field1.Parse(buffer);
            if(buffer.ReadBool())
                Field2 = buffer.ReadBool();
            if(buffer.ReadBool())
                Field3 = buffer.ReadInt(16);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            Field1.Encode(buffer);
            if(Field2.HasValue)
                buffer.WriteBool(Field2.Value);
            if(Field3.HasValue)
                buffer.WriteInt(16, Field3.Value);
        }

    }

    public class ACDTranslateSnappedMessage : GameMessage
    {
        public int Field0;
        public Vector3D Field1;
        public float Field2;
        public bool Field3;
        int _Field4;
        public int Field4 { get { return _Field4; } set { if(value < 0 || value > 0x1FFFFFF) throw new ArgumentOutOfRangeException(); _Field4 = value; } }
        int? _Field5;
        public int? Field5 { get { return _Field5; } set { if(value.HasValue && (value.Value < 0 || value.Value > 0xFFFF)) throw new ArgumentOutOfRangeException(); _Field5 = value; } }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = new Vector3D();
            Field1.Parse(buffer);
            Field2 = buffer.ReadFloat32();
            Field3 = buffer.ReadBool();
            Field4 = buffer.ReadInt(25);
            if(buffer.ReadBool())
                Field5 = buffer.ReadInt(16);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            Field1.Encode(buffer);
            buffer.WriteFloat32(Field2);
            buffer.WriteBool(Field3);
            buffer.WriteInt(25, Field4);
            if(Field5.HasValue)
                buffer.WriteInt(16, Field5.Value);
        }

    }

    public class ACDTranslateNormalMessage : GameMessage
    {
        public int Field0;
        public Vector3D Field1;
        public float? Field2;
        public bool? Field3;
        public float? Field4;
        int? _Field5;
        public int? Field5 { get { return _Field5; } set { if(value.HasValue && (value.Value < 0 || value.Value > 0x1FFFFFF)) throw new ArgumentOutOfRangeException(); _Field5 = value; } }
        int? _Field6;
        public int? Field6 { get { return _Field6; } set { if(value.HasValue && (value.Value < -1 || value.Value > 0xFFFFF)) throw new ArgumentOutOfRangeException(); _Field6 = value; } }
        public int? Field7;
        public int? Field8;
        int? _Field9;
        public int? Field9 { get { return _Field9; } set { if(value.HasValue && (value.Value < 0 || value.Value > 0xFFFF)) throw new ArgumentOutOfRangeException(); _Field9 = value; } }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            if(buffer.ReadBool())
            {
                Field1 = new Vector3D();
                Field1.Parse(buffer);
            }
            if(buffer.ReadBool())
                Field2 = buffer.ReadFloat32();
            if(buffer.ReadBool())
                Field3 = buffer.ReadBool();
            if(buffer.ReadBool())
                Field4 = buffer.ReadFloat32();
            if(buffer.ReadBool())
                Field5 = buffer.ReadInt(25);
            if(buffer.ReadBool())
                Field6 = buffer.ReadInt(21) + (-1);
            if(buffer.ReadBool())
                Field7 = buffer.ReadInt(32);
            if(buffer.ReadBool())
                Field8 = buffer.ReadInt(32);
            if(buffer.ReadBool())
                Field9 = buffer.ReadInt(16);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            if(Field1 != null)
            Field1.Encode(buffer);
            if(Field2.HasValue)
                buffer.WriteFloat32(Field2.Value);
            if(Field3.HasValue)
                buffer.WriteBool(Field3.Value);
            if(Field4.HasValue)
                buffer.WriteFloat32(Field4.Value);
            if(Field5.HasValue)
                buffer.WriteInt(25, Field5.Value);
            if(Field6.HasValue)
                buffer.WriteInt(21, Field6.Value - (-1));
            if(Field7.HasValue)
                buffer.WriteInt(32, Field7.Value);
            if(Field8.HasValue)
                buffer.WriteInt(32, Field8.Value);
            if(Field9.HasValue)
                buffer.WriteInt(16, Field9.Value);
        }

    }

    public class HirelingSwapMessage : GameMessage
    {
        public int Field0;

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
        }

    }

    public class AttributesSetValuesMessage : GameMessage
    {
        public int Field0;
        NetAttributeKeyValue[] _atKeyVals;
        public NetAttributeKeyValue[] atKeyVals { get { return _atKeyVals; } set { if(value != null && value.Length > 15) throw new ArgumentOutOfRangeException(); _atKeyVals = value; } }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            atKeyVals = new NetAttributeKeyValue[buffer.ReadInt(4)];
            for(int i = 0;i < _atKeyVals.Length;i++)
            {
                _atKeyVals[i] = new NetAttributeKeyValue();
                _atKeyVals[i].Parse(buffer);
            }
            for (int i = 0; i < atKeyVals.Length; i++) { atKeyVals[i].ParseValue(buffer); };
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(4, _atKeyVals.Length);
            for(int i = 0;i < _atKeyVals.Length;i++) _atKeyVals[i].Encode(buffer);
            for (int i = 0; i < atKeyVals.Length; i++) { atKeyVals[i].EncodeValue(buffer); };
        }

    }

    public class NetAttributeKeyValue
    {
        int? _Field0;
        public int? Field0 { get { return _Field0; } set { if(value.HasValue && (value.Value < 0 || value.Value > 0xFFFFF)) throw new ArgumentOutOfRangeException(); _Field0 = value; } }
        public GameAttribute Field1;

        public float Float; 
        public int Int; 

        public void Parse(GameBitBuffer buffer)
        {
            if(buffer.ReadBool())
                Field0 = buffer.ReadInt(20);
            Field1 = GameAttribute.Attributes[buffer.ReadInt(10)];
        }

        public void Encode(GameBitBuffer buffer)
        {
            if(Field0.HasValue)
                buffer.WriteInt(20, Field0.Value);
            buffer.WriteInt(10, Field1.Id);
        }

        public void ParseValue(GameBitBuffer buffer)
{
    switch (Field1.EncodingType)
    {
        case GameAttributeEncoding.Int:
            Int = buffer.ReadInt(Field1.BitCount);
            break;
        case GameAttributeEncoding.IntMinMax:
            Int = buffer.ReadInt(Field1.BitCount) + Field1.Min;
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

        public void EncodeValue(GameBitBuffer buffer)
{
    switch (Field1.EncodingType)
    {
        case GameAttributeEncoding.Int:
            buffer.WriteInt(Field1.BitCount, Int);
            break;
        case GameAttributeEncoding.IntMinMax:
            buffer.WriteInt(Field1.BitCount, Int - Field1.Min);
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
    }

    public class AttributeSetValueMessage : GameMessage
    {
        public int Field0;
        public NetAttributeKeyValue Field1;

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

    }

    public class NPCInteractOptionsMessage : GameMessage
    {
        public int Field0;
        NPCInteraction[] _tNPCInteraction;
        public NPCInteraction[] tNPCInteraction { get { return _tNPCInteraction; } set { if(value != null && value.Length > 20) throw new ArgumentOutOfRangeException(); _tNPCInteraction = value; } }
        int _Field2;
        public int Field2 { get { return _Field2; } set { if(value < 0 || value > 2) throw new ArgumentOutOfRangeException(); _Field2 = value; } }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            tNPCInteraction = new NPCInteraction[buffer.ReadInt(5)];
            for(int i = 0;i < _tNPCInteraction.Length;i++)
            {
                _tNPCInteraction[i] = new NPCInteraction();
                _tNPCInteraction[i].Parse(buffer);
            }
            Field2 = buffer.ReadInt(2);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(5, _tNPCInteraction.Length);
            for(int i = 0;i < _tNPCInteraction.Length;i++) _tNPCInteraction[i].Encode(buffer);
            buffer.WriteInt(2, Field2);
        }

    }

    public class NPCInteraction
    {
        int _Field0;
        public int Field0 { get { return _Field0; } set { if(value < 0 || value > 8) throw new ArgumentOutOfRangeException(); _Field0 = value; } }
        public int Field1;
        public int Field2;
        int _Field3;
        public int Field3 { get { return _Field3; } set { if(value < 0 || value > 3) throw new ArgumentOutOfRangeException(); _Field3 = value; } }

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

    }

    public class VisualInventoryMessage : GameMessage
    {
        public int Field0;
        public VisualEquipment Field1;

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

    }

    public class VisualEquipment
    {
        VisualItem[] _Field0;
        public VisualItem[] Field0 { get { return _Field0; } set { if(value != null && value.Length != 8) throw new ArgumentOutOfRangeException(); _Field0 = value; } }

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = new VisualItem[8];
            for(int i = 0;i < _Field0.Length;i++)
            {
                _Field0[i] = new VisualItem();
                _Field0[i].Parse(buffer);
            }
        }

        public void Encode(GameBitBuffer buffer)
        {
            for(int i = 0;i < _Field0.Length;i++) _Field0[i].Encode(buffer);
        }

    }

    public class VisualItem
    {
        public int Field0;
        int _Field1;
        public int Field1 { get { return _Field1; } set { if(value < 0 || value > 21) throw new ArgumentOutOfRangeException(); _Field1 = value; } }
        int _Field2;
        public int Field2 { get { return _Field2; } set { if(value < 0 || value > 14) throw new ArgumentOutOfRangeException(); _Field2 = value; } }
        int _Field3;
        public int Field3 { get { return _Field3; } set { if(value < -1 || value > 30) throw new ArgumentOutOfRangeException(); _Field3 = value; } }

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

    }

    public class UnlockDifficultyMessage : GameMessage
    {
        public int Field0;
        public bool Field1;

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

    }

    public class VersionsMessage : GameMessage
    {
        public int Field0;
        public int Field1;
        public string _Field2;
        public string Field2 { get { return _Field2; } set { if(value != null && value.Length > 32) throw new ArgumentOutOfRangeException(); _Field2 = value; } }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
            Field2 = buffer.ReadCharArray(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(32, Field1);
            buffer.WriteCharArray(32, Field2);
        }

    }

    public class LogoutTickTimeMessage : GameMessage
    {
        public bool Field0;
        public int Field1;
        public int Field2;

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

    }

    public class HirelingRequestLearnSkillMessage : GameMessage
    {
        public int Field0;
        public int Field1;

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

    }

    public class PlayErrorSoundMessage : GameMessage
    {
        public int Field0;

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
        }

    }

    public class RequestBuffCancelMessage : GameMessage
    {
        public int Field0;
        public int Field1;

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

    }

    public class GameTestingSamplingStartMessage : GameMessage
    {
        public int Field0;
        public int Field1;

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

    }

    public class SalvageResultsMessage : GameMessage
    {
        public int gbidOriginalItem;
        int _Field1;
        public int Field1 { get { return _Field1; } set { if(value < -1 || value > 10) throw new ArgumentOutOfRangeException(); _Field1 = value; } }
        public int Field2;
        int[] _gbidNewItems;
        public int[] gbidNewItems { get { return _gbidNewItems; } set { if(value != null && value.Length != 10) throw new ArgumentOutOfRangeException(); _gbidNewItems = value; } }

        public override void Parse(GameBitBuffer buffer)
        {
            gbidOriginalItem = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(4) + (-1);
            Field2 = buffer.ReadInt(32);
            gbidNewItems = new int[10];
            for(int i = 0;i < _gbidNewItems.Length;i++) _gbidNewItems[i] = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, gbidOriginalItem);
            buffer.WriteInt(4, Field1 - (-1));
            buffer.WriteInt(32, Field2);
            for(int i = 0;i < _gbidNewItems.Length;i++) buffer.WriteInt(32, _gbidNewItems[i]);
        }

    }

    public class DebugActorTooltipMessage : GameMessage
    {
        public int Field0;
        public string _Field1;
        public string Field1 { get { return _Field1; } set { if(value != null && value.Length > 512) throw new ArgumentOutOfRangeException(); _Field1 = value; } }

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

    }

    public class PlayerWarpedMessage : GameMessage
    {
        int _Field0;
        public int Field0 { get { return _Field0; } set { if(value < 0 || value > 11) throw new ArgumentOutOfRangeException(); _Field0 = value; } }
        public float Field1;

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

    }

    public class PlayCutsceneMessage : GameMessage
    {
        public int Field0;

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
        }

    }

    public class BossZoomMessage : GameMessage
    {
        public float Field0;
        public float Field1;

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

    }

    public class CameraYawMessage : GameMessage
    {
        public float Field0;
        public bool Field1;
        public float Field2;

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

    }

    public class CameraZoomMessage : GameMessage
    {
        public float Field0;
        public bool Field1;
        public float Field2;

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
        public string _Field9;
        public string Field9 { get { return _Field9; } set { if(value != null && value.Length > 128) throw new ArgumentOutOfRangeException(); _Field9 = value; } }

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

    }

    public class CameraFocusMessage : GameMessage
    {
        public int Field0;
        public bool Field1;
        public float Field2;

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

    }

    public class InterstitialMessage : GameMessage
    {
        int _Field0;
        public int Field0 { get { return _Field0; } set { if(value < -1 || value > 5) throw new ArgumentOutOfRangeException(); _Field0 = value; } }
        public bool Field1;

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

    }

    public class ActTransitionMessage : GameMessage
    {
        int _Field0;
        public int Field0 { get { return _Field0; } set { if(value < -1 || value > 1000) throw new ArgumentOutOfRangeException(); _Field0 = value; } }
        public bool Field1;

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

    }

    public class EncounterInviteStateMessage : GameMessage
    {
        public int Field0;
        public int Field1;
        public int Field2;

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

    }

    public class BossEncounterMessage : GameMessage
    {
        public int Field0;
        public int snoEncounter;

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

    }

    public class MapMarkerInfoMessage : GameMessage
    {
        public int Field0;
        public WorldPlace Field1;
        public int Field2;
        public int Field3;
        public int m_snoStringList;
        public int Field5;
        public bool Field6;
        public bool Field7;
        public bool Field8;
        public float Field9;
        public float Field10;
        public float Field11;
        public int Field12;

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = new WorldPlace();
            Field1.Parse(buffer);
            Field2 = buffer.ReadInt(32);
            Field3 = buffer.ReadInt(32);
            m_snoStringList = buffer.ReadInt(32);
            Field5 = buffer.ReadInt(32);
            Field6 = buffer.ReadBool();
            Field7 = buffer.ReadBool();
            Field8 = buffer.ReadBool();
            Field9 = buffer.ReadFloat32();
            Field10 = buffer.ReadFloat32();
            Field11 = buffer.ReadFloat32();
            Field12 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            Field1.Encode(buffer);
            buffer.WriteInt(32, Field2);
            buffer.WriteInt(32, Field3);
            buffer.WriteInt(32, m_snoStringList);
            buffer.WriteInt(32, Field5);
            buffer.WriteBool(Field6);
            buffer.WriteBool(Field7);
            buffer.WriteBool(Field8);
            buffer.WriteFloat32(Field9);
            buffer.WriteFloat32(Field10);
            buffer.WriteFloat32(Field11);
            buffer.WriteInt(32, Field12);
        }

    }

    public class TradeMessage : GameMessage
    {
        int _Field0;
        public int Field0 { get { return _Field0; } set { if(value < 0 || value > 9) throw new ArgumentOutOfRangeException(); _Field0 = value; } }
        public int Field1;
        public int Field2;
        public long Field3;
        public int Field4;
        int[] _Field5;
        public int[] Field5 { get { return _Field5; } set { if(value != null && value.Length != 5) throw new ArgumentOutOfRangeException(); _Field5 = value; } }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(4);
            Field1 = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(32);
            Field3 = buffer.ReadInt64(64);
            Field4 = buffer.ReadInt(32);
            Field5 = new int[5];
            for(int i = 0;i < _Field5.Length;i++) _Field5[i] = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(4, Field0);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(32, Field2);
            buffer.WriteInt64(64, Field3);
            buffer.WriteInt(32, Field4);
            for(int i = 0;i < _Field5.Length;i++) buffer.WriteInt(32, _Field5[i]);
        }

    }

    public class PlayerDeSyncSnapMessage : GameMessage
    {
        public WorldPlace Field0;
        int _Field1;
        public int Field1 { get { return _Field1; } set { if(value < 0 || value > 3) throw new ArgumentOutOfRangeException(); _Field1 = value; } }
        public int Field2;

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = new WorldPlace();
            Field0.Parse(buffer);
            Field1 = buffer.ReadInt(2);
            Field2 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            Field0.Encode(buffer);
            buffer.WriteInt(2, Field1);
            buffer.WriteInt(32, Field2);
        }

    }

    public class PlayerQuestMessage : GameMessage
    {
        int _Field0;
        public int Field0 { get { return _Field0; } set { if(value < -1 || value > 7) throw new ArgumentOutOfRangeException(); _Field0 = value; } }
        public int Field1;

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

    }

    public class LoreMessage : GameMessage
    {
        public int snoLore;

        public override void Parse(GameBitBuffer buffer)
        {
            snoLore = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, snoLore);
        }

    }

    public class TryWaypointMessage : GameMessage
    {
        public int Field0;
        int _Field1;
        public int Field1 { get { return _Field1; } set { if(value < -1 || value > 25) throw new ArgumentOutOfRangeException(); _Field1 = value; } }

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

    }

    public class WaypointActivatedMessage : GameMessage
    {
        public int Field0;
        public int Field1;
        public int Field2;
        public bool Field3;

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(32);
            Field3 = buffer.ReadBool();
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(32, Field2);
            buffer.WriteBool(Field3);
        }

    }

    public class WeatherOverrideMessage : GameMessage
    {
        public int snoWorld;
        public float Field1;
        public float Field2;

        public override void Parse(GameBitBuffer buffer)
        {
            snoWorld = buffer.ReadInt(32);
            Field1 = buffer.ReadFloat32();
            Field2 = buffer.ReadFloat32();
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, snoWorld);
            buffer.WriteFloat32(Field1);
            buffer.WriteFloat32(Field2);
        }

    }

    public class BlizzconCVarsMessage : GameMessage
    {
        public bool Field0;
        public bool Field1;
        public bool Field2;

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

    }

    public class WorldDeletedMessage : GameMessage
    {
        public int Field0;

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
        }

    }

    public class WorldStatusMessage : GameMessage
    {
        public int Field0;
        public bool Field1;

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

    }

    public class RemoveRagdollMessage : GameMessage
    {
        public int Field0;
        public int Field1;

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

    }

    public class FloatingAmountMessage : GameMessage
    {
        public WorldPlace Field0;
        public int Field1;
        public int? Field2;
        int _Field3;
        public int Field3 { get { return _Field3; } set { if(value < 0 || value > 32) throw new ArgumentOutOfRangeException(); _Field3 = value; } }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = new WorldPlace();
            Field0.Parse(buffer);
            Field1 = buffer.ReadInt(32);
            if(buffer.ReadBool())
                Field2 = buffer.ReadInt(32);
            Field3 = buffer.ReadInt(6);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            Field0.Encode(buffer);
            buffer.WriteInt(32, Field1);
            if(Field2.HasValue)
                buffer.WriteInt(32, Field2.Value);
            buffer.WriteInt(6, Field3);
        }

    }

    public class FloatingNumberMessage : GameMessage
    {
        public int Field0;
        public float Field1;
        int _Field2;
        public int Field2 { get { return _Field2; } set { if(value < 0 || value > 32) throw new ArgumentOutOfRangeException(); _Field2 = value; } }

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

    }

    public class SaviorMessage : GameMessage
    {
        public int Field0;
        public int Field1;

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

    }

    public class LowHealthCombatMessage : GameMessage
    {
        public float Field0;
        public int Field1;

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

    }

    public class KillCounterUpdateMessage : GameMessage
    {
        int _Field0;
        public int Field0 { get { return _Field0; } set { if(value < 0 || value > 3) throw new ArgumentOutOfRangeException(); _Field0 = value; } }
        public int Field1;
        public int Field2;
        public bool Field3;

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

    }

    public class ACDLookAtMessage : GameMessage
    {
        public int Field0;
        public int Field1;

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

    }

    public class ReturnPointInfoMessage : GameMessage
    {
        public int snoLevelArea;

        public override void Parse(GameBitBuffer buffer)
        {
            snoLevelArea = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, snoLevelArea);
        }

    }

    public class HearthPortalInfoMessage : GameMessage
    {
        public int snoLevelArea;
        public int Field1;

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

    }

    public class SavePointInfoMessage : GameMessage
    {
        public int snoLevelArea;

        public override void Parse(GameBitBuffer buffer)
        {
            snoLevelArea = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, snoLevelArea);
        }

    }

    public class MapRevealSceneMessage : GameMessage
    {
        public int Field0;
        public int snoScene;
        public PRTransform Field2;
        public int Field3;
        public bool Field4;

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            snoScene = buffer.ReadInt(32);
            Field2 = new PRTransform();
            Field2.Parse(buffer);
            Field3 = buffer.ReadInt(32);
            Field4 = buffer.ReadBool();
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(32, snoScene);
            Field2.Encode(buffer);
            buffer.WriteInt(32, Field3);
            buffer.WriteBool(Field4);
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

    }

    public class Quaternion
    {
        public Vector3D Field0;
        public float Field1;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = new Vector3D();
            Field0.Parse(buffer);
            Field1 = buffer.ReadFloat32();
        }

        public void Encode(GameBitBuffer buffer)
        {
            Field0.Encode(buffer);
            buffer.WriteFloat32(Field1);
        }

    }

    public class DeathFadeTimeMessage : GameMessage
    {
        public int Field0;
        int _Field1;
        public int Field1 { get { return _Field1; } set { if(value < -1 || value > 0x708) throw new ArgumentOutOfRangeException(); _Field1 = value; } }
        int _Field2;
        public int Field2 { get { return _Field2; } set { if(value < 0 || value > 0x708) throw new ArgumentOutOfRangeException(); _Field2 = value; } }
        public bool Field3;

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

    }

    public class RevealTeamMessage : GameMessage
    {
        int _Field0;
        public int Field0 { get { return _Field0; } set { if(value < -1 || value > 22) throw new ArgumentOutOfRangeException(); _Field0 = value; } }
        int _Field1;
        public int Field1 { get { return _Field1; } set { if(value < 0 || value > 3) throw new ArgumentOutOfRangeException(); _Field1 = value; } }
        int _Field2;
        public int Field2 { get { return _Field2; } set { if(value < -1 || value > 1) throw new ArgumentOutOfRangeException(); _Field2 = value; } }

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

    }

    public class HirelingInfoUpdateMessage : GameMessage
    {
        int _Field0;
        public int Field0 { get { return _Field0; } set { if(value < 0 || value > 3) throw new ArgumentOutOfRangeException(); _Field0 = value; } }
        public bool Field1;
        public int Field2;
        int _Field3;
        public int Field3 { get { return _Field3; } set { if(value < 0 || value > 127) throw new ArgumentOutOfRangeException(); _Field3 = value; } }

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

    }

    public class UIElementMessage : GameMessage
    {
        int _Field0;
        public int Field0 { get { return _Field0; } set { if(value < -1 || value > 12) throw new ArgumentOutOfRangeException(); _Field0 = value; } }
        public bool Field1;

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

    }

    public class TrickleMessage : GameMessage
    {
        public int Field0;
        public int Field1;
        public WorldPlace Field2;
        int? _Field3;
        public int? Field3 { get { return _Field3; } set { if(value.HasValue && (value.Value < -1 || value.Value > 7)) throw new ArgumentOutOfRangeException(); _Field3 = value; } }
        public int Field4;
        public float? Field5;
        int _Field6;
        public int Field6 { get { return _Field6; } set { if(value < 0 || value > 11) throw new ArgumentOutOfRangeException(); _Field6 = value; } }
        int _Field7;
        public int Field7 { get { return _Field7; } set { if(value < 0 || value > 63) throw new ArgumentOutOfRangeException(); _Field7 = value; } }
        public int? Field8;
        public int? Field9;
        public int? Field10;
        public int? Field11;
        public int? Field12;
        public float? Field13;
        public float? Field14;
        public bool? Field15;

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
            Field2 = new WorldPlace();
            Field2.Parse(buffer);
            if(buffer.ReadBool())
                Field3 = buffer.ReadInt(4) + (-1);
            Field4 = buffer.ReadInt(32);
            if(buffer.ReadBool())
                Field5 = buffer.ReadFloat32();
            Field6 = buffer.ReadInt(4);
            Field7 = buffer.ReadInt(6);
            if(buffer.ReadBool())
                Field8 = buffer.ReadInt(32);
            if(buffer.ReadBool())
                Field9 = buffer.ReadInt(32);
            if(buffer.ReadBool())
                Field10 = buffer.ReadInt(32);
            if(buffer.ReadBool())
                Field11 = buffer.ReadInt(32);
            if(buffer.ReadBool())
                Field12 = buffer.ReadInt(32);
            if(buffer.ReadBool())
                Field13 = buffer.ReadFloat32();
            if(buffer.ReadBool())
                Field14 = buffer.ReadFloat32();
            if(buffer.ReadBool())
                Field15 = buffer.ReadBool();
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(32, Field1);
            Field2.Encode(buffer);
            if(Field3.HasValue)
                buffer.WriteInt(4, Field3.Value - (-1));
            buffer.WriteInt(32, Field4);
            if(Field5.HasValue)
                buffer.WriteFloat32(Field5.Value);
            buffer.WriteInt(4, Field6);
            buffer.WriteInt(6, Field7);
            if(Field8.HasValue)
                buffer.WriteInt(32, Field8.Value);
            if(Field9.HasValue)
                buffer.WriteInt(32, Field9.Value);
            if(Field10.HasValue)
                buffer.WriteInt(32, Field10.Value);
            if(Field11.HasValue)
                buffer.WriteInt(32, Field11.Value);
            if(Field12.HasValue)
                buffer.WriteInt(32, Field12.Value);
            if(Field13.HasValue)
                buffer.WriteFloat32(Field13.Value);
            if(Field14.HasValue)
                buffer.WriteFloat32(Field14.Value);
            if(Field15.HasValue)
                buffer.WriteBool(Field15.Value);
        }

    }

    public class ACDChangeGBHandleMessage : GameMessage
    {
        public int Field0;
        public GBHandle Field1;

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

    }

    public class GBHandle
    {
        int _Field0;
        public int Field0 { get { return _Field0; } set { if(value < -2 || value > 37) throw new ArgumentOutOfRangeException(); _Field0 = value; } }
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

    }

    public class AimTargetMessage : GameMessage
    {
        public int Field0;
        int _Field1;
        public int Field1 { get { return _Field1; } set { if(value < -1 || value > 3) throw new ArgumentOutOfRangeException(); _Field1 = value; } }
        public int Field2;
        public WorldPlace Field3;

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(3) + (-1);
            Field2 = buffer.ReadInt(32);
            Field3 = new WorldPlace();
            Field3.Parse(buffer);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(3, Field1 - (-1));
            buffer.WriteInt(32, Field2);
            Field3.Encode(buffer);
        }

    }

    public class PlayerLevel : GameMessage
    {
        int _Field0;
        public int Field0 { get { return _Field0; } set { if(value < 0 || value > 7) throw new ArgumentOutOfRangeException(); _Field0 = value; } }
        int _Field1;
        public int Field1 { get { return _Field1; } set { if(value < 0 || value > 127) throw new ArgumentOutOfRangeException(); _Field1 = value; } }

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

    }

    public class ComplexEffectAddMessage : GameMessage
    {
        public int Field0;
        public int Field1;
        public int Field2;
        public int Field3;
        public int Field4;
        public int Field5;
        public int Field6;

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

    }

    public class FlippyMessage : GameMessage
    {
        public int Field0;
        public int Field1;
        public int Field2;
        public Vector3D Field3;

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

    }

    public class PetDetachMessage : GameMessage
    {
        public int Field0;
        public bool Field1;

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

    }

    public class PetMessage : GameMessage
    {
        int _Field0;
        public int Field0 { get { return _Field0; } set { if(value < 0 || value > 7) throw new ArgumentOutOfRangeException(); _Field0 = value; } }
        int _Field1;
        public int Field1 { get { return _Field1; } set { if(value < 0 || value > 31) throw new ArgumentOutOfRangeException(); _Field1 = value; } }
        public int Field2;
        int _Field3;
        public int Field3 { get { return _Field3; } set { if(value < -1 || value > 23) throw new ArgumentOutOfRangeException(); _Field3 = value; } }

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

    }

    public class HelperDetachMessage : GameMessage
    {
        public int Field0;

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
        }

    }

    public class InventoryRequestSocketMessage : GameMessage
    {
        public int Field0;
        public int Field1;

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

    }

    public class InventoryRequestUseMessage : GameMessage
    {
        public int Field0;
        int _Field1;
        public int Field1 { get { return _Field1; } set { if(value < -1 || value > 3) throw new ArgumentOutOfRangeException(); _Field1 = value; } }
        public int Field2;
        public WorldPlace Field3;

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(3) + (-1);
            Field2 = buffer.ReadInt(32);
            Field3 = new WorldPlace();
            Field3.Parse(buffer);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(3, Field1 - (-1));
            buffer.WriteInt(32, Field2);
            Field3.Encode(buffer);
        }

    }

    public class InventoryStackTransferMessage : GameMessage
    {
        public int Field0;
        public int Field1;
        public long Field2;

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

    }

    public class InventorySplitStackMessage : GameMessage
    {
        public int Field0;
        public long Field1;
        public InvLoc Field2;

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

    }

    public class InvLoc
    {
        public int Field0;
        int _Field1;
        public int Field1 { get { return _Field1; } set { if(value < -1 || value > 26) throw new ArgumentOutOfRangeException(); _Field1 = value; } }
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

    }

    public class InventoryDropStackPortionMessage : GameMessage
    {
        public int Field0;
        public long Field1;

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt64(64);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt64(64, Field1);
        }

    }

    public class InventoryRequestMoveMessage : GameMessage
    {
        public int Field0;
        public InvLoc Field1;

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

    }

    public class InventoryRequestQuickMoveMessage : GameMessage
    {
        public int Field0;
        public int Field1;
        int _Field2;
        public int Field2 { get { return _Field2; } set { if(value < -1 || value > 26) throw new ArgumentOutOfRangeException(); _Field2 = value; } }
        public int Field3;
        public int Field4;

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(5) + (-1);
            Field3 = buffer.ReadInt(32);
            Field4 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(5, Field2 - (-1));
            buffer.WriteInt(32, Field3);
            buffer.WriteInt(32, Field4);
        }

    }

    public class KillCountMessage : GameMessage
    {
        int _Field0;
        public int Field0 { get { return _Field0; } set { if(value < 0 || value > 7) throw new ArgumentOutOfRangeException(); _Field0 = value; } }
        public int Field1;
        public int Field2;
        public int Field3;

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

    }

    public class VictimMessage : GameMessage
    {
        int _Field0;
        public int Field0 { get { return _Field0; } set { if(value < 0 || value > 7) throw new ArgumentOutOfRangeException(); _Field0 = value; } }
        int _Field1;
        public int Field1 { get { return _Field1; } set { if(value < 0 || value > 100) throw new ArgumentOutOfRangeException(); _Field1 = value; } }
        int _Field2;
        public int Field2 { get { return _Field2; } set { if(value < -1 || value > 7) throw new ArgumentOutOfRangeException(); _Field2 = value; } }
        int _Field3;
        public int Field3 { get { return _Field3; } set { if(value < -1 || value > 11) throw new ArgumentOutOfRangeException(); _Field3 = value; } }
        public int snoKillerActor;
        int _Field5;
        public int Field5 { get { return _Field5; } set { if(value < -1 || value > 23) throw new ArgumentOutOfRangeException(); _Field5 = value; } }
        int[] _Field6;
        public int[] Field6 { get { return _Field6; } set { if(value != null && value.Length != 2) throw new ArgumentOutOfRangeException(); _Field6 = value; } }
        public int snoPowerDmgSource;

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(3);
            Field1 = buffer.ReadInt(7);
            Field2 = buffer.ReadInt(4) + (-1);
            Field3 = buffer.ReadInt(4) + (-1);
            snoKillerActor = buffer.ReadInt(32);
            Field5 = buffer.ReadInt(5) + (-1);
            Field6 = new int[2];
            for(int i = 0;i < _Field6.Length;i++) _Field6[i] = buffer.ReadInt(32);
            snoPowerDmgSource = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(3, Field0);
            buffer.WriteInt(7, Field1);
            buffer.WriteInt(4, Field2 - (-1));
            buffer.WriteInt(4, Field3 - (-1));
            buffer.WriteInt(32, snoKillerActor);
            buffer.WriteInt(5, Field5 - (-1));
            for(int i = 0;i < _Field6.Length;i++) buffer.WriteInt(32, _Field6[i]);
            buffer.WriteInt(32, snoPowerDmgSource);
        }

    }

    public class VoteKickMessage : GameMessage
    {
        int _Field0;
        public int Field0 { get { return _Field0; } set { if(value < -1 || value > 7) throw new ArgumentOutOfRangeException(); _Field0 = value; } }
        int _Field1;
        public int Field1 { get { return _Field1; } set { if(value < -1 || value > 7) throw new ArgumentOutOfRangeException(); _Field1 = value; } }
        public string _Field2;
        public string Field2 { get { return _Field2; } set { if(value != null && value.Length > 512) throw new ArgumentOutOfRangeException(); _Field2 = value; } }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(4) + (-1);
            Field1 = buffer.ReadInt(4) + (-1);
            Field2 = buffer.ReadCharArray(512);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(4, Field0 - (-1));
            buffer.WriteInt(4, Field1 - (-1));
            buffer.WriteCharArray(512, Field2);
        }

    }

    public class ChatMessage : GameMessage
    {
        int _Field0;
        public int Field0 { get { return _Field0; } set { if(value < 0 || value > 3) throw new ArgumentOutOfRangeException(); _Field0 = value; } }
        int _Field1;
        public int Field1 { get { return _Field1; } set { if(value < -1 || value > 7) throw new ArgumentOutOfRangeException(); _Field1 = value; } }
        public string _Field2;
        public string Field2 { get { return _Field2; } set { if(value != null && value.Length > 512) throw new ArgumentOutOfRangeException(); _Field2 = value; } }

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

    }

    public class TryChatMessage : GameMessage
    {
        int _Field0;
        public int Field0 { get { return _Field0; } set { if(value < 0 || value > 6) throw new ArgumentOutOfRangeException(); _Field0 = value; } }
        int _Field1;
        public int Field1 { get { return _Field1; } set { if(value < -1 || value > 7) throw new ArgumentOutOfRangeException(); _Field1 = value; } }
        public string _Field2;
        public string Field2 { get { return _Field2; } set { if(value != null && value.Length > 512) throw new ArgumentOutOfRangeException(); _Field2 = value; } }

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

    }

    public class TryConsoleCommand : GameMessage
    {
        int _Field0;
        public int Field0 { get { return _Field0; } set { if(value < -1 || value > 7) throw new ArgumentOutOfRangeException(); _Field0 = value; } }
        public string _Field1;
        public string Field1 { get { return _Field1; } set { if(value != null && value.Length > 512) throw new ArgumentOutOfRangeException(); _Field1 = value; } }
        public WorldPlace Field2;
        public int Field3;

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(4) + (-1);
            Field1 = buffer.ReadCharArray(512);
            Field2 = new WorldPlace();
            Field2.Parse(buffer);
            Field3 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(4, Field0 - (-1));
            buffer.WriteCharArray(512, Field1);
            Field2.Encode(buffer);
            buffer.WriteInt(32, Field3);
        }

    }

    public class LoopingAnimationPowerMessage : GameMessage
    {
        public int snoPower;
        public int snoData0;
        public int Field2;

        public override void Parse(GameBitBuffer buffer)
        {
            snoPower = buffer.ReadInt(32);
            snoData0 = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, snoPower);
            buffer.WriteInt(32, snoData0);
            buffer.WriteInt(32, Field2);
        }

    }

    public class SecondaryAnimationPowerMessage : GameMessage
    {
        public int snoPower;
        public AnimPreplayData Field1;

        public override void Parse(GameBitBuffer buffer)
        {
            snoPower = buffer.ReadInt(32);
            if(buffer.ReadBool())
            {
                Field1 = new AnimPreplayData();
                Field1.Parse(buffer);
            }
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, snoPower);
            if(Field1 != null)
            Field1.Encode(buffer);
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

    }

    public class TargetMessage : GameMessage
    {
        int _Field0;
        public int Field0 { get { return _Field0; } set { if(value < -1 || value > 3) throw new ArgumentOutOfRangeException(); _Field0 = value; } }
        public int Field1;
        public WorldPlace Field2;
        public int snoPower;
        public int Field4;
        int _Field5;
        public int Field5 { get { return _Field5; } set { if(value < 0 || value > 2) throw new ArgumentOutOfRangeException(); _Field5 = value; } }
        public AnimPreplayData Field6;

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(3) + (-1);
            Field1 = buffer.ReadInt(32);
            Field2 = new WorldPlace();
            Field2.Parse(buffer);
            snoPower = buffer.ReadInt(32);
            Field4 = buffer.ReadInt(32);
            Field5 = buffer.ReadInt(2);
            if(buffer.ReadBool())
            {
                Field6 = new AnimPreplayData();
                Field6.Parse(buffer);
            }
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(3, Field0 - (-1));
            buffer.WriteInt(32, Field1);
            Field2.Encode(buffer);
            buffer.WriteInt(32, snoPower);
            buffer.WriteInt(32, Field4);
            buffer.WriteInt(2, Field5);
            if(Field6 != null)
            Field6.Encode(buffer);
        }

    }

    public class PlayerActorSetInitialMessage : GameMessage
    {
        public int Field0;
        int _Field1;
        public int Field1 { get { return _Field1; } set { if(value < 0 || value > 7) throw new ArgumentOutOfRangeException(); _Field1 = value; } }

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

    }

    public class ProjectileStickMessage : GameMessage
    {
        public Vector3D Field0;
        public int Field1;
        public int Field2;

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

    }

    public class AffixMessage : GameMessage
    {
        public int Field0;
        int _Field1;
        public int Field1 { get { return _Field1; } set { if(value < 0 || value > 2) throw new ArgumentOutOfRangeException(); _Field1 = value; } }
        int[] _aAffixGBIDs;
        public int[] aAffixGBIDs { get { return _aAffixGBIDs; } set { if(value != null && value.Length > 32) throw new ArgumentOutOfRangeException(); _aAffixGBIDs = value; } }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(2);
            aAffixGBIDs = new int[buffer.ReadInt(6)];
            for(int i = 0;i < _aAffixGBIDs.Length;i++) _aAffixGBIDs[i] = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(2, Field1);
            buffer.WriteInt(6, aAffixGBIDs.Length);
            for(int i = 0;i < _aAffixGBIDs.Length;i++) buffer.WriteInt(32, _aAffixGBIDs[i]);
        }

    }

    public class ACDPickupFailedMessage : GameMessage
    {
        public int Field0;
        int _Field1;
        public int Field1 { get { return _Field1; } set { if(value < 0 || value > 7) throw new ArgumentOutOfRangeException(); _Field1 = value; } }

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

    }

    public class ACDChangeActorMessage : GameMessage
    {
        public int Field0;
        public int Field1;

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

    }

    public class ACDGroupMessage : GameMessage
    {
        public int Field0;
        public int Field1;
        public int Field2;

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

    }

    public class ACDShearMessage : GameMessage
    {
        public int Field0;
        public float Field1;

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

    }

    public class ACDWorldPositionMessage : GameMessage
    {
        public int Field0;
        public WorldLocationMessageData Field1;

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

    }

    public class ACDInventoryUpdateActorSNO : GameMessage
    {
        public int Field0;
        public int Field1;

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

    }

    public class ACDInventoryPositionMessage : GameMessage
    {
        public int Field0;
        public InventoryLocationMessageData Field1;
        public int Field2;

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

    }

    public class InventoryLocationMessageData
    {
        public int Field0;
        int _Field1;
        public int Field1 { get { return _Field1; } set { if(value < -1 || value > 26) throw new ArgumentOutOfRangeException(); _Field1 = value; } }
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

    }

    public class PlayerEnterKnownMessage : GameMessage
    {
        int _Field0;
        public int Field0 { get { return _Field0; } set { if(value < 0 || value > 7) throw new ArgumentOutOfRangeException(); _Field0 = value; } }
        public int Field1;

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

    }

    public class ACDEnterKnownMessage : GameMessage
    {
        public int Field0;
        public int Field1;
        int _Field2;
        public int Field2 { get { return _Field2; } set { if(value < 0 || value > 63) throw new ArgumentOutOfRangeException(); _Field2 = value; } }
        int _Field3;
        public int Field3 { get { return _Field3; } set { if(value < -1 || value > 1) throw new ArgumentOutOfRangeException(); _Field3 = value; } }
        public WorldLocationMessageData Field4;
        public InventoryLocationMessageData Field5;
        public GBHandle Field6;
        public int Field7;
        public int Field8;
        int _Field9;
        public int Field9 { get { return _Field9; } set { if(value < -1 || value > 10) throw new ArgumentOutOfRangeException(); _Field9 = value; } }
        public byte Field10;
        public int? Field11;
        public int? Field12;
        public int? Field13;

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(6);
            Field3 = buffer.ReadInt(2) + (-1);
            if(buffer.ReadBool())
            {
                Field4 = new WorldLocationMessageData();
                Field4.Parse(buffer);
            }
            if(buffer.ReadBool())
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
            if(buffer.ReadBool())
                Field11 = buffer.ReadInt(32);
            if(buffer.ReadBool())
                Field12 = buffer.ReadInt(32);
            if(buffer.ReadBool())
                Field13 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(6, Field2);
            buffer.WriteInt(2, Field3 - (-1));
            if(Field4 != null)
            Field4.Encode(buffer);
            if(Field5 != null)
            Field5.Encode(buffer);
            Field6.Encode(buffer);
            buffer.WriteInt(32, Field7);
            buffer.WriteInt(32, Field8);
            buffer.WriteInt(4, Field9 - (-1));
            buffer.WriteInt(8, Field10);
            if(Field11.HasValue)
                buffer.WriteInt(32, Field11.Value);
            if(Field12.HasValue)
                buffer.WriteInt(32, Field12.Value);
            if(Field13.HasValue)
                buffer.WriteInt(32, Field13.Value);
        }

    }

    public class RevealWorldMessage : GameMessage
    {
        public int Field0;
        public int Field1;
        public int Field2;
        public int Field3;
        public int Field4;
        public int Field5;
        public int Field6;
        public int Field7;

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(32);
            Field3 = buffer.ReadInt(32);
            Field4 = buffer.ReadInt(32);
            Field5 = buffer.ReadInt(32);
            Field6 = buffer.ReadInt(32);
            Field7 = buffer.ReadInt(32);
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
            buffer.WriteInt(32, Field7);
        }

    }

    public class EnterWorldMessage : GameMessage
    {
        public Vector3D Field0;
        public int Field1;
        public int Field2;

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

    }

    public class GameSetupMessage : GameMessage
    {
        public int Field0;

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
        }

    }

    public class ConnectionEstablishedMessage : GameMessage
    {
        int _Field0;
        public int Field0 { get { return _Field0; } set { if(value < 0 || value > 7) throw new ArgumentOutOfRangeException(); _Field0 = value; } }
        public int Field1;
        public int Field2;

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

    }

    public class QuitGameMessage : GameMessage
    {
        int _Field0;
        public int Field0 { get { return _Field0; } set { if(value < 0 || value > 12) throw new ArgumentOutOfRangeException(); _Field0 = value; } }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(4);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(4, Field0);
        }

    }

    public class JoinLANGameMessage : GameMessage
    {
        public int Field0;
        public string _Field1;
        public string Field1 { get { return _Field1; } set { if(value != null && value.Length > 128) throw new ArgumentOutOfRangeException(); _Field1 = value; } }
        public string _Field2;
        public string Field2 { get { return _Field2; } set { if(value != null && value.Length > 49) throw new ArgumentOutOfRangeException(); _Field2 = value; } }
        int _Field3;
        public int Field3 { get { return _Field3; } set { if(value < 2 || value > 17) throw new ArgumentOutOfRangeException(); _Field3 = value; } }

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

    }

    public class ANNDataMessage : GameMessage
    {
        public int Field0;

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
        }

    }

    public class BroadcastTextMessage : GameMessage
    {
        public string _Field0;
        public string Field0 { get { return _Field0; } set { if(value != null && value.Length > 512) throw new ArgumentOutOfRangeException(); _Field0 = value; } }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadCharArray(512);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteCharArray(512, Field0);
        }

    }

    public class DisplayGameTextMessage : GameMessage
    {
        public string _Field0;
        public string Field0 { get { return _Field0; } set { if(value != null && value.Length > 512) throw new ArgumentOutOfRangeException(); _Field0 = value; } }
        public int? Field1;
        public int? Field2;

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadCharArray(512);
            if(buffer.ReadBool())
                Field1 = buffer.ReadInt(32);
            if(buffer.ReadBool())
                Field2 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteCharArray(512, Field0);
            if(Field1.HasValue)
                buffer.WriteInt(32, Field1.Value);
            if(Field2.HasValue)
                buffer.WriteInt(32, Field2.Value);
        }

    }

    public class GBIDDataMessage : GameMessage
    {
        public int Field0;

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
        }

    }

    public class SNONameDataMessage : GameMessage
    {
        public SNOName Field0;

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = new SNOName();
            Field0.Parse(buffer);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            Field0.Encode(buffer);
        }

    }

    public class SNOName
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

    }

    public class SNODataMessage : GameMessage
    {
        public int Field0;

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
        }

    }

    public class UInt64DataMessage : GameMessage
    {
        public long Field0;

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt64(64);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt64(64, Field0);
        }

    }

    public class IntDataMessage : GameMessage
    {
        public int Field0;

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
        }

    }

    public class NetworkAddressMessage : GameMessage
    {
        public int Field0;
        public ushort Field1;

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = (ushort)buffer.ReadInt(16);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(16, Field1);
        }

    }

    public class DWordDataMessage : GameMessage
    {
        public int Field0;

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
        }

    }

    public class DataIDDataMessage : GameMessage
    {
        public int Field0;

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
        }

    }

    public class PlayerIndexMessage : GameMessage
    {
        int _Field0;
        public int Field0 { get { return _Field0; } set { if(value < -1 || value > 7) throw new ArgumentOutOfRangeException(); _Field0 = value; } }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(4) + (-1);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(4, Field0 - (-1));
        }

    }

    public class BoolDataMessage : GameMessage
    {
        public bool Field0;

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadBool();
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteBool(Field0);
        }

    }

    public class GenericBlobMessage : GameMessage
    {
        public byte[] Data;
        public override void Parse(GameBitBuffer buffer)
        {
            Data = buffer.ReadBlob(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteBlob(32, Data);
        }

    }

    public class SimpleMessage : GameMessage
    {

        public override void Parse(GameBitBuffer buffer)
        {
        }

        public override void Encode(GameBitBuffer buffer)
        {
        }

    }

    public class SetIdleAnimationMessage : GameMessage
    {
        public int Field0;
        public int Field1;

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

    }

    public class PlayAnimationMessage : GameMessage
    {
        public int Field0;
        int _Field1;
        public int Field1 { get { return _Field1; } set { if(value < 0 || value > 12) throw new ArgumentOutOfRangeException(); _Field1 = value; } }
        public float Field2;
        PlayAnimationMessageSpec[] _tAnim;
        public PlayAnimationMessageSpec[] tAnim { get { return _tAnim; } set { if(value != null && value.Length > 3) throw new ArgumentOutOfRangeException(); _tAnim = value; } }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(4);
            Field2 = buffer.ReadFloat32();
            tAnim = new PlayAnimationMessageSpec[buffer.ReadInt(2)];
            for(int i = 0;i < _tAnim.Length;i++)
            {
                _tAnim[i] = new PlayAnimationMessageSpec();
                _tAnim[i].Parse(buffer);
            }
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(4, Field1);
            buffer.WriteFloat32(Field2);
            buffer.WriteInt(2, _tAnim.Length);
            for(int i = 0;i < _tAnim.Length;i++) _tAnim[i].Encode(buffer);
        }

    }

    public class PlayAnimationMessageSpec
    {
        public int Field0;
        public int Field1;
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

    }

    public class Message : GameMessage
    {
        public int Field0;
        public int Field1;
        public int Field2;
        public int Field3;
        public float Field4;
        public float Field5;
        public int Field6;

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(32);
            Field3 = buffer.ReadInt(32);
            Field4 = buffer.ReadFloat32();
            Field5 = buffer.ReadFloat32();
            Field6 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(32, Field2);
            buffer.WriteInt(32, Field3);
            buffer.WriteFloat32(Field4);
            buffer.WriteFloat32(Field5);
            buffer.WriteInt(32, Field6);
        }

    }

    public class GoldModifiedMessage : GameMessage
    {
        public bool Field0;
        int _Field1;
        public int Field1 { get { return _Field1; } set { if(value < 0 || value > 2) throw new ArgumentOutOfRangeException(); _Field1 = value; } }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadBool();
            Field1 = buffer.ReadInt(2);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteBool(Field0);
            buffer.WriteInt(2, Field1);
        }

    }

    public class ACDCollFlagsMessage : GameMessage
    {
        public int Field0;
        int _Field1;
        public int Field1 { get { return _Field1; } set { if(value < 0 || value > 0xFFF) throw new ArgumentOutOfRangeException(); _Field1 = value; } }

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

    }

    public class RareItemNameMessage : GameMessage
    {
        public int Field0;
        public RareItemName Field1;

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

    }

    public class RareItemName
    {
        public bool Field0;
        public int snoAffixStringList;
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

    }

    public class RareMonsterNamesMessage : GameMessage
    {
        public int Field0;
        int[] _Field1;
        public int[] Field1 { get { return _Field1; } set { if(value != null && value.Length != 2) throw new ArgumentOutOfRangeException(); _Field1 = value; } }
        int[] _Field2;
        public int[] Field2 { get { return _Field2; } set { if(value != null && value.Length != 8) throw new ArgumentOutOfRangeException(); _Field2 = value; } }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = new int[2];
            for(int i = 0;i < _Field1.Length;i++) _Field1[i] = buffer.ReadInt(32);
            Field2 = new int[8];
            for(int i = 0;i < _Field2.Length;i++) _Field2[i] = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            for(int i = 0;i < _Field1.Length;i++) buffer.WriteInt(32, _Field1[i]);
            for(int i = 0;i < _Field2.Length;i++) buffer.WriteInt(32, _Field2[i]);
        }

    }

    public class LogoutContextMessage : GameMessage
    {
        public bool Field0;

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadBool();
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteBool(Field0);
        }

    }

    public class HeroStateMessage : GameMessage
    {
        public HeroStateData Field0;

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = new HeroStateData();
            Field0.Parse(buffer);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            Field0.Encode(buffer);
        }

    }

    public class HeroStateData
    {
        public int Field0;
        public int Field1;
        public int Field2;
        public int Field3;
        int _Field4;
        public int Field4 { get { return _Field4; } set { if(value < 0 || value > 0x3FFFFFFF) throw new ArgumentOutOfRangeException(); _Field4 = value; } }
        public PlayerSavedData Field5;
        public int Field6;
        PlayerQuestRewardHistoryEntry[] _tQuestRewardHistory;
        public PlayerQuestRewardHistoryEntry[] tQuestRewardHistory { get { return _tQuestRewardHistory; } set { if(value != null && value.Length > 100) throw new ArgumentOutOfRangeException(); _tQuestRewardHistory = value; } }

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(32);
            Field3 = buffer.ReadInt(32);
            Field4 = buffer.ReadInt(30);
            Field5 = new PlayerSavedData();
            Field5.Parse(buffer);
            Field6 = buffer.ReadInt(32);
            tQuestRewardHistory = new PlayerQuestRewardHistoryEntry[buffer.ReadInt(7)];
            for(int i = 0;i < _tQuestRewardHistory.Length;i++)
            {
                _tQuestRewardHistory[i] = new PlayerQuestRewardHistoryEntry();
                _tQuestRewardHistory[i].Parse(buffer);
            }
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(32, Field2);
            buffer.WriteInt(32, Field3);
            buffer.WriteInt(30, Field4);
            Field5.Encode(buffer);
            buffer.WriteInt(32, Field6);
            buffer.WriteInt(7, _tQuestRewardHistory.Length);
            for(int i = 0;i < _tQuestRewardHistory.Length;i++) _tQuestRewardHistory[i].Encode(buffer);
        }

    }

    public class PlayerSavedData
    {
        HotbarButtonData[] _Field0;
        public HotbarButtonData[] Field0 { get { return _Field0; } set { if(value != null && value.Length != 6) throw new ArgumentOutOfRangeException(); _Field0 = value; } }
        public HotbarButtonData Field1;
        public byte Field2;
        public int Field3;
        public int Field4;
        public HirelingSavedData Field5;
        public int Field6;
        public LearnedLore Field7;
        ActiveSkillSavedData[] _Field8;
        public ActiveSkillSavedData[] Field8 { get { return _Field8; } set { if(value != null && value.Length != 6) throw new ArgumentOutOfRangeException(); _Field8 = value; } }
        int[] _snoTraits;
        public int[] snoTraits { get { return _snoTraits; } set { if(value != null && value.Length != 3) throw new ArgumentOutOfRangeException(); _snoTraits = value; } }
        public SavePointData Field10;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = new HotbarButtonData[6];
            for(int i = 0;i < _Field0.Length;i++)
            {
                _Field0[i] = new HotbarButtonData();
                _Field0[i].Parse(buffer);
            }
            Field1 = new HotbarButtonData();
            Field1.Parse(buffer);
            Field2 = (byte)buffer.ReadInt(8);
            Field3 = buffer.ReadInt(32);
            Field4 = buffer.ReadInt(32);
            Field5 = new HirelingSavedData();
            Field5.Parse(buffer);
            Field6 = buffer.ReadInt(32);
            Field7 = new LearnedLore();
            Field7.Parse(buffer);
            Field8 = new ActiveSkillSavedData[6];
            for(int i = 0;i < _Field8.Length;i++)
            {
                _Field8[i] = new ActiveSkillSavedData();
                _Field8[i].Parse(buffer);
            }
            snoTraits = new int[3];
            for(int i = 0;i < _snoTraits.Length;i++) _snoTraits[i] = buffer.ReadInt(32);
            Field10 = new SavePointData();
            Field10.Parse(buffer);
        }

        public void Encode(GameBitBuffer buffer)
        {
            for(int i = 0;i < _Field0.Length;i++) _Field0[i].Encode(buffer);
            Field1.Encode(buffer);
            buffer.WriteInt(8, Field2);
            buffer.WriteInt(32, Field3);
            buffer.WriteInt(32, Field4);
            Field5.Encode(buffer);
            buffer.WriteInt(32, Field6);
            Field7.Encode(buffer);
            for(int i = 0;i < _Field8.Length;i++) _Field8[i].Encode(buffer);
            for(int i = 0;i < _snoTraits.Length;i++) buffer.WriteInt(32, _snoTraits[i]);
            Field10.Encode(buffer);
        }

    }

    public class HotbarButtonData
    {
        public int m_snoPower;
        int _Field1;
        public int Field1 { get { return _Field1; } set { if(value < -1 || value > 4) throw new ArgumentOutOfRangeException(); _Field1 = value; } }
        public int m_gbidItem;

        public void Parse(GameBitBuffer buffer)
        {
            m_snoPower = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(3) + (-1);
            m_gbidItem = buffer.ReadInt(32);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, m_snoPower);
            buffer.WriteInt(3, Field1 - (-1));
            buffer.WriteInt(32, m_gbidItem);
        }

    }

    public class HirelingSavedData
    {
        HirelingInfo[] _Field0;
        public HirelingInfo[] Field0 { get { return _Field0; } set { if(value != null && value.Length != 4) throw new ArgumentOutOfRangeException(); _Field0 = value; } }
        int _Field1;
        public int Field1 { get { return _Field1; } set { if(value < 0 || value > 3) throw new ArgumentOutOfRangeException(); _Field1 = value; } }
        public int Field2;

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = new HirelingInfo[4];
            for(int i = 0;i < _Field0.Length;i++)
            {
                _Field0[i] = new HirelingInfo();
                _Field0[i].Parse(buffer);
            }
            Field1 = buffer.ReadInt(2);
            Field2 = buffer.ReadInt(32);
        }

        public void Encode(GameBitBuffer buffer)
        {
            for(int i = 0;i < _Field0.Length;i++) _Field0[i].Encode(buffer);
            buffer.WriteInt(2, Field1);
            buffer.WriteInt(32, Field2);
        }

    }

    public class HirelingInfo
    {
        int _Field0;
        public int Field0 { get { return _Field0; } set { if(value < 0 || value > 3) throw new ArgumentOutOfRangeException(); _Field0 = value; } }
        public int Field1;
        int _Field2;
        public int Field2 { get { return _Field2; } set { if(value < 0 || value > 127) throw new ArgumentOutOfRangeException(); _Field2 = value; } }
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

    }

    public class LearnedLore
    {
        public int Field0;
        int[] _m_snoLoreLearned;
        public int[] m_snoLoreLearned { get { return _m_snoLoreLearned; } set { if(value != null && value.Length != 256) throw new ArgumentOutOfRangeException(); _m_snoLoreLearned = value; } }

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(32);
            m_snoLoreLearned = new int[256];
            for(int i = 0;i < _m_snoLoreLearned.Length;i++) _m_snoLoreLearned[i] = buffer.ReadInt(32);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            for(int i = 0;i < _m_snoLoreLearned.Length;i++) buffer.WriteInt(32, _m_snoLoreLearned[i]);
        }

    }

    public class ActiveSkillSavedData
    {
        public int snoSkill;
        int _Field1;
        public int Field1 { get { return _Field1; } set { if(value < -1 || value > 4) throw new ArgumentOutOfRangeException(); _Field1 = value; } }

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

    }

    public class SavePointData
    {
        public int snoWorld;
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

    }

    public class PlayerQuestRewardHistoryEntry
    {
        public int snoQuest;
        public int Field1;
        int _Field2;
        public int Field2 { get { return _Field2; } set { if(value < 0 || value > 3) throw new ArgumentOutOfRangeException(); _Field2 = value; } }

        public void Parse(GameBitBuffer buffer)
        {
            snoQuest = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(2);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, snoQuest);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(2, Field2);
        }

    }

    public class PlayerKickTimerMessage : GameMessage
    {
        public int Field0;
        public int Field1;

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

    }

    public class NewPlayerMessage : GameMessage
    {
        int _Field0;
        public int Field0 { get { return _Field0; } set { if(value < 0 || value > 7) throw new ArgumentOutOfRangeException(); _Field0 = value; } }
        public EntityId Field1;
        public EntityId Field2;
        public string _Field3;
        public string Field3 { get { return _Field3; } set { if(value != null && value.Length > 49) throw new ArgumentOutOfRangeException(); _Field3 = value; } }
        int _Field4;
        public int Field4 { get { return _Field4; } set { if(value < -1 || value > 22) throw new ArgumentOutOfRangeException(); _Field4 = value; } }
        int _Field5;
        public int Field5 { get { return _Field5; } set { if(value < -1 || value > 4) throw new ArgumentOutOfRangeException(); _Field5 = value; } }
        public int snoActorPortrait;
        int _Field7;
        public int Field7 { get { return _Field7; } set { if(value < 0 || value > 127) throw new ArgumentOutOfRangeException(); _Field7 = value; } }
        public HeroStateData Field8;
        public bool Field9;
        public int Field10;
        public int Field11;

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(3);
            Field1 = new EntityId();
            Field1.Parse(buffer);
            Field2 = new EntityId();
            Field2.Parse(buffer);
            Field3 = buffer.ReadCharArray(49);
            Field4 = buffer.ReadInt(5) + (-1);
            Field5 = buffer.ReadInt(3) + (-1);
            snoActorPortrait = buffer.ReadInt(32);
            Field7 = buffer.ReadInt(7);
            Field8 = new HeroStateData();
            Field8.Parse(buffer);
            Field9 = buffer.ReadBool();
            Field10 = buffer.ReadInt(32);
            Field11 = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(3, Field0);
            Field1.Encode(buffer);
            Field2.Encode(buffer);
            buffer.WriteCharArray(49, Field3);
            buffer.WriteInt(5, Field4 - (-1));
            buffer.WriteInt(3, Field5 - (-1));
            buffer.WriteInt(32, snoActorPortrait);
            buffer.WriteInt(7, Field7);
            Field8.Encode(buffer);
            buffer.WriteBool(Field9);
            buffer.WriteInt(32, Field10);
            buffer.WriteInt(32, Field11);
        }

    }

    public class SwapSceneMessage : GameMessage
    {
        public int Field0;
        public int Field1;
        public int Field2;

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

    }

    public class DestroySceneMessage : GameMessage
    {
        public int Field0;
        public int Field1;

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

    }

    public class RevealSceneMessage : GameMessage
    {
        public int Field0;
        public SceneSpecification Field1;
        public int Field2;
        public int snoScene;
        public PRTransform Field4;
        public int Field5;
        public int snoSceneGroup;
        int[] _arAppliedLabels;
        public int[] arAppliedLabels { get { return _arAppliedLabels; } set { if(value != null && value.Length > 256) throw new ArgumentOutOfRangeException(); _arAppliedLabels = value; } }

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
            arAppliedLabels = new int[buffer.ReadInt(9)];
            for(int i = 0;i < _arAppliedLabels.Length;i++) _arAppliedLabels[i] = buffer.ReadInt(32);
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
            for(int i = 0;i < _arAppliedLabels.Length;i++) buffer.WriteInt(32, _arAppliedLabels[i]);
        }

    }

    public class SceneSpecification
    {
        public int Field0;
        public IVector2D Field1;
        int[] _arSnoLevelAreas;
        public int[] arSnoLevelAreas { get { return _arSnoLevelAreas; } set { if(value != null && value.Length != 4) throw new ArgumentOutOfRangeException(); _arSnoLevelAreas = value; } }
        public int snoPrevWorld;
        public int Field4;
        public int snoPrevLevelArea;
        public int snoNextWorld;
        public int Field7;
        public int snoNextLevelArea;
        public int snoMusic;
        public int snoCombatMusic;
        public int snoAmbient;
        public int snoReverb;
        public int snoWeather;
        public int snoPresetWorld;
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
            arSnoLevelAreas = new int[4];
            for(int i = 0;i < _arSnoLevelAreas.Length;i++) _arSnoLevelAreas[i] = buffer.ReadInt(32);
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
            for(int i = 0;i < _arSnoLevelAreas.Length;i++) buffer.WriteInt(32, _arSnoLevelAreas[i]);
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

    }

    public class SceneCachedValues
    {
        public int Field0;
        public int Field1;
        public int Field2;
        public AABB Field3;
        public AABB Field4;
        int[] _Field5;
        public int[] Field5 { get { return _Field5; } set { if(value != null && value.Length != 4) throw new ArgumentOutOfRangeException(); _Field5 = value; } }
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
            for(int i = 0;i < _Field5.Length;i++) _Field5[i] = buffer.ReadInt(32);
            Field6 = buffer.ReadInt(32);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, Field0);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(32, Field2);
            Field3.Encode(buffer);
            Field4.Encode(buffer);
            for(int i = 0;i < _Field5.Length;i++) buffer.WriteInt(32, _Field5[i]);
            buffer.WriteInt(32, Field6);
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

    }

    public class AssignTraitsMessage : GameMessage
    {
        int[] _snoPower;
        public int[] snoPower { get { return _snoPower; } set { if(value != null && value.Length != 3) throw new ArgumentOutOfRangeException(); _snoPower = value; } }

        public override void Parse(GameBitBuffer buffer)
        {
            snoPower = new int[3];
            for(int i = 0;i < _snoPower.Length;i++) _snoPower[i] = buffer.ReadInt(32);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            for(int i = 0;i < _snoPower.Length;i++) buffer.WriteInt(32, _snoPower[i]);
        }

    }

    public class UnassignSkillMessage : GameMessage
    {
        int _Field0;
        public int Field0 { get { return _Field0; } set { if(value < 0 || value > 5) throw new ArgumentOutOfRangeException(); _Field0 = value; } }

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(3);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(3, Field0);
        }

    }

    public class AssignSkillMessage : GameMessage
    {
        public int snoPower;
        int _Field1;
        public int Field1 { get { return _Field1; } set { if(value < -1 || value > 4) throw new ArgumentOutOfRangeException(); _Field1 = value; } }
        int _Field2;
        public int Field2 { get { return _Field2; } set { if(value < 0 || value > 5) throw new ArgumentOutOfRangeException(); _Field2 = value; } }

        public override void Parse(GameBitBuffer buffer)
        {
            snoPower = buffer.ReadInt(32);
            Field1 = buffer.ReadInt(3) + (-1);
            Field2 = buffer.ReadInt(3);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(32, snoPower);
            buffer.WriteInt(3, Field1 - (-1));
            buffer.WriteInt(3, Field2);
        }

    }

    public class QuestCounterMessage : GameMessage
    {
        public int snoQuest;
        public int snoLevelArea;
        public int Field2;
        public int Field3;
        public int Field4;
        public int Field5;

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

    }

    public class QuestMeterMessage : GameMessage
    {
        public int snoQuest;
        public int Field1;
        public float Field2;

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

    }

    public class QuestUpdateMessage : GameMessage
    {
        public int snoQuest;
        public int snoLevelArea;
        public int Field2;
        public bool Field3;
        public bool Field4;

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

    }

    public class CrafterLevelUpMessage : GameMessage
    {
        public int Field0;
        public int Field1;
        public int Field2;

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

    }

    public class CraftingResultsMessage : GameMessage
    {
        public int Field0;
        public int Field1;
        public int Field2;

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

    }

    public class EnchantItemMessage : GameMessage
    {
        public int Field0;
        public int Field1;

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

    }

    public class GameIdMessage : GameMessage
    {
        public GameId Field0;

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = new GameId();
            Field0.Parse(buffer);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            Field0.Encode(buffer);
        }

    }

    public class EntityIdMessage : GameMessage
    {
        public EntityId Field0;

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = new EntityId();
            Field0.Parse(buffer);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            Field0.Encode(buffer);
        }

    }

    public class GameSyncedDataMessage : GameMessage
    {
        public GameSyncedData Field0;

        public override void Parse(GameBitBuffer buffer)
        {
            Field0 = new GameSyncedData();
            Field0.Parse(buffer);
        }

        public override void Encode(GameBitBuffer buffer)
        {
            Field0.Encode(buffer);
        }

    }

    public class GameSyncedData
    {
        int _Field0;
        public int Field0 { get { return _Field0; } set { if(value < 0 || value > 3) throw new ArgumentOutOfRangeException(); _Field0 = value; } }
        public int Field1;
        public int Field2;
        public int Field3;
        public int Field4;
        public int Field5;
        public int Field6;
        int[] _Field7;
        public int[] Field7 { get { return _Field7; } set { if(value != null && value.Length != 2) throw new ArgumentOutOfRangeException(); _Field7 = value; } }
        int[] _Field8;
        public int[] Field8 { get { return _Field8; } set { if(value != null && value.Length != 2) throw new ArgumentOutOfRangeException(); _Field8 = value; } }

        public void Parse(GameBitBuffer buffer)
        {
            Field0 = buffer.ReadInt(2);
            Field1 = buffer.ReadInt(32);
            Field2 = buffer.ReadInt(32);
            Field3 = buffer.ReadInt(32);
            Field4 = buffer.ReadInt(32);
            Field5 = buffer.ReadInt(32);
            Field6 = buffer.ReadInt(32);
            Field7 = new int[2];
            for(int i = 0;i < _Field7.Length;i++) _Field7[i] = buffer.ReadInt(32);
            Field8 = new int[2];
            for(int i = 0;i < _Field8.Length;i++) _Field8[i] = buffer.ReadInt(32);
        }

        public void Encode(GameBitBuffer buffer)
        {
            buffer.WriteInt(2, Field0);
            buffer.WriteInt(32, Field1);
            buffer.WriteInt(32, Field2);
            buffer.WriteInt(32, Field3);
            buffer.WriteInt(32, Field4);
            buffer.WriteInt(32, Field5);
            buffer.WriteInt(32, Field6);
            for(int i = 0;i < _Field7.Length;i++) buffer.WriteInt(32, _Field7[i]);
            for(int i = 0;i < _Field8.Length;i++) buffer.WriteInt(32, _Field8[i]);
        }

    }

