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

namespace AutoComplete
{
    public partial class Form1 : Form
    {
        public AutoComplete ac;
        public Form1()
        {
            InitializeComponent();
            
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text Files(*.txt)|*.txt";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.LoadFile(ofd.FileName, RichTextBoxStreamType.UnicodePlainText);
                StreamReader strReader = new StreamReader(ofd.FileName, System.Text.Encoding.GetEncoding("Arabic"));                
                int c = ofd.FileName.LastIndexOf('\\');               
                toolStripStatusLabel2.Text = ofd.FileName.Substring(c+1);                
                toolStripStatusLabel2.Text += "  ";
            }
        }

        private void btnTrain_Click(object sender, EventArgs e)
        {
            ac = new AutoComplete();
            ac.cutWords(richTextBox1.Text);
            ac.train();
            toolStripProgressBar1.Value = 0;
            timer1.Enabled = true;
            textBox1.Enabled = true;
           // while (toolStripProgressBar1.Value != toolStripProgressBar1.Maximum) ;            
               // toolStripStatusLabel3.Text = "  تم تدريب النموذج";
                //timer1.Enabled = false;            
                foreach (Node nd in ac.Nodes)
                {
                    textBox1.AutoCompleteCustomSource.Add(nd.Name);
                    contextMenuStrip1.Items.Add(nd.Name);
                }
                textBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                textBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (richTextBox1.TextLength > 3)
                btnTrain.Enabled = true;
            else
                btnTrain.Enabled = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (toolStripProgressBar1.Value < toolStripProgressBar1.Maximum)
                toolStripProgressBar1.Value++;
            else
            {
                toolStripStatusLabel3.Text = "  تم تدريب النموذج";
                timer1.Enabled = false;
            }
        }
        void fillList()
        {
            List<Node> list = new List<Node>();
            string[] words;
            int[] wieghts;
            cutWords(textBox1.Text);
                    listBox1.Items.Clear();
                    if (phras.Count > 0)
                    {
                        Node nd = ac.BFSsearch(phras[phras.Count - 1]);
                        if (nd != null)
                        {
                            foreach (Node nod in nd.Next)
                            {
                                list.Add(nod);
                            }
                            words = new string[list.Count];
                            wieghts = new int[list.Count];
                            for (int i = 0; i < list.Count; i++)
                            {
                                words[i] = list[i].Name;
                                wieghts[i]=(ac.findLine(nd, list[i])).Freq;
                            }
                            sort(words, wieghts);
                            for (int i = 0; i < words.Length; i++)
                            {
                                listBox1.Items.Add(words[i]);
                            }
                            if (nd.Next.Count > 0)
                            {
                                listBox1.SelectedIndex = 0;
                                listBox1.Location = new Point(640 - textBox1.Text.Length * 7, 135);
                                listBox1.Visible = true;
                            }
                        }
                    }
        }
        void sort(string[] s,int[] w)
        {
            bool changed = true;
            while (changed)
            {
                changed = false;
                for (int i = 0; i < w.Length; i++)
                {
                    if (i + 1 < w.Length)
                    {
                        if (w[i] < w[i + 1])
                        {
                            int t = w[i];
                            w[i] = w[i + 1];
                            w[i + 1] = t;
                            string ts = s[i];
                            s[i] = s[i + 1];
                            s[i + 1] = ts;
                            changed = true;
                        }
                    }
                }
            }
        }
        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {  
            if (e.KeyCode == Keys.Space)
            {
                if (textBox1.TextLength > 0)
                {
                    fillList();
                }
            }
            else if (e.KeyCode == Keys.Down)
            { listBox1.Focus(); }

            else if (e.KeyCode == Keys.Back)
            {
                if (textBox1.TextLength > 0)
                {
                    fillList();
                }
                else
                {
                    listBox1.Visible = false;
                    listBox1.Items.Clear();
                }
            }       
        }

        private void listBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter||e.KeyCode==Keys.Tab)
            {
                textBox1.Text += listBox1.SelectedItem;
                listBox1.Visible = false;
                textBox1.Focus();
                textBox1.SelectionStart = textBox1.TextLength;                
            }
        }
        List<string> phras = new List<string>();
        public void cutWords(string txt)
        {
            string str;
            phras.Clear();
            int wordPos = 0;
            for (int i = 0; i < txt.Length; i++)
            {
                if (txt[i] == ' ')
                {
                    str = txt[wordPos].ToString();
                    for (int j = wordPos + 1; j < i; j++)
                    {
                        str += txt[j];
                    }
                    // str = txt.Substring(wordPos, i - 1);
                    phras.Add(str);
                    if (i + 1 < txt.Length && txt[i + 1] == ' ')
                        i++;
                    wordPos = i + 1;
                }
            }
        }
    }
}
