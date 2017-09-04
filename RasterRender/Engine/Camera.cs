
using System;
using System.Collections.Generic;
using RasterRender.Engine.Mathf;
using RasterRender.Engine.Simple;
using Color = System.Drawing.Color;

namespace RasterRender.Engine
{
    //欧拉相机模型
    public class Camera
    {
        public int state; //相机状态
        public int attr; //相机属性

        public Vector4 position; //相机位置

        public Vector4 u, v, n; //UVN相机模型的朝向向量
        public Vector4 eye; //UVN相机模型的目标位置
        public Vector4 up; //UVN相机模型的上方向

        public float view_dist_h; //水平视距和垂直视距
        public float view_dist_w;

        public float fov; //水平方向和垂直方向视野

        //如果视野不是90度,3d裁剪面方程将为一般性平面方程
        public float near_clip_z; //近裁面
        private float far_clip_z; //远裁面

        private float viewport_width; //屏幕/视口大小
        private float viewport_Height;

        //宽高比
        private float aspect_ratio; //屏幕的宽高比

        //是否需要下述矩阵取决于变换方法
        //例如,以手工方式进行透视变换,屏幕变换时,不需要这些矩阵
        //然而提供这些矩阵提高了灵活性

        private Matrix4x4 mcam; //用于存储世界坐标到相机坐标变换矩阵
        private Matrix4x4 mper; //用于存储相机坐标到透视坐标变换矩阵
        private Matrix4x4 mscr; //用于存储透视坐标到屏幕坐标变换矩阵

        public bool[,] wireFrameBuffer;

        public float[,] zInvBuffer;
        public Color[,] colorBuffer;

        /// <summary>
        /// 这个函数初始化相机对象
        /// </summary>
        /// <param name="attr">相机属性</param>
        /// <param name="pos">相机的初始位置</param>
        /// <param name="near">近裁面</param>
        /// <param name="far">远裁面</param>
        /// <param name="fov">视野,单位为度</param>
        /// <param name="width">屏幕/视口宽度</param>
        /// <param name="height">>屏幕/视口高度</param>
        public void Init(int attr, Vector4 pos, Vector4 eye, Vector4 up, float near, float far, float fov,
            float width, float height)
        {
            this.attr = attr;

            this.position = pos;
            this.eye = eye;
            this.up = up;
            SetMcamMatrixUVN();

            this.near_clip_z = near;
            this.far_clip_z = far;

            this.viewport_width = width;
            this.viewport_Height = height;

            this.aspect_ratio = (float)width / height;
            this.fov = fov;
            float tan_fov_div2 = (float)Math.Tan(fov / 360f * Math.PI);
            this.view_dist_w = 1.0f / tan_fov_div2;
            mper.Init(
                view_dist_w, 0, 0, 0,
                0, view_dist_w * aspect_ratio, 0, 0,
                0, 0, 1, 1,
                0, 0, 0, 0);
            mscr.Init(
                viewport_width - 1, 0, 0, 0,
                0, viewport_Height - 1, 0, 0,
                0, 0, 1, 0,
                viewport_width - 1, viewport_Height - 1, 0, 0.5f);

            wireFrameBuffer = new bool[(int)width, (int)height];
            colorBuffer = new Color[(int)width, (int)height];
            zInvBuffer = new float[(int)width, (int)height];
            _texture = new Color[textureWidth, textureWidth];
            for (int i = 0; i < textureWidth; i++)
            {
                for (int j = 0; j < textureWidth; j++)
                {
                    _texture[i, j] = (i / (textureWidth / 10) + j / (textureWidth / 10)) % 2 == 0 ? Color.Aqua : Color.AliceBlue;
                }
            }
        }

        private int textureWidth = 100;

        public void SetMcamMatrixUVN(Vector4 position, Vector4 eye, Vector4 up)
        {
            this.position = position;
            this.eye = eye;
            this.up = up;
            SetMcamMatrixUVN();
        }

        private void SetMcamMatrixUVN()
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
            n = this.eye;
            //2.相机的上方向
            v = this.up;
            //3..u = (n X v)
            u = n * v;

            mt_uvn.Init(
                u.x, v.x, n.x, 0,
                u.y, v.y, n.y, 0,
                u.z, v.z, n.z, 0,
                0, 0, 0, 1);

            //将平移矩阵乘以uvn矩阵,并将结果存储到相机变换矩阵中
            mcam = mt_inv * mt_uvn;
        }

        public void Draw
        public void DrawPrimitives(List<Vertex> verts, List<int> triangles)
        {
            for (int i = 0; i < viewport_width; i++)
            {
                for (int j = 0; j < viewport_Height; j++)
                {
                    colorBuffer[i, j] = Color.Black;
                    zInvBuffer[i, j] = float.MinValue;
                }
            }
            //将所有坐标准换为屏幕坐标先
            List<Vertex> t_verts = new List<Vertex>();
            foreach (var vert in verts)
            {
                t_verts.Add(GetScreenPoint(vert));
            }

            for (int i = 0; i < triangles.Count; i++)
            {
                DrawPrimitive(t_verts[triangles[i]], t_verts[triangles[++i]], t_verts[triangles[++i]]);
            }
        }

        public Vertex GetScreenPoint(Vertex v)
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
            return v;
        }

        /// <summary>
        ///按照y坐标由大到小的方式逐行渲染,讲道理的话传入这里的坐标应该已经是屏幕坐标了
        /// </summary>
        /// <param name="verts"></param>
        /// <param name="uvs"></param>
        /// <param name="index1"></param>
        /// <param name="index2"></param>
        /// <param name="index3"></param>
        private void DrawPrimitive(Vertex v1, Vertex v2, Vertex v3)
        {
            if (CheckCVV(v1.pos) != 0 || CheckCVV(v2.pos) != 0 || CheckCVV(v3.pos) != 0)
                return;

            //todo 暂时不考率uv
            if (v1.pos.y < v2.pos.y)
            {
                Vertex t = v1;
                v1 = v2;
                v2 = t;
            }
            if (v1.pos.y < v3.pos.y)
            {
                Vertex t = v1;
                v1 = v3;
                v3 = t;
            }
            if (v2.pos.y < v3.pos.y)
            {
                Vertex t = v2;
                v2 = v3;
                v3 = t;
            }


            if (FloatNear(v1.pos.y, v2.pos.y) && FloatNear(v3.pos.y, v2.pos.y) ||
                FloatNear(v1.pos.x, v2.pos.x) && FloatNear(v1.pos.x, v3.pos.x))
            {
                //线 不画
            }

            if (FloatNear(v1.pos.y, v2.pos.y))
            {
                DrawBottomTriangle(v1, v2, v3);
            }
            else if (FloatNear(v3.pos.y, v2.pos.y))
            {
                DrawTopTriangle(v1, v2, v3);
            }
            else
            {
                //分成两个三角形 所以要计算 线段[left,v3]与直线y=right.y的交点
                Vertex v4 = Vertex.Lerp(v1, v3, (v2.pos.y - v1.pos.y) / (v3.pos.y - v1.pos.y));
                DrawTopTriangle(v1, v2, v4);
                DrawBottomTriangle(v2, v4, v3);
            }

        }

        // 检查齐次坐标同 cvv 的边界用于视锥裁剪
        private int CheckCVV(Vector3 v)
        {

            int check = 0;

            if (v.z < 0.0f) check |= 1;
            if (v.z > far_clip_z) check |= 2;
            if (v.x < 0.0f) check |= 4;
            if (v.x > viewport_width) check |= 8;
            if (v.y < 0.0f) check |= 16;
            if (v.y > viewport_Height) check |= 32;
            return check;
        }

        private bool FloatNear(float a, float b)
        {
            return (int)(a + 0.5f) == (int)(b + 0.5f);
        }

        /// <summary>
        /// 顶点在上
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="v3"></param>
        private void DrawBottomTriangle(Vertex v1, Vertex v2, Vertex v3)
        {
            if (v1.pos.x > v2.pos.x)
            {
                var t = v1;
                v1 = v2;
                v2 = t;
            }

            float top = v1.pos.y;
            float bottom = v3.pos.y;

            int count = 0;
            for (float i = bottom; i < top; i++)
            {
                float t = (i - bottom) / (top - bottom);
                var line = new ScanLine(Vertex.Lerp(v3, v1, t), Vertex.Lerp(v3, v2, t));
                DrawScanLine(line);
                count++;
            }
        }

        private void DrawTopTriangle(Vertex v1, Vertex v2, Vertex v3)
        {
            if (v2.pos.x > v3.pos.x) { Vertex t = v2; v2 = v3; v3 = t; }

            float top = v1.pos.y;
            float bottom = v3.pos.y;

            int count = 0;
            for (float i = bottom; i < top; i++)
            {
                float t = (i - bottom) / (top - bottom);
                var line = new ScanLine(Vertex.Lerp(v2, v1, t), Vertex.Lerp(v3, v1, t));
                DrawScanLine(line);
                count++;
            }
        }

        private void DrawScanLine(ScanLine line)
        {
            int startX = MathUtil.Round(line.left.pos.x);
            int endX = MathUtil.Round(line.right.pos.x);
            int y = MathUtil.Round(line.left.pos.y);

            Vertex step = Vertex.Division(line.left, line.right, line.right.pos.x - line.left.pos.x);

            for (int i = startX, j = 0; i <= endX; i++, j++)
            {
                float curZInv = line.left.pos.z + j * step.pos.z;
                if (zInvBuffer[i, y] <= curZInv)
                {
                    var uv = line.left.uv + j * step.uv;
                    colorBuffer[i, y] = GetColor(uv.x, uv.y, curZInv);
                    if (i == startX || i == endX)
                    {
                        colorBuffer[i, y] = Color.Aqua;
                    }
                    zInvBuffer[i, y] = curZInv;
                }
            }
        }

        /// <summary>
        /// 纹理取样
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private Color GetColor(float u, float v, float zInv)
        {
            u = MathUtil.Clamp01(u / zInv);
            v = MathUtil.Clamp01(v / zInv);
            int x = (int)(u * (textureWidth - 1));
            int y = (int)(v * (textureWidth - 1));
            return _texture[x, y];
        }

        private Color[,] _texture;

        /// <summary>
        /// 线框模式
        /// </summary>
        /// <param name="verts"></param>
        /// <param name="triangles"></param>
        public void DrawWireFrame(List<Vertex> verts, List<int> triangles)
        {
            for (int i = 0; i < viewport_width; i++)
            {
                for (int j = 0; j < viewport_Height; j++)
                {
                    wireFrameBuffer[i, j] = false;
                }
            }
            List<Vertex> t_verts = new List<Vertex>();
            foreach (var vert in verts)
            {
                t_verts.Add(GetScreenPoint(vert));
            }

            for (int i = 0; i < triangles.Count; i += 3)
            {
                //DrawWire(t_verts[triangles[i]].pos, t_verts[triangles[i + 1]].pos);
                //DrawWire(t_verts[triangles[i + 1]].pos, t_verts[triangles[i + 2]].pos);
                //DrawWire(t_verts[triangles[i + 2]].pos, t_verts[triangles[i]].pos);
            }
        }

        private void DrawWire(Vector2 a, Vector2 b)
        {
            Vector2 t = a;
            if (a.x > b.x)
            {
                a = b;
                b = t;
            }
            int startX = (int)MathUtil.Clamp((a.x + 0.5f), 0, viewport_width);
            int startY = (int)MathUtil.Clamp((a.y + 0.5f), 0, viewport_width);
            int endX = (int)MathUtil.Clamp((b.x + 0.5f), 0, viewport_width);
            int endY = (int)MathUtil.Clamp((b.y + 0.5f), 0, viewport_width);



            int dx = endX - startX;
            int dy = endY - startY;
            int stepY = dy > 0 ? 1 : -1;

            int j = startY;
            int d = 0;
            for (int i = startX; i <= endX; i++)
            {
                wireFrameBuffer[i, j] = true;
                d += dy;
                while (Math.Abs(d) > dx)
                {
                    j += stepY;
                    if (j >= startY && j < endY || j <= startY && j >= endY)
                    {
                        wireFrameBuffer[i, j] = true;
                    }
                    else
                    {
                        break;
                    }
                    d -= stepY * dx;
                }
            }
        }

        struct ScanLine
        {
            public Vertex left, right;

            public ScanLine(Vertex left, Vertex right)
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
    }
}