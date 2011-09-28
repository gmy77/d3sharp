using D3Sharp.Core.Common.Toons;
using D3Sharp.Net.Game.Message.Definitions.ACD;

namespace D3Sharp.Core.Ingame.Actors
{
    public class Actor
    {
        public int ID;
        public int snoID;
        public float PosX, PosY, PosZ;
        public ACDEnterKnownMessage RevealMessage;
        public string ActorLine;

        public void Reveal(Toon t)
        {
            if (RevealMessage != null) t.Owner.LoggedInBNetClient.InGameClient.SendMessage(RevealMessage);
            t.Owner.LoggedInBNetClient.InGameClient.FlushOutgoingBuffer();
        }
    }
}
