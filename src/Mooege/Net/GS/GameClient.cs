/*
 * Copyright (C) 2011 - 2012 mooege project - http://www.mooege.org
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
using Mooege.Common.Logging;
using Mooege.Core.GS.Games;
using Mooege.Core.GS.Players;
using Mooege.Net.GS.Message;
using Mooege.Net.GS.Message.Definitions.Tick;
using Mooege.Net.MooNet;

namespace Mooege.Net.GS
{
    public sealed class GameClient : IClient
    {
        private static readonly Logger Logger = LogManager.CreateLogger();

        public IConnection Connection { get; set; }
        public MooNetClient BnetClient { get; set; }

        private readonly GameBitBuffer _incomingBuffer = new GameBitBuffer(512);
        private readonly GameBitBuffer _outgoingBuffer = new GameBitBuffer(ushort.MaxValue);

        public Game Game { get; set; }
        public Player Player { get; set; }

        public bool TickingEnabled { get; set; }

        private object _bufferLock = new object(); // we should be locking on this private object, locking on gameclient (this) may cause deadlocks. detailed information: http://msdn.microsoft.com/fr-fr/magazine/cc188793%28en-us%29.aspx /raist.

        public bool IsLoggingOut;

        public GameClient(IConnection connection)
        {
            this.TickingEnabled = false;
            this.Connection = connection;
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
                    var message = _incomingBuffer.ParseMessage();
                    if (message == null) continue;
                    try
                    {
                        Logger.LogIncomingPacket(message); // change ConsoleTarget's level to Level.Dump in program.cs if u want to see messages on console.

                        if (message.Consumer != Consumers.None)
                        {
                            if (message.Consumer == Consumers.ClientManager) ClientManager.Instance.Consume(this, message); // Client should be greeted by ClientManager and sent initial game-setup messages.
                            else this.Game.Route(this, message);
                        }

                        else if (message is ISelfHandler) (message as ISelfHandler).Handle(this); // if message is able to handle itself, let it do so.
                        else Logger.Warn("{0} - ID:{1} has no consumer or self-handler.", message.GetType(), message.Id);

                    }
                    catch (NotImplementedException)
                    {
                        Logger.Warn("Unhandled game message: 0x{0:X4} {1}", message.Id, message.GetType().Name);
                    }
                }

                _incomingBuffer.Position = end;
            }
            _incomingBuffer.ConsumeData();
        }

        public void SendMessage(GameMessage message, bool flushImmediatly = false)
        {
            lock (this._bufferLock)
            {
                Logger.LogOutgoingPacket(message);
                _outgoingBuffer.EncodeMessage(message); // change ConsoleTarget's level to Level.Dump in program.cs if u want to see messages on console.
                if (flushImmediatly) this.SendTick();
            }
        }

        public void SendTick()
        {
            lock (this._bufferLock)
            {
                if (_outgoingBuffer.Length <= 32) return;

                if (this.TickingEnabled) this.SendMessage(new GameTickMessage(this.Game.TickCounter)); // send the tick.
                this.FlushOutgoingBuffer();
            }
        }

        private void FlushOutgoingBuffer()
        {
            lock (this._bufferLock)
            {
                if (_outgoingBuffer.Length <= 32) return;

                var data = _outgoingBuffer.GetPacketAndReset();
                Connection.Send(data);
            }
        }
    }
}
