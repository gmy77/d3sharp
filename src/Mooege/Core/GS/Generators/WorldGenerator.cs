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

using System.Collections.Generic;
using System.Linq;
using Mooege.Common;
using Mooege.Common.MPQ;
using Mooege.Core.GS.Common.Types.Math;
using Mooege.Core.GS.Common.Types.SNO;
using Mooege.Core.GS.Games;
using Mooege.Core.GS.Map;
using Mooege.Common.Helpers;


namespace Mooege.Core.GS.Generators
{
    public static class WorldGenerator
    {
        static readonly Logger Logger = LogManager.CreateLogger();

        public static World Generate(Game game, int worldSNO)
        {
            if (!MPQStorage.Data.Assets[SNOGroup.Worlds].ContainsKey(worldSNO))
            {
                Logger.Error("Can't find a valid world definition for sno: {0}", worldSNO);
                return null;
            }

            var worldAsset = MPQStorage.Data.Assets[SNOGroup.Worlds][worldSNO];
            var worldData = (Mooege.Common.MPQ.FileFormats.World)worldAsset.Data;

            if (worldData.SceneParams.SceneChunks.Count == 0)
            {
                Logger.Error("World {0} [{1}] is a dynamic world! Can't generate dynamic worlds yet!", worldAsset.Name, worldAsset.SNOId);
                return null;
            }

            var world = new World(game, worldSNO);

            // Create a clusterID => Cluster Dictionary
            var clusters = new Dictionary<int, Mooege.Common.MPQ.FileFormats.SceneCluster>();
            foreach (var cluster in worldData.SceneClusterSet.SceneClusters)
                clusters[cluster.ClusterId] = cluster;

            // Scenes are not aligned to (0, 0) but apparently need to be -farmy
            float minX = worldData.SceneParams.SceneChunks.Min(x => x.PRTransform.Vector3D.X);
            float minY = worldData.SceneParams.SceneChunks.Min(x => x.PRTransform.Vector3D.Y);

            // Count all occurences of each cluster /fasbat
            var clusterCount = new Dictionary<int, int>();

            foreach (var sceneChunk in worldData.SceneParams.SceneChunks)
            {
                var cID = sceneChunk.SceneSpecification.ClusterID;
                if (cID != -1 && clusters.ContainsKey(cID)) // Check for wrong clusters /fasbat
                {
                    if (!clusterCount.ContainsKey(cID))
                        clusterCount[cID] = 0;
                    clusterCount[cID]++;
                }
            }

            // For each cluster generate a list of randomly selected subcenes /fasbat
            var clusterSelected = new Dictionary<int, List<Mooege.Common.MPQ.FileFormats.SubSceneEntry>>();
            foreach (var cID in clusterCount.Keys)
            {
                var selected = new List<Mooege.Common.MPQ.FileFormats.SubSceneEntry>();
                clusterSelected[cID] = selected;
                var count = clusterCount[cID];
                foreach (var group in clusters[cID].SubSceneGroups) // First select from each subscene group /fasbat
                {
                    for (int i = 0; i < group.I0 && count > 0; i++, count--) //TODO Rename I0 to requiredCount? /fasbat
                    {
                        var subSceneEntry = RandomHelper.RandomItem(group.Entries, entry => entry.Probability);
                        selected.Add(subSceneEntry);
                    }

                    if (count == 0)
                        break;
                }

                while (count > 0) // Fill the rest with defaults /fasbat
                {
                    var subSceneEntry = RandomHelper.RandomItem(clusters[cID].Default.Entries, entry => entry.Probability);
                    selected.Add(subSceneEntry);
                    count--;
                }
            }

            foreach (var sceneChunk in worldData.SceneParams.SceneChunks)
            {
                var position = sceneChunk.PRTransform.Vector3D - new Vector3D(minX, minY, 0);
                var scene = new Scene(world, position, sceneChunk.SNOName.SNOId, null)
                {
                    MiniMapVisibility = SceneMiniMapVisibility.Revealed,                    
                    RotationAmount = sceneChunk.PRTransform.Quaternion.W,
                    RotationAxis = sceneChunk.PRTransform.Quaternion.Vector3D,
                    SceneGroupSNO = -1
                };
               
                // If the scene has a subscene (cluster ID is set), choose a random subscenes from the cluster load it and attach it to parent scene /farmy
                if (sceneChunk.SceneSpecification.ClusterID != -1)
                {
                    if (!clusters.ContainsKey(sceneChunk.SceneSpecification.ClusterID))
                    {
                        Logger.Warn("Referenced clusterID {0} not found for chunk {1} in world {2}", sceneChunk.SceneSpecification.ClusterID, sceneChunk.SNOName.SNOId, worldSNO);
                    }
                    else
                    {
                        var entries = clusterSelected[sceneChunk.SceneSpecification.ClusterID]; // Select from our generated list /fasbat
                        Mooege.Common.MPQ.FileFormats.SubSceneEntry subSceneEntry = null;

                        if (entries.Count > 0)
                        {
                            subSceneEntry = RandomHelper.RandomItem<Mooege.Common.MPQ.FileFormats.SubSceneEntry>(entries, entry => 1); // TODO Just shuffle the list, dont random every time. /fasbat
                            entries.Remove(subSceneEntry);
                        }
                        else
                            Logger.Error("No SubScenes defined for cluster {0} in world {1}", sceneChunk.SceneSpecification.ClusterID, world.DynamicID);

                        Vector3D pos = FindSubScenePosition(sceneChunk); // TODO According to BoyC, scenes can have more than one subscene, so better enumerate over all subscenepositions /farmy

                        if (pos == null)
                        {
                            Logger.Error("No scene position marker for SubScenes of Scene {0} found", sceneChunk.SNOName.SNOId);
                        }
                        else
                        {
                            var subScenePosition = scene.Position + pos;
                            var subscene = new Scene(world, subScenePosition, subSceneEntry.SNOScene, scene)
                            {
                                MiniMapVisibility = SceneMiniMapVisibility.Revealed,
                                RotationAmount = sceneChunk.PRTransform.Quaternion.W,
                                RotationAxis = sceneChunk.PRTransform.Quaternion.Vector3D,
                                Specification = sceneChunk.SceneSpecification
                            };
                            scene.Subscenes.Add(subscene);
                            subscene.LoadActors();
                        }
                    }

                }
                scene.Specification = sceneChunk.SceneSpecification;
                scene.LoadActors();
            }

            return world;
        }

        /// <summary>
        /// Loads all markersets of a scene and looks for the one with the subscene position
        /// </summary>
        private static Vector3D FindSubScenePosition(Mooege.Common.MPQ.FileFormats.SceneChunk sceneChunk)
        {
            var mpqScene = MPQStorage.Data.Assets[SNOGroup.Scene][sceneChunk.SNOName.SNOId].Data as Mooege.Common.MPQ.FileFormats.Scene;

            foreach (var markerSet in mpqScene.MarkerSets)
            {
                var mpqMarkerSet = MPQStorage.Data.Assets[SNOGroup.MarkerSet][markerSet].Data as Mooege.Common.MPQ.FileFormats.MarkerSet;
                foreach (var marker in mpqMarkerSet.Markers)
                    if (marker.Int0 == 16)      // TODO Make this an enum value /farmy
                        return marker.PRTransform.Vector3D;
            }

            return null;
        }
    }
}