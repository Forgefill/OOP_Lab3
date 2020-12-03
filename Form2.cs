using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace OOP_Lab3
{
    public partial class Form2 : Form
    {
        Form1 parent;
        public Form2(Form1 a)
        {
            InitializeComponent();
            parent = a;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "") listBox1.Items.Add(textBox1.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<string> result = new List<string>();
            foreach(string i in listBox1.Items)
            {
                result.Add(i);
            }
            parent.words = result;
            Close();
        }
    }
}
