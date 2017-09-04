
using System;
using RasterRender.Engine.Mathf;

namespace RasterRender.Engine.Simple
{
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
            return new Vector4(b.x - a.x, b.y - a.y, b.z - a.z, 1.0f);
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

    class Simple3DEngine
    {
        protected Vector4 position; //相机位置

        protected Vector4 u, v, n; //UVN相机模型的朝向向量
        protected Vector4 eye; //UVN相机模型的目标位置
        protected Vector4 up; //UVN相机模型的上方向

        protected float height; //垂直视距
        protected float width;  // 水平视距
        private float aspect_ratio; //屏幕的宽高比
        protected float fov;    //视角大小

        protected float clipMin;
        protected float clipMax;

        private Matrix4x4 mcam; //用于存储世界坐标到相机坐标变换矩阵
        private Matrix4x4 mper; //用于存储相机坐标到透视坐标变换矩阵
        private Matrix4x4 mscr; //用于存储透视坐标到屏幕坐标变换矩阵

        public float[,] zInvBuffer;     //深度缓存   
        public Color[,] colorBuffer;    //颜色缓存

        public void SetCameraLookAt(Vector4 position, Vector4 eye, Vector4 up)
        {
            this.position = position;
            this.eye = eye;
            this.up = up;

            SetMcamMatrix();
        }
        public void SetCameraProperty(int width, int height, float fov, float clipMin, float clipMax)
        {
            this.width = width;
            this.height = height;
            this.aspect_ratio = (float)width/height;
            this.fov = fov;
            this.clipMin = clipMin;
            this.clipMax = clipMax;

            float tan_fov_div2 = (float)Math.Tan(fov / 2 / 180f * Math.PI);
            float viewW = 1.0f / tan_fov_div2;
            mper.Init(
                viewW, 0, 0, 0,
                0, viewW * aspect_ratio, 0, 0,
                0, 0, 1, 1,
                0, 0, 0, 0);
            mscr.Init(
                width - 1, 0, 0, 0,
                0, height - 1, 0, 0,
                0, 0, 1, 0,
                width - 1, height - 1, 0, 0.5f);

            colorBuffer = new Color[width, height];
            zInvBuffer = new float[width, height];
        }
        private void SetMcamMatrix()
        {
            //这个函数根绝注视向量eye,上向量v和右向量u创建一个相机变换矩阵
            //并将这个存储到相机对象中,这些值都是从相机对象中提取的

            Matrix4x4 mt_inv = new Matrix4x4(); //逆相机平移矩阵
            Matrix4x4 mt_uvn = new Matrix4x4(); //UVN相机变换矩阵

            //第一步,根据相机位置创建逆平移矩阵
            mt_inv.Init(1, 0, 0, 0,
                0, 1, 0, 0,
                0, 0, 1, 0,
                -this.position.x, -this.position.y, -this.position.z, 1);

            //第二步,uvn矩阵
            //1.n=目标位置-观察参考点
            Vector4 n = this.eye;
            //2.相机的上方向
            Vector4 v = this.up;
            //3..u = (n X v)
            Vector4 u = n * v;

            mt_uvn.Init(
                u.x, v.x, n.x, 0,
                u.y, v.y, n.y, 0,
                u.z, v.z, n.z, 0,
                0, 0, 0, 1);

            //将平移矩阵乘以uvn矩阵,并将结果存储到相机变换矩阵中
            mcam = mt_inv * mt_uvn;
        }
        private void TransformWorldToScreen(ref Vertex v)
        {
            //转换为本地坐标
            Vector4 t_v = v.pos * this.mcam;

            //转换为视口坐标
            t_v = t_v * this.mper;
            t_v.x /= t_v.w;
            t_v.y /= t_v.w;
            t_v.w = 1.0f;

            //转换为屏幕坐标
            t_v = t_v * this.mscr;
            t_v.x *= t_v.w;
            t_v.y *= t_v.w;
            t_v.w = 1.0f;

            v.pos = t_v;
        }
        // 检查齐次坐标同 cvv 的边界用于视锥裁剪
        private int CheckCVV(Vector3 v)
        {
            int check = 0;

            if (v.z < 0.0f) check |= 1;
            if (v.z > clipMax) check |= 2;
            if (v.x < 0.0f) check |= 4;
            if (v.x > width) check |= 8;
            if (v.y < 0.0f) check |= 16;
            if (v.y > height) check |= 32;
            return check;
        }
        public void DrawPrimitive(Vertex v1, Vertex v2, Vertex v3)
        {
            TransformWorldToScreen(ref v1);
            TransformWorldToScreen(ref v2);
            TransformWorldToScreen(ref v3);
            if (CheckCVV(v1.pos) != 0 || CheckCVV(v2.pos) != 0 || CheckCVV(v3.pos) != 0)
                return;

            Vertex[] verts = new[] { v1, v2, v3 };
            v2f[] v2fs = new v2f[3];
            for(int i =0; i < 3; i++)
            {
                v2fs[i] = VertexShader(ref verts[i]);
            }

            for (int i = 0; i < 3; i++)
            {
                for (int j = i + 1; j < 3; j++)
                {
                    if (v2fs[i].pos.y < v2fs[j].pos.y)
                    {
                        v2f t = v2fs[i];
                        v2fs[i] = v2fs[j];
                        v2fs[j] = t;
                    }
                }
            }

            if (MathUtil.FloatNear(v1.pos.y, v2.pos.y) && MathUtil.FloatNear(v3.pos.y, v2.pos.y) ||
                MathUtil.FloatNear(v1.pos.x, v2.pos.x) && MathUtil.FloatNear(v1.pos.x, v3.pos.x))
            {
                //线 不画
                return;
            }

            if (MathUtil.FloatNear(v2fs[0].pos.y, v2fs[1].pos.y))
            {
                DrawBottomTriangle(v2fs);
            }
            else if (MathUtil.FloatNear(v2fs[2].pos.y, v2fs[1].pos.y))
            {
                DrawTopTriangle(v2fs);
            }
            else
            {
                //分成两个三角形 所以要计算 线段[left,v3]与直线y=right.y的交点
                v2f v4 = v2f.Lerp(v2fs[0], v2fs[2], (v2fs[1].pos.y - v2fs[0].pos.y) / (v2fs[2].pos.y - v2fs[0].pos.y));
                DrawTopTriangle(new v2f[] { v2fs[0], v2fs[1], v4 });
                DrawBottomTriangle(new v2f[] { v2fs[1], v4, v2fs[2] });
            }
        }
        private void DrawBottomTriangle(v2f[] v2fs)
        {
            if (v2fs[0].pos.x > v2fs[1].pos.x)
            {
                var t = v2fs[0];
                v2fs[0] = v2fs[1];
                v2fs[1] = t;
            }

            float top = v2fs[0].pos.y;
            float bottom = v2fs[2].pos.y;

            int count = 0;
            for (float i = bottom; i < top; i++)
            {
                float t = (i - bottom) / (top - bottom);
                var line = new ScanLine(v2f.Lerp(v2fs[2], v2fs[0], t), v2f.Lerp(v2fs[2], v2fs[1], t));
                DrawScanLine(line);
                count++;
            }
        }
        private void DrawTopTriangle( v2f[] v2fs)
        {
            if (v2fs[1].pos.x > v2fs[2].pos.x) { v2f t = v2fs[1]; v2fs[1] = v2fs[2]; v2fs[2] = t; }

            float top = v2fs[0].pos.y;
            float bottom = v2fs[2].pos.y;

            int count = 0;
            for (float i = bottom; i < top; i++)
            {
                float t = (i - bottom) / (top - bottom);
                var line = new ScanLine(v2f.Lerp(v2fs[1], v2fs[0], t), v2f.Lerp(v2fs[2], v2fs[0], t));
                DrawScanLine(line);
                count++;
            }
        }
        private void DrawScanLine(ScanLine line)
        {
            int startX = MathUtil.Round(line.left.pos.x);
            int endX = MathUtil.Round(line.right.pos.x);
            int y = MathUtil.Round(line.left.pos.y);

            //为了加速 并不重复计算插值 z值计算有问题
            v2f step = v2f.Division(line.left, line.right, line.right.pos.x - line.left.pos.x);

            for (int i = startX, j = 0; i <= endX; i++, j++)
            {
                float curZInv = line.left.pos.z + j * step.pos.z;
                v2f curV2f = line.left + j * step;
                //深度缓存
                if (zInvBuffer[i, y] <= curV2f.pos.z)
                {
                    if (i == startX || i == endX || i == endX - 1 || i == endX - 2)
                    {
                        colorBuffer[i, y] = new Color() { r = 0xFF / 255, g = 0x00 / 255, b = 0xFF / 255 };
                    }
                    colorBuffer[i, y] = FragShader(ref curV2f);
                    
                    zInvBuffer[i, y] = curZInv;
                }
            }
        }
        struct ScanLine
        {
            public v2f left, right;

            public ScanLine(v2f left, v2f right)
            {
                this.left = left;
                this.right = right;

                float leftZInv = 1.0f / left.pos.z;
                this.left.pos.z = leftZInv;
                this.left.uv = leftZInv * this.left.uv;

                float rightZInv = 1.0f / right.pos.z;
                this.right.pos.z = rightZInv;
                this.right.uv = rightZInv * this.right.uv;
            }
        }
        struct v2f
        {
            public Vector4 pos;
            public Color color;
            public TexCoord uv;

            public static v2f Lerp(v2f v1, v2f v2, float t)
            {
                return new v2f()
                {
                    pos = Vector4.Lerp(v1.pos, v2.pos, t),
                    color = Color.Lerp(v1.color, v2.color, t),
                    uv = TexCoord.Lerp(v1.uv, v2.uv, t),
                };
            }

            public static v2f Division(v2f v1, v2f v2, float t)
            {
                float tInv = 1 / t;
                return new v2f()
                {
                    pos = tInv * (v2.pos - v1.pos),
                    uv = tInv * (v2.uv - v1.uv),
                    color = new Color()
                    {
                        r = tInv * (v2.color.r - v1.color.r),
                        g = tInv * (v2.color.g - v1.color.g),
                        b = tInv * (v2.color.b - v1.color.b),
                        a = tInv * (v2.color.a - v1.color.a),
                    }
                };
            }

            public static v2f operator +(v2f v1, v2f v2)
            {
                return new v2f()
                {
                    pos = v1.pos + v2.pos,
                    uv = v1.uv + v2.uv,
                    color = new Color()
                    {
                        r = (v1.color.r + v2.color.r),
                        g = (v1.color.g + v2.color.g),
                        b = (v1.color.b + v2.color.b),
                        a = (v1.color.a + v2.color.a),
                    },
                };
            }
            public static v2f operator *(float f, v2f v)
            {
                v.pos = f * v.pos; 
                v.uv = f * v.uv;
                v.color = new Color()
                {
                    r = f * v.color.r,
                    g = f * v.color.g,
                    b = f * v.color.b,
                    a = f * v.color.a,
                };
                return v;
            }
        }

        public void InitTexture()
        {
            _texture = new Color[textureWidth, textureWidth];
            int cube = textureWidth/10;
            for (int i = 0; i < textureWidth; i++)
            {
                for (int j = 0; j < textureWidth; j++)
                {
                    _texture[i, j] = (i/cube%2 + j/cube%2)%2 == 0
                        ? new Color() {r = 0xFF/255, g = 0x00/255, b = 0xFF/255}
                        : new Color() {r = 1, g = 1, b = 1, a = 1};
                }
            }
        }

        private int textureWidth = 200;
        private static Color[,] _texture = new Color[2, 2] { { new Color() { r = 0xFF / 255, g = 0x00 / 255, b = 0xFF / 255 }, new Color() { r = 1, g = 1, b = 1, a = 1 } }, { new Color() { r = 1, g = 1, b = 1, a = 1 }, new Color() { r = 0xFF / 255, g = 0x00 / 255, b = 0xFF / 255 } } };
        private Color ReadTextture(float u, float v, float zInv)
        {
            u = MathUtil.Clamp01(u / zInv);
            v = MathUtil.Clamp01(v / zInv);
            int x = (int)(u * (textureWidth - 1));
            int y = (int)(v * (textureWidth - 1));
            return _texture[x, y];
        }
        private v2f VertexShader(ref Vertex v)
        {
            return new v2f()
            {
                pos = v.pos,
                uv = v.uv,
                color = v.color,
            };
        }
        private Color FragShader(ref v2f IN)
        {
            return IN.color;
            return ReadTextture(IN.uv.x, IN.uv.y, IN.pos.z);
        }

    }
}
