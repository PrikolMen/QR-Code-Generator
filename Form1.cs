using QRCoder;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace QR_Code_Generator
{

    public partial class Form1 : Form
    {

        QRCodeGenerator qrGenerator = new QRCodeGenerator();
        readonly Font textFont;
        Bitmap Image;

        SaveFileDialog saveFileDialog = new SaveFileDialog();

        private int ImageSize()
        {
            int size = (int)numericUpDown1.Value;
            if (size < 16)
            {
                size = 32;
            }

            return size;
        }

        private void Clear()
        {
            richTextBox1.Clear();

            int size = ImageSize();
            Bitmap bmp = new Bitmap(512, 512);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                RectangleF rectf = new RectangleF(0, 0, bmp.Width, bmp.Height);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

                StringFormat format = new StringFormat()
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };

                g.DrawString("Enter the text...", textFont, Brushes.Black, rectf, format);
                g.Flush();
            }

            pictureBox1.Image = bmp;
            Image = null;
        }

        public Form1()
        {
            saveFileDialog.Filter = "Image|*.png;*.bmp;*.jpg";
            textFont = new Font("Tahoma", 16);
            InitializeComponent();
            Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Image == null)
            {
                MessageBox.Show("The QR code is invalid.");
                return;
            }

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                ImageFormat imageFormat = ImageFormat.Png;
                string fileName = saveFileDialog.FileName;
                switch (Path.GetExtension(fileName))
                {
                    case ".jpg":
                        imageFormat = ImageFormat.Jpeg;
                        break;
                    case ".bmp":
                        imageFormat = ImageFormat.Bmp;
                        break;
                }

                Image.Save(saveFileDialog.FileName, imageFormat);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            string text = richTextBox1.Text;
            if (text== null || text.Length == 0) {
                Clear();
                return;
            }

            QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Image = qrCode.GetGraphic(ImageSize());
            pictureBox1.Image = Image;
        }
    }
}