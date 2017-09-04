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


        public Form1()
        {
            InitializeComponent();

            Init();

            StartLoop();
        }

        private void Init()
        {
            engine = new Simple3DEngine();
            engine.SetCameraLookAt(new Vector4(0, 3f, 0f, 1), new Vector4(0, -1, 0, 1), new Vector4(0, 0, 1, 1));
            engine.SetCameraProperty(width, height, 90f, 0.1f, 50f);
            engine.InitTexture();
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
               // CamreaMove();
                DrawBox();
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
                    bitmap.SetPixel(i, height - 1 - j, Color.FromArgb(ConverTo256(r),ConverTo256(g),ConverTo256(b),ConverTo256(a)));
                }
            }
        }

        private int ConverTo256(float f)
        {
            return (int)MathUtil.Clamp((int)(f * 255), 0, 255);
        }

        private void DrawWireFrame()
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    var originClor = bitmap.GetPixel(i, height - 1 - j);
                    //bitmap.SetPixel(i, height - 1 - j, _camera.wireFrameBuffer[i, j] ? Color.Green : originClor);
                }
            }
        }

        private void DrawBox()
        {
            //DrawPanel(0, 1, 2, 3);
            //DrawPanel(4, 5, 6, 7);
            //DrawPanel(0, 4, 5, 1);
            //DrawPanel(1, 5, 6, 2);
            DrawPanel(2, 6, 7, 3);
            DrawPanel(3, 7, 4, 0);
        }
        private void DrawPanel(int a, int b, int c, int d)
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
