using Google.ProtocolBuffers;

namespace D3Sharp.Core.Toons
{
    public class ToonHandleHelper
    {
        public ulong ID { get; private set; }
        public uint Program { get; private set; }
        public uint Region { get; private set; }
        public uint Realm { get; private set; }

        public ToonHandleHelper(ulong id)
        {
            this.ID = id;
            this.Program = 00004433;
            this.Region = 62;
            this.Realm = 01;
        }

        public ToonHandleHelper(D3.OnlineService.EntityId entityID)
        {
            var stream = CodedInputStream.CreateInstance(entityID.ToByteArray());
            this.ID = stream.ReadUInt64();
            this.Program = stream.ReadUInt32();
            this.Region = stream.ReadRawVarint32();
            this.Realm = stream.ReadRawVarint32();
        }

        public D3.OnlineService.EntityId ToD3EntityID()
        {
            return D3.OnlineService.EntityId.CreateBuilder().SetIdHigh(216174302532224051).SetIdLow(this.ID).Build();
        }

        public bnet.protocol.EntityId ToBnetEntityID()
        {
            return bnet.protocol.EntityId.CreateBuilder().SetHigh(216174302532224051).SetLow(this.ID).Build();
        }
    }
}
