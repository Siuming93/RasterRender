using System.Drawing;
using System.Timers;
using System.Windows.Forms;
using Timer = System.Timers.Timer;

namespace RasterRender
{
    public partial class Form1 : Form
    {
        private Graphics _canvas;

        public Form1()
        {
            InitializeComponent();

            StartLoop();
        }

        private void StartLoop()
        {
            Timer timer = new Timer(1000f/60);
            timer.Elapsed += Tick;
            timer.Start();
        }

        private object lockObj = new object();
        private void Tick(object sender, ElapsedEventArgs e)
        {
            lock (lockObj)
            {
                _canvas = _canvas ?? this.CreateGraphics();
                _canvas.DrawLine(new Pen(Color.Aqua, 1f), 0f, 0f, 100, 100);
            }
        }
    }
}
