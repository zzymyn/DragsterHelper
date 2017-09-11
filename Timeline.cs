using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace DragsterHelper
{
    public partial class Timeline : Form
    {
        private static Regex s_StellaName = new Regex(@"Stella.*Dragster", RegexOptions.IgnoreCase | RegexOptions.Multiline);

        private Thread m_Thread = null;

        private Stopwatch m_Stopwatch = null;
        private Bitmap m_TargetBitmap = null;
        private Bitmap m_CurrentBitmap = null;
        private Bitmap m_RenderBitmap = null;

        private int m_PrevFrameBeg = 0;
        private int m_PrevFrameEnd = 0;

        public Timeline()
        {
            InitializeComponent();

            m_TargetBitmap = new Bitmap(Data.Keys.Length, 5);
            for (int x = 0; x < Data.Keys.Length; ++x)
            {
                var k = Data.Keys[x];
                if (k.Gas)
                    m_TargetBitmap.SetPixel(x, 1, Color.Red);
                if (k.Shift)
                    m_TargetBitmap.SetPixel(x, 3, Color.Green);
            }
        }

        private void Timeline_Load(object sender, EventArgs e)
        {
            Left = Screen.PrimaryScreen.Bounds.Left;
            Width = Screen.PrimaryScreen.Bounds.Width;
            Top = Screen.PrimaryScreen.Bounds.Top;
            Height = 200;

            m_Thread = new Thread(() => RunThread());
            m_Thread.Start();

            BeginInvoke((Action)(() =>
            {
                ActivateStella();
            }));
        }

        private void Timeline_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_Thread.Abort();
        }

        private void RunThread()
        {
            while (true)
            {
                if ((Win32.GetAsyncKeyState(Keys.Right) & 0x8000) != 0)
                {
                    m_CurrentBitmap = new Bitmap(Data.Keys.Length, 5);
                    m_PrevFrameBeg = 0;
                    m_PrevFrameEnd = 0;
                    m_Stopwatch = Stopwatch.StartNew();
                }

                if (m_Stopwatch != null)
                {
                    var currFrameBeg = (int)Math.Floor(30.0 * m_Stopwatch.Elapsed.TotalSeconds);
                    var currFrameEnd = (int)Math.Floor(30.0 * m_Stopwatch.Elapsed.TotalSeconds + 0.1);

                    bool changed = false;

                    if (currFrameBeg >= 0 && currFrameBeg < m_CurrentBitmap.Width && currFrameBeg != m_PrevFrameBeg)
                    {
                        if ((Win32.GetAsyncKeyState(Keys.LControlKey) & 0x8000) != 0 || (Win32.GetAsyncKeyState(Keys.Space) & 0x8000) != 0)
                        {
                            m_CurrentBitmap.SetPixel(currFrameBeg, 1, Color.Red);
                            changed = true;
                        }
                        if ((Win32.GetAsyncKeyState(Keys.Left) & 0x8000) != 0)
                        {
                            m_CurrentBitmap.SetPixel(currFrameBeg, 3, Color.Green);
                            changed = true;
                        }
                    }

                    if (changed)
                    {
                        lock (m_Thread)
                        {
                            m_RenderBitmap = new Bitmap(m_CurrentBitmap);
                        }
                    }

                    m_PrevFrameBeg = currFrameBeg;
                    m_PrevFrameEnd = currFrameEnd;
                }
            }
        }

        private void m_Timer_Tick(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void Timeline_Paint(object sender, PaintEventArgs e)
        {
            Bitmap renderBitmap = null;
            lock (m_Thread)
            {
                if (m_RenderBitmap != null)
                    renderBitmap = new Bitmap(m_RenderBitmap);
            }

            e.Graphics.PixelOffsetMode = PixelOffsetMode.Half;
            e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;

            var s = ClientSize;

            if (m_TargetBitmap != null)
                e.Graphics.DrawImage(m_TargetBitmap, 0.0f, 0.0f, s.Width, 0.5f * s.Height);
            if (renderBitmap != null)
                e.Graphics.DrawImage(renderBitmap, 0.0f, 0.5f * s.Height, s.Width, 0.5f * s.Height);

            if (m_Stopwatch != null)
            {
                int currFrame = (int)Math.Floor(30.0 * m_Stopwatch.Elapsed.TotalSeconds);

                int xTime = s.Width * currFrame / m_TargetBitmap.Width;
                e.Graphics.DrawRectangle(Pens.Black, xTime, 0, 10, s.Height);
            }
        }

        private void ActivateStella()
        {
            var stellaHwnd = FindStellaWindow();

            if (stellaHwnd != IntPtr.Zero)
            {
                // Make Stella the top
                Win32.BringWindowToTop(stellaHwnd);
            }
        }

        private static IntPtr FindStellaWindow()
        {
            IntPtr stellaHwnd = IntPtr.Zero;

            Win32.EnumWindows((IntPtr hwnd, IntPtr lParam) =>
            {
                var text = Win32.GetWindowText(hwnd);
                if (!s_StellaName.IsMatch(text))
                    return true;
                stellaHwnd = hwnd;
                return true;
            }, IntPtr.Zero);

            return stellaHwnd;
        }
    }
}
