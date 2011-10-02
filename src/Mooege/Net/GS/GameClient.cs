/*
 * Copyright (C) 2011 mooege project
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */

using System;
using System.Linq;
using System.Collections.Generic;
using Mooege.Common;
using Mooege.Core.GS.Universe;
using Mooege.Net.GS.Message;
using Mooege.Net.MooNet;
using Mooege.Core.Common.Items;

namespace Mooege.Net.GS
{
    public sealed class GameClient : IGameClient
    {
        static readonly Logger Logger = LogManager.CreateLogger();

        public IConnection Connection { get; set; }
        public MooNetClient BnetClient { get; set; }

        private readonly GameBitBuffer _incomingBuffer = new GameBitBuffer(512);
        private readonly GameBitBuffer _outgoingBuffer = new GameBitBuffer(ushort.MaxValue);

        // for some testing
        public Dictionary<int, Item> items = new Dictionary<int, Item>();  // array of items without specific place in inventory
        //
        public Universe Universe;    
        public Player Player { get; set; }
        public int PacketId = 0x227 + 20;
        public int Tick = 0;
        public int ObjectId = 0x78f50114 + 100;
        public IList<int> ObjectIdsSpawned = null;

        public bool IsLoggingOut;

        public GameClient(IConnection connection, Universe universe)
        {            
            this.Connection = connection;
            this.Universe = universe;
            _outgoingBuffer.WriteInt(32, 0);
        }

        public void Parse(ConnectionDataEventArgs e)
        {
            //Console.WriteLine(e.Data.Dump());

            _incomingBuffer.AppendData(e.Data.ToArray());

            while (_incomingBuffer.IsPacketAvailable())
            {
                int end = _incomingBuffer.Position;
                end += _incomingBuffer.ReadInt(32) * 8;

                while ((end - _incomingBuffer.Position) >= 9)
                {
                    GameMessage message = _incomingBuffer.ParseMessage();
                    if (message == null) continue;
                    try
                    {
                        if (message.Consumer != Consumers.None) this.Universe.Route(this, message);
                        else if (message is ISelfHandler) (message as ISelfHandler).Handle(this); // if message is able to handle itself, let it do so.
                        else Logger.Debug("{0} has no consumer or self-handler.", message.GetType());

                        //Logger.LogIncoming(message);
                    }
                    catch (NotImplementedException)
                    {
                        Logger.Debug("Unhandled game message: 0x{0:X4} {1}", message.Id, message.GetType().Name);
                    }
                }

                _incomingBuffer.Position = end;
            }
            _incomingBuffer.ConsumeData();
            FlushOutgoingBuffer();
        }

        public void SendMessage(GameMessage message)
        {
            //Logger.LogOutgoing(message);
            _outgoingBuffer.EncodeMessage(message);
        }

        public void SendMessageNow(GameMessage message)
        {
            SendMessage(message);
            FlushOutgoingBuffer();
        }

        public void FlushOutgoingBuffer()
        {
            if (_outgoingBuffer.Length > 32)
            {
                var data = _outgoingBuffer.GetPacketAndReset();
                Connection.Send(data);
            }
        }
    }
}
