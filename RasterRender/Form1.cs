using System;
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
            _camera.Init(0, new Vector4(0, -2, 0, 1), new Vector4(0, 1, 0, 1), new Vector4(0, 1, 0, 1), 0.1f, 5f, 90f, 200f, 200f);
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
        private void Tick(object sender, ElapsedEventArgs e)
        {
            lock (lockObj)
            {
                try
                {

                    //todo 访问相机的渲染缓存 并渲染
                    if (_canvas == null || _canvas.IsVisibleClipEmpty)
                        return;
                    index++;
                    _canvas.Clip = new Region(new Rectangle(0, 0, 200, 200));
                    _canvas.Clear(Color.Azure);
                    for (int i = 0; i < 200; i++)
                    {
                        _canvas.DrawLine(new Pen(Color.Aqua, 1f), 100f, 100f, 100 + 100 * MathUtil.Fast_Cos(index + i * 5), 100 + 100 * MathUtil.Fast_Sin(index + i * 5));
                    }
                    _canvas.DrawLine(new Pen(Color.Aqua, 1f), 100f, 100f, 200, 200);
                }
                catch (Exception exception)
                {
                }

            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            lock (lockObj)
            {
                _canvas = this.CreateGraphics();
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
