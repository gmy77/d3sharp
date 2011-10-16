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

namespace Mooege.Core.GS.Actors.Buffs
{
    //buff that changes singe boolean attribute
    public class BooleanPropertyBuff : TimedBuff
    {
        public BooleanPropertyBuff(float duration, Actor target, GameAttributeB attribute, FloatingNumberMessage.FloatType? start_msg = null, FloatingNumberMessage.FloatType? end_msg = null) 
            : base(duration, target) 
        {
            _attribute = attribute;
            _startFloatingMessage = start_msg;
            _endFloatingMessage = end_msg;
        }

        GameAttributeB _attribute = null;

        public GameAttributeB Attribute { get { return _attribute; } }

        FloatingNumberMessage.FloatType? _startFloatingMessage = null;
        FloatingNumberMessage.FloatType? _endFloatingMessage = null;

        void SendFloatingMessage(FloatingNumberMessage.FloatType? type, int number = 0)
        {
            if (type != null)
            {
                Target.World.BroadcastIfRevealed(new FloatingNumberMessage
                {
                    Id = 0xd0,
                    ActorID = Target.DynamicID,
                    Number = 0,
                    Type = FloatingNumberMessage.FloatType.Frozen
                }, Target);
            }
        }

        public override void Apply()
        {
            base.Apply();
            Target.setAttribute(_attribute, new GameAttributeValue(1));
            //Target.Attributes[_attribute] = true;

            SendFloatingMessage(_startFloatingMessage);
        }

        public override void Remove()
        {
            base.Remove();
            Target.setAttribute(_attribute, new GameAttributeValue(0));
            //Target.Attributes[_attribute] = false;

            SendFloatingMessage(_endFloatingMessage);
        }
    }
}
