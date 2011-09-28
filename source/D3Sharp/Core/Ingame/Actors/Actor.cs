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
            if (t.RevealedActors.Contains(this)) return; //already revealed

            if (RevealMessage != null)
            {
                t.InGameClient.SendMessage(RevealMessage);
                t.RevealedActors.Add(this);
            }
            t.InGameClient.FlushOutgoingBuffer();
        }

        public void Destroy(IngameToon t)
        {
            if (!t.RevealedActors.Contains(this)) return; //not revealed yet

            if (RevealMessage != null)
            {
                t.InGameClient.SendMessage(new ANNDataMessage() { Id=0x3c, Field0=ID, });
                t.InGameClient.FlushOutgoingBuffer();
                t.RevealedActors.Remove(this);
            }
        }
    }
}
