using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoComplete
{
    public partial class Form2 : Form
    {
        AutoComplete ac;
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
        public void showForm2( AutoComplete a)
        {
            ac = a;
            foreach (Node nd in ac.Nodes)
            {
                textBox1.AutoCompleteCustomSource.Add(nd.Name);                
            }
            textBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
            this.Show();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            int x=e.KeyChar.CompareTo(' ');
            if(x==0)
            {
                listBox1.Items.Add("malek");
                listBox1.SelectedIndex = 0;
                listBox1.Visible = true;
                listBox1.Focus();
            }
        }
    }
}
