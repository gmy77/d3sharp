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

using Mooege.Core.GS.Universe;
using Mooege.Net.GS.Message.Definitions.ACD;
using Mooege.Net.GS.Message.Definitions.Misc;
using Mooege.Net.GS.Message.Fields;

namespace Mooege.Core.GS.Actors
{
    public class Actor
    {
        public int Id;
        public int SnoId;
        public int WorldId;
        public int Field2;
        public int Field3;
        public float Scale;
        public float RotationAmount;
        public Vector3D RotationAxis;
        public Vector3D Position = new Vector3D();
        public InventoryLocationMessageData InventoryLocationData;
        public GBHandle GBHandle;
        public int Field7;
        public int Field8;
        public int Field9;
        public byte Field10;        

        public void Reveal(Hero toon)
        {
            if (toon.RevealedActors.Contains(this)) return; //already revealed
            toon.RevealedActors.Add(this);

            var msg = new ACDEnterKnownMessage
                          {
                              Field0 = Id,
                              Field1 = SnoId,
                              Field2 = Field2,
                              Field3 = Field3,
                              Field4 = new WorldLocationMessageData
                                           {
                                               Field0 = Scale,
                                               Field1 = new PRTransform { Field0 = new Quaternion { Amount = RotationAmount, Axis = RotationAxis, }, ReferencePoint = Position, },
                                               Field2 = WorldId,
                                           },
                              Field5 = InventoryLocationData,
                              Field6 = GBHandle,
                              Field7 = Field7,
                              Field8 = Field8,
                              Field9 = Field9,
                              Field10 = Field10,
                          };

            toon.InGameClient.SendMessage(msg);
            toon.InGameClient.FlushOutgoingBuffer();
        }

        public void Destroy(Hero hero)
        {
            if (!hero.RevealedActors.Contains(this)) return; //not revealed yet

            hero.InGameClient.SendMessage(new ANNDataMessage() { Id=0x3c, Field0=Id, });
            hero.InGameClient.FlushOutgoingBuffer();
            hero.RevealedActors.Remove(this);
        }

        public bool ParseFrom(int worldId, string line)
        {
            var data = line.Split(' ');

            if (int.Parse(data[2]) == 0) return false; //skip inventory using items as their use is unknown

            this.WorldId = worldId;

            this.Id = int.Parse(data[4]);            
            this.SnoId = int.Parse(data[5]);
            this.Field2 = int.Parse(data[6]);
            this.Field3 = int.Parse(data[7]);

            if (int.Parse(data[2]) > 0)
            {
                this.Scale = float.Parse(data[8], System.Globalization.CultureInfo.InvariantCulture);
                this.RotationAmount = float.Parse(data[12], System.Globalization.CultureInfo.InvariantCulture);
                this.RotationAxis = new Vector3D()
                                        {
                                            X = float.Parse(data[9], System.Globalization.CultureInfo.InvariantCulture),
                                            Y = float.Parse(data[10], System.Globalization.CultureInfo.InvariantCulture),
                                            Z = float.Parse(data[11], System.Globalization.CultureInfo.InvariantCulture),
                                        };

                this.Position = new Vector3D()
                                    {
                                        X = float.Parse(data[13], System.Globalization.CultureInfo.InvariantCulture),
                                        Y = float.Parse(data[14], System.Globalization.CultureInfo.InvariantCulture),
                                        Z = float.Parse(data[15], System.Globalization.CultureInfo.InvariantCulture),
                                    };
            }

            // data[14] = world_id

            if (int.Parse(data[3]) > 0)
            {
                this.InventoryLocationData = new InventoryLocationMessageData()
                {
                    Field0 = int.Parse(data[17]),
                    Field1 = int.Parse(data[18]),
                    Field2 = new IVector2D()
                    {
                        Field0 = int.Parse(data[19]),
                        Field1 = int.Parse(data[20]),
                    }
                };
            }

            this.GBHandle = new GBHandle()
            {
                Field0 = int.Parse(data[21]),
                Field1 = int.Parse(data[22]),
            };

            this.Field7 = int.Parse(data[23]);
            this.Field8 = int.Parse(data[24]);
            this.Field9 = int.Parse(data[25]);
            this.Field10 = byte.Parse(data[26]);

            return true;
        }
    }
}
