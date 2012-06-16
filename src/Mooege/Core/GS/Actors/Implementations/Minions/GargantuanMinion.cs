using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mooege.Core.GS.Powers;
using Mooege.Core.GS.AI.Brains;
using Mooege.Net.GS.Message;
using Mooege.Core.GS.Ticker;
using Mooege.Core.GS.Players;
using Mooege.Net.GS.Message.Definitions.Pet;

namespace Mooege.Core.GS.Actors.Implementations.Minions
{
    class GargantuanMinion : Minion
    {
        public GargantuanMinion(Map.World world, PowerContext context, int GargID)
            : base(world, 122305, context.User, null)
        {
            Scale = 1f;
            //TODO: get a proper value for this.
            this.WalkSpeed *= 5;
            SetBrain(new MinionBrain(this));

            (Brain as MinionBrain).AddPresetPower(30005);
            (Brain as MinionBrain).AddPresetPower(30001);
            (Brain as MinionBrain).AddPresetPower(30592);
            (Brain as MinionBrain).AddPresetPower(30550);
            Attributes[GameAttribute.Hitpoints_Max] = 5f;
            Attributes[GameAttribute.Hitpoints_Cur] = 5f;
            Attributes[GameAttribute.Attacks_Per_Second] = 1.0f;


            Attributes[GameAttribute.Damage_Weapon_Min, 0] = 5f;
            Attributes[GameAttribute.Damage_Weapon_Delta, 0] = 7f;

            Attributes[GameAttribute.Pet_Type] = 0x8;
            //Pet_Owner and Pet_Creator seems to be 0
            (context.User as Player).InGameClient.SendMessage(new PetMessage()
            {
                Field0 = 0,
                Field1 = GargID,
                PetId = this.DynamicID,
                Field3 = 0x8,
            });
        }
    }
}
