using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace notebook
{
    public partial class Form4 : Form
    {
        RichTextBox RichTextBox;
        public Form4(ref RichTextBox richTextBox)
        {
            RichTextBox = richTextBox;
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(textBox1.Text) - 1 <= RichTextBox.GetLineFromCharIndex(RichTextBox.TextLength))
            {
                RichTextBox.SelectionStart = RichTextBox.GetFirstCharIndexFromLine(Convert.ToInt32(textBox1.Text) - 1);
                this.Hide();
            }
            else
            {
                MessageBox.Show("行数超过了总行数", "记事本 - 跳行", MessageBoxButtons.OK);
                if(DialogResult==DialogResult.OK)
                {
                    textBox1.Text = (RichTextBox.GetLineFromCharIndex(RichTextBox.TextLength) + 1).ToString();
                }
            }
        }

        private void Form4_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
        }

        private void Form4_Activated(object sender, EventArgs e)
        {
            textBox1.Text = (RichTextBox.GetLineFromCharIndex(RichTextBox.SelectionStart) + 1).ToString();
            textBox1.SelectAll();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((int)e.KeyChar < 48 || (int)e.KeyChar > 57)
            {
                e.Handled = true;
                errorProvider1.SetError(this.textBox1, "你只能在此处键入数字");
            }
            else
                errorProvider1.Dispose();
        }
    }
}
