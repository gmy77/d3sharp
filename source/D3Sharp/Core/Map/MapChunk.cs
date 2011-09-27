using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D3Sharp.Net.Game;
using D3Sharp.Net.Game.Messages.Map;
using D3Sharp.Net.Game.Messages.Scene;

namespace D3Sharp.Core.Map
{
    sealed public class MapChunk
    {
        public RevealSceneMessage Scene;
        public MapRevealSceneMessage Map;
        public string SceneLine;
        public string MapLine;
        public int originalsortorder;
    }
}
