using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Face;
using Emgu.CV.Structure;
using System;
using System.Drawing;
using System.IO;

namespace FaceDetection
{
    public class RecognizerEngine
    {
        private FaceRecognizer _faceRecognizer;
        private DataStoreAccess _dataStoreAccess;
        private string _recognizerFilePath;

        public RecognizerEngine(string databasePath, string recognizerFilePath)
        {
            _recognizerFilePath = recognizerFilePath;
            _dataStoreAccess = new DataStoreAccess(databasePath);
            _faceRecognizer = new EigenFaceRecognizer(80, double.PositiveInfinity);
        }

        public bool TrainRecognizer()
        {
            var allFaces = _dataStoreAccess.CallFaces("ALL_USERS");
            if (allFaces != null)
            {
                var faceImages = new Mat[allFaces.Count];
                var faceLabels = new int[allFaces.Count];
                for (int i = 0; i < allFaces.Count; i++)
                {
                    Stream stream = new MemoryStream();
                    stream.Write(allFaces[i].Image, 0, allFaces[i].Image.Length);
                    var faceImage = new Image<Gray, byte>(new Bitmap(stream));
                    faceImages[i] = faceImage.Resize(320, 240, Inter.Cubic).Mat;
                    faceLabels[i] = allFaces[i].UserId;
                }
                _faceRecognizer.Train(faceImages, faceLabels);
                _faceRecognizer.Write(_recognizerFilePath);
            }
            return true;

        }

        public void LoadRecognizerData()
        {
            _faceRecognizer.Read(_recognizerFilePath);
        }

        public int RecognizeUser(Image<Gray, byte> userImage)
        {
            //Stream stream = new MemoryStream();
            //stream.Write(userImage, 0, userImage.Length);
            //var faceImage = new Image<Gray, byte>(new Bitmap(stream));
            _faceRecognizer.Read(_recognizerFilePath);

            var result = _faceRecognizer.Predict(userImage);

            return result.Label;
        }
    }
}
