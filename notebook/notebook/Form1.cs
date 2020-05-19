using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Printing;

namespace notebook
{
    public partial class Form1 : Form
    {
        Form2 f; Form3 form3; Form4 form4;
        string filename = string.Empty;
        string fileText = string.Empty;
        StringReader lineReader;
        private void openFileDialog1()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "文本文档(*.txt)|*.txt|所有文件(*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filename = openFileDialog1.FileName;
                string Name = filename.Substring(filename.LastIndexOf("\\") * 1);
                Name = Name.Replace("\\", "");
                this.Text = Name + " - 记事本";
                FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                StreamReader sr = new StreamReader(fs);
                richTextBox1.Text = sr.ReadToEnd();
                sr.Close();
                fileText = richTextBox1.Text;
                撤销ToolStripMenuItem.Enabled = false;
                撤销UToolStripMenuItem.Enabled = false;
                toolStripButton10.Enabled = false;
            }
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            IDataObject dataObject = Clipboard.GetDataObject();
            if (dataObject.GetDataPresent(DataFormats.Text))
            {
                粘贴PToolStripMenuItem.Enabled = true;
                粘贴ToolStripMenuItem.Enabled = true;
                toolStripButton6.Enabled = true;
            }
            else
            {
                粘贴PToolStripMenuItem.Enabled = false;
                粘贴ToolStripMenuItem.Enabled = false;
                toolStripButton6.Enabled = false;
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            bool i = true;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            if (string.IsNullOrEmpty(filename))
            {
                if (richTextBox1.Text != "")

                {
                    DialogResult result = MessageBox.Show("是否将更改保存到 无标题?", "记事本", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes)
                    {
                        saveFileDialog1.FileName = "*.txt";
                        saveFileDialog1.Filter = "文本文档(*.txt)|*.txt|所有文件(*.*)|*.*";
                        saveFileDialog1.Title = "另存为";
                        saveFileDialog1.FilterIndex = 1;
                        saveFileDialog1.RestoreDirectory = true;
                        if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                        {
                            richTextBox1.SaveFile(saveFileDialog1.FileName, RichTextBoxStreamType.PlainText);
                            filename = saveFileDialog1.FileName;
                            StreamWriter sw = new StreamWriter(filename);
                            string tempS = richTextBox1.Text.Replace("\n", "\r\n");
                            sw.Write(tempS);
                            sw.Flush();
                            sw.Close();
                            fileText = richTextBox1.Text;
                            this.Text = saveFileDialog1.FileName.Substring(saveFileDialog1.FileName.LastIndexOf("\\") * 1).Replace("\\", "") + " - 记事本";
                            e.Cancel = true;
                        }
                        else
                            e.Cancel = true;
                    }
                    if (result == DialogResult.Cancel)
                        e.Cancel = true;
                }
                i = false;
            }
            if (!string.IsNullOrEmpty(filename) && i)
            {
                if (fileText != richTextBox1.Text)
                {
                    DialogResult result = MessageBox.Show("是否将更改保存到\n" + filename + "?", "记事本", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes)
                    {
                        StreamWriter sw = new StreamWriter(filename);
                        string tempS = richTextBox1.Text.Replace("\n", "\r\n");
                        sw.Write(tempS);
                        sw.Flush();
                        sw.Close();
                    }
                    if (result == DialogResult.Cancel)
                        e.Cancel = true;
                }
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            撤销UToolStripMenuItem.Enabled = true;
            撤销ToolStripMenuItem.Enabled = true;
            toolStripButton10.Enabled = true;
            if (richTextBox1.Text == "")
            {
                查找FToolStripMenuItem.Enabled = false;
                查找下一个NToolStripMenuItem.Enabled = false;
                替换RToolStripMenuItem.Enabled = false;
                toolStripButton8.Enabled = false;
            }
            else
            {
                查找FToolStripMenuItem.Enabled = true;
                查找下一个NToolStripMenuItem.Enabled = true;
                替换RToolStripMenuItem.Enabled = true;
                toolStripButton8.Enabled = true;
            }
        }
        private void richTextBox1_SelectionChanged(object sender, EventArgs e)
        {
            if (richTextBox1.SelectedText != "")
            {
                剪切TToolStripMenuItem1.Enabled = true;
                复制CToolStripMenuItem1.Enabled = true;
                删除DToolStripMenuItem.Enabled = true;
                剪切TToolStripMenuItem.Enabled = true;
                复制CToolStripMenuItem.Enabled = true;
                删除LToolStripMenuItem.Enabled = true;
                toolStripButton4.Enabled = true;
                toolStripButton5.Enabled = true;
                toolStripButton7.Enabled = true;
            }

            else
            {
                剪切TToolStripMenuItem1.Enabled = false;
                复制CToolStripMenuItem1.Enabled = false;
                删除DToolStripMenuItem.Enabled = false;
                剪切TToolStripMenuItem.Enabled = false;
                复制CToolStripMenuItem.Enabled = false;
                删除LToolStripMenuItem.Enabled = false;
                toolStripButton4.Enabled = false;
                toolStripButton5.Enabled = false;
                toolStripButton7.Enabled = false;
            }
            if (richTextBox1.SelectedText == richTextBox1.Text)
            {
                全选AToolStripMenuItem.Enabled = false;
                全选AToolStripMenuItem1.Enabled = false;

            }
            else
            {
                全选AToolStripMenuItem.Enabled = true;
                全选AToolStripMenuItem1.Enabled = true;
            }
            toolStripStatusLabel1.Text = "第" + (richTextBox1.GetLineFromCharIndex(richTextBox1.SelectionStart) + 1).ToString() + "行, 第" + (richTextBox1.SelectionStart - richTextBox1.GetFirstCharIndexOfCurrentLine() + 1).ToString() + "列";
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            f = new Form2(ref richTextBox1);
            form3 = new Form3(ref richTextBox1);
            form4 = new Form4(ref richTextBox1);
        }
        /*******************************  文件  **************************************/

        private void 新建NToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog(); int i = 0;
            if (string.IsNullOrEmpty(filename) && richTextBox1.Text != "")
            {
                DialogResult result = MessageBox.Show("是否将更改保存到 无标题?", "记事本", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    saveFileDialog1.FileName = "*.txt";
                    saveFileDialog1.Filter = "文本文档(*.txt)|*.txt|所有文件(*.*)|*.*";
                    saveFileDialog1.Title = "另存为";
                    saveFileDialog1.FilterIndex = 1;
                    saveFileDialog1.RestoreDirectory = true;
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        filename = saveFileDialog1.FileName;
                        StreamWriter sw = new StreamWriter(filename);
                        string tempS = richTextBox1.Text.Replace("\n", "\r\n");
                        sw.Write(tempS);
                        sw.Flush();
                        sw.Close();
                        this.Text = filename.Substring(filename.LastIndexOf("\\") * 1).Replace("\\", "") + " - 记事本";

                        fileText = richTextBox1.Text;
                        i = 1;
                        撤销ToolStripMenuItem.Enabled = false;
                        撤销UToolStripMenuItem.Enabled = false;
                        toolStripButton10.Enabled = false;
                    }
                }
                if (result == DialogResult.No)
                {
                    richTextBox1.Clear();
                    撤销ToolStripMenuItem.Enabled = false;
                    撤销UToolStripMenuItem.Enabled = false;
                    toolStripButton10.Enabled = false;
                }
            }
            if (!string.IsNullOrEmpty(filename) && i == 0)
            {
                if (fileText != richTextBox1.Text)
                {
                    DialogResult result = MessageBox.Show("是否将更改保存到\n" + filename + "?", "记事本", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes)
                    {
                        StreamWriter sw = new StreamWriter(filename);
                        string tempS = richTextBox1.Text.Replace("\n", "\r\n");
                        sw.Write(tempS);
                        sw.Flush();
                        sw.Close();
                        this.Text = "无标题 - 记事本";
                        richTextBox1.Clear();
                        filename = string.Empty;
                        撤销ToolStripMenuItem.Enabled = false;
                        撤销UToolStripMenuItem.Enabled = false;
                        toolStripButton10.Enabled = false;
                    }
                    if (result == DialogResult.No)
                    {
                        this.Text = "无标题 - 记事本";
                        richTextBox1.Clear();
                        filename = string.Empty;
                        撤销ToolStripMenuItem.Enabled = false;
                        撤销UToolStripMenuItem.Enabled = false;
                        toolStripButton10.Enabled = false;
                    }
                }
                else
                {
                    this.Text = "无标题 - 记事本";
                    richTextBox1.Clear();
                    filename = string.Empty;
                    撤销ToolStripMenuItem.Enabled = false;
                    撤销UToolStripMenuItem.Enabled = false;
                    toolStripButton10.Enabled = false;
                }
            }
        }
        private void 打开OToolStripMenuItem_Click(object sender, EventArgs e)
        {

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            bool i = true;
            if (string.IsNullOrEmpty(filename))
            {
                if (richTextBox1.Text != "")
                {
                    DialogResult result = MessageBox.Show("是否将更改保存到 无标题?", "记事本", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes)
                    {
                        saveFileDialog1.FileName = "*.txt";
                        saveFileDialog1.Filter = "文本文档(*.txt)|*.txt|所有文件(*.*)|*.*";
                        saveFileDialog1.Title = "另存为";
                        saveFileDialog1.FilterIndex = 1;
                        saveFileDialog1.RestoreDirectory = true;
                        if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                        {
                            string tempS = richTextBox1.Text.Replace("\n", "\r\n");
                            filename = saveFileDialog1.FileName;
                            StreamWriter sw = new StreamWriter(filename);
                            sw.Write(tempS);
                            sw.Flush();
                            sw.Close();
                            filename = saveFileDialog1.FileName;
                            string Name = filename.Substring(filename.LastIndexOf("\\") * 1).Replace("\\", "");
                            this.Text = Name + " - 记事本";

                            fileText = richTextBox1.Text;
                            撤销ToolStripMenuItem.Enabled = false;
                            撤销UToolStripMenuItem.Enabled = false;
                            toolStripButton10.Enabled = false;
                        }

                    }
                    if (result == DialogResult.No)
                    {
                        openFileDialog1();
                    }
                }
                else
                {
                    openFileDialog1();
                }
                i = false;
            }
            if (!string.IsNullOrEmpty(filename) && i)
            {
                if (fileText != richTextBox1.Text)
                {
                    DialogResult result = MessageBox.Show("是否将更改保存到\n" + filename + "?", "记事本", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes)
                    {
                        StreamWriter sw = new StreamWriter(filename);
                        string tempS = richTextBox1.Text.Replace("\n", "\r\n");
                        sw.Write(tempS);
                        sw.Flush();
                        sw.Close();
                        openFileDialog1();
                    }
                    if (result == DialogResult.No)
                    {
                        openFileDialog1();
                    }
                }
                else
                {
                    openFileDialog1();
                }
            }
        }
        private void 保存SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "文本文档|*.txt|所有文件(*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            string tempS = richTextBox1.Text.Replace("\n", "\r\n");
            if (string.IsNullOrEmpty(filename))
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    filename = saveFileDialog1.FileName;
                    StreamWriter sw = new StreamWriter(filename);
                    sw.Write(tempS);
                    sw.Flush();
                    sw.Close();
                    this.Text = saveFileDialog1.FileName.Substring(saveFileDialog1.FileName.LastIndexOf("\\") * 1).Replace("\\", "") + " - 记事本";
                    fileText = richTextBox1.Text;

                }
            }
            else
            {
                StreamWriter sw = new StreamWriter(filename);
                sw.Write(tempS);
                sw.Flush();
                sw.Close();
                fileText = richTextBox1.Text;

            }
        }
        private void 另存为AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "文本文档|*.txt|所有文件(*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.FileName = "*.txt";
            string tempS = richTextBox1.Text.Replace("\n", "\r\n");
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filename = saveFileDialog1.FileName.Substring(saveFileDialog1.FileName.LastIndexOf("\\") * 1).Replace("\\", "");
                StreamWriter sw = new StreamWriter(saveFileDialog1.FileName);
                sw.Write(tempS);
                sw.Flush();
                sw.Close();
                this.Text = filename + " - 记事本";
                filename = saveFileDialog1.FileName;
                fileText = richTextBox1.Text;

            }

        }
        private void 退出XToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
            
        }

        /****************************  右键菜单  ***************************************/

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
        }
        private void 撤销UToolStripMenuItem_Click(object sender, EventArgs e)
        {
            撤销ToolStripMenuItem_Click(null, null);
        }
        private void 剪切TToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            richTextBox1.Cut();
        }
        private void 复制CToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            richTextBox1.Copy();
            粘贴PToolStripMenuItem.Enabled = true;
            粘贴ToolStripMenuItem.Enabled = true;
        }
        private void 粘贴PToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Paste(DataFormats.GetFormat(TextDataFormat.UnicodeText.ToString()));
        }
        private void 删除DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectedText = "";
        }
        private void 全选AToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectAll();
        }

        /****************************  编辑  ***************************************/

        private void 编辑EToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }
        private void 撤销ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text == fileText)
                richTextBox1.Redo();
            else
                richTextBox1.Undo();
        }
        private void 剪切TToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Cut();
        }
        private void 复制CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Copy();
            粘贴PToolStripMenuItem.Enabled = true;
            粘贴ToolStripMenuItem.Enabled = true;
        }
        private void 粘贴ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Paste(DataFormats.GetFormat(TextDataFormat.UnicodeText.ToString()));
            richTextBox1.Text = richTextBox1.Text;
        }
        private void 删除LToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectedText = "";
        }
        private void 全选AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectAll();
        }
        private void 时间日期DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectedText = DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + " " + DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Day.ToString();
        }

        private void 查找FToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (f.Visible)
                f.Focus();
            else if (form3.Visible)
                form3.Focus();
            else
            {
                f.Show();
                f.Location = new Point(this.Location.X + 60, this.Location.Y + 200);
            }
        }

        private void 查找下一个NToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!f.button1.Enabled)
                查找FToolStripMenuItem_Click(null, null);
            else
                f.button1_Click(null, null);
        }

        private void 替换RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (f.Visible)
                f.Focus();
            else if (form3.Visible)
                form3.Focus();
            else
            {
                form3.Show();
                form3.Location = new Point(this.Location.X + 70, this.Location.Y + 140);
            }
        }

        private void 自动换行WToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (自动换行WToolStripMenuItem.Checked)
            {
                自动换行WToolStripMenuItem.Checked = false;
                richTextBox1.WordWrap = false;
                richTextBox1.SelectionStart = 0;
                转到GToolStripMenuItem.Enabled = true;
            }
            else
            {
                自动换行WToolStripMenuItem.Checked = true;
                richTextBox1.WordWrap = true;
                richTextBox1.SelectionStart = 0;
                转到GToolStripMenuItem.Enabled = false;
            }
        }

        private void 转到GToolStripMenuItem_Click(object sender, EventArgs e)
        {
            form4.ShowDialog();
            form4.Location = new Point(this.Location.X + 10, this.Location.Y + 45);
        }

        private void 状态栏ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (状态栏ToolStripMenuItem.Checked)
            {
                状态栏ToolStripMenuItem.Checked = false;
                statusStrip1.Visible = false;
            }
            else
            {
                状态栏ToolStripMenuItem.Checked = true;
                statusStrip1.Visible = true;
            }
        }

        private void 输入法ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (输入法ToolStripMenuItem.Text == "关闭输入法(&L)")
            {
                输入法ToolStripMenuItem.Text = "打开输入法(&O)";
                richTextBox1.ImeMode = ImeMode.NoControl;
            }
            else
            {
                输入法ToolStripMenuItem.Text = "关闭输入法(&L)";
                richTextBox1.ImeMode = ImeMode.On;
            }
        }

        private void 字体FToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fontDialog1.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.Font = fontDialog1.Font;
            }
        }

        private void 查看帮助HToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("咨询开发者邮箱:732352310@qq.com", "帮助");
        }

        private void 关于记事本AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("天津大学仁爱学院 洪绍华\n开发环境\n硬件环境：\n  处理器：Intel(R) Core(TM) i5 - 4210M CPU @ 2.60GHz 2.60GHz\n  安装内存：8.00GB\n  系统类型：64位操作系统，基于x64的处理器\n软件环境：\n  运行软件：Visual Studio2017", "关于", MessageBoxButtons.OK);
        }
        /****************************  工具栏***************************************/
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            新建NToolStripMenuItem_Click(e, e);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            打开OToolStripMenuItem_Click(e, e);
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            保存SToolStripMenuItem_Click(e, e);
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            剪切TToolStripMenuItem_Click(e, e);
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            复制CToolStripMenuItem_Click(e, e);
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            粘贴PToolStripMenuItem_Click(e, e);
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            删除DToolStripMenuItem_Click(e, e);
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            查找FToolStripMenuItem_Click(e, e);
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            替换RToolStripMenuItem_Click(e, e);
        }

        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            撤销ToolStripMenuItem_Click(e, e);
        }

        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.ForeColor = colorDialog1.Color;
                toolStripButton11.ForeColor = colorDialog1.Color;
            }
        }

        private void toolStripButton12_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.BackColor = colorDialog1.Color;
            }
        }

        private void 打印ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(printDocument_PrintPage);
            pageSetupDialog1.Document = this.printDocument1;
            lineReader = new StringReader(richTextBox1.Text);
            printDialog1.Document = printDocument1;
            if (printDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    printDocument1.Print();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "打印出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    printDocument1.PrintController.OnEndPrint(printDocument1, new System.Drawing.Printing.PrintEventArgs());
                }
            }
        }
        private void printDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics; //获得绘图对象
            float linesPerPage = 0; //页面的行号
            float yPosition = 0;   //绘制字符串的纵向位置
            int count = 0; //行计数器
            float leftMargin = e.MarginBounds.Left; //左边距
            float topMargin = e.MarginBounds.Top; //上边距
            string line = null;
            Font printFont = this.richTextBox1.Font; //当前的打印字体
            SolidBrush myBrush = new SolidBrush(Color.Black);//刷子
            linesPerPage = e.MarginBounds.Height / printFont.GetHeight(g);//每页可打印的行数
            while (count < linesPerPage && ((line = lineReader.ReadLine()) != null))
            {
                yPosition = topMargin + (count * printFont.GetHeight(g));
                g.DrawString(line, printFont, myBrush, leftMargin, yPosition, new StringFormat());
                count++;
            }
            if (line != null)
                e.HasMorePages = true;
            else
                e.HasMorePages = false;
        }

        private void ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            pageSetupDialog1.Document = printDocument1;
            pageSetupDialog1.ShowDialog();
        }
    }
}