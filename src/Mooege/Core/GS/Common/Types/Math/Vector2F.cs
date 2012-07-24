﻿/*
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
using System.Globalization;

namespace Mooege.Core.GS.Common.Types.Math
{
    public struct Vector2F : IEquatable<Vector2F>
    {
        public float X;
        public float Y;
        private static Vector2F _zero;
        private static Vector2F _one;
        private static Vector2F _unitX;
        private static Vector2F _unitY;

        public static Vector2F Zero
        {
            get { return _zero; }
        }

        public static Vector2F One
        {
            get { return _one; }
        }

        public static Vector2F UnitX
        {
            get { return _unitX; }
        }

        public static Vector2F UnitY
        {
            get { return _unitY; }
        }

        public Vector2F(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        public Vector2F(float value)
        {
            this.X = this.Y = value;
        }

        public override string ToString()
        {
            CultureInfo currentCulture = CultureInfo.CurrentCulture;
            return string.Format(currentCulture, "{{X:{0} Y:{1}}}",
                                 new object[] {this.X.ToString(currentCulture), this.Y.ToString(currentCulture)});
        }

        public bool Equals(Vector2F other)
        {
            return ((this.X == other.X) && (this.Y == other.Y));
        }

        public override bool Equals(object obj)
        {
            bool flag = false;
            if (obj is Vector2F)
            {
                flag = this.Equals((Vector2F) obj);
            }
            return flag;
        }

        public override int GetHashCode()
        {
            return (this.X.GetHashCode() + this.Y.GetHashCode());
        }

        public float Length()
        {
            float num = (this.X*this.X) + (this.Y*this.Y);
            return (float) System.Math.Sqrt((double) num);
        }

        public float LengthSquared()
        {
            return ((this.X*this.X) + (this.Y*this.Y));
        }

        public static float Distance(Vector2F value1, Vector2F value2)
        {
            float num2 = value1.X - value2.X;
            float num = value1.Y - value2.Y;
            float num3 = (num2*num2) + (num*num);
            return (float) System.Math.Sqrt((double) num3);
        }

        public static void Distance(ref Vector2F value1, ref Vector2F value2, out float result)
        {
            float num2 = value1.X - value2.X;
            float num = value1.Y - value2.Y;
            float num3 = (num2*num2) + (num*num);
            result = (float) System.Math.Sqrt((double) num3);
        }

        public static float DistanceSquared(Vector2F value1, Vector2F value2)
        {
            float num2 = value1.X - value2.X;
            float num = value1.Y - value2.Y;
            return ((num2*num2) + (num*num));
        }

        public static void DistanceSquared(ref Vector2F value1, ref Vector2F value2, out float result)
        {
            float num2 = value1.X - value2.X;
            float num = value1.Y - value2.Y;
            result = (num2*num2) + (num*num);
        }

        public static float Dot(Vector2F value1, Vector2F value2)
        {
            return ((value1.X*value2.X) + (value1.Y*value2.Y));
        }

        public static void Dot(ref Vector2F value1, ref Vector2F value2, out float result)
        {
            result = (value1.X*value2.X) + (value1.Y*value2.Y);
        }

        public void Normalize()
        {
            float num2 = (this.X*this.X) + (this.Y*this.Y);
            float num = 1f/((float) System.Math.Sqrt((double) num2));
            this.X *= num;
            this.Y *= num;
        }

        public static Vector2F Normalize(Vector2F value)
        {
            Vector2F vector;
            float num2 = (value.X*value.X) + (value.Y*value.Y);
            float num = 1f/((float) System.Math.Sqrt((double) num2));
            vector.X = value.X*num;
            vector.Y = value.Y*num;
            return vector;
        }

        public static void Normalize(ref Vector2F value, out Vector2F result)
        {
            float num2 = (value.X*value.X) + (value.Y*value.Y);
            float num = 1f/((float) System.Math.Sqrt((double) num2));
            result.X = value.X*num;
            result.Y = value.Y*num;
        }

        /// <summary>
        /// Returns the angle in radians between this vector and another vector
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public float Angle(Vector2F other)
        {
            return (float)System.Math.Acos(Dot(this, other) / Length() / other.Length());
        }

        /// <summary>
        /// Returns the rotation of this vector to the x unity vector in radians
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public float Rotation()
        {
            return Angle(Vector2F.UnitY) > Angle(-Vector2F.UnitY) ? -Angle(Vector2F.UnitX) : Angle(Vector2F.UnitX);
        }


        public static Vector2F Reflect(Vector2F vector, Vector2F normal)
        {
            Vector2F vector2F;
            float num = (vector.X*normal.X) + (vector.Y*normal.Y);
            vector2F.X = vector.X - ((2f*num)*normal.X);
            vector2F.Y = vector.Y - ((2f*num)*normal.Y);
            return vector2F;
        }

        public static void Reflect(ref Vector2F vector, ref Vector2F normal, out Vector2F result)
        {
            float num = (vector.X*normal.X) + (vector.Y*normal.Y);
            result.X = vector.X - ((2f*num)*normal.X);
            result.Y = vector.Y - ((2f*num)*normal.Y);
        }

        public static Vector2F Min(Vector2F value1, Vector2F value2)
        {
            Vector2F vector;
            vector.X = (value1.X < value2.X) ? value1.X : value2.X;
            vector.Y = (value1.Y < value2.Y) ? value1.Y : value2.Y;
            return vector;
        }

        public static void Min(ref Vector2F value1, ref Vector2F value2, out Vector2F result)
        {
            result.X = (value1.X < value2.X) ? value1.X : value2.X;
            result.Y = (value1.Y < value2.Y) ? value1.Y : value2.Y;
        }

        public static Vector2F Max(Vector2F value1, Vector2F value2)
        {
            Vector2F vector;
            vector.X = (value1.X > value2.X) ? value1.X : value2.X;
            vector.Y = (value1.Y > value2.Y) ? value1.Y : value2.Y;
            return vector;
        }

        public static void Max(ref Vector2F value1, ref Vector2F value2, out Vector2F result)
        {
            result.X = (value1.X > value2.X) ? value1.X : value2.X;
            result.Y = (value1.Y > value2.Y) ? value1.Y : value2.Y;
        }

        public static Vector2F Clamp(Vector2F value1, Vector2F min, Vector2F max)
        {
            Vector2F vector;
            float x = value1.X;
            x = (x > max.X) ? max.X : x;
            x = (x < min.X) ? min.X : x;
            float y = value1.Y;
            y = (y > max.Y) ? max.Y : y;
            y = (y < min.Y) ? min.Y : y;
            vector.X = x;
            vector.Y = y;
            return vector;
        }

        public static void Clamp(ref Vector2F value1, ref Vector2F min, ref Vector2F max, out Vector2F result)
        {
            float x = value1.X;
            x = (x > max.X) ? max.X : x;
            x = (x < min.X) ? min.X : x;
            float y = value1.Y;
            y = (y > max.Y) ? max.Y : y;
            y = (y < min.Y) ? min.Y : y;
            result.X = x;
            result.Y = y;
        }

        public static Vector2F Lerp(Vector2F value1, Vector2F value2, float amount)
        {
            Vector2F vector;
            vector.X = value1.X + ((value2.X - value1.X)*amount);
            vector.Y = value1.Y + ((value2.Y - value1.Y)*amount);
            return vector;
        }

        public static void Lerp(ref Vector2F value1, ref Vector2F value2, float amount, out Vector2F result)
        {
            result.X = value1.X + ((value2.X - value1.X)*amount);
            result.Y = value1.Y + ((value2.Y - value1.Y)*amount);
        }
     
        public static Vector2F Negate(Vector2F value)
        {
            Vector2F vector;
            vector.X = -value.X;
            vector.Y = -value.Y;
            return vector;
        }

        public static void Negate(ref Vector2F value, out Vector2F result)
        {
            result.X = -value.X;
            result.Y = -value.Y;
        }

        public static Vector2F Add(Vector2F value1, Vector2F value2)
        {
            Vector2F vector;
            vector.X = value1.X + value2.X;
            vector.Y = value1.Y + value2.Y;
            return vector;
        }

        public static void Add(ref Vector2F value1, ref Vector2F value2, out Vector2F result)
        {
            result.X = value1.X + value2.X;
            result.Y = value1.Y + value2.Y;
        }

        public static Vector2F Subtract(Vector2F value1, Vector2F value2)
        {
            Vector2F vector;
            vector.X = value1.X - value2.X;
            vector.Y = value1.Y - value2.Y;
            return vector;
        }

        public static void Subtract(ref Vector2F value1, ref Vector2F value2, out Vector2F result)
        {
            result.X = value1.X - value2.X;
            result.Y = value1.Y - value2.Y;
        }

        public static Vector2F Multiply(Vector2F value1, Vector2F value2)
        {
            Vector2F vector;
            vector.X = value1.X*value2.X;
            vector.Y = value1.Y*value2.Y;
            return vector;
        }

        public static void Multiply(ref Vector2F value1, ref Vector2F value2, out Vector2F result)
        {
            result.X = value1.X*value2.X;
            result.Y = value1.Y*value2.Y;
        }

        public static Vector2F Multiply(Vector2F value1, float scaleFactor)
        {
            Vector2F vector;
            vector.X = value1.X*scaleFactor;
            vector.Y = value1.Y*scaleFactor;
            return vector;
        }

        public static void Multiply(ref Vector2F value1, float scaleFactor, out Vector2F result)
        {
            result.X = value1.X*scaleFactor;
            result.Y = value1.Y*scaleFactor;
        }

        public static Vector2F Divide(Vector2F value1, Vector2F value2)
        {
            Vector2F vector;
            vector.X = value1.X/value2.X;
            vector.Y = value1.Y/value2.Y;
            return vector;
        }

        public static void Divide(ref Vector2F value1, ref Vector2F value2, out Vector2F result)
        {
            result.X = value1.X/value2.X;
            result.Y = value1.Y/value2.Y;
        }

        public static Vector2F Divide(Vector2F value1, float divider)
        {
            Vector2F vector;
            float num = 1f/divider;
            vector.X = value1.X*num;
            vector.Y = value1.Y*num;
            return vector;
        }

        public static void Divide(ref Vector2F value1, float divider, out Vector2F result)
        {
            float num = 1f/divider;
            result.X = value1.X*num;
            result.Y = value1.Y*num;
        }

        public static Vector2F operator -(Vector2F value)
        {
            Vector2F vector;
            vector.X = -value.X;
            vector.Y = -value.Y;
            return vector;
        }

        public static bool operator ==(Vector2F value1, Vector2F value2)
        {
            return ((value1.X == value2.X) && (value1.Y == value2.Y));
        }

        public static bool operator !=(Vector2F value1, Vector2F value2)
        {
            if (value1.X == value2.X)
            {
                return !(value1.Y == value2.Y);
            }
            return true;
        }

        public static Vector2F operator +(Vector2F value1, Vector2F value2)
        {
            Vector2F vector;
            vector.X = value1.X + value2.X;
            vector.Y = value1.Y + value2.Y;
            return vector;
        }

        public static Vector2F operator -(Vector2F value1, Vector2F value2)
        {
            Vector2F vector;
            vector.X = value1.X - value2.X;
            vector.Y = value1.Y - value2.Y;
            return vector;
        }

        public static Vector2F operator *(Vector2F value1, Vector2F value2)
        {
            Vector2F vector;
            vector.X = value1.X*value2.X;
            vector.Y = value1.Y*value2.Y;
            return vector;
        }

        public static Vector2F operator *(Vector2F value, float scaleFactor)
        {
            Vector2F vector;
            vector.X = value.X*scaleFactor;
            vector.Y = value.Y*scaleFactor;
            return vector;
        }

        public static Vector2F operator *(float scaleFactor, Vector2F value)
        {
            Vector2F vector;
            vector.X = value.X*scaleFactor;
            vector.Y = value.Y*scaleFactor;
            return vector;
        }

        public static Vector2F operator /(Vector2F value1, Vector2F value2)
        {
            Vector2F vector;
            vector.X = value1.X/value2.X;
            vector.Y = value1.Y/value2.Y;
            return vector;
        }

        public static Vector2F operator /(Vector2F value1, float divider)
        {
            Vector2F vector;
            float num = 1f/divider;
            vector.X = value1.X*num;
            vector.Y = value1.Y*num;
            return vector;
        }

        static Vector2F()
        {
            _zero = new Vector2F();
            _one = new Vector2F(1f, 1f);
            _unitX = new Vector2F(1f, 0f);
            _unitY = new Vector2F(0f, 1f);
        }
    }
}
