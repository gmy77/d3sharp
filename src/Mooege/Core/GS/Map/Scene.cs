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

        public void Reveal(Hero hero)
        {
            if (hero.RevealedScenes.Contains(this)) return; //already revealed

            if (SceneData != null)
            {
                hero.InGameClient.SendMessage(SceneData);
                hero.RevealedScenes.Add(this);
            }
            if (Map != null) hero.InGameClient.SendMessage(Map);
            hero.InGameClient.FlushOutgoingBuffer();
        }

        public void Destroy(Hero hero)
        {
            if (!hero.RevealedScenes.Contains(this)) return; //not revealed yet
            if (SceneData != null)
            {
                hero.InGameClient.SendMessage(new DestroySceneMessage() { Id = 0x35, Field0 = SceneData.WorldID, Field1 = ID });
                hero.InGameClient.FlushOutgoingBuffer();
                hero.RevealedScenes.Remove(this);
            }
        }

    }
}
