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

using System.Text;

namespace Mooege.Net.GS.Message.Definitions.Effect
{
    /// <summary>
    /// Sent to the client to play a special effect on the actor
    /// </summary>
    [Message(Opcodes.PlayEffectMessage)]
    public class PlayEffectMessage : GameMessage
    {
        public uint ActorId; // Actor's DynamicID
        public Effect Effect;
        public int? OptionalParameter;

        public PlayEffectMessage() : base(Opcodes.PlayEffectMessage) { }

        public override void Parse(GameBitBuffer buffer)
        {
            ActorId = buffer.ReadUInt(32);
            Effect = (Effect)buffer.ReadInt(7) + (-1);
            if (buffer.ReadBool())
            {
                OptionalParameter = buffer.ReadInt(32);
            }
        }

        public override void Encode(GameBitBuffer buffer)
        {
            buffer.WriteUInt(32, ActorId);
            buffer.WriteInt(7, (int)Effect - (-1));
            buffer.WriteBool(OptionalParameter.HasValue);
            if (OptionalParameter.HasValue)
            {
                buffer.WriteInt(32, OptionalParameter.Value);
            }
        }

        public override void AsText(StringBuilder b, int pad)
        {
            b.Append(' ', pad);
            b.AppendLine("PlayEffectMessage:");
            b.Append(' ', pad++);
            b.AppendLine("{");
            b.Append(' ', pad); b.AppendLine("ActorID: 0x" + ActorId.ToString("X8") + " (" + ActorId + ")");
            b.Append(' ', pad); b.AppendLine("Effect: 0x" + ((int)Effect).ToString("X8") + " (" + Effect + ")");
            if (OptionalParameter.HasValue)
            {
                b.Append(' ', pad); b.AppendLine("OptionalParameter.Value: 0x" + OptionalParameter.Value.ToString("X8") + " (" + OptionalParameter.Value + ")");
            }
            b.Append(' ', --pad);
            b.AppendLine("}");
        }
    }

    // if you have any questions ask me - farmy
    public enum Effect
    {
        Hit = 0,                        // plays a hit sound, takes another parameter for the specific sound
        Unknown1 = 1,
        Unknown2 = 2,                   // Used on goldcoins, healthglobes...items                  
        Unknown3 = 3,                   // Used on goldcoins, healthglobes...items
        UnknownSound = 4,               // Weird sound effect...pickup sound?  only used on PlayerActors

        /// <summary>
        /// Gold pickup (golden glow)
        /// </summary>
        GoldPickup = 5,

        /// <summary>
        /// Level up message (sign) in the center of the screen
        /// TODO: it's not level-up, it's current player level info (including unlocked skills/slots)
        /// </summary>
        LevelUp = 6,

        /// <summary>
        /// Health orb pickup (red imapct effect and noise)
        /// </summary>
        HealthOrbPickup = 7,

        /// <summary>
        /// violet / pink circular effect on the actor
        /// </summary>
        ArcanePowerGain = 8,

        /// <summary>
        /// Holy effect
        /// TODO find out what that is and give the enum a proper name
        /// </summary>
        Holy1 = 9,

        /// <summary>
        /// Holy effect
        /// TODO find out what that is and give the enum a proper name
        /// </summary>
        Holy2 = 10,

        Unknown11 = 11,                 // played on chests, switches, spawner ...
        Unknown12 = 12,                 // played on a lot of things

        /// <summary>
        /// Breathing effect. Takes a particle effect sno as parameter. See comments
        /// TODO find out if that is the same as Breathing2
        /// </summary>
        Breathing1 = 13,          // OOOkay... you CAN play this with a particle sno in field2 but i dont know if that is the intention. The place suggests its a breathing effect like 27958

        /// <summary>
        /// Sound effect. Takes a sound sno as parameter
        /// </summary>
        Sound = 14,

        Unknown15 = 15,

        /// <summary>
        /// Breathing effect. Takes a particle effect sno as parameter.
        /// TODO find out if that is the same as Breathing1
        /// </summary>
        Breathing2 = 16,          // same as BreathingEffect1

        /// <summary>
        /// Plays a sound and shows a text informing the player he cannot carry any more
        /// TODO find out if that is the same as PickupFailOverburden2
        /// </summary>
        PickupFailOverburden1 = 17,

        /// <summary>
        /// Plays a sound and shows a text informing the player he cannot carry any more
        /// TODO find out if that is the same as PickupFailOverburden1
        /// </summary>
        PickupFailOverburden2 = 18,     // same as PickupFail1

        /// <summary>
        /// Plays a sound and shows a text informing the player he is
        /// not allowed to have more of this item type
        /// </summary>
        PickupFailNotAllowedMore = 19,

        /// <summary>
        /// Shows a text informing the player that the item he wants to pick up
        /// does not belong to him
        /// </summary>
        PickupFailIsNotYours = 20,

        Unknown21 = 21,
        Unknown22 = 22,
        Unknown23 = 23,                 // only one param, played on spawners

        /// <summary>
        /// Splashes blood towards another actor. Takes the splash target dynamic id as parameter or -1 for an undirected splash
        /// </summary>
        BloodSplash = 24,

        Unknown25 = 25,
        Unknown26 = 26,
        IcyEffect = 27, // light blue cloud
        IcyEffect2 = 28,  // darker blue
        IcyEffect3 = 29, // bright blue glow
        Unknown30 = 30,
        Unknown31 = 31, // takes another value that looks like an id but is not an actor

        /// <summary>
        /// Plays an effect group. Takes the sno of the effect group as parameter
        /// </summary>
        PlayEffectGroup = 32,

        Unknown33 = 33,
        LoudNoise = 34,                 // plays a loud sound: TODO rename this enum value, testet with monk
        LoudNoise2 = 35,                // plays a loud sound: TODO rename this enum value, tested with monk
        Unknown36 = 36,

        /// <summary>
        /// Energy / Furty / Mana etc pickup indicator, right globe flashes
        /// </summary>
        SecondaryRessourceEffect = 37,

        Unknown38 = 38,
        Unknown39 = 39,

        /// <summary>
        /// Plays a gore effect
        /// </summary>
        Gore = 40,

        /// <summary>
        /// Plays a gore effect with fire component
        /// </summary>
        GoreFire = 41,

        /// <summary>
        /// Plays a gore effect with poison component
        /// </summary>
        GorePoison = 42,

        /// <summary>
        /// Plays a gore effect with arcane component
        /// </summary>
        GoreArcane = 43,

        /// <summary>
        /// Plays a gore effect with holy component
        /// </summary>
        GoreHoly = 44,

        /// <summary>
        /// Plays a gore effect with electro component
        /// </summary>
        GoreElectro = 45,

        IceBreak = 46,
        Inferno = 47,    // infernolike explosion
        Darker = 48,
        Red = 49,
        Lila1 = 50,
        Lila2 = 51,
        Burned1 = 52,
        Blue1 = 53,
        Blue2 = 54,
        Burned2 = 55,
        Green = 56,
        Unknown57 = 57,
        Unknown58 = 58,// Used only on shield skeletons
        Unknown59 = 59, // takes another value. have seen only field2==1, mostly used on shield skeletons
        Unknown60 = 60, // takes another parameter. have seen it only once with field2 == 1 on a shield skeleton
        Unknown61 = 61,// used often but i dont see anything

        /// <summary>
        /// Blue bubbles around the actor
        /// </summary>
        ManaPickup = 62,

        Unknown63 = 63,
        Unknown64 = 64,
        Unknown65 = 65,
        Unknown66 = 66,
        Unknown67 = 67,     // hmm...actor disappears... maybe death?
        //Unknown68 = 68,  // that one crashes
        Unknown69 = 69,

        /// <summary>
        /// Displays a checkpoint (reached) sign
        /// </summary>
        Checkpoint = 70
    }
}
