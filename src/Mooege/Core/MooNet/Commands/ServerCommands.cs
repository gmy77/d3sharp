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

using System;
using System.Linq;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using Mooege.Core.MooNet.Accounts;
using Mooege.Core.MooNet.Online;
using Mooege.Core.MooNet.Toons;
using Mooege.Net.MooNet;

namespace Mooege.Core.MooNet.Commands
{
    [CommandGroup("stats", "Renders statistics.\nUsage: stats [system].")]
    public class StatsCommand : CommandGroup
    {
        [DefaultCommand]
        public string Stats(string[] @params, MooNetClient invokerClient)
        {
            return string.Format("Total Accounts: {0}, Total Toons: {1} Online Players: {2} ",
                                 AccountManager.TotalAccounts, ToonManager.TotalToons, PlayerManager.OnlinePlayers.Count);
        }

        [Command("system", "Renders system statistics.", Account.UserLevels.Admin)]
        public string Detailed(string[] @params, MooNetClient invokerClient)
        {
            var output = new StringBuilder();

            output.AppendFormat("GC Allocated Memory: {0}KB ", GC.GetTotalMemory(true) / 1024);

            if (PerformanceCounterCategory.Exists("Processor") && PerformanceCounterCategory.CounterExists("% Processor Time", "Processor"))
            {
                var processorTimeCounter = new PerformanceCounter { CategoryName = "Processor", CounterName = "% Processor Time", InstanceName = "_Total" };
                output.AppendFormat("Processor Time: {0}%", processorTimeCounter.NextValue());
            }

            if (PerformanceCounterCategory.Exists(".NET CLR LocksAndThreads"))
            {
                if (PerformanceCounterCategory.CounterExists("# of current physical Threads", ".NET CLR LocksAndThreads"))
                {
                    var physicalThreadsCounter = new PerformanceCounter { CategoryName = ".NET CLR LocksAndThreads", CounterName = "# of current physical Threads", InstanceName = Process.GetCurrentProcess().ProcessName };
                    output.AppendFormat("\nPhysical Threads: {0} ", physicalThreadsCounter.NextValue());
                }

                if (PerformanceCounterCategory.CounterExists("# of current logical Threads", ".NET CLR LocksAndThreads"))
                {
                    var logicalThreadsCounter = new PerformanceCounter { CategoryName = ".NET CLR LocksAndThreads", CounterName = "# of current logical Threads", InstanceName = Process.GetCurrentProcess().ProcessName };
                    output.AppendFormat("Logical Threads: {0} ", logicalThreadsCounter.NextValue());
                }

                if (PerformanceCounterCategory.CounterExists("Contention Rate / sec", ".NET CLR LocksAndThreads"))
                {
                    var contentionRateCounter = new PerformanceCounter { CategoryName = ".NET CLR LocksAndThreads", CounterName = "Contention Rate / sec", InstanceName = Process.GetCurrentProcess().ProcessName };
                    output.AppendFormat("Contention Rate: {0}/sec", contentionRateCounter.NextValue());
                }
            }

            if (PerformanceCounterCategory.Exists(".NET CLR Exceptions") && PerformanceCounterCategory.CounterExists("# of Exceps Thrown", ".NET CLR Exceptions"))
            {
                var exceptionsThrownCounter = new PerformanceCounter { CategoryName = ".NET CLR Exceptions", CounterName = "# of Exceps Thrown", InstanceName = Process.GetCurrentProcess().ProcessName };
                output.AppendFormat("\nExceptions Thrown: {0}", exceptionsThrownCounter.NextValue());
            }

            return output.ToString();
        }
    }

    [CommandGroup("uptime", "Renders uptime statistics.")]
    public class UptimeCommand : CommandGroup
    {
        [DefaultCommand]
        public string Uptime(string[] @params, MooNetClient invokerClient)
        {
            var uptime = DateTime.Now - Program.StartupTime;
            return string.Format("Uptime: {0} days, {1} hours, {2} minutes, {3} seconds.", uptime.Days, uptime.Hours, uptime.Minutes, uptime.Seconds);
        }
    }

    [CommandGroup("version", "Renders server version.")]
    public class VersionCommand : CommandGroup
    {
        [DefaultCommand]
        public string Version(string[] @params, MooNetClient invokerClient)
        {
            return "mooege " + Assembly.GetExecutingAssembly().GetName().Version;
        }
    }

    [CommandGroup("motd", "Renders message of the day.")]
    public class MOTDCommand : CommandGroup
    {
        [DefaultCommand]
        public string MOTD(string[] @params, MooNetClient invokerClient)
        {
            return "Message of the day:" + Net.MooNet.Config.Instance.MOTD;
        }
    }

    [CommandGroup("server", "Allows you to control servers and start/stop them.", Account.UserLevels.Admin)]
    public class ServerCommand : CommandGroup
    {
        [Command("start", "usage: server start [moonet|gs]")]
        public string Start(string[] @params, MooNetClient invokerClient)
        {
            var startMooNet = false;
            var startGS = false;
            var output = string.Empty;

            if (@params.Count() > 0)
            {
                switch (@params[0])
                {
                    case "moonet":
                        startMooNet = true;
                        break;
                    case "gs":
                        startGS = true;
                        break;
                }
            }
            else
            {
                startMooNet = true;
                startGS = true;
            }

            if (startMooNet)
            {
                if (!Program.StartMooNet())
                    output += "MooNet server is already running. ";
            }

            if (startGS)
            {
                if (!Program.StartGS())
                    output += "GS is already running. ";

            }

            return output;
        }

        [Command("stop", "usage: server stop [moonet|gs]")]
        public string Stop(string[] @params, MooNetClient invokerClient)
        {
            var stopMooNet = false;
            var stopGS = false;
            var output = string.Empty;

            if (@params.Count() > 0)
            {
                switch (@params[0])
                {
                    case "moonet":
                        stopMooNet = true;
                        break;
                    case "gs":
                        stopGS = true;
                        break;
                }
            }
            else
            {
                stopMooNet = true;
                stopGS = true;
            }

            if (stopMooNet)
            {
                if (!Program.StopMooNet())
                    output += "MooNet server is already stopped. ";
            }

            if (stopGS)
            {
                if (!Program.StopGS())
                    output += "GS is already stopped. ";
            }

            return output;
        }

        [Command("shutdown", "usage: server shutdown")]
        public string Shutdown(string[] @params, MooNetClient invokerClient)
        {
            Program.Shutdown();
            return string.Empty;
        }
    }
}