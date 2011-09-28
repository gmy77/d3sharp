using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using D3Sharp.Core.Common.Toons;
using D3Sharp.Net.Game;
using D3Sharp.Net.Game.Message.Fields;

namespace D3Sharp.Core.Ingame.Universe
{
    public class Player
    {
        public GameClient Client { get; set; }
        public Toon Toon { get; set; }

        //this is the main universe reference to handle most of the player-game interactions and manage game state
        public Universe Universe;        

        public Player(Universe universe)
        {
            this.Universe = universe;
        }
    }
}
