using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtractText
{
    public partial class frmCapture : Form
    {
        Rectangle rec = new Rectangle(0, 0, 0, 0);

        public frmCapture()
        {
            InitializeComponent();
            Top = 0;
            Left = 0;
            Width = Screen.PrimaryScreen.Bounds.Width;
            Height = Screen.PrimaryScreen.Bounds.Height;
            this.DoubleBuffered = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.Aquamarine, rec);
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                rec = new Rectangle(e.X, e.Y, 0, 0);
                Invalidate();
            }
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                rec.Width = e.X - rec.X;
                rec.Height = e.Y - rec.Y;
                Invalidate();
            }
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            Hide();
            using (Bitmap bitmap = new Bitmap(rec.Width, rec.Height))
            {
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CopyFromScreen(rec.X, rec.Y, 0, 0, rec.Size, CopyPixelOperation.SourceCopy);
                }
                bitmap.Save($@"{ Application.StartupPath }/capture.jpg", ImageFormat.Bmp);
            }
            base.OnMouseUp(e);
            Close();
        }

        private void frmCapture_Load(object sender, EventArgs e)
        {

        }
    }
}
