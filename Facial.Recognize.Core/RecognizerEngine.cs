namespace Facial.Recognize.Core
{
    using Emgu.CV;
    using Emgu.CV.CvEnum;
    using Emgu.CV.Face;
    using Emgu.CV.Structure;
    using Facial.Recognize.Core.Data;
    using Facial.Recognize.Core.Dto;
    using Facial.Recognize.Core.Utils;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Threading.Tasks;
    using static Emgu.CV.Face.FaceRecognizer;

    public class RecognizerEngine : IRecognizerEngine
    {
        #region Variables
        private readonly EigenFaceRecognizer _eigenFaceRecognizer;
        private readonly CascadeClassifier _faceDetection;
        private readonly IServiceProvider _serviceProvider;
        private List<Image<Gray, byte>> _faces = new List<Image<Gray, byte>>();
        private List<int> _labels = new List<int>();
        #endregion

        #region Constants
        private const int PROCESS_IMAGE_WIDTH = 320;
        private const int PROCESS_IMAGE_HEIGHT = 240;
        private string YML_PATH = $"{AppDomain.CurrentDomain.BaseDirectory}Faces\\TrainedFaces.yml";
        #endregion

        public RecognizerEngine(IServiceProvider serviceProvider, EigenFaceRecognizer eigenFaceRecognizer,
            CascadeClassifier cascadeClassifier)
        {
            _eigenFaceRecognizer = eigenFaceRecognizer;
            _serviceProvider = serviceProvider;
            _faceDetection = cascadeClassifier;
            Task.Run(async () => await TrainRecognizer());
        }

        public async Task TrainRecognizer()
        {
            if (File.Exists(YML_PATH))
            {
                _eigenFaceRecognizer.Read(YML_PATH);
            }

            using (var scope = _serviceProvider.CreateScope())
            {
                var dataStoreAccess = scope.ServiceProvider.GetService<IDataStoreAccess>();

                var allFaces = await dataStoreAccess.CallFacesAsync("ALL_USERS");

                foreach (var face in allFaces)
                {
                    var mat = new Mat();
                    CvInvoke.Imdecode(face.Image, ImreadModes.Unchanged, mat);
                    var faceImage = mat.ToImage<Gray, byte>();
                    _faces.Add(faceImage);
                    _labels.Add(face.UserId);
                }

                if (allFaces.Count > 0)
                    _eigenFaceRecognizer.Train(ImageHelper.ConvertImageToMat(_faces.ToArray()), _labels.ToArray());
            }
        }

        public PredictionResult Recognize(Image<Gray, byte> image)
        {
            var faces = _faceDetection.DetectMultiScale(image, 1.3, 5);

            foreach (var face in faces)
            {
                var processImage = image.Copy(face).Resize(PROCESS_IMAGE_WIDTH, PROCESS_IMAGE_HEIGHT, Inter.Cubic);

                var result = _eigenFaceRecognizer.Predict(processImage);

                return result;
            }

            return default;
        }

        public async Task Train(Image<Gray, byte> image, string username, string userId)
        {
            var faces = _faceDetection.DetectMultiScale(image, 1.3, 5);

            if (faces.Length > 0)
            {
                var processImage = image.Copy(faces[0]).Resize(PROCESS_IMAGE_WIDTH, PROCESS_IMAGE_HEIGHT, Inter.Cubic);

                _faces.Add(processImage);
                _labels.Add(Convert.ToInt32(userId));

                using (var scope = _serviceProvider.CreateScope())
                {
                    var dataStoreAccess = scope.ServiceProvider.GetService<IDataStoreAccess>();

                    await dataStoreAccess.SaveFaceAsync(new FaceDto
                    {
                        Image = processImage.ToJpegData(),
                        Label = username,
                        UserId = int.Parse(userId)
                    });
                }

                _eigenFaceRecognizer.Train(ImageHelper.ConvertImageToMat(_faces.ToArray()), _labels.ToArray());

                if (File.Exists(YML_PATH)) File.Delete(YML_PATH);

                _eigenFaceRecognizer.Write(YML_PATH);
            }
        }

        public async Task DetectFace(Image<Bgr, byte> imgSrc, bool useRecognize = false)
        {
            var frame = imgSrc.Convert<Gray, byte>();

            var faces = _faceDetection.DetectMultiScale(frame, 1.3, 5);

            foreach (var face in faces)
            {
                if (useRecognize)
                {
                    try
                    {
                        var processImage = frame.Copy(face).Resize(PROCESS_IMAGE_WIDTH, PROCESS_IMAGE_HEIGHT, Inter.Cubic);

                        var resultPredict = _eigenFaceRecognizer.Predict(processImage);

                        if (resultPredict.Label > -1)
                        {
                            using (var scope = _serviceProvider.CreateScope())
                            {
                                var dataStoreAccess = scope.ServiceProvider.GetService<IDataStoreAccess>();

                                var user = await dataStoreAccess.GetByIdAsync(resultPredict.Label);

                                CvInvoke.PutText(imgSrc, user.Label, new Point(face.X - 2, face.Y - 2), FontFace.HersheyScriptSimplex, 1, new MCvScalar(0, 0, 255), 2);
                            }
                        }
                    }
                    catch (Exception ex) { }
                    finally
                    {
                        CvInvoke.Rectangle(imgSrc, face, new MCvScalar(255, 0, 0), 2);
                    }
                }
            }
        }
    }
}