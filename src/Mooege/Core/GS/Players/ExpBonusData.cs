using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mooege.Common;
using Mooege.Net.GS.Message.Definitions.Combat;
using Mooege.Common.Helpers;

namespace Mooege.Core.GS.Players
{
    public class ExpBonusData
    {
        private static readonly Logger Logger = LogManager.CreateLogger();

        /// <summary>
        /// The referenced player.
        /// </summary>
        private Player _player;

        /// <summary>
        /// The time between two kills to still count as a killstreak.
        /// </summary>
        private int _killstreakTickTime;

        /// <summary>
        /// The player's killcounter in a killstreak.
        /// </summary>
        private int _killstreakPlayer;

        /// <summary>
        /// The environment's killcounter in a killstreak.
        /// </summary>
        private int _killstreakEnvironment;

        /// <summary>
        /// The last tick in which the player killed any monster.
        /// </summary>
        private int _lastMonsterKillTick;

        /// <summary>
        /// The last tick in which the player attacked any monster.
        /// </summary>
        private int _lastMonsterAttackTick;

        /// <summary>
        /// The number of monster-kills of the player's latest monster-attack.
        /// </summary>
        private int _lastMonsterAttackKills;

        /// <summary>
        /// The last tick in which environement got destroyed by the player.
        /// </summary>
        private int _lastEnvironmentDestroyTick;

        /// <summary>
        /// The number of monster-kills of the last environment-destruction.
        /// </summary>
        private int _lastEnvironmentDestroyMonsterKills;

        /// <summary>
        /// The last tick in which destroyed environment killed a monster.
        /// </summary>
        private int _lastEnvironmentDestroyMonsterKillTick;

        public ExpBonusData(Player player)
        {
            this._player = player;
            this._killstreakTickTime = 200;
            this._killstreakPlayer = 0;
            this._killstreakEnvironment = 0;
            this._lastMonsterKillTick = 0;
            this._lastMonsterAttackTick = 0;
            this._lastMonsterAttackKills = 0;
            this._lastEnvironmentDestroyTick = 0;
            this._lastEnvironmentDestroyMonsterKills = 0;
            this._lastEnvironmentDestroyMonsterKillTick = 0;
        }

        public void Update(int attackerActorType, int defeatedActorType)
        {
            if (attackerActorType == 7) // Player
            {
                if (defeatedActorType == 1) // Monster
                {
                    // Massacre
                    if (this._lastMonsterKillTick + this._killstreakTickTime > this._player.InGameClient.Game.Tick)
                    {
                        this._killstreakPlayer++;
                    }
                    else
                    {
                        this._killstreakPlayer = 1;
                    }

                    // MightyBlow
                    if (Math.Abs(this._lastMonsterAttackTick - this._player.InGameClient.Game.Tick) <= 20)
                    {
                        this._lastMonsterAttackKills++;
                    }
                    else
                    {
                        this._lastMonsterAttackKills = 1;
                    }

                    this._lastMonsterKillTick = this._player.InGameClient.Game.Tick;
                }
                else if (defeatedActorType == 5) // Environment
                {
                    // Destruction
                    if (this._lastEnvironmentDestroyTick + this._killstreakTickTime > this._player.InGameClient.Game.Tick)
                    {
                        this._killstreakEnvironment++;
                    }
                    else
                    {
                        this._killstreakEnvironment = 1;
                    }

                    this._lastEnvironmentDestroyTick = this._player.InGameClient.Game.Tick;
                }
            }
            else if (attackerActorType == 5) // Environment
            {
                // Pulverized
                if (Math.Abs(this._lastEnvironmentDestroyMonsterKillTick - this._player.InGameClient.Game.Tick) <= 20)
                {
                    this._lastEnvironmentDestroyMonsterKills++;
                }
                else
                {
                    this._lastEnvironmentDestroyMonsterKills = 1;
                }

                this._lastEnvironmentDestroyMonsterKillTick = this._player.InGameClient.Game.Tick;
            }
        }

        public void Check(byte bonusType)
        {
            int defeated = 0;
            int expBonus = 0;

            switch (bonusType)
            {
                case 0: // Massacre
                    {
                        if ((this._killstreakPlayer > 5) && (this._lastMonsterKillTick + this._killstreakTickTime <= this._player.InGameClient.Game.Tick))
                        {
                            defeated = this._killstreakPlayer;
                            expBonus = (this._killstreakPlayer - 5) * 10;

                            this._killstreakPlayer = 0;
                        }
                        break;
                    }
                case 1: // Destruction
                    {
                        if ((this._killstreakEnvironment > 5) && (this._lastEnvironmentDestroyTick + this._killstreakTickTime <= this._player.InGameClient.Game.Tick))
                        {
                            defeated = this._killstreakEnvironment;
                            expBonus = (this._killstreakEnvironment - 5) * 5;

                            this._killstreakEnvironment = 0;
                        }
                        break;
                    }
                case 2: // Mighty Blow
                    {
                        if (this._lastMonsterAttackKills > 5)
                        {
                            defeated = this._lastMonsterAttackKills;
                            expBonus = (this._lastMonsterAttackKills - 5) * 5;
                        }
                        this._lastMonsterAttackKills = 0;
                        break;
                    }
                case 3: // Pulverized
                    {
                        if (this._lastEnvironmentDestroyMonsterKills > 3)
                        {
                            defeated = this._lastEnvironmentDestroyMonsterKills;
                            expBonus = (this._lastEnvironmentDestroyMonsterKills - 3) * 10;
                        }
                        this._lastEnvironmentDestroyMonsterKills = 0;
                        break;
                    }
                default:
                    {
                        Logger.Warn("Invalid Exp-Bonus-Type was checked.");
                        return;
                    }
            }

            if (expBonus > 0)
            {
                this._player.InGameClient.SendMessage(new KillCounterUpdateMessage()
                {
                    Id = 0xcd,
                    Field0 = bonusType,
                    Field1 = defeated,
                    Field2 = expBonus,
                    Field3 = false,
                });

                this._player.UpdateExp(expBonus);
                this._player.PlayHeroConversation(0x0002A73F, RandomHelper.Next(0, 8));
            }
        }

        public void MonsterAttacked(int monsterAttackTick)
        {
            this._lastMonsterAttackTick = monsterAttackTick;
        }
    }
}
