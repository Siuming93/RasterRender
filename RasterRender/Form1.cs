using System;
using System.Collections.Generic;
using System.Drawing;
using System.Timers;
using System.Windows.Forms;
using RasterRender.Engine.Mathf;
using Timer = System.Timers.Timer;
using RasterRender.Engine;
using RasterRender.Const;
using RasterRender.Engine.Simple;
using Color = System.Drawing.Color;

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

            Init();

            StartLoop();
        }

        private void Init()
        {
            engine = new Simple3DEngine();
            engine.SetCameraLookAt(new Vector4(0, 10f, 0, 1), new Vector4(0, -1, 0, 1), new Vector4(0, 0, 1, 1));
            engine.SetCameraProperty(width, height, 90f, 0.1f, 50f);
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
        Simple3DEngine engine = new Simple3DEngine();
        private object lockObj = new object();
        private int index;
        private Bitmap bitmap = new Bitmap(width, height);
        private const int width = 400, height = 600;
        private void Tick(object sender, ElapsedEventArgs e)
        {
            lock (lockObj)
            {
                //todo 访问相机的渲染缓存 并渲染
                if (_canvas == null || _canvas.IsVisibleClipEmpty)
                {
                    return;
                }
               // CamreaMove();
                DrawBox();
                //_camera.DrawWireFrame(PrimitiveConst.CubeVertexs, PrimitiveConst.CubeTriangles);
                //_camera.DrawPrimitives(PrimitiveConst.CubeVertexs, PrimitiveConst.CubeTriangles);
                //_camera.DrawPrimitives(PrimitiveConst.CubeVertexs, new List<int>() {  2, 7, 3, }, PrimitiveConst.CubeUVs);
                //_camera.DrawPrimitives(PrimitiveConst.CubeVertexs, new List<int>() { 2, 4, 6, 2, 7, 3,  }, PrimitiveConst.CubeUVs);
                //_camera.DrawWireFrame(PrimitiveConst.CubeVertexs, new List<int>() { 4,5,6,1,2,3,});

                DrawRenderTexture();
                 //DrawWireFrame();

                if (_canvas != null && !_canvas.IsVisibleClipEmpty)
                {
                    _canvas.DrawImage(bitmap, new Point(0, 0));
                }
            }
        }

        private void DrawRenderTexture()
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    float r = engine.colorBuffer[i, j].r, g = engine.colorBuffer[i, j].g, b = engine.colorBuffer[i, j].b, a = engine.colorBuffer[i, j].a;
                    bitmap.SetPixel(i, height - 1 - j, Color.FromArgb((int)r*255, (int)g *255, (int)b *255, (int)a *255));
                }
            }
        }

        private void DrawWireFrame()
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    var originClor = bitmap.GetPixel(i, height - 1 - j);
                    bitmap.SetPixel(i, height - 1 - j, _camera.wireFrameBuffer[i, j] ? Color.Green : originClor);
                }
            }
        }

        private void DrawBox()
        {
            DrawPanel(_camera, 0, 1, 2, 3);
            DrawPanel(_camera, 4, 5, 6, 7);
            DrawPanel(_camera, 0, 4, 5, 1);
            DrawPanel(_camera, 1, 5, 6, 2);
            DrawPanel(_camera, 2, 6, 7, 3);
            DrawPanel(_camera, 3, 7, 4, 0);
        }
        private void DrawPanel(Camera camera, int a, int b, int c, int d)
        {
            Vertex p1 = PrimitiveConst.mesh[a], p2 = PrimitiveConst.mesh[b], p3 = PrimitiveConst.mesh[c], p4 = PrimitiveConst.mesh[d];
            p1.uv.x = 0; p1.uv.y = 0; p2.uv.x = 0; p2.uv.y = 1;
            p3.uv.x = 1; p3.uv.y = 1; p4.uv.x = 1; p4.uv.y = 0;

            engine.DrawPrimitive(p1, p2, p3);
            engine.DrawPrimitive(p3, p4, p1);
        }

        int dir;
        private float x = -2;
        float y = -2;
        float z = 5;
        private void CamreaMove()
        {
            index++;
            dir = (int)Math.Pow(-1, (int)index / 30);
            y += dir * 0.1f;
            x += dir*0.2f;
            //z += dir * 0.025f;

            engine.SetCameraLookAt(new Vector4(x, y, z), new Vector3(-x, 0, -z).Normalize(), new Vector4(0, 1, 0, 1));
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            lock (lockObj)
            {
                _canvas = this.CreateGraphics();
                _canvas.Clear(Color.Blue);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CamreaMove();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopLoop();
            _canvas.Dispose();
            _canvas = null;
        }
    }
}
