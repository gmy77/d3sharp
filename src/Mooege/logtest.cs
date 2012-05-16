using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;

namespace Mooege
{
        public static class logtest
        {
            private static Logger logger = LogManager.GetCurrentClassLogger();

            public static void test()
            {
                logger.Trace("Sample trace message");
                logger.Debug("Sample debug message");
                logger.Info("Sample informational message");
                logger.Warn("Sample warning message");
                logger.Error("Sample error message");
                logger.Fatal("Sample fatal error message");

                logger.FatalException("test exception", new Exception("tefdsfs"));
            }
        }

}
