using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Chrome_Canary_x64_Launcher
{
    public partial class Form1 : Form 
    {
        string test;

        public Form1()
        {
            InitializeComponent();
        }
        private void RadioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked == true)
            {
                test = "3";
            }
        }

        private void RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                test = "1";
            }
        }

        private void RadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true)
            {
                test = "2";
            }
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            if (test("3"))
            {
                Console.WriteLine("Test 3");
            }
        }
    }
}
