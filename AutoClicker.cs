using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;

namespace AutoClicker
{
    public partial class AutoClicker : Form
    {
        private bool autoClickerActive = false;
        private bool mouseButtonDown = false;
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        public AutoClicker()
        {
            InitializeComponent();
            this.KeyPreview = true;
            this.KeyDown += AutoClicker_KeyDown;
            this.KeyUp += AutoClicker_KeyUp;
            this.MouseDown += AutoClicker_MouseDown;
            this.MouseUp += AutoClicker_MouseUp;
        }

        private void AutoClicker_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F6)
            {
                ToggleAutoClicker();
            }
        }

        private void AutoClicker_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseButtonDown = true;
                StartAutoClickerIfActive();
            }
        }

        private void AutoClicker_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseButtonDown = false;
            }
        }

        private void AutoClicker_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F6)
            {
                ToggleAutoClicker();
            }
        }

        private void ToggleAutoClicker()
        {
            autoClickerActive = !autoClickerActive;
            if (autoClickerActive)
            {
                StartAutoClickerIfActive();
            }
        }

        private void StartAutoClickerIfActive()
        {
            if (autoClickerActive && mouseButtonDown)
            {
                // Start the auto-clicking loop
                Thread autoClickThread = new Thread(AutoClickLoop);
                autoClickThread.Start();
            }
        }

        private void AutoClickLoop()
        {
            while (autoClickerActive && mouseButtonDown)
            {
                mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                Thread.Sleep(500); // Click every 500 milliseconds
                mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
            }
        }
    }
}
