using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mooege.Common.Helpers;
using Mooege.Core.GS.Map;
using Mooege.Net.GS.Message;
using Mooege.Net.GS.Message.Definitions.Tick;
using Mooege.Net.GS.Message.Definitions.World;
using Mooege.Net.GS.Message.Fields;
using Mooege.Net.GS.Message.Definitions.Animation;
using Mooege.Net.GS.Message.Definitions.Effect;
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
