using System;
using System.Collections.Generic;

namespace RasterRender.Engine
{
    #region math
    public struct Color
    {
        public Color(float r, float g, float b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = 1f;
        }
        public float r, g, b, a;

        public override string ToString()
        {
            return string.Format("r:{0} g:{1} b:{2} a:{3}", r, g, b, a);
        }

        public static Color Lerp(Color a, Color b, float t)
        {
            return new Color()
            {
                r = MathUtil.Lerp(a.r, b.r, t),
                g = MathUtil.Lerp(a.g, b.g, t),
                b = MathUtil.Lerp(a.b, b.b, t),
                a = MathUtil.Lerp(a.a, b.a, t),
            };
        }

    }
    public struct TexCoord
    {
        public float x, y;

        public float this[int index]
        {
            get { return index == 0 ? x : y; }
        }

        public TexCoord(float x = 0, float y = 0)
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString()
        {
            return string.Format("u:{0} v:{1}", x, y);
        }

        public static implicit operator TexCoord(Vector3 v)
        {
            return new TexCoord(v.x, v.y);
        }

        public static TexCoord operator *(float f, TexCoord v)
        {
            return new TexCoord(f * v.x, f * v.y);
        }

        public static TexCoord operator +(TexCoord v1, TexCoord v2)
        {
            return new TexCoord(v1.x + v2.x, v2.y + v1.y);
        }

        public static TexCoord operator -(TexCoord v1, TexCoord v2)
        {
            return new TexCoord(v1.x - v2.x, v1.y - v2.y);
        }

        public static TexCoord Lerp(TexCoord a, TexCoord b, float t)
        {
            return new TexCoord(MathUtil.Lerp(a.x, b.x, t), MathUtil.Lerp(a.y, b.y, t));
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

        public static Vector3 operator *(float f, Vector3 v)
        {
            return new Vector3(v.x * f, v.y * f, v.z * f);
        }

        public static Vector3 operator -(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static Vector3 Lerp(Vector3 a, Vector3 b, float t)
        {
            return new Vector3(MathUtil.Lerp(a.x, b.x, t), MathUtil.Lerp(a.y, b.y, t), MathUtil.Lerp(a.z, b.z, t));
        }

        public static implicit operator Vector3(Vector4 v)
        {
            return new Vector3(v.x, v.y, v.z);
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

        public Vector4(float x = 0, float y = 0, float z = 0, float w = 1)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }
        public override string ToString()
        {
            return string.Format("x:{0} y:{1} z:{2} w:{3}", x, y, z, w);
        }
        public Vector4 Normoalize()
        {
            float x2 = (float)Math.Pow(x, 2);
            float y2 = (float)Math.Pow(y, 2);
            float z2 = (float)Math.Pow(z, 2);
            float sum = x2 + y2 + z2;
            ;
            return new Vector4()
            {
                x = x > 0 ? (float)Math.Sqrt(x2 / sum) : -(float)Math.Sqrt(x2 / sum),
                y = y > 0 ? (float)Math.Sqrt(y2 / sum) : -(float)Math.Sqrt(y2 / sum),
                z = z > 0 ? (float)Math.Sqrt(z2 / sum) : -(float)Math.Sqrt(z2 / sum),
            };
        }

        public static Vector4 Lerp(Vector4 a, Vector4 b, float t)
        {
            return new Vector4()
            {
                x = MathUtil.Lerp(a.x, b.x, t),
                y = MathUtil.Lerp(a.y, b.y, t),
                z = MathUtil.Lerp(a.z, b.z, t),
                w = 1.0f,
            };
        }

        public static implicit operator Vector4(Vector3 v)
        {
            return new Vector4(v.x, v.y, v.z, 0);
        }

        public static Vector4 operator -(Vector4 a, Vector4 b)
        {
            return new Vector4(a.x - b.x, a.y - b.y, a.z - b.z, 1.0f);
        }

        public static Vector4 operator +(Vector4 a, Vector4 b)
        {
            return new Vector4(b.x + a.x, b.y + a.y, b.z + a.z, 1.0f);
        }

        public static Vector4 operator *(Vector4 a, Vector4 b)
        {
            return new Vector4(b.z * a.y - a.z * b.y, a.z * b.x - a.x * b.z, a.x * b.y - b.x * a.y, 1.0f);
        }

        public static Vector4 operator *(float f, Vector4 v)
        {
            return new Vector4(v.x * f, v.y * f, v.z * f, v.w * f);
        }
    }
    public struct Vertex
    {
        public Vector4 pos;
        public TexCoord uv;
        public Color color;
        public float rhw;
        public override string ToString()
        {
            return pos.ToString();
        }
        public static Vertex Lerp(Vertex v1, Vertex v2, float t)
        {
            return new Vertex()
            {
                pos = Vector3.Lerp(v1.pos, v2.pos, t),
                uv = TexCoord.Lerp(v1.uv, v2.uv, t)
            };
        }
        public static Vertex Division(Vertex v1, Vertex v2, float t)
        {
            return new Vertex()
            {
                pos = 1 / t * (v2.pos - v1.pos),
                uv = 1 / t * (v2.uv - v1.uv),
            };
        }
    }

    public struct Matrix4X3
    {

    }
    public struct Matrix4x4
    {
        public float m00;
        public float m10;
        public float m20;
        public float m30;
        public float m01;
        public float m11;
        public float m21;
        public float m31;
        public float m02;
        public float m12;
        public float m22;
        public float m32;
        public float m03;
        public float m13;
        public float m23;
        public float m33;

        public float this[int row, int column]
        {
            get
            {
                return this[row + column * 4];
            }
            set
            {
                this[row + column * 4] = value;
            }
        }

        public float this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return this.m00;
                    case 1:
                        return this.m10;
                    case 2:
                        return this.m20;
                    case 3:
                        return this.m30;
                    case 4:
                        return this.m01;
                    case 5:
                        return this.m11;
                    case 6:
                        return this.m21;
                    case 7:
                        return this.m31;
                    case 8:
                        return this.m02;
                    case 9:
                        return this.m12;
                    case 10:
                        return this.m22;
                    case 11:
                        return this.m32;
                    case 12:
                        return this.m03;
                    case 13:
                        return this.m13;
                    case 14:
                        return this.m23;
                    case 15:
                        return this.m33;
                    default:
                        throw new IndexOutOfRangeException("Invalid matrix index!");
                }
            }
            set
            {
                switch (index)
                {
                    case 0:
                        this.m00 = value;
                        break;
                    case 1:
                        this.m10 = value;
                        break;
                    case 2:
                        this.m20 = value;
                        break;
                    case 3:
                        this.m30 = value;
                        break;
                    case 4:
                        this.m01 = value;
                        break;
                    case 5:
                        this.m11 = value;
                        break;
                    case 6:
                        this.m21 = value;
                        break;
                    case 7:
                        this.m31 = value;
                        break;
                    case 8:
                        this.m02 = value;
                        break;
                    case 9:
                        this.m12 = value;
                        break;
                    case 10:
                        this.m22 = value;
                        break;
                    case 11:
                        this.m32 = value;
                        break;
                    case 12:
                        this.m03 = value;
                        break;
                    case 13:
                        this.m13 = value;
                        break;
                    case 14:
                        this.m23 = value;
                        break;
                    case 15:
                        this.m33 = value;
                        break;
                    default:
                        throw new IndexOutOfRangeException("Invalid matrix index!");
                }
            }
        }

        public void Init(float m00, float m01, float m02, float m03,
            float m10, float m11, float m12, float m13,
            float m20, float m21, float m22, float m23,
            float m30, float m31, float m32, float m33)
        {
            this.m00 = m00;
            this.m01 = m01;
            this.m02 = m02;
            this.m03 = m03;

            this.m10 = m10;
            this.m11 = m11;
            this.m12 = m12;
            this.m13 = m13;

            this.m20 = m20;
            this.m21 = m21;
            this.m22 = m22;
            this.m23 = m23;

            this.m30 = m30;
            this.m31 = m31;
            this.m32 = m32;
            this.m33 = m33;
        }

        public static Matrix4x4 identity
        {
            get
            {
                return new Matrix4x4()
                {
                    m00 = 1f,
                    m01 = 0.0f,
                    m02 = 0.0f,
                    m03 = 0.0f,
                    m10 = 0.0f,
                    m11 = 1f,
                    m12 = 0.0f,
                    m13 = 0.0f,
                    m20 = 0.0f,
                    m21 = 0.0f,
                    m22 = 1f,
                    m23 = 0.0f,
                    m30 = 0.0f,
                    m31 = 0.0f,
                    m32 = 0.0f,
                    m33 = 1f,
                };
            }
        }

        public static Matrix4x4 zero
        {
            get
            {
                return new Matrix4x4()
                {
                    m00 = 0.0f,
                    m01 = 0.0f,
                    m02 = 0.0f,
                    m03 = 0.0f,
                    m10 = 0.0f,
                    m11 = 0.0f,
                    m12 = 0.0f,
                    m13 = 0.0f,
                    m20 = 0.0f,
                    m21 = 0.0f,
                    m22 = 0.0f,
                    m23 = 0.0f,
                    m30 = 0.0f,
                    m31 = 0.0f,
                    m32 = 0.0f,
                    m33 = 0.0f
                };
            }
        }

        public static Matrix4x4 operator *(Matrix4x4 lhs, Matrix4x4 rhs)
        {
            return new Matrix4x4()
            {
                m00 = (float)((double)lhs.m00 * (double)rhs.m00 + (double)lhs.m01 * (double)rhs.m10 + (double)lhs.m02 * (double)rhs.m20 + (double)lhs.m03 * (double)rhs.m30),
                m01 = (float)((double)lhs.m00 * (double)rhs.m01 + (double)lhs.m01 * (double)rhs.m11 + (double)lhs.m02 * (double)rhs.m21 + (double)lhs.m03 * (double)rhs.m31),
                m02 = (float)((double)lhs.m00 * (double)rhs.m02 + (double)lhs.m01 * (double)rhs.m12 + (double)lhs.m02 * (double)rhs.m22 + (double)lhs.m03 * (double)rhs.m32),
                m03 = (float)((double)lhs.m00 * (double)rhs.m03 + (double)lhs.m01 * (double)rhs.m13 + (double)lhs.m02 * (double)rhs.m23 + (double)lhs.m03 * (double)rhs.m33),
                m10 = (float)((double)lhs.m10 * (double)rhs.m00 + (double)lhs.m11 * (double)rhs.m10 + (double)lhs.m12 * (double)rhs.m20 + (double)lhs.m13 * (double)rhs.m30),
                m11 = (float)((double)lhs.m10 * (double)rhs.m01 + (double)lhs.m11 * (double)rhs.m11 + (double)lhs.m12 * (double)rhs.m21 + (double)lhs.m13 * (double)rhs.m31),
                m12 = (float)((double)lhs.m10 * (double)rhs.m02 + (double)lhs.m11 * (double)rhs.m12 + (double)lhs.m12 * (double)rhs.m22 + (double)lhs.m13 * (double)rhs.m32),
                m13 = (float)((double)lhs.m10 * (double)rhs.m03 + (double)lhs.m11 * (double)rhs.m13 + (double)lhs.m12 * (double)rhs.m23 + (double)lhs.m13 * (double)rhs.m33),
                m20 = (float)((double)lhs.m20 * (double)rhs.m00 + (double)lhs.m21 * (double)rhs.m10 + (double)lhs.m22 * (double)rhs.m20 + (double)lhs.m23 * (double)rhs.m30),
                m21 = (float)((double)lhs.m20 * (double)rhs.m01 + (double)lhs.m21 * (double)rhs.m11 + (double)lhs.m22 * (double)rhs.m21 + (double)lhs.m23 * (double)rhs.m31),
                m22 = (float)((double)lhs.m20 * (double)rhs.m02 + (double)lhs.m21 * (double)rhs.m12 + (double)lhs.m22 * (double)rhs.m22 + (double)lhs.m23 * (double)rhs.m32),
                m23 = (float)((double)lhs.m20 * (double)rhs.m03 + (double)lhs.m21 * (double)rhs.m13 + (double)lhs.m22 * (double)rhs.m23 + (double)lhs.m23 * (double)rhs.m33),
                m30 = (float)((double)lhs.m30 * (double)rhs.m00 + (double)lhs.m31 * (double)rhs.m10 + (double)lhs.m32 * (double)rhs.m20 + (double)lhs.m33 * (double)rhs.m30),
                m31 = (float)((double)lhs.m30 * (double)rhs.m01 + (double)lhs.m31 * (double)rhs.m11 + (double)lhs.m32 * (double)rhs.m21 + (double)lhs.m33 * (double)rhs.m31),
                m32 = (float)((double)lhs.m30 * (double)rhs.m02 + (double)lhs.m31 * (double)rhs.m12 + (double)lhs.m32 * (double)rhs.m22 + (double)lhs.m33 * (double)rhs.m32),
                m33 = (float)((double)lhs.m30 * (double)rhs.m03 + (double)lhs.m31 * (double)rhs.m13 + (double)lhs.m32 * (double)rhs.m23 + (double)lhs.m33 * (double)rhs.m33)
            };
        }

        public static Vector4 operator *(Vector4 v, Matrix4x4 matrix)
        {
            return new Vector4()
            {
                x = v.x * matrix.m00 + v.y * matrix.m10 + v.z * matrix.m20 + v.w * matrix.m30,
                y = v.x * matrix.m01 + v.y * matrix.m11 + v.z * matrix.m21 + v.w * matrix.m31,
                z = v.x * matrix.m02 + v.y * matrix.m12 + v.z * matrix.m22 + v.w * matrix.m32,
                w = v.x * matrix.m03 + v.y * matrix.m13 + v.z * matrix.m23 + v.w * matrix.m33,
            };
        }

        public static Vector4 operator *(Vector3 v, Matrix4x4 matrix)
        {
            Vector4 v4 = new Vector4(v.x, v.y, v.z, 1) * matrix;
            return new Vector4(v4.x, v4.y, v4.z);
        }
    }
    #endregion
    public class MathUtil
    {
        public static float Lerp(float a, float b, float t)
        {
            return a + (b - a)*t;
        }

        public static float Clamp(float value, float min, float max)
        {
            if (value < min)
                return min;
            if (value > max)
                return max;
            return value;
        }

        public static float Clamp01(float value)
        {
            if ((double)value < 0.0)
                return 0.0f;
            if ((double)value > 1.0)
                return 1f;
            return value;
        }

        private static Dictionary<int, float> _cos = new Dictionary<int, float>();
        public static float Fast_Cos(float angle)
        {
            int index = (int)(angle + 0.5f);
            if (!_cos.ContainsKey(index % 360))
            {
                _cos[index % 360] = (float)Math.Cos(index);
            }

            return _cos[index % 360];
        }

        private static Dictionary<int, float> _sin = new Dictionary<int, float>();
        public static float Fast_Sin(float angle)
        {
            int index = (int)(angle + 0.5f);
            if (!_sin.ContainsKey(index % 360))
            {
                _sin[index % 360] = (float)Math.Sin(index);
            }

            return _sin[index % 360];
        }

        public static int Round(float a)
        {
            return (int) (a + 0.5f);
        }


        public static bool FloatNear(float a, float b)
        {
            return (int)(a + 0.5) == (int)(b + 0.5);
        }

    }
}
