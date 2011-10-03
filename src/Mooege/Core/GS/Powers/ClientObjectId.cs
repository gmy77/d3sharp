using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mooege.Net.GS;

namespace Mooege.Core.GS.Powers
{
    // simple open object that allows one to track what client an object id was created on
    public class ClientObjectId
    {
        public GameClient client;
        public int id;

        public static ClientObjectId GenerateNewId(GameClient client)
        {
            return new ClientObjectId()
            {
                client = client,
                id = client.Universe.NextObjectId
            };
        }
    }
}
