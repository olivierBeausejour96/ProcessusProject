using System;
using System.Windows;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace VersionOfficielle
{
    public partial class frmMain : Form
    {

        public frmMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Analyseur qwe = new Analyseur(Properties.Resources.Sample1);

            MessageBox.Show(qwe.STvalueStatement1());
            MessageBox.Show(qwe.STvalueStatement2());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Analyseur qwe = new Analyseur(Properties.Resources.Sample1);

            MessageBox.Show(qwe.MTvalueStatement1());
            MessageBox.Show(qwe.MTvalueStatement2());
        }
    }
}
