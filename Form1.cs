using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NopePad
{
    public partial class Form1 : Form
    {
        private string fileName = null;
        private bool isUnsaved = false;
        private bool ignoreTextChangedEvent = false;
        public Form1()
        {
            InitializeComponent();
            UpdateTitle();
        }
        private void UpdateTitle()
        {
            string file;
            if (string.IsNullOrEmpty(fileName))
                file = "Unnamed";
            else
                file = Path.GetFileName(fileName);
            if (isUnsaved)
                Text = file + "* - Notepad";
            else 
                Text = file + " - Notepad";
        }

        private void öppnaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var eventArgs = new FormClosingEventArgs(CloseReason.None, false);
            Form1_FormClosing(null, eventArgs);

            if (eventArgs.Cancel)
                return;

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ignoreTextChangedEvent = true;
                textBox.Text = File.ReadAllText(openFileDialog.FileName);
                fileName = openFileDialog.FileName;
                isUnsaved = false;
                UpdateTitle();
            }
        }

        private void avslutaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void sparaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    fileName = saveFileDialog.FileName;
                else
                    return;
            }

            File.WriteAllText(fileName, textBox.Text);
            isUnsaved = false;
            UpdateTitle();
        }

        private void sparaSomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                fileName = saveFileDialog.FileName;
            else
                return;

            UpdateTitle();
        }

        private void SaveFile()
        {
            if (string.IsNullOrEmpty(fileName))
            {
                if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    fileName = saveFileDialog.FileName;
                else
                    return;
            }

            File.WriteAllText(fileName, textBox.Text);
            isUnsaved = false;
            UpdateTitle();
        }

        private void nyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var eventArgs = new FormClosingEventArgs(CloseReason.None, false);
            Form1_FormClosing(null, eventArgs);

            if (eventArgs.Cancel)
                return;

            textBox.Text = string.Empty;
            fileName = null;
            isUnsaved = false;
            UpdateTitle();
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            isUnsaved = true;
            if (ignoreTextChangedEvent)
            {
                ignoreTextChangedEvent = false;
                return;
            }

            UpdateTitle();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(isUnsaved)
            {
                var res = MessageBox.Show(this, "Whould you like to save changes?", "Notepad", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);

                if (res == System.Windows.Forms.DialogResult.Yes)
                {
                    SaveFile();
                }
                else if (res == System.Windows.Forms.DialogResult.No)
                {
                    //Do nothing
                }
                else if (res == System.Windows.Forms.DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }


    }
}
