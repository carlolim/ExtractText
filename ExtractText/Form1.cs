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
            ExtractText();
        }

        private void ExtractText()
        {
            string path = $@"{Application.StartupPath}\capture.jpg";
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
                            File.Delete(path);
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
    }
}
