#define VertexColor

using System;

namespace RasterRender.Engine
{
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
            this.eye = eye.Normoalize();
            this.up = up.Normoalize();

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
            for (int i = 0; i < 3; i++)
            {
                v2fs[i].pos.z = 1.0f / v2fs[i].pos.z;
                v2fs[i].uv = v2fs[i].pos.z * v2fs[i].uv;
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
            for (float i = bottom; i < top; i++)
            {
                float t = (i - bottom) / (top - bottom);
                var line = new ScanLine(v2f.Lerp(v2fs[0], v2fs[2], t), v2f.Lerp(v2fs[1], v2fs[2], t));
                DrawScanLine(line);
            }
        }
        private void DrawTopTriangle( v2f[] v2fs)
        {
            if (v2fs[1].pos.x > v2fs[2].pos.x) { v2f t = v2fs[1]; v2fs[1] = v2fs[2]; v2fs[2] = t; }
            float top = v2fs[0].pos.y;
            float bottom = v2fs[2].pos.y;
            for (float i = bottom; i < top; i++)
            {
                float t = (i - bottom) / (top - bottom);
                var line = new ScanLine(v2f.Lerp(v2fs[1], v2fs[0], t), v2f.Lerp(v2fs[2], v2fs[0], t));
                DrawScanLine(line);
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
                if (zInvBuffer[i, y] < curV2f.pos.z)
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
        private static Color[,] _texture;
        private Color ReadTextture(float u, float v, float zInv)
        {
            u = MathUtil.Clamp01(u / zInv);
            v = MathUtil.Clamp01(v / zInv);
            int x = (int)(u * (textureWidth - 1));
            int y = (int)(v * (textureWidth - 1));
            return _texture[x, y];
        }

        private Color ReadTextture(float u, float v)
        {
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
#if VertexColor
            return IN.color;
#else
            return ReadTextture(IN.uv.x, IN.uv.y, IN.pos.z);
#endif
        }

    }
}
