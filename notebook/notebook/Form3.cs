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
    public partial class Form3 : Form
    {
        RichTextBox richTextBox;
        public Form3(ref RichTextBox richTextBox1)
        {
            richTextBox = richTextBox1;
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int i = richTextBox.SelectionStart + richTextBox.SelectedText.Length;
            RichTextBoxFinds richTextBoxFinds = new RichTextBoxFinds();

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

        private void button2_Click(object sender, EventArgs e)
        {
            if (richTextBox.SelectedText != textBox1.Text)
                button1_Click(null, null);
            else
            {
                richTextBox.SelectedText = textBox2.Text;
                button1_Click(null, null);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int i = richTextBox.Find(textBox1.Text, 0, RichTextBoxFinds.None);
            string str;
            if (i >= 0)
            {
                if (i == 0)
                    str = "";
                else
                str = richTextBox.Text.Substring(0, i);
                if (checkBox1.Checked)
                    richTextBox.Text = richTextBox.Text.Replace(textBox1.Text, textBox2.Text);
                else
                {
                    for (i += textBox1.TextLength; richTextBox.Find(textBox1.Text, i, RichTextBoxFinds.None) > 0; i = richTextBox.Find(textBox1.Text, i, RichTextBoxFinds.None) + textBox1.TextLength)
                    {
                        str += textBox2.Text;
                        // if (richTextBox.Find(textBox1.Text, i + richTextBox.Find(textBox1.Text, i, RichTextBoxFinds.None) + textBox1.TextLength, RichTextBoxFinds.None) < 0)
                        str += richTextBox.Text.Substring(i, richTextBox.Find(textBox1.Text, i, RichTextBoxFinds.None)-i);
                    }
                    str += textBox2.Text;

                    if (i != richTextBox.TextLength)
                        str += richTextBox.Text.Substring(i,richTextBox.TextLength-i);
                    richTextBox.Text = str;
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                button1.Enabled = true;
                button2.Enabled = true;
                button3.Enabled = true;
            }
            else
            {
                button1.Enabled = false;
                button2.Enabled = false;
                button3.Enabled = false;
            }
        }
    }
}
