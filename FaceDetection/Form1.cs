﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
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
        private string cascaeClassifierPath = Path.GetFullPath(@"../../data/haarcascade_frontalface_default.xml");
        private string cascaeClassifierEyePath = Path.GetFullPath(@"../../data/haarcascade_eye.xml");
        private List<Mat> faceImages = new List<Mat>();
        private List<string> faceLabels = new List<string>();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (capture == null)
            {
                capture = new VideoCapture();
                face = new CascadeClassifier(cascaeClassifierPath);
                //dataStoreAccess = new DataStoreAccess("sample.db");
                //recognizerEngine = new RecognizerEngine($"sample.db", $"{path}TrainedFaces.xml", dataStoreAccess);
                recognizerEngine = new RecognizerEngine();
                timer1.Enabled = true;
                timer1.Start();
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (capture != null)
            {
                long detectionTime;
                List<Rectangle> faces = new List<Rectangle>();
                List<Rectangle> eyes = new List<Rectangle>();

                currentFrame = capture.QueryFrame();
                var imgSrc = currentFrame.ToImage<Bgr, byte>();

                DetectFace.Detect(imgSrc, cascaeClassifierPath, cascaeClassifierEyePath, faces, eyes, out detectionTime);

                foreach (Rectangle face in faces)
                {
                    var username = "Unknown";
                    CvInvoke.Rectangle(imgSrc, face, new Bgr(Color.Red).MCvScalar, 2);
                    var grayFace = imgSrc.Copy(face).Convert<Gray, byte>().Resize(100, 100, Inter.Cubic);
                    var result = recognizerEngine.Recognize(grayFace, faceImages, faceLabels);
                    if (!string.IsNullOrEmpty(result))
                    {
                        username = result;
                    }
                    //else
                    //{
                    //    username = dataStoreAccess.GetUsername(result);
                    //}
                    imgSrc.Draw(username, new Point(face.X - 2, face.Y - 2), FontFace.HersheyTriplex, 0.5d, new Bgr(Color.LightGreen));
                }

                //foreach (Rectangle eye in eyes)
                //    CvInvoke.Rectangle(imgSrc, eye, new Bgr(Color.Blue).MCvScalar, 2);

                pictureBox1.Image = imgSrc.Bitmap;
            }
        }

        private void BtnAddFace_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtUserName.Text))
            {
                using (var imgSrc = currentFrame.ToImage<Bgr, byte>())
                {
                    var faces = face.DetectMultiScale(imgSrc, 1.2, 10, new Size(20, 20), Size.Empty);
                    TrainedFace = imgSrc.Copy(faces[0]).Convert<Gray, byte>().Resize(100, 100, Inter.Cubic);
                    TrainedFace._EqualizeHist();
                    ptbAddFace.Image = TrainedFace.Bitmap;

                    //dataStoreAccess.SaveFace(txtUserName.Text, ConvertImageToBytes(TrainedFace.Bitmap));

                    //recognizerEngine.TrainRecognizer();

                    faceImages.Add(TrainedFace.Mat);
                    faceLabels.Add(txtUserName.Text);
                }
            }
            else
            {
                MessageBox.Show("Please enter face's name.");
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
    }
}
