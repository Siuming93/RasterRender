﻿using System;

namespace RasterRender.Engine.Mathf
{
    public struct Vector2
    {
        public float x, y;

        public float this[int index]
        {
            get { return index == 0 ? x : y; }
        }

        public Vector2(float x = 0, float y = 0)
        {
            this.x = x;
            this.y = y;
        }

        public static implicit operator Vector2(Vector3 v)
        {
            return new Vector2(v.x, v.y);
        }
    }

    public struct Vector3
    {
        public float x, y, z;

        public float this[int index]
        {
            get { return index == 0 ? x : index == 1 ? y : z; }
        }

        public Vector3(float x = 0, float y = 0, float z = 0)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public float Length()
        {
            return (float)Math.Sqrt(this.x * this.x + this.y * this.y + this.z * this.z);
        }

        public Vector3 Normalize()
        {
            float length_inv = 1 / this.Length();
            return new Vector3(this.x * length_inv, this.y * length_inv, this.z * length_inv);
        }

        public static Vector3 zero = new Vector3();
    }

    public struct Vector4
    {
        public float x, y, z, w;

        public float this[int index]
        {
            get { return index == 0 ? x : index == 1 ? y : index == 2 ? z : w; }
        }

        public Vector4(float x = 0, float y = 0, float z = 0, float w = 0)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public static implicit operator Vector4(Vector3 v)
        {
            return new Vector4(v.x, v.y, v.z, 0);
        }

        public static Vector4 operator -(Vector4 a, Vector4 b)
        {
            return new Vector4(b.x - a.x, b.y - a.y, b.z - a.z, 1.0f);
        }

        public static Vector4 operator *(Vector4 a, Vector4 b)
        {
            return new Vector4(b.z*a.y - a.z*b.y, a.z*b.x-a.x*b.z, b.z - a.z, 1.0f);
        }
    }
}
