using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace Morfologia
{
    public partial class Form1 : Form
    {
        string NazwaObr;
        Bitmap obraz;

        public Form1()
        {
            InitializeComponent();
        }

        void wczytajObraz()
        {
            try
            {
                openFileDialog1.Filter = "Pliki obrazu|*.jpg; *.jpeg; *.png; *.gif; *.bmp; *.tif";
                DialogResult result = openFileDialog1.ShowDialog();

                if (result == DialogResult.OK)
                {
                    NazwaObr = openFileDialog1.FileName.ToString();
                    if (NazwaObr != null)
                    {
                        obraz = new Bitmap(NazwaObr);
                        obraz = new Bitmap(obraz, 256, 256);
                        ObrazWejscie.Image = obraz;
                    }
                }
                //NazwaObr=openFileDialog1.FileName;
                obrazToolStripMenuItem.Enabled = true;
                ObrazWyjscie.Image = new Bitmap(1, 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void otwórzObrazToolStripMenuItem_Click(object sender, EventArgs e)
        {
            wczytajObraz();
        }

        private void wyjścieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void filtracjaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Morfologia morf = new Morfologia(5,0);
            Bitmap obraz = (Bitmap)ObrazWejscie.Image;
            ObrazWyjscie.Image = morf.Filtracja(obraz);
        }

        private void dyls5K0ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Morfologia morf = new Morfologia(5, 0);
            Bitmap obraz = (Bitmap)ObrazWejscie.Image;
            ObrazWyjscie.Image = morf.Erozja(morf.Dylatacja(obraz));
        }

        private void dyls5K45ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Morfologia morf = new Morfologia(5, 45);
            Bitmap obraz = (Bitmap)ObrazWejscie.Image; 
            ObrazWyjscie.Image = morf.Erozja(morf.Dylatacja(obraz));
        }

        private void dyls11K0ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Morfologia morf = new Morfologia(11, 0);
            Bitmap obraz = (Bitmap)ObrazWejscie.Image;
            ObrazWyjscie.Image = morf.Erozja(morf.Dylatacja(obraz));
        }

        private void dyls11K45ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Morfologia morf = new Morfologia(11, 45);
            Bitmap obraz = (Bitmap)ObrazWejscie.Image;
            ObrazWyjscie.Image = morf.Erozja(morf.Dylatacja(obraz));
        }

        private void eros5K0ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Morfologia morf = new Morfologia(5, 0);
            Bitmap obraz = (Bitmap)ObrazWejscie.Image;
            ObrazWyjscie.Image = morf.Erozja(obraz);
        }

        private void eros5K45ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Morfologia morf = new Morfologia(5, 45);
            Bitmap obraz = (Bitmap)ObrazWejscie.Image;
            ObrazWyjscie.Image = morf.Erozja(obraz);
        }

        private void eross11K0ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Morfologia morf = new Morfologia(11, 0);
            Bitmap obraz = (Bitmap)ObrazWejscie.Image;
            ObrazWyjscie.Image = morf.Erozja(obraz);
        }

        private void eross11K45ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Morfologia morf = new Morfologia(11, 45);
            Bitmap obraz = (Bitmap)ObrazWejscie.Image;
            ObrazWyjscie.Image = morf.Erozja(obraz);
        }

        private void grs5K0ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Morfologia morf = new Morfologia(5, 0);
            Bitmap obraz = (Bitmap)ObrazWejscie.Image;
            ObrazWyjscie.Image = morf.Gradient(obraz);
        }

        private void grs5K0ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Morfologia morf = new Morfologia(5, 45);
            Bitmap obraz = (Bitmap)ObrazWejscie.Image;
            ObrazWyjscie.Image = morf.Gradient(obraz);
        }

        private void grs5K0ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Morfologia morf = new Morfologia(11, 0);
            Bitmap obraz = (Bitmap)ObrazWejscie.Image;
            ObrazWyjscie.Image = morf.Gradient(obraz);
        }

        private void grs5K0ToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Morfologia morf = new Morfologia(11, 45);
            Bitmap obraz = (Bitmap)ObrazWejscie.Image;
            ObrazWyjscie.Image = morf.Gradient(obraz);
        }

        private void zapiszObrazToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Title = "Save an Image File";
            saveFileDialog1.Filter = "Tiff Image|*.tif";
            saveFileDialog1.ShowDialog();
            System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog1.OpenFile();

            ObrazWyjscie.Image.Save(fs, System.Drawing.Imaging.ImageFormat.Tiff);
        }
    }
}
