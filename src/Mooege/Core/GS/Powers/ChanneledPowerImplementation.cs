using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mooege.Core.GS.Common.Types;

namespace Mooege.Core.GS.Powers
{
    public abstract class ChanneledPowerImplementation : PowerImplementation
    {
        public bool ChannelOpen = false;
        public float RunDelay = 1.0f;

        public virtual void OnChannelOpen() { }
        public virtual void OnChannelClose() { }
        public virtual void OnChannelUpdated() { }
        public abstract IEnumerable<TickTimer> RunChannel();

        private TickTimer _runTimeout = null;

        public sealed override IEnumerable<TickTimer> Run()
        {
            if (_runTimeout == null || _runTimeout.TimedOut())
            {
                _runTimeout = WaitSeconds(RunDelay);
                foreach (TickTimer timeout in RunChannel())
                    yield return timeout;
            }

            yield break;
        }
    }
}
