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
    public partial class Form2 : Form
    {
        RichTextBox richTextBox;
        public Form2(ref RichTextBox richTextBox1)
        {
            richTextBox = richTextBox1;
            InitializeComponent();

        }
        private void Form2_Load(object sender, EventArgs e)
        {

        }
        public void button1_Click(object sender, EventArgs e)
        {
            int i = richTextBox.SelectionStart + richTextBox.SelectedText.Length;
            RichTextBoxFinds richTextBoxFinds = new RichTextBoxFinds();
            if (radioButton1.Checked)
            {
                if (richTextBox.SelectionStart == 0)
                    MessageBox.Show("找不到\"" + textBox1.Text + "\"", "记事本", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                else
                {
                    if (checkBox1.Checked)
                        richTextBoxFinds = RichTextBoxFinds.MatchCase | RichTextBoxFinds.Reverse;
                    else
                        richTextBoxFinds = RichTextBoxFinds.Reverse;
                    if (richTextBox.SelectedText.ToLower()== textBox1.Text.ToLower())
                        i--;
                    if (richTextBox.Find(textBox1.Text, 0, i, richTextBoxFinds) < 0)
                        MessageBox.Show("找不到\"" + textBox1.Text + "\"", "记事本", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    else
                    {
                        richTextBox.Select(richTextBox.Find(textBox1.Text, 0, i, richTextBoxFinds), textBox1.TextLength);
                    }
                }
            }
            else if (radioButton2.Checked)
            {
                if (i == richTextBox.Text.Length)
                    MessageBox.Show("找不到\"" + textBox1.Text + "\"", "记事本", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                else
                {
                    if (checkBox1.Checked)
                        richTextBoxFinds = RichTextBoxFinds.MatchCase;
                    else
                        richTextBoxFinds = RichTextBoxFinds.None;
                    if (richTextBox.Find(textBox1.Text, i, richTextBoxFinds) < 0 || richTextBox.Text.Length == i)
                        MessageBox.Show("找不到\"" + textBox1.Text + "\"", "记事本", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    else
                    {
                        richTextBox.Select(richTextBox.Find(textBox1.Text, i, richTextBoxFinds), textBox1.Text.Length);
                    }
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
                button1.Enabled = true;
            else
                button1.Enabled = false;
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
