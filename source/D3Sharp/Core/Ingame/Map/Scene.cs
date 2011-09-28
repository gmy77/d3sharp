using D3Sharp.Core.Common.Toons;
using D3Sharp.Core.Ingame.Universe;
using D3Sharp.Net.Game.Message.Definitions.Scene;
using D3Sharp.Net.Game.Message.Definitions.Map;

namespace D3Sharp.Core.Ingame.Map
{
    sealed public class Scene
    {
        public int ID;
        public RevealSceneMessage SceneData;
        public MapRevealSceneMessage Map;

        public void Reveal(IngameToon t)
        {
            if (SceneData != null) t.InGameClient.SendMessage(SceneData);
            if (Map != null) t.InGameClient.SendMessage(Map);
            t.InGameClient.FlushOutgoingBuffer();
        }

        public void Destroy(IngameToon t)
        {
            if (SceneData != null)
            {
                t.InGameClient.SendMessage(new DestroySceneMessage() { Id=0x35, Field0 = SceneData.WorldID, Field1 = ID });
                t.InGameClient.FlushOutgoingBuffer();
            }
        }

    }
}
