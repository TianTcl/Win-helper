using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;
using System.IO;

namespace TclP07CS___SysUI_Ctrl
{
    public partial class main : Form
    {
        public main()
        {
            InitializeComponent(); menuStrip1.Cursor = Cursors.Default;
        }

        private void taskbar_c_CheckedChanged(object sender, EventArgs e)
        {
            if (taskbar_c.Checked == true)
            {
                Taskbar.Show();
            }
            else if (taskbar_c.Checked == false)
            {
                Taskbar.Hide();
            }
        }

        private void vol_mixer_b_Click(object sender, EventArgs e)
        {
            Run.exe("SndVol");
        }

        private void volmute_c_CheckedChanged(object sender, EventArgs e)
        {
            Volume.Toggle_mute(this.Handle);
        }

        void power_c(double option, string action_m, string delay)
        {
            DialogResult perm = MessageBox.Show("Are you going to "+action_m+delay,"Permission",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if (perm == DialogResult.Yes)
            {
                if (option == 1)
                {
                    MessageBox.Show("Shutting down...", "Power", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Thread.Sleep(1500); SendKeys.Send("{ENTER}");
                    Run.argexe("shutdown.exe", "-s -t 5 -c \"TclP07CS forces shutting down.\"");
                }
                else if (option == 2)
                {
                    MessageBox.Show("Hibernating...", "Power", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Thread.Sleep(1500); SendKeys.Send("{ENTER}");
                    Run.argexe("shutdown.exe", "-h");
                }
                else if (option == 3)
                {
                    MessageBox.Show("Restarting...", "Power", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Thread.Sleep(1500); SendKeys.Send("{ENTER}");
                    Run.argexe("shutdown.exe", "-r -t 5 -c \"TclP07CS forces restarting.\"");
                }
                else if (option == 4)
                {
                    MessageBox.Show("Logging off...", "Power", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Thread.Sleep(1500); SendKeys.Send("{ENTER}");
                    Run.argexe("shutdown.exe", "-l");
                }
            }
        }

        private void shutdownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            power_c(1, "shutdown", " in 5 seconds.");
        }

        private void sleepToolStripMenuItem_Click(object sender, EventArgs e)
        {
            power_c(2, "hibernate", ".");
        }

        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            power_c(3, "restart", " in 5 seconds.");
        }

        private void logOffToolStripMenuItem_Click(object sender, EventArgs e)
        {
            power_c(4, "Log off", ".");
        }

        private void lockComputerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(@"C:\Users\tianc\Desktop\TclP06CS.exe.lnk");
        }

        private void txt_copy_b_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Clipboard.SetText(txt_input_t.Text.Trim());
        }

        private void txt_paste_b_Click(object sender, EventArgs e)
        {
            string txt_clipboard_v = "";
            if (System.Windows.Forms.Clipboard.ContainsText(TextDataFormat.Text))
                txt_clipboard_v = System.Windows.Forms.Clipboard.GetText(TextDataFormat.Text);
            txt_input_t.Text = txt_clipboard_v;
        }

        private void txt_clear_b_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Clipboard.Clear();
        }

        private void msc_browse_b_Click(object sender, EventArgs e)
        {
            OpenFileDialog msc_location_d = new OpenFileDialog();
            if (msc_location_d.ShowDialog() == DialogResult.OK)
            {
                msc_location_v.Text = msc_location_d.InitialDirectory + msc_location_d.FileName;
            }
        }

        System.Media.SoundPlayer msc_player = new System.Media.SoundPlayer();
        [DllImport("winmm.dll")]
        public static extern long waveOutSetVolume(UInt32 deviceID, UInt32 Volume);
        [DllImport("winmm.dll")]
        public static extern long waveOutGetVolume(UInt32 deviceID, out UInt32 Volume);

        void GetVolume()
        {
            UInt32 d, v; d = 0; long i = waveOutGetVolume(d, out v); UInt32 vleft = v & 0xFFFF;
            UInt32 vright = (v & 0xFFFF0000) >> 16;
            msc_volume_s.Value = (int.Parse(vleft.ToString()) | int.Parse(vright.ToString())) * (msc_volume_s.Maximum - msc_volume_s.Minimum) / 0xFFFF;
        }

        private void msc_volume_s_Scroll(object sender, EventArgs e)
        {
            UInt32 Value = (System.UInt32)((double)0xffff * (double)msc_volume_s.Value / (double)(msc_volume_s.Maximum - msc_volume_s.Minimum));//trackbar's value from 0x0000 to 0xFFFF     
            if (Value < 0) Value = 0;
            if (Value > 0xffff) Value = 0xffff;
            UInt32 left = (System.UInt32)Value;//valume of left channel   
            UInt32 right = (System.UInt32)Value;//right  
            waveOutSetVolume(0, left << 16 | right);
        }

        private void msc_play_b_Click(object sender, EventArgs e)
        {
            msc_player.SoundLocation = msc_location_v.Text.Trim();
            msc_player.Play();
        }

        private void msc_loop_b_Click(object sender, EventArgs e)
        {
            msc_player.SoundLocation = msc_location_v.Text.Trim();
            msc_player.PlayLooping();
        }

        private void msc_stop_b_Click(object sender, EventArgs e)
        {
            msc_player.Stop();
        }

        private void txt_delete_b_Click(object sender, EventArgs e)
        {
            txt_input_t.Text = "";
        }

        public int random_int(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        private void randomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Double ranscr = random_int(1, 4);
            if (ranscr == 1) { Run.scr("Bubbles"); }
            if (ranscr == 2) { Run.scr("Mystify"); }
            if (ranscr == 3) { Run.scr("Ribbons"); }
            if (ranscr == 4) { Run.scr("ssText3d"); }
        }

        private void scrsaver(object sender, EventArgs e)
        {
            ToolStripMenuItem what = sender as ToolStripMenuItem;
            Run.scr(what.Name);
        }

        private void main_Load(object sender, EventArgs e)
        {
            foreach (var this_app_runs in Process.GetProcessesByName("TclP07CS - SysUI Ctrl.exe")) { this_app_runs.Kill(); }
        }

        private void main_DoubleClick(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Maximized;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void basicToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Run.argexe("Explorer", @"shell:::{BB06C0E4-D293-4f75-8A90-CB05B6477EEE}");
        }

        private void advancedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Run.exe("msinfo32");
        }

        private void all_set(object sender, EventArgs e)
        {
            ToolStripMenuItem what = sender as ToolStripMenuItem;
            Run.ms(what.Name);
        }

        private void main_set(object sender, EventArgs e)
        {
            ToolStripMenuItem what = sender as ToolStripMenuItem;
            Run.cpl(what.Name);
        }

        private void systemInformationToolStripMenuItem_DoubleClick(object sender, EventArgs e)
        {
            Run.ms("about");
        }

        private void btn_bt_open_set_Click(object sender, EventArgs e)
        {
            Run.ms("bluetooth");
        }

        private void addRemoveProgramsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Run.ms();
        }

        private void acessibilityPropertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Run.exe("Control");
        }

        private void applicationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem what = sender as ToolStripMenuItem;
            Run.exe(what.Name);
        }

        private void mostlyUsedAppsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem what = sender as ToolStripMenuItem;
            Run.exe(what.Name);
        }

        private void runToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Run.argexe("rundll32.exe", "shell32.dll,#61");
        }

        private void taskSchedulerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("Taskschd.msc");
        }
    }

    public class Taskbar
    {
        [DllImport("user32.dll")]
        private static extern int FindWindow(string className, string windowText);

        [DllImport("user32.dll")]
        private static extern int ShowWindow(int hwnd, int command);

        [DllImport("user32.dll")]
        public static extern int FindWindowEx(int parentHandle, int childAfter, string className, int windowTitle);

        [DllImport("user32.dll")]
        private static extern int GetDesktopWindow();

        private const int SW_HIDE = 0;
        private const int SW_SHOW = 1;

        protected static int Handle
        {
            get
            {
                return FindWindow("Shell_TrayWnd", "");
            }
        }

        protected static int HandleOfStartButton
        {
            get
            {
                int handleOfDesktop = GetDesktopWindow();
                int handleOfStartButton = FindWindowEx(handleOfDesktop, 0, "button", 0);
                return handleOfStartButton;
            }
        }

        private Taskbar()
        {
            // hide ctor
        }

        public static void Show()
        {
            ShowWindow(Handle, SW_SHOW);
            ShowWindow(HandleOfStartButton, SW_SHOW);
        }

        public static void Hide()
        {
            ShowWindow(Handle, SW_HIDE);
            ShowWindow(HandleOfStartButton, SW_HIDE);
        }
    }

    public class Volume
    {
        private const int APPCOMMAND_VOLUME_MUTE = 0x80000;
        private const int WM_APPCOMMAND = 0x319;

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessageW(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        public static void Toggle_mute(IntPtr handle)
        {
            SendMessageW(handle, WM_APPCOMMAND, handle, (IntPtr)APPCOMMAND_VOLUME_MUTE);
        }
    }

    public class Run
    {
        public static void scr(string app)
        {
            Process.Start(@"resx\"+app+".scr");
        }

        public static void exe(string app)
        {
            Process.Start(app+".exe");
        }

        public static void cpl(string app)
        {
            Process.Start(app+".cpl");
        }

        public static void ms(string app="")
        {
            Process.Start("ms-settings:"+app);
        }

        public static void argexe(string app, string arg)
        {
            Process.Start(app,arg);
        }
    }
}
