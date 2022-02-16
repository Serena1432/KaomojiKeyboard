using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KaomojiKeyboard
{
    public partial class PromptForm : Form
    {
        public PromptForm(string text, string title)
        {
            InitializeComponent();
            this.Text = title;
            label1.Text = text;
        }

        public string Value { get; set; }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (String.IsNullOrEmpty(textBox1.Text)) MessageBox.Show("You need to type anything!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    this.Value = textBox1.Text;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
        }

        private void PromptForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult != DialogResult.OK) this.DialogResult = DialogResult.Cancel;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBox1.Text)) MessageBox.Show("You need to type anything!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                this.Value = textBox1.Text;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
