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

using Mooege.Net.GS.Message;
using Mooege.Net.GS.Message.Definitions.Misc;
using Mooege.Core.GS.Powers;

namespace Mooege.Core.GS.Actors.Buffs
{
    //buff that changes single boolean attribute
    public class BooleanAttributeBuff : TimedBuff
    {
        GameAttributeB _attribute = null;
        public GameAttributeB Attribute { get { return _attribute; } }
        FloatingNumberMessage.FloatType? _message;

        public BooleanAttributeBuff(TickTimer timeout,
                                   GameAttributeB attribute,
                                   FloatingNumberMessage.FloatType? message = null)
            : base(timeout) 
        {
            _attribute = attribute;
            _message = message;
        }

        void SendFloatingMessage(FloatingNumberMessage.FloatType type)
        {
            Target.World.BroadcastIfRevealed(new FloatingNumberMessage
            {
                Id = 0xd0,
                ActorID = Target.DynamicID,
                Type = type
            }, Target);
        }

        public override void Apply()
        {
            Target.Attributes[_attribute] = true;

            GameAttributeMap map = new GameAttributeMap();
            map[_attribute] = true;
            foreach (var message in map.GetMessageList(Target.DynamicID))
                Target.World.BroadcastIfRevealed(message, Target);

            if (_message.HasValue)
                SendFloatingMessage(_message.Value);
        }

        public override void Remove()
        {
            Target.Attributes[_attribute] = false;

            GameAttributeMap map = new GameAttributeMap();
            map[_attribute] = false;
            foreach (var message in map.GetMessageList(Target.DynamicID))
                Target.World.BroadcastIfRevealed(message, Target);
        }
    }
}
