using D3Sharp.Core.Common.Toons;
using D3Sharp.Net.Game.Message.Definitions.Scene;
using D3Sharp.Net.Game.Message.Definitions.Map;

namespace D3Sharp.Core.Ingame.Map
{
    sealed public class Scene
    {
        public int ID;
        public RevealSceneMessage SceneData;
        public MapRevealSceneMessage Map;
        public string SceneLine;
        public string MapLine;

        public void Reveal(Toon t)
        {
            if (SceneData != null) t.Owner.LoggedInBNetClient.InGameClient.SendMessage(SceneData);
            if (Map != null) t.Owner.LoggedInBNetClient.InGameClient.SendMessage(Map);
            t.Owner.LoggedInBNetClient.InGameClient.FlushOutgoingBuffer();
        }
    }
}
