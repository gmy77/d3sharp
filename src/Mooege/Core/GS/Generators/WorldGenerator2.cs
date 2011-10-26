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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mooege.Common;
using Mooege.Common.MPQ;
using Mooege.Core.GS.Map;

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



            foreach(var sceneChunk in worldData.SceneParams.SceneChunks)
            {
                var scene = new Scene(world, sceneChunk.SNOName.SNOId, null);
                scene.MiniMapVisibility = 2;
                
                //scene.Position = sceneChunk.PRTransform.V;
                //scene.RotationAmount = sceneChunk.PRTransform.Q.Float0; // rename rotation-amount and axis to Quaternion.
                //scene.RotationAxis.X = sceneChunk.PRTransform.Q.Vector3D.X;
                //scene.RotationAxis.Y = sceneChunk.PRTransform.Q.Vector3D.Y;
                //scene.RotationAxis.Z = sceneChunk.PRTransform.Q.Vector3D.Z;

                scene.Position = new Net.GS.Message.Fields.Vector3D
                                     {
                                         X = sceneChunk.PRTransform.V.X -= 240, // i don't know why we need this but otherwise it does not work/raist
                                         Y = sceneChunk.PRTransform.V.Y -= 480, // i don't know why we need this but otherwise it does not work/raist
                                         Z = sceneChunk.PRTransform.V.Z
                                     };

                //  71150 = X-=540, Y+=600
                // 109362 = X-=240, Y-=400
                
                scene.RotationAmount = 1;
                scene.RotationAxis.X = 0;
                scene.RotationAxis.Y = 0;
                scene.RotationAxis.Z = 0;

                scene.SceneGroupSNO = -1;
                SetSceneSpecification(scene, sceneChunk.SceneSpecification);
            }

            //  71150 =  X: 3143,75,  Y: 2828,75,  Z: 59,07559
            // 109362 =   X: 83,75,  Y: 123,75,  Z: 0,2000023
            world.StartPosition.X = 83.75f;
            world.StartPosition.Y = 123.75f;
            world.StartPosition.Z = 0.2000023f;

            return world;
        }

        private static void SetSceneSpecification(Scene scene, Mooege.Common.MPQ.FileFormats.SceneSpecification specification)
        {
            scene.SceneSpec = new Net.GS.Message.Fields.SceneSpecification
                                  {
                                      CellZ=specification.CellZ,
                                      Cell = new Net.GS.Message.Fields.IVector2D
                                                 {
                                                   X=specification.Cell.X,
                                                   Y=specification.Cell.Y,
                                                 },
                                      arSnoLevelAreas=specification.SNOLevelAreas,
                                      snoPrevWorld=specification.SNOPrevWorld,
                                      Field4=specification.Int1,
                                      snoPrevLevelArea=specification.SNOPrevLevelArea,
                                      snoNextWorld=specification.SNONextWorld,
                                      Field7=specification.Int3,
                                      snoNextLevelArea=specification.SNONextLevelArea,
                                      snoMusic=specification.SNOMusic,
                                      snoCombatMusic=specification.SNOCombatMusic,
                                      snoAmbient=specification.SNOAmbient,
                                      snoReverb=specification.SNOReverb,
                                      snoWeather=specification.SNOWeather,
                                      snoPresetWorld=specification.SNOPresetWorld,
                                      Field15=specification.Int4,
                                      Field16=specification.Int5,
                                      Field17=specification.Int6,
                                      Field18=specification.Int7,
                                      tCachedValues=new Net.GS.Message.Fields.SceneCachedValues
                                                        {
                                                            Field0=specification.SceneCachedValues.Int0,
                                                            Field1=specification.SceneCachedValues.Int1,
                                                            Field2=specification.SceneCachedValues.Int2,
                                                            Field3=new Net.GS.Message.Fields.AABB
                                                                       {
                                                                             Min=specification.SceneCachedValues.AABB1.Min,
                                                                             Max=specification.SceneCachedValues.AABB1.Max
                                                                       },
                                                            Field4=new Net.GS.Message.Fields.AABB
                                                                       {
                                                                           Min = specification.SceneCachedValues.AABB2.Min,
                                                                           Max = specification.SceneCachedValues.AABB2.Max
                                                                       },
                                                            Field5=specification.SceneCachedValues.Int5,
                                                            Field6=specification.SceneCachedValues.Int6
                                                        },

                                  };
        }
    }
}
