using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace D3Sharp.Net.Game.Message
{
    public interface IMessageConsumer
    {
        void Consume(GameClient client, GameMessage message);
    }

    public enum Consumers
    {
        None,
        Universe,
        PlayerManager,
        Skillset
    }
}
