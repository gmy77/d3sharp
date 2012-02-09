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

using System.IO;
using Version = Mooege.Common.Versions.VersionInfo.MooNet.Achievements;
using Mooege.Common.Logging;

namespace Mooege.Core.MooNet.Achievement
{
    public static class AchievementManager
    {
        private static readonly Logger Logger = LogManager.CreateLogger();

        public static readonly bnet.protocol.achievements.AchievementFile Achievements;

        static AchievementManager()
        {
            if (File.Exists(Version.AchievementFilename))
            {
                var br = new BinaryReader(File.Open(Version.AchievementFilename, FileMode.Open));
                Achievements = bnet.protocol.achievements.AchievementFile.ParseFrom(br.ReadBytes((int)br.BaseStream.Length));
                br.Close();
                Logger.Info("Achievement file loaded from disk.");
            }
            else
            {
                Logger.Info("Achievement file not found. Attempting to download...");
                var attempts = 0;
                byte[] data = new byte[] { };
                while (attempts < 5)
                {
                    try
                    {
                        data = new System.Net.WebClient().DownloadData(Version.AchievementURL);
                        break;
                    }
                    catch (System.Net.WebException)
                    {
                        attempts++;
                    }
                }
                try
                {
                    Achievements = bnet.protocol.achievements.AchievementFile.ParseFrom(data);
                    if (attempts < 5)
                    {
                        var br = new BinaryWriter(File.Open(Version.AchievementFilename, FileMode.CreateNew));
                        br.Write(data);
                        br.Close();
                    }
                    else
                    {
                        Logger.Error("AchievementFile could not be downloaded. Aborted after 5 tries.");
                    }
                }
                catch (Google.ProtocolBuffers.InvalidProtocolBufferException)
                {
                    Achievements = bnet.protocol.achievements.AchievementFile.CreateBuilder().Build();
                    Logger.Error("AchievementFile could not be downloaded and parsed correctly.");
                }
                catch (IOException)
                {
                    Logger.Error("{0} could not be written to.", Version.AchievementFilename);
                }

            }
        }

        public static int TotalAchievements
        {
            get { return Achievements.AchievementCount; }
        }

        public static int TotalCategories
        {
            get { return Achievements.CategoryCount; }
        }

        public static int TotalCriteria
        {
            get { return Achievements.CriteriaCount; }
        }
    }
}
