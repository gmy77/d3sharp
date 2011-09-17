using System.Linq;
using System.IO;
using D3Sharp.Net;
using D3Sharp.Net.Packets;
using D3Sharp.Utils.Extensions;
using D3Sharp.Core.Storage;
using Google.ProtocolBuffers;

namespace D3Sharp.Core.Services
{
    [Service(serviceID: 0x2, serverHash: 0x4124C31B, clientHash: 0x0)]
    public class ToonExternalService : Service
    {
        [ServiceMethod(0x1)]
        public void ToonListRequest(IClient client, Packet packetIn)
        {            
            Logger.Trace("RPC:ToonExternal:ToonListRequest()");
            var response = bnet.protocol.toon.external.ToonListResponse.CreateBuilder().Build();

            var packet = new Packet(
                new Header(0xfe, 0x0, packetIn.Header.RequestID, (uint)response.SerializedSize),
                response.ToByteArray());

            client.Send(packet);
        }

        [ServiceMethod(0x2)]
        public void SelectToonRequest(IClient client, Packet packetIn)
        {
            Logger.Trace("RPC:ToonExternal:SelectToonRequest()");
            //var request = bnet.protocol.toon.external.SelectToonRequest.ParseFrom(packetIn.Payload.ToArray());
            var response = bnet.protocol.toon.external.SelectToonResponse.CreateBuilder().Build();

            var packet = new Packet(
                new Header(0xfe, 0x0, packetIn.Header.RequestID, (uint)response.SerializedSize),
                response.ToByteArray());

            client.Send(packet);
        }

        [ServiceMethod(0x3)]
        public void CreateToonRequest(IClient client, Packet packetIn)
        {
            Logger.Trace("RPC:ToonExternal:CreateToonRequest()");
            var request = bnet.protocol.toon.external.CreateToonRequest.ParseFrom(packetIn.Payload.ToArray());
            var hcp = D3.OnlineService.HeroCreateParams.ParseFrom(request.AttributeList[0].Value.MessageValue);
            
            ulong eid_high=0x0300016200004433; // ToonHandle
            ulong eid_low=0xFFFFFFFFFFFFFFFF; // Actual id?

            var equipment = D3.Hero.VisualEquipment.CreateBuilder().Build();
            var heroDigest = D3.Hero.Digest.CreateBuilder().SetVersion(1)
                .SetHeroId(D3.OnlineService.EntityId.CreateBuilder().SetIdHigh(eid_high).SetIdLow(eid_low).Build())
                .SetHeroName(request.Name)
                .SetGbidClass(hcp.GbidClass)
                .SetPlayerFlags(hcp.IsFemale ? (uint)0x2000002 : 0x00)
                .SetLevel(1)
                .SetVisualEquipment(equipment)
                //.SetQuestHistory(0, questhistory)
                .SetLastPlayedAct(0)
                .SetHighestUnlockedAct(0)
                .SetLastPlayedDifficulty(0)
                .SetHighestUnlockedDifficulty(0)
                .SetLastPlayedQuest(1)
                .SetLastPlayedQuestStep(1)
                .SetTimePlayed(0)
                .Build();
            
            // Can't seem to figure out how to get the right format for the ColumnId
            var eid=/*D3.OnlineService*/bnet.protocol.EntityId.CreateBuilder().SetHigh(eid_high).SetLow(eid_low).Build();
            byte[] eid_bytes;
            using (var stream = new MemoryStream()) {
                var output = CodedOutputStream.CreateInstance(stream);
                output.WriteUInt64NoTag(eid.High);
                output.WriteUInt64NoTag(eid.Low);
                output.Flush();
                eid_bytes=stream.ToArray();
            }
            // In this we have the tags, which are not in the ColumnId, and eid.High is coded backwards (otherwise this would be correct)
            Logger.Debug("D3OS EID bytes:\n{0}", eid.ToByteArray().Dump());
            // And in the raw format, the data for eid.High is never even close to what it should be
            Logger.Debug("D3OS EID raw:\n{0}", eid_bytes.Dump());
            
            // Hard coding eid as ColumnId for certain form
            var colid = bnet.protocol.storage.ColumnId.CreateBuilder()
                .SetHash(ByteString.CopyFrom(new byte[] {0xA1, 0x81, 0xA8, 0x35, 0x68, 0x24, 0x41, 0x60, 0x09, 0x7C, 0x05, 0x1B, 0x11, 0xA8, 0x7F, 0x04}))
                .Build();
            
            var cell = new Cell(colid, heroDigest.ToByteString());
            // Remove cell if it already exists
            StorageManager.Tables[StorageManager.ToonTable.Id].Rows[StorageManager.CharacterRow.Id].Cells.Remove(colid);
            StorageManager.Tables[StorageManager.ToonTable.Id].Rows[StorageManager.CharacterRow.Id].AddCell(cell);
            
            var response = bnet.protocol.toon.external.CreateToonResponse.CreateBuilder()
                .SetToon(bnet.protocol.EntityId.CreateBuilder().SetHigh(eid_high).SetLow(eid_low))
                .Build();

            var packet = new Packet(
                new Header(0xfe, 0x0, packetIn.Header.RequestID, (uint)response.SerializedSize),
                response.ToByteArray());

            client.Send(packet);
        }
    }
}
