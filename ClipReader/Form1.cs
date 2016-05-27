using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SpeechLib;

namespace ClipReader
{
    public partial class Form1 : Form
    {
        private bool nowClose = false;
        private bool winVisible = true;
        private bool paused = false;
        private int speed = 0;
        private String clipboardText = "";

        private SpVoice speech;

        public Form1()
        {
            InitializeComponent();
            int w = Screen.PrimaryScreen.WorkingArea.Width - this.Width;
            int h = Screen.PrimaryScreen.WorkingArea.Height - this.Height;
            this.Location = new Point(w, h);
            winVisible = true;
            speech = new SpVoice();
            speech.Rate = speed;
            timer1.Start();
        }

        private void Form1_Resize(object sender, System.EventArgs e)
        {
            if (FormWindowState.Minimized == WindowState)
                Hide();
        }

        private void CloseMenuItem_Click(object sender, EventArgs e)
        {
            nowClose = true;
            Close();
        }

        private void RestoreMenuItem_Click(object sender, EventArgs e)
        {
            if (winVisible)
            {
                Hide();
                winVisible = false;
            }
            else
            {
                Show();
                winVisible = true;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!nowClose)
            {
                Form1.ActiveForm.WindowState = FormWindowState.Minimized;
                e.Cancel = true;
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (winVisible)
            {
                Hide();
                winVisible = false;
            }
            else
            {
                Show();
                winVisible = true;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            String newClipboardText = Clipboard.GetText();
            if (clipboardText != newClipboardText)
            {
                clipboardText = newClipboardText;
                READ();
            }
        }

        private void READ()
        {
            speech.Rate = speed;
            speech.Speak(clipboardText, SpeechVoiceSpeakFlags.SVSFlagsAsync);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (paused)
            {
                paused = false;
                speech.Resume();
                timer1.Start();
                button1.Text = "PAUSE";
            }
            else
            {
                paused = true;
                speech.Pause();
                timer1.Stop();
                button1.Text = "UNPAUSE";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            speech.Speak("", SpeechVoiceSpeakFlags.SVSFPurgeBeforeSpeak);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            speed = trackBar1.Value;
            speech.Rate = speed;
        }
    }    

}
