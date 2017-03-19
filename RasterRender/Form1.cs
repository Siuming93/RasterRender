using System;
using System.Collections.Generic;
using System.Drawing;
using System.Timers;
using System.Windows.Forms;
using RasterRender.Engine.Mathf;
using Timer = System.Timers.Timer;
using RasterRender.Engine;
using RasterRender.Const;

namespace RasterRender
{
    public partial class Form1 : Form
    {
        private Graphics _canvas;
        private Timer _timer;

        private Camera _camera;

        public Form1()
        {
            InitializeComponent();

            InitScene();

            StartLoop();
        }

        private void InitScene()
        {
            InitGameObject();
            InitCamera();
        }

        private void InitGameObject()
        {
            Scene.instance.AddGameObject(new GameObject() { verts = PrimitiveConst.CubeVertexs, triangles = PrimitiveConst.CubeTriangles });
        }
        private void InitCamera()
        {
            _camera = new Camera();
            _camera.Init(0, new Vector4(0, 1f, -5, 1), new Vector4(0, 0, 1, 1), new Vector4(0, 0, 1, 1), 0.1f, 5f, 90f, width, height);
            _camera.up = new Vector4(0, 1, 0, 1);
            _camera.BuildMcamMatrixUVN();
        }

        private void StartLoop()
        {
            _timer = _timer ?? new Timer(1000f / 60);
            _timer.Elapsed += Tick;
            _timer.Start();
        }

        private void StopLoop()
        {
            _timer.Stop();
        }

        private object lockObj = new object();
        private int index;
        private Bitmap bitmap = new Bitmap(width, height);
        private const int width = 400, height = 400;
        private void Tick(object sender, ElapsedEventArgs e)
        {
            lock (lockObj)
            {
                //todo 访问相机的渲染缓存 并渲染
                if (_canvas == null || _canvas.IsVisibleClipEmpty)
                {
                    return;
                }
                _camera.DrawWireFrame(PrimitiveConst.CubeVertexs, PrimitiveConst.CubeTriangles);
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        bitmap.SetPixel(i, j, _camera.wireFrameBuffer[i, j] ? Color.White : Color.Black);
                    }
                }
                _canvas.DrawImage(bitmap, new Point(0, 0));

            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            lock (lockObj)
            {
                _canvas = this.CreateGraphics();
                _canvas.Clear(Color.Blue);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopLoop();
            _canvas.Dispose();
            _canvas = null;
        }
    }
}
