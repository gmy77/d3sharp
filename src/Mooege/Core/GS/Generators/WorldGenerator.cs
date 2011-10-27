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
using System.IO;
using System.Collections.Generic;
using Mooege.Core.GS.Common.Types.Collusion;
using Mooege.Core.GS.Common.Types.Math;
using Mooege.Core.GS.Common.Types.Scene;
using duct;
using Mooege.Common;
using Mooege.Core.GS.Game;
using Mooege.Core.GS.Map;
using Mooege.Core.GS.Data.SNO;
using Mooege.Net.GS.Message.Fields;
using Mooege.Net.GS.Message.Definitions.Map;
using Mooege.Net.GS.Message.Definitions.Scene;

// NOTE: Static-world loader for now until we get actual generation code in. duct# can be tossed once we do.

namespace Mooege.Core.GS.Generators
{
    public class WorldGenerator
    {
        static readonly Logger Logger = LogManager.CreateLogger();

        private static readonly Template _tplWorldPath = new Template(null, new[]{VariableType.INTEGER}, false);
        private static readonly Template _tplWorldSNO = new Template(new[]{"WorldSNO"}, new[]{VariableType.INTEGER}, false);
        private static readonly Template _tplStartPos = new Template(new[]{"StartPos"}, new[]{VariableType.FLOAT, VariableType.FLOAT, VariableType.FLOAT}, false);

        private static readonly Template _tplSceneUnified = new Template
        (
            new[]{"Unified"},
            new[]
            {
                VariableType.INTEGER, // SceneSNO
                // *SceneSpec*
                VariableType.INTEGER, // CellZ
                VariableType.INTEGER, VariableType.INTEGER, // Cell.X, Cell.Y
                VariableType.INTEGER, VariableType.INTEGER, VariableType.INTEGER, VariableType.INTEGER, // arSnoLevelAreas[0,1,2,3]
                VariableType.INTEGER, // snoPrevWorld
                VariableType.INTEGER, // Field4
                VariableType.INTEGER, // snoPrevLevelArea
                VariableType.INTEGER, // snoNextWorld
                VariableType.INTEGER, // Field7
                VariableType.INTEGER, // snoNextLevelArea
                VariableType.INTEGER, // snoMusic
                VariableType.INTEGER, // snoCombatMusic
                VariableType.INTEGER, // snoAmbient
                VariableType.INTEGER, // snoReverb
                VariableType.INTEGER, // snoWeather
                VariableType.INTEGER, // snoPresetWorld
                VariableType.INTEGER, // Field15
                VariableType.INTEGER, // Field16
                VariableType.INTEGER, // Field17
                VariableType.INTEGER, // Field18
                VariableType.INTEGER, // tCachedValues.Field0
                VariableType.INTEGER, // tCachedValues.Field1
                VariableType.INTEGER, // tCachedValues.Field2
                VariableType.FLOAT, VariableType.FLOAT, VariableType.FLOAT, // tCachedValues.Field3.Min.X,Y,Z
                VariableType.FLOAT, VariableType.FLOAT, VariableType.FLOAT, // tCachedValues.Field3.Max.X,Y,Z
                VariableType.FLOAT, VariableType.FLOAT, VariableType.FLOAT, // tCachedValues.Field4.Min.X,Y,Z
                VariableType.FLOAT, VariableType.FLOAT, VariableType.FLOAT, // tCachedValues.Field4.Max.X,Y,Z
                VariableType.INTEGER, VariableType.INTEGER, VariableType.INTEGER, VariableType.INTEGER, // Field5[0,1,2,3]
                VariableType.INTEGER, // tCachedValues.Field6
                VariableType.FLOAT, VariableType.FLOAT, VariableType.FLOAT, // Transform.Rotation.Axis.X,Y,Z
                VariableType.FLOAT, VariableType.FLOAT, VariableType.FLOAT, // Transform.ReferencePoint.X,Y,Z
                VariableType.FLOAT,   // Transform.Rotation.Amount
                VariableType.INTEGER // snoSceneGroup
            },
            false
        );

        private Dictionary<int, string> Paths;

        public Mooege.Core.GS.Game.Game Game { get; private set; }

        public WorldGenerator(Mooege.Core.GS.Game.Game game)
        {
            this.Game = game;
            this.Paths = new Dictionary<int, string>();
            LoadList("Assets/Maps/worlds.txt");
        }

        public World GenerateWorld(int worldSNO)
        {
            if (HasWorldPath(worldSNO))
            {
                string path = GetWorldPath(worldSNO);
                try
                {
                    Node root = ScriptFormatter.LoadFromFile(path);
                    return LoadNode(root);
                }
                catch (Exception e)
                {
                    Logger.ErrorException(e, "Failed to load world from {0}", path);
                }
            }
            else
            {
                Logger.Error("No file has been mapped for WorldSNO {0}", worldSNO);
            }
            return null;
        }

        private World LoadNode(Node root)
        {
            IntVariable ivar = (IntVariable)_tplWorldSNO.GetMatchingValue(root);
            if (ivar == null)
                throw new Exception(String.Format("Could not find value {0} in script", _tplWorldSNO.Identity[0]));
            World world = new World(this.Game, ivar.Value);

            Identifier iden = _tplStartPos.GetMatchingIdentifier(root);
            if (iden == null)
                throw new Exception(String.Format("Could not find identifier {0} in script", _tplStartPos.Identity[0]));
            //world.StartPosition.X = iden.GetFloat(0).Value;
            //world.StartPosition.Y = iden.GetFloat(1).Value;
            //world.StartPosition.Z = iden.GetFloat(2).Value;

            string name = "scenes";
            Node node = root.GetNode(name, false);
            if (node == null)
                throw new Exception(String.Format("Could not find node {0} in script", name));
            LoadWorldScenes(node, world);
            world.SortScenes();
            return world;
        }

        private void LoadWorldScenes(Node root, World world)
        {
            foreach (Variable v in root)
            {
                if (v is Node)
                {
                    LoadScene((Node)v, world);
                }
            }
        }

        private void LoadScene(Node root, World world)
        {
            Identifier iden = _tplSceneUnified.GetMatchingIdentifier(root);
            if (iden == null)
                throw new Exception(String.Format("Could not find identifier {0} in script", _tplSceneUnified.Identity[0]));
            Scene scene = CreateScene(iden, world, null);

            Node subs = root.GetNode("subs");
            if (subs != null)
            {
                foreach (Variable v in subs)
                {
                    iden = v as Identifier;
                    if (_tplSceneUnified.ValidateIdentifier(iden))
                        CreateScene(iden, world, scene);
                    else
                        Logger.Warn("Unrecognized variable named \"{0}\" in sub-scene node", v.Name);
                }
            }
        }

        private Scene CreateScene(Identifier iden, World world, Scene parent)
        {
            int i = 0;
            Scene scene = new Scene(world, iden.GetInt(i++).Value, parent);
            scene.MiniMapVisibility = 2;
            if (!SNODatabase.Instance.IsOfGroup(scene.SceneSNO, SNOGroup.Scenes))
            {
                Logger.Warn("SceneSNO {0} doesn't appear to be a valid SNO ID..", scene.SceneSNO);
            }
            if (parent != null)
                parent.Subscenes.Add(scene);
            scene.Specification = new SceneSpecification
            {
                CellZ = iden.GetInt(i++).Value,
                Cell = new Vector2D
                {
                    X = iden.GetInt(i++).Value,
                    Y = iden.GetInt(i++).Value
                },
                SNOLevelAreas = new int[4]
                {
                    iden.GetInt(i++).Value,
                    iden.GetInt(i++).Value,
                    iden.GetInt(i++).Value,
                    iden.GetInt(i++).Value
                },
                SNOPrevWorld        = iden.GetInt(i++).Value,
                Unknown1              = iden.GetInt(i++).Value,
                SNOPrevLevelArea    = iden.GetInt(i++).Value,
                SNONextWorld        = iden.GetInt(i++).Value,
                Unknown2              = iden.GetInt(i++).Value,
                SNONextLevelArea    = iden.GetInt(i++).Value,
                SNOMusic            = iden.GetInt(i++).Value,
                SNOCombatMusic      = iden.GetInt(i++).Value,
                SNOAmbient          = iden.GetInt(i++).Value,
                SNOReverb           = iden.GetInt(i++).Value,
                SNOWeather          = iden.GetInt(i++).Value,
                SNOPresetWorld      = iden.GetInt(i++).Value,
                Unknown3             = iden.GetInt(i++).Value,
                Unknown4             = iden.GetInt(i++).Value,
                Unknown5             = iden.GetInt(i++).Value,
                ClusterID             = iden.GetInt(i++).Value,
                SceneCachedValues = new SceneCachedValues
                {
                    Unknown1 = iden.GetInt(i++).Value,
                    Unknown2 = iden.GetInt(i++).Value,
                    Unknown3 = iden.GetInt(i++).Value,
                    AABB1 = new AABB
                    {
                        Min = new Vector3D
                        {
                            X = iden.GetFloat(i++).Value,
                            Y = iden.GetFloat(i++).Value,
                            Z = iden.GetFloat(i++).Value
                        },
                        Max = new Vector3D
                        {
                            X = iden.GetFloat(i++).Value,
                            Y = iden.GetFloat(i++).Value,
                            Z = iden.GetFloat(i++).Value
                        },
                    },
                    AABB2 = new AABB
                    {
                        Min = new Vector3D
                        {
                            X = iden.GetFloat(i++).Value,
                            Y = iden.GetFloat(i++).Value,
                            Z = iden.GetFloat(i++).Value
                        },
                        Max = new Vector3D
                        {
                            X = iden.GetFloat(i++).Value,
                            Y = iden.GetFloat(i++).Value,
                            Z = iden.GetFloat(i++).Value
                        },
                    },
                    Unknown4 = new int[4]
                    {
                        iden.GetInt(i++).Value,
                        iden.GetInt(i++).Value,
                        iden.GetInt(i++).Value,
                        iden.GetInt(i++).Value
                    },
                    Unknown5 = iden.GetInt(i++).Value
                },
            };
            scene.RotationAxis.X = iden.GetFloat(i++).Value;
            scene.RotationAxis.Y = iden.GetFloat(i++).Value;
            scene.RotationAxis.Z = iden.GetFloat(i++).Value;
            scene.RotationAmount = iden.GetFloat(i++).Value;
            scene.Position.X = iden.GetFloat(i++).Value;
            scene.Position.Y = iden.GetFloat(i++).Value;
            scene.Position.Z = iden.GetFloat(i++).Value;
            scene.SceneGroupSNO = iden.GetInt(i++).Value;
            //scene.AppliedLabels = new int[0];
            return scene;
        }

        private bool HasWorldPath(int worldSNO)
        {
            return this.Paths.ContainsKey(worldSNO);
        }

        private string GetWorldPath(int worldSNO)
        {
            return this.Paths[worldSNO];
        }

        private void LoadList(string listPath)
        {
            var owd = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory(Path.GetDirectoryName(listPath));
            try
            {
                Node root = ScriptFormatter.LoadFromFile(Path.GetFileName(listPath));
                LoadListNode(root);
            }
            catch (Exception e)
            {
                Logger.ErrorException(e, "Failed to load path list from {0}", listPath);
            }
            Directory.SetCurrentDirectory(owd);
        }

        private void LoadListNode(Node root)
        {
            foreach (Variable v in root)
            {
                IntVariable val = v as IntVariable;
                if (_tplWorldPath.ValidateValue(val))
                    this.Paths[val.Value] = Path.Combine(Directory.GetCurrentDirectory(), val.Name);
                else
                    Logger.Warn("Unrecognized variable named \"{0}\" in path list", v.Name);
            }
        }
    }
}
