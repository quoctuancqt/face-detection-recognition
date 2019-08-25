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
        private FaceRecognizer _faceRecognizerFisher;
        private FaceRecognizer _faceRecognizerLBPH;
        private DataStoreAccess _dataStoreAccess;
        private string _recognizerFilePath;
        private int _eigenThreshold = 3000;
        public RecognizerEngine(string databasePath, string recognizerFilePath, DataStoreAccess dataStoreAccess)
        {
            _recognizerFilePath = recognizerFilePath;
            _dataStoreAccess = dataStoreAccess;
            _faceRecognizer = new EigenFaceRecognizer(4, 5000);
            _faceRecognizerFisher = new FisherFaceRecognizer(4, 5000);
            _faceRecognizerLBPH = new LBPHFaceRecognizer(4, 8, 8, 8, 5000);
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
                    faceImages[i] = faceImage.Resize(100, 100, Inter.Cubic).Mat;
                    faceLabels[i] = allFaces[i].UserId;
                }
                _faceRecognizer.Train(faceImages, faceLabels);
                _faceRecognizerFisher.Train(faceImages, faceLabels);
                _faceRecognizerLBPH.Train(faceImages, faceLabels);
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
            //_faceRecognizer.Read(_recognizerFilePath);
            //_faceRecognizerFisher.Read(_recognizerFilePath);
            //_faceRecognizerLBPH.Read(_recognizerFilePath);

            var result = _faceRecognizer.Predict(userImage);
            var resultFish = _faceRecognizerFisher.Predict(userImage);
            var LBPresult = _faceRecognizerLBPH.Predict(userImage);

            if (result.Label != -1 && resultFish.Label != -1 && LBPresult.Label != -1)
            {
                if (result.Label == resultFish.Label && resultFish.Label == LBPresult.Label)
                {
                    return result.Label;
                }
            }
            else
            {
                return -1;
            }

            return -1;
        }
    }
}
