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
    public static class PowerMath
    {
        #region Vector operations

        public static Vector2F VectorWithoutZ(Vector3D v)
        {
            return new Vector2F(v.X, v.Y);
        }

        public static Vector3D VectorWithZ(Vector2F v, float z)
        {
            return new Vector3D(v.X, v.Y, z);
        }

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

        public static Vector3D TranslateDirection(Vector3D source, Vector3D destination, Vector3D point, float amount)
        {
            Vector3D norm = Normalize(new Vector3D(destination.X - source.X, destination.Y - source.Y,
                                                   destination.Z - source.Z));
            return new Vector3D(point.X + norm.X * amount,
                                point.Y + norm.Y * amount,
                                point.Z + norm.Z * amount);
        }

        public static Vector3D TranslateDirection2D(Vector3D source, Vector3D destination, Vector3D point, float amount)
        {
            Vector3D norm = Normalize(new Vector3D(destination.X - source.X, destination.Y - source.Y, 0f));
            return new Vector3D(point.X + norm.X * amount,
                                point.Y + norm.Y * amount,
                                point.Z);
        }

        public static Vector3D VectorRotateZ(Vector3D v, float radians)
        {
            float cosRad = (float)Math.Cos(radians);
            float sinRad = (float)Math.Sin(radians);

            return new Vector3D(v.X * cosRad - v.Y * sinRad,
                                v.X * sinRad + v.Y * cosRad,
                                v.Z);
        }

        public const float DegreesToRadians = (float)(Math.PI / 180.0);

        public static Vector3D[] GenerateSpreadPositions(Vector3D center, Vector3D targetPosition, float spacingDegrees, int count)
        {
            Vector3D baseRotation = targetPosition - center;
            float spacing = spacingDegrees * DegreesToRadians;
            float median = count % 2 == 0 ? spacing * (count + 1) / 2.0f : spacing * (float)Math.Ceiling(count / 2.0f);
            Vector3D[] output = new Vector3D[count];

            float offset = 1f;
            for (int i = 0; i < count; ++i)
            {
                output[i] = center + VectorRotateZ(baseRotation, offset * spacing - median);
                offset += 1f;
            }

            return output;
        }

        #endregion

        #region Geometry intersection/collision tests

        public static bool PointInRotatedRect(Vector2F point, Vector2F rectMidpointStart, Vector2F rectMidpointEnd, float rectHeight)
        {
            // create rectangle lengthwise from points, with specified height
            Vector2F beam_diff = rectMidpointStart - rectMidpointEnd;
            Vector2F beam_norm = Vector2F.Normalize(beam_diff);

            float length = beam_diff.Length();

            float angX = -beam_norm.Y;
            float angY = beam_norm.X;

            // rectangle points
            float p1x = rectMidpointEnd.X - angX * rectHeight / 2;
            float p1y = rectMidpointEnd.Y - angY * rectHeight / 2;
            float p2x = p1x + angX * rectHeight;
            float p2y = p1y + angY * rectHeight;
            float p3x = p1x + beam_norm.X * length;
            float p3y = p1y + beam_norm.Y * length;
            float p4x = p3x + angX * rectHeight;
            float p4y = p3y + angY * rectHeight;

            return PointInRectangle(point, p1x, p1y, p2x, p2y, p3x, p3y, p4x, p4y);
        }

        public static bool PointInBeam(Vector3D point, Vector3D beamStart, Vector3D beamEnd, float beamThickness)
        {
            return CircleInBeam(new Circle(point.X, point.Y, 0), beamStart, beamEnd, beamThickness);
        }

        public static bool CircleInBeam(Circle circle, Vector3D beamStart, Vector3D beamEnd, float beamThickness)
        {
            // NOTE: right now this does everything in 2d, ignoring Z

            // offset start beam position by beam thickness
            beamStart = TranslateDirection2D(beamStart, beamEnd, beamStart, beamThickness);

            return MovingCircleCollides(new Circle(beamStart.X, beamStart.Y, beamThickness),
                                        VectorWithoutZ(beamEnd - beamStart),
                                        circle);
        }

        public static bool PointInRectangle(Vector2F point, float r1x, float r1y,
                                                            float r2x, float r2y,
                                                            float r3x, float r3y,
                                                            float r4x, float r4y)
        {
            Vector2F corner = new Vector2F(r1x, r1y);
            Vector2F local_point = point - corner;
            Vector2F side1 = new Vector2F(r2x, r2y) - corner;
            Vector2F side2 = new Vector2F(r4x, r4y) - corner;
            return (0 <= Vector2F.Dot(local_point, side1) &&
                    Vector2F.Dot(local_point, side1) <= Vector2F.Dot(side1, side1) &&
                    0 <= Vector2F.Dot(local_point, side2) &&
                    Vector2F.Dot(local_point, side2) <= Vector2F.Dot(side2, side2));
        }

        public static Vector2F ClosestPointOnLineSegment(Vector2F point, Vector2F lineStart, Vector2F lineEnd)
        {
            Vector2F line = lineEnd - lineStart;
            float lineMag = line.X * line.X + line.Y * line.Y;
            if (lineMag == 0f)
                return lineStart;

            float t = Vector2F.Dot(point - lineStart, line) / lineMag;
            t = Math.Min(Math.Max(0, t), 1);
            return lineStart + line * t;
        }

        public static bool MovingCircleCollides(Circle mover, Vector2F velocity, Circle target)
        {
            Vector2F closest = ClosestPointOnLineSegment(target.Center,
                                                         mover.Center,
                                                         mover.Center + velocity);
            float distanceSq = (float)(Math.Pow(closest.X - target.Center.X, 2) + Math.Pow(closest.Y - target.Center.Y, 2));
            return distanceSq <= (float)Math.Pow(mover.Radius + target.Radius, 2);
        }

        public static bool ArcCircleCollides(Vector2F arcCenter, Vector2F arcDirection, float arcRadius, float arcLength, Circle circle)
        {
            Vector2F arcDelta = arcDirection - arcCenter;
            Vector2F circleDelta = circle.Center - arcCenter;
            if (arcDelta == circleDelta)
                return true;

            if (arcDelta.Angle(circleDelta) < arcLength / 2f)
                return Vector2F.Distance(arcCenter, circle.Center) <= arcRadius + circle.Radius;
            else
                return false;
        }

        #endregion
    }
}
