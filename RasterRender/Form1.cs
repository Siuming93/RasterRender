using System;
using System.Drawing;
using System.Timers;
using System.Windows.Forms;
using RasterRender.Engine.Mathf;
using Timer = System.Timers.Timer;

namespace RasterRender
{
    public partial class Form1 : Form
    {
        private Graphics _canvas;
        private Timer _timer;
        public Form1()
        {
            InitializeComponent();

            StartLoop();
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
                    if (_canvas == null||_canvas.IsVisibleClipEmpty)
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
