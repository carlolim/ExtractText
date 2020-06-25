using Emgu.CV;
using Emgu.CV.OCR;
using Emgu.CV.Structure;
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

namespace ExtractText
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void btnCapture_Click(object sender, EventArgs e)
        {
            var frm = new frmCapture();
            Hide();
            frm.ShowDialog();
            Show();
            ExtractText($@"{Application.StartupPath}\capture.jpg");
        }

        private void ExtractText(string path)
        {
            if (File.Exists(path))
            {
                Task.Run(() =>
                {
                    using (var img = new Image<Bgr, byte>(path))
                    {
                        string tessdata = $@"{Application.StartupPath}\tessdata";
                        using (var ocrProvider = new Tesseract(tessdata, "eng", OcrEngineMode.TesseractLstmCombined))
                        {
                            ocrProvider.SetImage(img);
                            ocrProvider.Recognize();
                            string text = ocrProvider.GetUTF8Text().TrimEnd();
                            SetTextResult(text);
                        }
                    }
                });
            }
        }

        delegate void SetTextCallback(string text);
        private void SetTextResult(string result)
        {
            if (txtResult.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetTextResult);
                this.Invoke(d, new object[] { result });
            }
            else
            {
                txtResult.Text = result;
            }
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog theDialog = new OpenFileDialog();
            theDialog.Title = "Open Text File";
            theDialog.Filter = "Files|*.jpg;*.jpeg;*.png;*.bmp;";
            theDialog.Multiselect = false;
            theDialog.InitialDirectory = @"C:\";
            if (theDialog.ShowDialog() == DialogResult.OK)
            {
                ExtractText(theDialog.FileName);
            }
        }
    }
}
