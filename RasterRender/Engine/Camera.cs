
//using System;
//using RasterRender.MathUtils;

//namespace RasterRender
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

//            _colorBuffer = new float[width, height][];
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
