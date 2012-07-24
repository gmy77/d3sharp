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
    class MysticAllyMinion : Minion
    {
        //female ->123885
        //male -> 169904

        public MysticAllyMinion(Map.World world, PowerContext context, int MysticAllyID)
            : base(world, 123885, context.User, null)
        {
            Scale = 1.35f; //they look cooler bigger :)
            //TODO: get a proper value for this.
            this.WalkSpeed *= 5;
            SetBrain(new MinionBrain(this));
            (Brain as MinionBrain).AddPresetPower(169081); //melee_instant
            (Brain as MinionBrain).AddPresetPower(169155); //Rune_aKick
            (Brain as MinionBrain).AddPresetPower(169325); //Rune_bWaveAttack
            (Brain as MinionBrain).AddPresetPower(169715); //Rune_cGroundPunch
            (Brain as MinionBrain).AddPresetPower(169728); //Rune_dAoeAttack
            //TODO: These values should most likely scale, but we don't know how yet, so just temporary values.
            Attributes[GameAttribute.Hitpoints_Max] = context.ScriptFormula(0) * context.User.Attributes[GameAttribute.Hitpoints_Max_Total];
            Attributes[GameAttribute.Hitpoints_Cur] = Attributes[GameAttribute.Hitpoints_Max];
            Attributes[GameAttribute.Attacks_Per_Second] = 1.0f;

            Attributes[GameAttribute.Damage_Weapon_Min, 0] = context.ScriptFormula(1);

            Attributes[GameAttribute.Pet_Type] = 0x8;
            //Pet_Owner and Pet_Creator seems to be 0
            (context.User as Player).InGameClient.SendMessage(new PetMessage()
            {
                Field0 = 0,
                Field1 = MysticAllyID,
                PetId = this.DynamicID,
                Field3 = 0x8,
            });
        }
    }
}
