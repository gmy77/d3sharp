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
using Mooege.Core.GS.Common.Types.Math;
using Mooege.Core.GS.Common.Types.Misc;

namespace Mooege.Core.GS.Powers
{
    // Collection of useful functions that operate on things Powers work with.
    // alot of it probably should be factored into the Vector3D, Actor, etc classes.
    public class PowerMath
    {
        // 2d vector class for effects/collisions and such
        // NOTE: not a complete vector class, just adding stuff as needed.
        public class Vec2D
        {
            public float X;
            public float Y;

            public Vec2D(float x, float y)
            {
                X = x;
                Y = y;
            }

            public Vec2D(Vector2F v)
            {
                X = v.X;
                Y = v.Y;
            }

            public static Vec2D WithoutZ(Vector3D v)
            {
                return new Vec2D(v.X, v.Y);
            }

            public Vec2D Copy()
            {
                return new Vec2D(X, Y);
            }

            public Vector3D ToVector3D(float z = 0f)
            {
                return new Vector3D(X, Y, z);
            }

            public Vector2F ToVector2F()
            {
                return new Vector2F(X, Y);
            }

            public static Vec2D operator +(Vec2D a, Vec2D b)
            {
                return new Vec2D(a.X + b.X, a.Y + b.Y);
            }

            public static Vec2D operator -(Vec2D a, Vec2D b)
            {
                return new Vec2D(a.X - b.X, a.Y - b.Y);
            }

            public static Vec2D operator *(Vec2D a, Vec2D b)
            {
                return new Vec2D(a.X * b.X, a.Y * b.Y);
            }

            public static Vec2D operator *(Vec2D a, float s)
            {
                return new Vec2D(a.X * s, a.Y * s);
            }

            public Vec2D Normalize()
            {
                float length = Length();
                if (length == 0)
                    return new Vec2D(0, 0);

                Vec2D v = Copy();
                float len = 1f / Length();
                v.X *= len;
                v.Y *= len;
                return v;
            }

            public float Dot(Vec2D v)
            {
                return X * v.X + Y * v.Y;
            }

            public float Length()
            {
                return (float)Math.Sqrt(X*X + Y*Y);
            }
        }

        #region Math helpers

        public static Vector3D Normalize(Vector3D v)
        {
            float mag = v.X * v.X + v.Y * v.Y + v.Z * v.Z;
            if (mag == 0)
                return new Vector3D(0, 0, 0);

            Vector3D r = new Vector3D(v);
            float len = 1f / (float)Math.Sqrt(mag);
            r.X *= len;
            r.Y *= len;
            r.Z *= len;
            return r;
        }

        public static float Distance(Vector3D a, Vector3D b)
        {
            return (float)Math.Sqrt(Math.Pow(a.X - b.X, 2) +
                                    Math.Pow(a.Y - b.Y, 2) +
                                    Math.Pow(a.Z - b.Z, 2));
        }

        public static float Distance2D(Vector3D a, Vector3D b)
        {
            return (float)Math.Sqrt(Math.Pow(a.X - b.X, 2) +
                                    Math.Pow(a.Y - b.Y, 2));
        }

        public static float AngleLookAt(Vector3D source, Vector3D target)
        {
            return (float)Math.Atan2(target.Y - source.Y, target.X - source.X);
        }

        public static Vector3D ProjectAndTranslate2D(Vector3D a, Vector3D b, Vector3D position, float amount)
        {
            Vector3D r = new Vector3D(position);
            Vector3D ab_norm = Normalize(new Vector3D(b.X - a.X, b.Y - a.Y, 0/*b.Z - a.Z*/)); // 2D
            r.X += ab_norm.X * amount;
            r.Y += ab_norm.Y * amount;
            //r.Z += ab_norm.Z * amount;
            return r;
        }

        public static bool PointInBeam2D(Vec2D point, Vec2D beamStart, Vec2D beamEnd, float beamThickness)
        {
            // create rectangle from points with specified thickness
            Vec2D beam_diff = beamStart - beamEnd;
            Vec2D beam_norm = beam_diff.Normalize();

            float length = beam_diff.Length();

            float angX = -beam_norm.Y;
            float angY = beam_norm.X;

            // rectangle points
            float p1x = beamEnd.X - angX * beamThickness / 2;
            float p1y = beamEnd.Y - angY * beamThickness / 2;
            float p2x = p1x + angX * beamThickness;
            float p2y = p1y + angY * beamThickness;
            float p3x = p1x + beam_norm.X * length;
            float p3y = p1y + beam_norm.Y * length;
            float p4x = p3x + angX * beamThickness;
            float p4y = p3y + angY * beamThickness;

            return PointInRectangle(point, p1x, p1y, p2x, p2y, p3x, p3y, p4x, p4y);
        }

        public static bool PointInBeam(Vector3D point, Vector3D beamStart, Vector3D beamEnd, float beamThickness)
        {
            // NOTE: this does everything in 2d, ignoring Z
            // offset start beam position by beam thickness
            beamStart = ProjectAndTranslate2D(beamStart, beamEnd, beamStart, beamThickness);
            return MovingCircleCollides(new Circle(beamStart.X, beamStart.Y, beamThickness),
                                        Vec2D.WithoutZ(beamEnd - beamStart),
                                        new Circle(point.X, point.Y, 0));
        }

        public static bool PointInRectangle(Vec2D point, float r1x, float r1y,
                                                         float r2x, float r2y,
                                                         float r3x, float r3y,
                                                         float r4x, float r4y)
        {
            Vec2D corner = new Vec2D(r1x, r1y);
            Vec2D local_point = point - corner;
            Vec2D side1 = new Vec2D(r2x, r2y) - corner;
            Vec2D side2 = new Vec2D(r4x, r4y) - corner;
            return (0 <= local_point.Dot(side1) &&
                    local_point.Dot(side1) <= side1.Dot(side1) &&
                    0 <= local_point.Dot(side2) &&
                    local_point.Dot(side2) <= side2.Dot(side2));
        }
        
        public static Vec2D ClosestPointOnLineSegment(Vec2D lineStart, Vec2D lineEnd, Vec2D point)
        {
            Vec2D line = lineEnd - lineStart;
            float lineMag = line.X * line.X + line.Y * line.Y;
            if (lineMag == 0f)
                return lineStart.Copy();

            float t = (point - lineStart).Dot(line) / lineMag;
            t = Math.Min(Math.Max(0, t), 1);
            return lineStart + line * t;
        }

        public static bool MovingCircleCollides(Circle mover, Vec2D velocity, Circle target)
        {
            Vec2D closest = ClosestPointOnLineSegment(new Vec2D(mover.Center),
                                                      new Vec2D(mover.Center) + velocity,
                                                      new Vec2D(target.Center));
            float distanceSq = (float)(Math.Pow(closest.X - target.Center.X, 2) + Math.Pow(closest.Y - target.Center.Y, 2));
            return distanceSq <= (float)Math.Pow(mover.Radius + target.Radius, 2);
        }                        

        #endregion // Math helpers
    }
}
