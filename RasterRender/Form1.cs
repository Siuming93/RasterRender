using System;
using System.Drawing;
using System.Timers;
using System.Windows.Forms;
using Timer = System.Timers.Timer;
using RasterRender.Engine;
using RasterRender.Const;
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
            engine.SetCameraLookAt(new Vector4(-2f, 2f, 2f, 1), new Vector4(1, -1, -1, 1), new Vector4(1, -1, 2, 1));
            engine.SetCameraProperty(width, height, 120f, 0.1f, 50f);
            engine.InitTexture();
        }

        private void StartLoop()
        {
            _timer = _timer ?? new Timer(1000f / 5);
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
        private Bitmap bitmap = new Bitmap(800, 800);
        private const int width = 800, height = 800;
        private void Tick(object sender, ElapsedEventArgs e)
        {
            lock (lockObj)
            {
                //todo 访问相机的渲染缓存 并渲染
                if (_canvas == null || _canvas.IsVisibleClipEmpty)
                {
                    return;
                }
                //CamreaMove();
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
            int w = 800, h = 800;
            for (int i = 0; i < w; i++)
            {
                int x = (int)(i / (float)w * width);
                for (int j = 0; j < h; j++)
                {
                    int y = (int)(j / (float)h * height);
                    float r = engine.colorBuffer[x, y].r, g = engine.colorBuffer[x, y].g, b = engine.colorBuffer[x, y].b, a = engine.colorBuffer[x, y].a;
                    var c = Color.FromArgb(ConverTo256(r), ConverTo256(g), ConverTo256(b), ConverTo256(a));
                    if(c.A == 0)
                    {
                        bitmap.SetPixel(i, h - 1 - j, Color.SkyBlue);
                    }
                    else
                    {
                        bitmap.SetPixel(i, h - 1 - j, c);
                    }
                }
            }
        }

        private int ConverTo256(float f)
        {
            return (int)MathUtil.Clamp((int)(f * 255), 0, 255);
        }

        private void DrawBox()
        {
            DrawPanel(0, 1, 2, 3);
            DrawPanel(4, 5, 6, 7);
            DrawPanel(0, 4, 5, 1);
            DrawPanel(2, 6, 7, 3);
            DrawPanel(1, 5, 6, 2);
            DrawPanel(3, 7, 4, 0);
        }
        private void DrawPanel(int a, int b, int c, int d)
        {
            Vertex p1 = PrimitiveConst.mesh[a], p2 = PrimitiveConst.mesh[b], p3 = PrimitiveConst.mesh[c], p4 = PrimitiveConst.mesh[d];
            p1.uv.x = 0; p1.uv.y = 0; p2.uv.x = 0; p2.uv.y = 1;
            p3.uv.x = 1; p3.uv.y = 1; p4.uv.x = 1; p4.uv.y = 0;

            engine.DrawPrimitive(p1, p2, p3);
            engine.DrawPrimitive(p4, p3, p1);
        }

        int dir;
        private float x = 0;
        float y = 3;
        float z = 0;
        private void CamreaMove()
        {
            index++;
            dir = (int)Math.Pow(-1, (int)index / 30);
            y += dir * 0.1f;
            x += dir*0.2f;
            //z += dir * 0.025f;
            engine.SetCameraLookAt(new Vector4(x, y, z), new Vector4(0, -1, 0, 1), new Vector4(0, 0, 1, 1));
            engine.SetCameraProperty(width, height, 90f, 0.1f, 50f);
        }


        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            lock (lockObj)
            {
                _canvas = this.CreateGraphics();
                _canvas.Clear(Color.SkyBlue);
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
