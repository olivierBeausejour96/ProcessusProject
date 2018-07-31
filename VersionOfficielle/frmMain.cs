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
            //Analyse tous les sample selon la strategie SingleThread et affiche le temps dexecution
            var watch = System.Diagnostics.Stopwatch.StartNew();

            List<Bitmap> samples = new List<Bitmap>();

            samples.Add(Properties.Resources.Sample0);
            samples.Add(Properties.Resources.Sample1);
            samples.Add(Properties.Resources.Sample2);
            samples.Add(Properties.Resources.Sample3);
            samples.Add(Properties.Resources.Sample4);

            foreach (var sample in samples)
            {
                Analyseur qwe = new Analyseur(sample);

                qwe.STvalueStatement1();
                qwe.STvalueStatement2();
            }

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            MessageBox.Show("Time taken for SingleThread: " + elapsedMs + "ms");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Analyse tous les sample selon la strategie MultiThread et affiche le temps dexecution
            var watch = System.Diagnostics.Stopwatch.StartNew();
            Analyseur qwe = new Analyseur(Properties.Resources.Sample1);

            List<Bitmap> samples = new List<Bitmap>();

            samples.Add(Properties.Resources.Sample0);
            samples.Add(Properties.Resources.Sample1);
            samples.Add(Properties.Resources.Sample2);
            samples.Add(Properties.Resources.Sample3);
            samples.Add(Properties.Resources.Sample4);

            foreach (var sample in samples)
            {
                qwe.MTvalueStatement1();
                qwe.MTvalueStatement2();

                watch.Stop();
            }

            var elapsedMs = watch.ElapsedMilliseconds;
            MessageBox.Show("Time taken for SingleThread: " + elapsedMs + "ms");
        }

        private void qwe(Bitmap sample)
        {
            //Analyse sample avec la méthode SingleThread et Multithread et affiche le résultat
            Analyseur qwe = new Analyseur(sample);
            string text = "***\nSingleThreadResult\n***\n";
            text += qwe.STvalueStatement1();
            text += "\n\n***\nMultiThreadResult\n***\n";
            text += qwe.MTvalueStatement1();

            MessageBox.Show(text);

            text = "***\nSingleThreadResult\n***\n";
            text += qwe.STvalueStatement2();
            text += "\n\n***\nMultiThreadResult\n***\n";
            text += qwe.MTvalueStatement2();

            MessageBox.Show(text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //s0
            qwe(Properties.Resources.Sample0);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //s1
            qwe(Properties.Resources.Sample1);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //2
            qwe(Properties.Resources.Sample2);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //s3
            qwe(Properties.Resources.Sample3);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //s4
            qwe(Properties.Resources.Sample4);
        }
    }
}
