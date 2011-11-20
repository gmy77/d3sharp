using Mooege.Core.GS.Common.Types.TagMap;
using Mooege.Core.GS.Map;

namespace Mooege.Core.GS.Actors
{
    public class ServerProp : Actor
    {
        public override ActorType ActorType
        {
            get { return ActorType.ServerProp; }
        }

        public ServerProp(World world, int snoId, TagMap tags)
            : base(world, snoId, tags)
        {
            this.Field2 = 16;
            this.Field7 = 0x00000001;
            this.CollFlags = 0; // a hack for passing through blockers /fasbat
        }
    }
}
