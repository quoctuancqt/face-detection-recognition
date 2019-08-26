using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Face;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
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

        public RecognizerEngine() { }

        public bool TrainRecognizer(ref List<Mat> faceImages, ref List<int> faceLabels)
        {
            var allFaces = _dataStoreAccess.CallFaces("ALL_USERS");
            if (allFaces != null)
            {
                //faceImages = new Mat[allFaces.Count];
                //faceLabels = new int[allFaces.Count];
                for (int i = 0; i < allFaces.Count; i++)
                {
                    Stream stream = new MemoryStream();
                    stream.Write(allFaces[i].Image, 0, allFaces[i].Image.Length);
                    var faceImage = new Image<Gray, byte>(new Bitmap(stream));
                    faceImages.Add(faceImage.Resize(100, 100, Inter.Cubic).Mat);
                    faceLabels.Add(allFaces[i].UserId);
                }
                _faceRecognizer.Train(faceImages.ToArray(), faceLabels.ToArray());
                //_faceRecognizerFisher.Train(faceImages.ToArray(), faceLabels.ToArray());
                //_faceRecognizerLBPH.Train(faceImages.ToArray(), faceLabels.ToArray());
                _faceRecognizer.Write(_recognizerFilePath);
            }
            return true;

        }

        public void LoadRecognizerData()
        {
            _faceRecognizer.Read(_recognizerFilePath);
        }

        public int RecognizeUser(Image<Gray, byte> userImage, List<Mat> faceImages)
        {
            _faceRecognizer.Read(_recognizerFilePath);
            _faceRecognizerFisher.Read(_recognizerFilePath);
            _faceRecognizerLBPH.Read(_recognizerFilePath);

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

            return result.Label;
        }

        public List<Mat> Recognize(Image<Gray, byte> userImage, List<Mat> faceImages)
        {
            var result = new List<Mat>();
            long matchTime;

            foreach (var face in faceImages)
            {
                result.Add(DrawMatches.Draw(userImage.Mat, face, out matchTime));

            }

            return result;
        }
    }
}
