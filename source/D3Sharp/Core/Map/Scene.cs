using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D3Sharp.Net.Game.Messages.Scene;
using D3Sharp.Net.Game.Messages.Map;
using D3Sharp.Core.Toons;

namespace D3Sharp.Core.Map
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
            if (SceneData != null) t.client.SendMessage(SceneData);
            if (Map != null) t.client.SendMessage(Map);
            t.client.FlushOutgoingBuffer();
        }
    }
}
