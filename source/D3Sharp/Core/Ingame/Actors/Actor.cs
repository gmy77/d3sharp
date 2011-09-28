using D3Sharp.Core.Common.Toons;
using D3Sharp.Core.Ingame.Universe;
using D3Sharp.Net.Game.Message.Definitions.ACD;
using D3Sharp.Net.Game.Message.Definitions.Misc;

namespace D3Sharp.Core.Ingame.Actors
{
    public class Actor
    {
        public int ID;
        public int snoID;
        public float PosX, PosY, PosZ;
        public ACDEnterKnownMessage RevealMessage;

        public void Reveal(IngameToon t)
        {
            if (RevealMessage != null) t.InGameClient.SendMessage(RevealMessage);
            t.InGameClient.FlushOutgoingBuffer();
        }

        public void Destroy(Toon t)
        {
            if (RevealMessage != null)
            {
                t.Owner.LoggedInBNetClient.InGameClient.SendMessage(new ANNDataMessage() { Id=0x3c, Field0=ID, });
                t.Owner.LoggedInBNetClient.InGameClient.FlushOutgoingBuffer();
            }
        }
    }
}
