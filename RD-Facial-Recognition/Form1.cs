using System;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Face;
using System.Collections.Generic;
using System.IO;
using Emgu.CV.CvEnum;
using System.Drawing;
using System.Drawing.Imaging;
using RD_Facial_Recognition.Storage;
using System.Linq;

namespace RD_Facial_Recognition
{
    public partial class Form1 : Form
    {
        public ICapture Capture { get; set; }
        public EigenFaceRecognizer EigenFaceRecognizer { get; set; }
        public CascadeClassifier FaceDetection { get; set; }
        public DataStoreAccess DataStoreAccess { get; set; }
        public Mat Frame { get; set; }
        public List<Image<Gray, byte>> Faces { get; set; }
        public List<int> Labels { get; set; }
        public int ProcessImageWidth { get; set; } = 128;
        public int ProcessImageHeight { get; set; } = 150;
        public int TimerCounter { get; set; } = 0;
        public int TimeLimit { get; set; } = 30;
        public int ScanCounter { get; set; } = 0;
        public string YMLPath { get; set; } = $"{AppDomain.CurrentDomain.BaseDirectory}Faces/TrainedFaces.yml";
        public string ConnectionString { get; set; } = "TrainedFaces.db";
        public Timer Timer { get; set; }
        public string UserName { get; set; }

        public Form1()
        {
            InitializeComponent();

            EigenFaceRecognizer = new EigenFaceRecognizer(4, 800);
            DataStoreAccess = new DataStoreAccess(ConnectionString);
            FaceDetection = new CascadeClassifier(Path.GetFullPath($"{AppDomain.CurrentDomain.BaseDirectory}haarcascade_frontalface_default.xml"));
            Frame = new Mat();
            Faces = new List<Image<Gray, byte>>();
            Labels = new List<int>();

            if (File.Exists(YMLPath))
            {
                EigenFaceRecognizer.Read(YMLPath);

            }

            var allFaces = DataStoreAccess.CallFaces("ALL_USERS");
            if (allFaces != null)
            {
                for (int i = 0; i < allFaces.Count; i++)
                {
                    Stream stream = new MemoryStream();
                    stream.Write(allFaces[i].Image, 0, allFaces[i].Image.Length);
                    var faceImage = new Image<Gray, byte>(new Bitmap(stream));
                    Faces.Add(faceImage);
                    Labels.Add(allFaces[i].UserId);
                }

                EigenFaceRecognizer.Train(ConvertImageToMat(Faces).ToArray(), Labels.ToArray());

                btnPredict.Enabled = true;
                MessageBox.Show("Training Completed!");
            }
            else
            {
                MessageBox.Show("Nothing to traing!");
            }

            BeginCapture();
        }

        private void BeginCapture()
        {
            if (Capture == null)
            {
                Capture = new VideoCapture();
            }

            Application.Idle += Capture_ImageGrabbed;

            rtbOutPut.AppendText($"Webcam starting... {Environment.NewLine}");
        }

        private void Capture_ImageGrabbed(object sender, EventArgs e)
        {
            Frame = Capture.QueryFrame();
            var imageFrame = Frame.ToImage<Bgr, byte>();

            if (imageFrame != null)
            {
                var grayFrame = imageFrame.Convert<Gray, byte>();
                var faces = FaceDetection.DetectMultiScale(grayFrame, 1.3, 5);

                foreach (var face in faces)
                {
                    CvInvoke.Rectangle(imageFrame, face, new MCvScalar(255, 0, 0, 255), 2);
                }

                imbWebcam.Image = imageFrame;
            }
        }

        private void BtnTrain_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtUserId.Text))
            {
                Timer = new Timer();
                Timer.Interval = 500;
                Timer.Tick += Timer_Tick;
                Timer.Start();
            }
        }

        private void BtnPredict_Click(object sender, EventArgs e)
        {
            Frame = Capture.QueryFrame();
            var imageFrame = Frame.ToImage<Gray, byte>();

            if (imageFrame != null)
            {
                var faces = FaceDetection.DetectMultiScale(imageFrame, 1.3, 5);
                var userDetected = new List<string>();

                foreach (var face in faces)
                {
                    var processImage = imageFrame.Copy(face).Resize(ProcessImageWidth, ProcessImageHeight, Inter.Cubic);
                    var resultEigen = EigenFaceRecognizer.Predict(processImage);

                    if (resultEigen.Label != -1)
                    {
                        userDetected.Add(DataStoreAccess.GetUsername(resultEigen.Label));
                    }
                    else
                    {
                        userDetected.Add("Unknown");
                    }
                }

                lbResult.Text = string.Join(",", userDetected);
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Frame = Capture.QueryFrame();
            var imageFrame = Frame.ToImage<Gray, byte>();

            if (TimerCounter < TimeLimit)
            {
                TimerCounter++;

                if (imageFrame != null)
                {
                    var faces = FaceDetection.DetectMultiScale(imageFrame, 1.3, 5);

                    if (faces.Length > 0)
                    {
                        var processImage = imageFrame.Copy(faces[0]).Resize(ProcessImageWidth, ProcessImageHeight, Inter.Cubic);
                        Faces.Add(processImage);
                        Labels.Add(Convert.ToInt32(txtUserId.Text));
                        ScanCounter++;
                        rtbOutPut.AppendText($"{ScanCounter} Success Scan Taken... {Environment.NewLine}");
                        rtbOutPut.ScrollToCaret();
                    }
                }
            }
            else
            {
                var trainFaces = ConvertImageToMat(Faces);

                foreach (var face in trainFaces)
                {
                    DataStoreAccess.SaveFace(Convert.ToInt32(txtUserId.Text), txtUserName.Text, ConvertImageToBytes(face.Bitmap));
                }

                EigenFaceRecognizer.Train(trainFaces.ToArray(), Labels.ToArray());

                EigenFaceRecognizer.Write(YMLPath);
                Timer.Stop();
                TimerCounter = 0;
                btnPredict.Enabled = true;
                rtbOutPut.AppendText($"Training Completed! {Environment.NewLine}");
                MessageBox.Show("Training Completed!");
            }
        }

        private byte[] ConvertImageToBytes(Bitmap bitmap)
        {
            using (var ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }

        private List<Mat> ConvertImageToMat<TColor, TDepth>(List<Image<TColor, TDepth>> src)
            where TColor : struct, IColor where TDepth : new()
        {
            var result = new List<Mat>();

            foreach (var face in Faces)
            {
                result.Add(face.Mat);
            }

            return result;
        }
    }
}
