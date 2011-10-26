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
using Mooege.Core.GS.Common.Types.Collusion;
using Mooege.Core.GS.Common.Types.Math;
using Mooege.Core.GS.Common.Types.Scene;
using Mooege.Core.GS.Map;
using Mooege.Core.GS.Common.Types.SNO;

namespace Mooege.Core.GS.Generators
{
    public static class WorldGenerator2
    {
        static readonly Logger Logger = LogManager.CreateLogger();

        public static World Generate(Game.Game game, int worldSNO)
        {
            var world = new World(game, worldSNO);

            if(!MPQStorage.Data.Assets[SNOGroup.Worlds].ContainsKey(worldSNO))
            {
                Logger.Error("Can't find a valid world definition for sno: {0}", worldSNO);
                return null;
            }

            var worldAsset = MPQStorage.Data.Assets[SNOGroup.Worlds][worldSNO];
            var worldData = (Mooege.Common.MPQ.FileFormats.World) worldAsset.Data;

            if (worldData.SceneParams.SceneChunks.Count == 0)
                Logger.Error("World {0} [{1}] is a dynamic world! Can't generate dynamic worlds yet!", worldAsset.Name, worldAsset.SNOId);

            // Create a clusterID => Cluster Dictionary
            Dictionary<int, Mooege.Common.MPQ.FileFormats.SceneCluster> clusters = new Dictionary<int,Mooege.Common.MPQ.FileFormats.SceneCluster>();
            foreach (var cluster in worldData.SceneClusterSet.SceneClusters)
                clusters[cluster.ClusterId] = cluster;

            // Scenes are not aligned to (0, 0) but apparently need to be -farmy
            float minX = worldData.SceneParams.SceneChunks.Min(x => x.PRTransform.Vector3D.X);
            float minY = worldData.SceneParams.SceneChunks.Min(x => x.PRTransform.Vector3D.Y);

            foreach(var sceneChunk in worldData.SceneParams.SceneChunks)
            {
                var scene = new Scene(world, sceneChunk.SNOName.SNOId, null);
                scene.MiniMapVisibility = 2;
                
                scene.Position = new Vector3D
                                     {
                                         X = sceneChunk.PRTransform.Vector3D.X - minX,
                                         Y = sceneChunk.PRTransform.Vector3D.Y - minY,
                                         Z = sceneChunk.PRTransform.Vector3D.Z
                                     };

                // rename rotation-amount and axis to Quaternion.
                scene.RotationAmount = 1;
                scene.RotationAxis.X = 0;
                scene.RotationAxis.Y = 0;
                scene.RotationAxis.Z = 0;

                scene.SceneGroupSNO = -1;

                // If the scene has a subscene (cluster ID is set), then choose a random subscenes from the cluster
                if (sceneChunk.SceneSpecification.ClusterID != -1)
                {
                    if (!clusters.ContainsKey(sceneChunk.SceneSpecification.ClusterID))
                    {
                        Logger.Error("Referenced clusterID {0} not found for chunk {1} in world {2}", sceneChunk.SceneSpecification.ClusterID, sceneChunk.SNOName.SNOId, worldSNO);
                    }
                    else
                    {
                        if (clusters[sceneChunk.SceneSpecification.ClusterID].Default.Entries.Count > 0)
                        {
                            var x = clusters[sceneChunk.SceneSpecification.ClusterID].Default.Entries[0];
                            Vector3D pos = FindSubScenePosition(sceneChunk);

                            if (pos == null)
                            {
                                Logger.Error("No scene position marker for SubScenes of Scene {0} found", sceneChunk.SNOName.SNOId);
                            }
                            else
                            {
                                Scene subscene = new Scene(world, x.SNOScene, scene);

                                subscene.Position = new Vector3D()
                                {
                                    X = scene.Position.X + pos.X,
                                    Y = scene.Position.Y + pos.Y,
                                    Z = scene.Position.Z + pos.Z
                                };
                                subscene.MiniMapVisibility = 2;
                                subscene.RotationAmount = 1;
                                subscene.Specification = sceneChunk.SceneSpecification;
                                scene.Subscenes.Add(subscene);
                            }
                        }
                    }
                }

                scene.Specification = sceneChunk.SceneSpecification;
            }

            //  71150 =  X: 3143,75,  Y: 2828,75,  Z: 59,07559
            // 109362 =   X: 83,75,  Y: 123,75,  Z: 0,2000023
            world.StartPosition.X = 3143.75f;
            world.StartPosition.Y = 2828.75f;
            world.StartPosition.Z = 59.2000023f;

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
                    if (marker.Int0 == 16)      // TODO Make this an enum value
                        return marker.PRTransform.Vector3D;
            }

            return null;
        }
    }
}
