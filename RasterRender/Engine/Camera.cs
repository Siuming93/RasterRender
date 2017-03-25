
using System;
using System.Collections.Generic;
using System.Drawing;
using RasterRender.Engine.Mathf;

namespace RasterRender.Engine
{
    //欧拉相机模型
    public class Camera
    {
        public int state;           //相机状态
        public int attr;            //相机属性

        public Vector4 position;    //相机位置

        public Vector4 direction;   //欧拉角度或者UVN相机模型的方向

        public Vector4 u, v, n;     //UVN相机模型的朝向向量
        public Vector4 eye;         //UVN相机模型的目标位置
        public Vector4 up;          //UVN相机模型的上方向


        public float view_dist_h;   //水平视距和垂直视距
        public float view_dist_w;

        public float fov;           //水平方向和垂直方向视野

        //3d裁剪面
        //如果视野不是90度,3d裁剪面方程将为一般性平面方程
        public float near_clip_z;   //近裁面
        float far_clip_z;           //远裁面


        Plane3D rt_clip_plane;      //右裁剪面
        Plane3D lt_clip_plane;      //左裁剪面
        Plane3D tp_clip_plane;      //上裁剪面
        Plane3D bt_clip_plane;      //下裁剪面

        float viewplane_width;      //视平面的宽度和高度
        float viewplane_height;     //对于归一化投影 大小为2x2 否则大小与视口或屏幕窗口相同

        float viewport_width;       //屏幕/视口大小
        float viewport_Height;
        float viewport_center_x;    //视口的中心	
        float viewport_center_y;

        //宽高比
        float aspect_ratio;         //屏幕的宽高比

        //是否需要下述矩阵取决于变换方法
        //例如,以手工方式进行透视变换,屏幕变换时,不需要这些矩阵
        //然而提供这些矩阵提高了灵活性

        Matrix4x4 mcam;             //用于存储世界坐标到相机坐标变换矩阵
        Matrix4x4 mper;             //用于存储相机坐标到透视坐标变换矩阵
        Matrix4x4 mscr;             //用于存储透视坐标到屏幕坐标变换矩阵

        public bool[,] wireFrameBuffer;

        public float[,] zBuffer;
        public Color[,] colorBuffer;


        /// <summary>
        /// 这个函数初始化相机对象
        /// </summary>
        /// <param name="attr">相机属性</param>
        /// <param name="pos">相机的初始位置</param>
        /// <param name="dir">相机的初始角度</param>
        /// <param name="target">UVN相机的初始目标位置</param>
        /// <param name="near">近裁面</param>
        /// <param name="far">远裁面</param>
        /// <param name="fov">视野,单位为度</param>
        /// <param name="width">屏幕/视口宽度</param>
        /// <param name="height">>屏幕/视口高度</param>
        public void Init(int attr, Vector4 pos, Vector4 dir, Vector4 eye, float near, float far, float fov,
            float width, float height)
        {
            this.attr = attr;

            this.position = pos;
            this.direction = dir;           //欧拉相机的方向向量或者角度

            //对于UVN相机
            u = new Vector4(1, 0, 0);
            v = new Vector4(0, 1, 0);
            n = new Vector4(0, 0, 1);

            this.eye = eye;           //UVN目标

            this.near_clip_z = near;
            this.far_clip_z = far;

            this.viewport_width = width;
            this.viewport_Height = height;
            wireFrameBuffer = new bool[(int)width, (int)height];
            colorBuffer = new Color[(int)width, (int)height];
            zBuffer = new float[(int)width, (int)height];

            this.viewport_center_x = (width - 1) / 2;
            this.viewport_center_y = (height - 1) / 2;

            this.aspect_ratio = (float)width / height;

            //将所有矩阵都设为单位矩阵
            mcam = Matrix4x4.identity;
            mper = Matrix4x4.identity;
            mscr = Matrix4x4.identity;

            this.fov = fov;

            //将视平面大小设置为2*(2/ar)
            this.viewplane_width = 2.0f;
            this.viewplane_height = 2.0f / this.aspect_ratio;
            //根据fov和视平面大小计算视距
            float tan_fov_div2 = (float)Math.Tan(fov / 360f);

            this.view_dist_w = 0.5f * this.viewplane_width * tan_fov_div2;

            //建立裁剪面  所有面都过原点 所以只需要计算法向量即可   
            //右裁剪面计算 (tan_fov_div2, 0, 1)为右裁剪面上一点,向量(-1, 0, tan_fov_div2)与其垂直,它的单位向量可以作为法向量.
            this.rt_clip_plane.Init(Vector3.zero, new Vector3(-1, 0, tan_fov_div2).Normalize());

            this.lt_clip_plane.Init(Vector3.zero, new Vector3(1, 0, tan_fov_div2).Normalize());

            this.rt_clip_plane.Init(Vector3.zero, new Vector3(0, -1, tan_fov_div2).Normalize());

            this.rt_clip_plane.Init(Vector3.zero, new Vector3(0, 1, tan_fov_div2).Normalize());
        }

        private void BuildMcamMatrixEuler()
        {
            //这个函数根据欧拉角度计算相机变换矩阵
            //并将其存储到要传入的相机对象中

            //要创建相机变换矩阵,需要这样做:
            //Mcam = mt(-1)*my(-1)*mx(-1)*mz(-1)
            //即相机评语矩阵的逆矩阵乘以相机绕y,x,z 轴 的旋转矩阵的逆矩阵
            //采用什么样的旋转矩阵顺序完全取决于用户,因此这里没有强制采取某种顺序
            //而是根据参数cam_rot_seq的值来决定采用哪种顺序.

            Matrix4x4 mt_inv = new Matrix4x4();     //相机平移矩阵的逆矩阵
            Matrix4x4 mx_inv = new Matrix4x4();     //相机绕x轴的旋转矩阵的逆矩阵
            Matrix4x4 my_inv = new Matrix4x4();     //相机绕y轴旋转矩阵的逆矩阵
            Matrix4x4 mz_inv = new Matrix4x4();     //相机绕z轴旋转矩阵的逆矩阵
            Matrix4x4 mrot;      //所有旋转矩阵的积
            Matrix4x4 mtmp;      //用于存储临时矩阵

            //第一步,计算相机平移矩阵的逆矩阵
            mt_inv.Init(1, 0, 0, 0,
                0, 1, 0, 0,
                0, 0, 1, 0,
                -this.position.x, -this.position.y, -this.position.z, 1);

            //第二步,创建旋转矩阵的逆矩阵
            //要计算正规旋转矩阵的逆矩阵,可以将其转置  
            //也可以将每个旋转角度取负  还可以用高斯消元法??
            float theta_x = this.direction.x;
            float theta_y = this.direction.y;
            float theta_z = this.direction.z;

            //计算角度x的正弦和余弦
            float cos_theta = MathUtil.Fast_Cos(theta_x);
            float sin_theta = MathUtil.Fast_Sin(theta_x);
            mx_inv.Init(1, 0, 0, 0,
                0, cos_theta, sin_theta, 0,
                0, -sin_theta, cos_theta, 0,
                0, 0, 0, 1);

            //计算角度y的正弦和余弦
            cos_theta = MathUtil.Fast_Cos(theta_y);
            sin_theta = MathUtil.Fast_Sin(theta_y);
            my_inv.Init(cos_theta, 0, -sin_theta, 0,
                0, 1, 0, 0,
                sin_theta, 0, cos_theta, 0,
                0, 0, 0, 1);

            //计算角度z的正弦和余弦
            cos_theta = MathUtil.Fast_Cos(theta_z);
            sin_theta = MathUtil.Fast_Sin(theta_z);
            mz_inv.Init(cos_theta, -sin_theta, 0, 0,
                sin_theta, cos_theta, 0, 0,
                0, 0, 1, 0,
                0, 0, 0, 1);

            //旋转顺序为XYZ
            mrot = mx_inv * my_inv * mz_inv;

            //将其乘以逆平移矩阵,并将结果存储到相机对象的相机变换矩阵中
            this.mcam = mt_inv * mrot;
        }

        public void BuildMcamMatrixUVN(Vector4 position, Vector4 eye, Vector4 up)
        {
            this.position = position;
            this.eye = eye;
            this.up = up;
            BuildMcamMatrixUVN();
        }

        public void BuildMcamMatrixUVN()
        {
            //这个函数根绝注视向量eye,上向量v和右向量u创建一个相机变换矩阵
            //并将这个存储到相机对象中,这些值都是从相机对象中提取的

            Matrix4x4 mt_inv = new Matrix4x4();       //逆相机平移矩阵
            Matrix4x4 mt_uvn = new Matrix4x4();        //UVN相机变换矩阵

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

            mt_uvn.Init(u.x, v.x, n.x, 0,
                u.y, v.y, n.y, 0,
                u.z, v.z, n.z, 0,
                0, 0, 0, 1);

            //将平移矩阵乘以uvn矩阵,并将结果存储到相机变换矩阵中
            mcam = mt_inv * mt_uvn;
        }

        //public ConverseToLocale(

        public void World_To_Camera(GameObject gameObject)
        {
            //这是一个基于矩阵的函数
            //根据传入的相机变换矩阵,将物体的世界坐标变换为相机坐标
            //完全不考虑多边形本身,只对vlist_trans[]中的顶点进行变换
            //因为渲染列表中的多边形表示的几何体都通过了背面剔除

            //将物体的每个顶点变换为相机坐标
            //这里假设顶点已经被变换为了世界坐标
            //且结果存储在vlist_trans[]中

            for (int vertex = 0; vertex < gameObject.num_verteices; vertex++)
            {
                //使用相机对象中的矩阵mcam对定点进行变换
                Vector4 result;     //用于存储每次变换的记过
                result = gameObject.vlist_trans[vertex] * this.mcam;

                gameObject.vlist_trans[vertex] = result;

            }
        }

        public void World_To_Camera(List<Polygon> rend_list)
        {
            //这个函数基于矩阵的
            //它根据传入的相机变换矩阵将渲染列表中没个多边形变换为相机坐标
            //如果在流水线上游已经将每个物体转化为多边形
            //并将它们插入到渲染类表中
            //将使用这个函数,而不是基于物体的函数对顶点进行变换
            //将物体转换为多边形的操作实在物体剔除,局部变换,局部坐标到世界坐标变换以及背面剔除之后进行的
            //
        }


        public void DrawPrimitives(List<Vector3> verts, List<int> triangles, List<Vector2> uvs)
        {
            for (int i = 0; i < viewport_width; i++)
            {
                for (int j = 0; j < viewport_width; j++)
                {
                    colorBuffer[i,j]=Color.Black;
                    zBuffer[i, j] = float.MaxValue;
                }
            }
            //将所有坐标准换为屏幕坐标先
            List<Vector3> t_verts = new List<Vector3>();
            foreach (var vert in verts)
            {
                var t_v = vert * this.mcam;
                t_v = 1 / t_v.z * t_v;
                t_v.x = ((1 + t_v.x) / 2 * (viewport_width - 1) + 0.5f);
                t_v.y = ((1 + t_v.y) / 2 * (viewport_Height - 1) + 0.5f);
                t_verts.Add(t_v);
            }

            for (int i = 0; i < triangles.Count; i++)
            {
                DrawPrimitive(t_verts, triangles[i], triangles[++i], triangles[++i]);
            }
        }

        /// <summary>
        ///按照y坐标由大到小的方式逐行渲染,讲道理的话传入这里的坐标应该已经是屏幕坐标了
        /// </summary>
        /// <param name="verts"></param>
        /// <param name="uvs"></param>
        /// <param name="index1"></param>
        /// <param name="index2"></param>
        /// <param name="index3"></param>
        private void DrawPrimitive(List<Vector3> verts, int index1, int index2, int index3)
        {
            Vector3 v1 = verts[index1], v2 = verts[index2], v3 = verts[index3];

            //todo 暂时不考率uv
            if (v1.y < v2.y) { Vector3 t = v1; v1 = v2; v2 = t; }
            if (v1.y < v3.y) { Vector3 t = v1; v1 = v3; v3 = t; }
            if (v2.y < v3.y) { Vector3 t = v2; v2 = v3; v3 = t; }


            if (FloatNear(v1.y, v2.y) && FloatNear(v3.y, v2.y) ||
                FloatNear(v1.x, v2.x) && FloatNear(v1.x, v3.x))
            {
                //线 不画
            }

            if (FloatNear(v1.y, v2.y))
            {
                DrawBottomTriangle(v1, v2, v3);
            }
            else if (FloatNear(v3.y, v2.y))
            {
                DrawTopTriangle(v1, v2, v3);
            }
            else
            {
                //分成两个三角形 所以要计算 线段[left,v3]与直线y=right.y的交点
                Vector3 v4 = Vector3.Lerp(v1, v3, (v2.y-v1.y)/(v3.y - v1.y));
                DrawTopTriangle(v1, v2, v4);
                DrawBottomTriangle(v2, v4, v3);
            }

        }

        private bool FloatNear(float a, float b)
        {
            return (int) (a + 0.5f) == (int) (b + 0.5f);
        }

        /// <summary>
        /// 顶点在上
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="v3"></param>
        private void DrawBottomTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            if (v1.x > v2.x)
            {
                var t = v1;
                v1 = v2;
                v2 = t;
            }

            float top = v1.y;
            float bottom = v3.y;

            int count = 0;
            for (float i = bottom; i < top; i++)
            {
                float t = (i - bottom)/(top - bottom);
                var line = new ScanLine() { left = Vector3.Lerp(v3, v1, t), right = Vector3.Lerp(v3, v2, t) };
                DrawScanLine(line);
                count++;
            }
        }

        private void DrawTopTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            if (v2.x > v3.x) { Vector3 t = v2; v2 = v3; v3 = t; }

            float top = v1.y;
            float bottom = v3.y;

            int count = 0;
            for (float i = bottom; i < top; i++)
            {
                float t = (i - bottom) / (top - bottom);
                var line = new ScanLine() { left = Vector3.Lerp(v2, v1, t), right = Vector3.Lerp(v3, v1, t) };
                DrawScanLine(line);
                count++;
            }
        }

        private void DrawScanLine(ScanLine line)
        {
            int startX = MathUtil.Round(line.left.x);
            int endX = MathUtil.Round(line.right.x);
            int y = MathUtil.Round(line.left.y);

            Vector3 step = Vector3.Lerp(line.left, line.right, 1 / (line.left.x - line.right.x));

            for (int i = startX; i <= endX; i++)
            {
                if (zBuffer[i, y] >= line.left.z + i * step.z)
                {
                    var uv = line.uv1 + i * line.uvStep;
                    colorBuffer[i, y] = GetColorInt(0, 0);
                    if(i==startX||i==endX)
                        colorBuffer[i, y] = Color.Aqua;
                    zBuffer[i, y] = line.left.z + i * step.z;


                }
            }
        }

        public Color GetColorInt(float x, float y)
        {
            return Color.DarkGray;
        }

        public void DrawWireFrame(List<Vector3> verts, List<int> triangles)
        {
            for (int i = 0; i < viewport_width; i++)
            {
                for (int j = 0; j < viewport_width; j++)
                {
                    wireFrameBuffer[i, j] = false;
                }
            }
            List<Vector3> t_verts = new List<Vector3>();
            foreach (var vert in verts)
            {
                var t_v = vert * this.mcam;
                t_v = 1 / t_v.z * t_v;
                t_v.x += 1;
                t_v.y += 1;
                t_verts.Add((viewport_width - 1) / 2 * t_v);
            }

            for (int i = 0; i < triangles.Count; i += 3)
            {
                DrawWire(t_verts[triangles[i]], t_verts[triangles[i + 1]]);
                DrawWire(t_verts[triangles[i + 1]], t_verts[triangles[i + 2]]);
                DrawWire(t_verts[triangles[i + 2]], t_verts[triangles[i]]);
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
            public Vector3 left, right, vStep;
            public Vector2 uv1, uv2, uvStep;
        }
    }
}
//{ 
//    public class Camera
//    {
//        public Transform transform;

//        public Matrix4x4 world;         //从世界坐标转换到相机坐标的转换矩阵  是坐标平移矩阵T和一个旋转矩阵R的合并矩阵
//        public Matrix4x4 view;          //视口转换矩阵
//        public Matrix4x4 projection;    //投影矩阵
//        public Matrix4x4 WVP;           //world*view*projection

//        public int width { get; private set; }
//        public int height { get; private set; }
//        public float fov { get; private set; }
//        public float apsect { get; private set; }
//        public float zn { get; private set; }
//        public float zf { get; private set; }

//        public Vector eye { get; private set; }
//        public Vector up { get; private set; }

//        private float[] _defaultColor = { 1.0f, 1.0f, 1.0f, 1.0f };
//        private float[,][] _colorBuffer;
//        private float[,] _zBuffer;

//        public Camera()
//        {
//            SetPerspective(1, 1, 1, 1, 1);
//            LookAt(Vector.zero, new Vector(0, 0, 1), Vector.up);
//        }

//        public void SetPerspective(int width, int height, float fov,float zn, float zf)
//        {
//            this.width = width;
//            this.height = height;
//            this.fov = fov;
//            this.apsect = (float)width / height;
//            this.zf = zf;
//            this.zn = zn;

//           D _colorBuffer = new float[width, height][];
//            for (int i = 0; i < width; i++)
//            {
//                for (int j = 0; j < height; j++)
//                {
//                    _colorBuffer[i, j] = _defaultColor.Clone() as float[];
//                }
//            }

//            UpdateWVP();
//        }

//        public void LookAt(Vector at, Vector eye, Vector up)
//        {
//            this.transform.position = at;
//            this.eye = eye;
//            this.up = up;

//            UpdateWVP();
//        }

//        public void Render()
//        {

//        }


//        private void UpdateWVP()
//        {
//            Vector xAxis = Vector.Cross(eye, up);
//            Vector yAxis = up.Normalize();
//            Vector zAxis = eye.Normalize();

//            world=new Matrix4x4()
//            {
//                m00 = xAxis.x,
//                m01 = xAxis.y,
//                m02 = xAxis.z,
//                m03 = this.transform.position.x,

//                m10 = yAxis.x,
//                m11 = yAxis.y,
//                m12 = yAxis.z,
//                m13 = this.transform.position.y,

//                m20 = zAxis.x,
//                m21 = zAxis.y,
//                m22 = zAxis.z,
//                m23 = this.transform.position.z,

//                m30 = 0, m31 = 0, m32 = 0, m33 = 1,
//            };

//            float fax = 1 / (float)Math.Tan(fov / 2);
//            view = new Matrix4x4()
//            {
//                m00 = apsect * fax, m01 = 0, m02 = 0, m03 = 0,
//                m10 = 0, m11 = fax, m12 = 0, m13 = 0,
//                m20 = 0, m21 = 0, m22 = 1, m23 = 1,
//                m30 = 0, m31 = 0, m32 = 0, m33 = 0,
//            };

//            projection=new Matrix4x4()
//            {
//                m00 = (float)width / 2, m01 = 0, m02 = 0, m03 = 0,
//                m10 = 0, m11 = -(float)height / 2, m12 = 0, m13 = 0,
//                m20 = 0, m21 = 0, m22 = 1, m23 = 0,
//                m30 = (float)width / 2, m31 = (float)height / 2, m32 = 0, m33 = 0,
//            };

//            //world = new Matrix4x4()
//            //{
//            //    m00 = 1, m01 = 0, m02 = 0, m03 = 0,
//            //    m10 = 0, m11 = 1, m12 = 0, m13 = 0,
//            //    m20 = 0, m21 = 0, m22 = 1, m23 = 0,
//            //    m30 = this.transform.position.x, m31 = this.transform.position.y, m32 = this.transform.position.z, m33 = 0,
//            //}*new Matrix4x4()
//            //{             
//            //    m00 = xAxis.x, m01 = xAxis.y, m02 = xAxis.z, m03 = 0,
//            //    m10 = yAxis.x, m11 = yAxis.y, m12 = yAxis.z, m13 = 0,
//            //    m20 = zAxis.x, m21 = zAxis.y, m22 = zAxis.z, m23 = 0,
//            //    m30 = 0, m31 = 0, m32 = 0, m33 = 1,
//            //};
//        }
//    }
//}
