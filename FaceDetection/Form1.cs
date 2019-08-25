using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace FaceDetection
{
    public partial class Form1 : Form
    {
        private ICapture capture;
        private CascadeClassifier face;
        private CascadeClassifier eye;
        private RecognizerEngine recognizerEngine;
        private DataStoreAccess dataStoreAccess;
        private Mat currentFrame;
        private Image<Gray, byte> TrainedFace = null;
        private string path = AppDomain.CurrentDomain.BaseDirectory;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (capture == null)
            {
                capture = new VideoCapture();
                var cascaeClassifierPath = Path.GetFullPath(@"../../data/haarcascade_frontalface_default.xml");
                face = new CascadeClassifier(cascaeClassifierPath);
                recognizerEngine = new RecognizerEngine($"sample.db", $"{path}TrainedFaces.xml");
                dataStoreAccess = new DataStoreAccess("sample.db");
                recognizerEngine.TrainRecognizer();
                timer1.Enabled = true;
                timer1.Start();
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (capture != null)
            {
                currentFrame = capture.QueryFrame();
                var imgSrc = currentFrame.ToImage<Bgr, byte>();
                var faces = face.DetectMultiScale(imgSrc, 1.2, 10, new Size(20, 20));
                foreach (var face in faces)
                {
                    var userName = string.Empty;
                    try
                    {
                        var grayFace = imgSrc.Copy(face).Convert<Gray, byte>();
                        grayFace._EqualizeHist();
                        var userId = recognizerEngine.RecognizeUser(grayFace.Resize(320, 240, Inter.Cubic));
                        userName = dataStoreAccess.GetUsername(userId);
                    }
                    catch (Exception ex)
                    {
                        userName = "Unknown";
                    }

                    imgSrc.Draw(userName, new Point(face.X - 2, face.Y - 2), FontFace.HersheyTriplex, 0.5d, new Bgr(Color.LightGreen));
                    imgSrc.Draw(face, new Bgr(0, 0, 255), 2);
                }
                pictureBox1.Image = imgSrc.Bitmap;
            }
        }

        private void BtnAddFace_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtUserName.Text))
            {
                var imgSrc = currentFrame.ToImage<Bgr, byte>();
                var faces = face.DetectMultiScale(imgSrc, 1.2, 10, new Size(20, 20));
                TrainedFace = imgSrc.Copy(faces[0]).Convert<Gray, byte>().Resize(320, 240, Inter.Cubic);
                TrainedFace._EqualizeHist();
                ptbAddFace.Image = TrainedFace.Bitmap;

                dataStoreAccess.SaveFace(txtUserName.Text, ConvertImageToBytes(TrainedFace.Bitmap));

                recognizerEngine.TrainRecognizer();
            }
            else
            {
                MessageBox.Show("Please enter face's name.");
            }
        }

        private byte[] ConvertImageToBytes(Bitmap bitmap)
        {
            var ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Png);
            return ms.ToArray();
        }
    }
}
