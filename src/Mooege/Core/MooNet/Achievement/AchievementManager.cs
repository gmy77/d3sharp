/*
 * Copyright (C) 2011 mooege project
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
using Mooege.Common.Versions;
using Mooege.Common.Logging;

namespace Mooege.Core.MooNet.Achievement
{
    public static class AchievementManager
    {
        private static readonly Logger Logger = LogManager.CreateLogger();

        public static readonly bnet.protocol.achievements.AchievementFile Achievements;

        static AchievementManager()
        {
            if (File.Exists(VersionInfo.MooNet.AchievementFileHash + ".achv"))
            {
                var br = new BinaryReader(File.Open(VersionInfo.MooNet.AchievementFileHash + ".achv", FileMode.Open));
                Achievements = bnet.protocol.achievements.AchievementFile.ParseFrom(br.ReadBytes((int)br.BaseStream.Length));
                br.Close();
                Logger.Info("Achievement file loaded from disk.");
            }
            else
            {
                Logger.Info("Achievement file not found. Downloading...");
                byte[] data = new System.Net.WebClient().DownloadData("http://us.depot.battle.net:1119/" + VersionInfo.MooNet.AchievementFileHash + ".achv");
                var br = new BinaryWriter(File.Open(VersionInfo.MooNet.AchievementFileHash + ".achv", FileMode.CreateNew));
                br.Write(data);
                br.Close();
                Achievements = bnet.protocol.achievements.AchievementFile.ParseFrom(data);
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
