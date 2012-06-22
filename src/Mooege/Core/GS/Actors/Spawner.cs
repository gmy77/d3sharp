using Mooege.Core.GS.Common.Types.TagMap;
using Mooege.Core.GS.Map;
using Mooege.Core.GS.Common.Types.Math;
using Mooege.Core.GS.Games;
using Mooege.Core.GS.Common.Types.SNO;


namespace Mooege.Core.GS.Actors
{
    public class Spawner : Actor
    {

        /// <summary>
        /// What actor this gizmo will spawn
        /// </summary>
        public SNOHandle ActorToSpawnSNO { get; private set; }

        public override ActorType ActorType
        {
            get { return ActorType.Gizmo; }
        }

        public Spawner(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
            this.Field2 = 8;
            this.Field7 = 0x00000000;

            //Actor.Data.TagMap contains: {66368 = 291072}
            //public static TagKeyInt Spawn2 = new TagKeyInt(291072);
            //TODO: Find why Tags is not the same as Actor.Data.TagMap
            if (Tags.ContainsKey(MarkerKeys.SpawnActor))
                this.ActorToSpawnSNO = Tags[MarkerKeys.SpawnActor];

        }

        /// <summary>
        /// Rewrite the quest handling event
        /// </summary>
        /// <param name="quest"></param>
        protected override void quest_OnQuestProgress(Quest quest)
        {
            //Spawn if this is spawner
            if (World.Game.Quests.IsInQuestRange(_questRange))
            {
                this.Spawn();
            }
        }

        /// <summary>
        /// Override for AfterChangeWorld
        /// </summary>
        public override void AfterChangeWorld()
        {
            base.AfterChangeWorld();
        }

        /// <summary>
        /// Main spawn method
        /// </summary>
        public void Spawn()
        {
            if (this.ActorToSpawnSNO == null)
            {
               // Logger.Debug("Triggered spawner with no ActorToSpawnSNO found.");
                //Try revealing this
                foreach (var player in this.World.Players.Values)
                {
                    base.Reveal(player);
                }
                return;
            }
            var location = new PRTransform()
            {
                Quaternion = new Quaternion
                {
                    W = this.RotationW,
                    Vector3D = this.RotationAxis
                },
                Vector3D = this.Position
            };

            Mooege.Core.GS.Generators.WorldGenerator.loadActor(ActorToSpawnSNO, location, this.World, ((Mooege.Common.MPQ.FileFormats.Actor)ActorToSpawnSNO.Target).TagMap);

            //once target spawned this can be destroyed
            this.Destroy();
        }

        /// <summary>
        /// Reveal Override. For Spawner Gizmos there is no reveal necessary.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public override bool Reveal(Players.Player player)
        {
            return false;
        }
    }
}
