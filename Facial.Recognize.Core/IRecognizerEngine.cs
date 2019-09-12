namespace Facial.Recognize.Core
{
    using Emgu.CV;
    using Emgu.CV.Face;
    using Emgu.CV.Structure;
    using System.Threading.Tasks;

    public interface IRecognizerEngine
    {
        Task TrainRecognizer();

        FaceRecognizer.PredictionResult Recognize(Image<Gray, byte> image);

        Task Train(Image<Gray, byte> image, string username, string userId);

        Task DetectFace(Image<Bgr, byte> imgSrc, bool useRecognize = false);
    }
}
