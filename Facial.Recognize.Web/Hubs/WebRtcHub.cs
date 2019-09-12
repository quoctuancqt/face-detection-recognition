using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Facial.Recognize.Core;
using Facial.Recognize.Core.Data;
using Facial.Recognize.Web.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Facial.Recognize.Web.Hubs
{
    public class WebRtcHub : Hub
    {
        private readonly IRecognizerEngine _recognizerEngine;
        private readonly TrainningFaceContext _trainningFaceContext;
        private readonly IConfiguration _configuration;

        public WebRtcHub(IRecognizerEngine recognizerEngine, TrainningFaceContext trainningFaceContext, IConfiguration configuration)
        {
            _recognizerEngine = recognizerEngine;
            _trainningFaceContext = trainningFaceContext;
            _configuration = configuration;
        }

        public async Task StartConection(object data)
        {
            await Clients.All.SendCoreAsync("MessagerReceived", new[] { new { Offset = 0 } });
        }

        public async Task SendStream(StreamFaceData streamVideo)
        {
            var block = streamVideo.Buffer.Split(";");

            var contentType = block[0].Split(":")[1];

            var realData = block[1].Split(",")[1];

            var bufferData = Convert.FromBase64String(realData);

            var mat = new Mat();

            CvInvoke.Imdecode(bufferData, ImreadModes.Unchanged, mat);

            var image = mat.ToImage<Bgr, byte>();

            await _recognizerEngine.DetectFace(image, true);

            var outputData = image.ToJpegData();

            var output = $"data:{contentType};base64,{Convert.ToBase64String(outputData)}";

            streamVideo.Buffer = output;

            await Clients.All.SendCoreAsync("MessagerReceived", new[] { new { streamVideo } });
        }

        public async Task TrainFace(StreamTrainData streamVideo)
        {
            if (await CanTrainAsync(streamVideo.UserId))
            {
                var block = streamVideo.Buffer.Split(";");

                var contentType = block[0].Split(":")[1];

                var realData = block[1].Split(",")[1];

                var bufferData = Convert.FromBase64String(realData);

                var mat = new Mat();

                CvInvoke.Imdecode(bufferData, ImreadModes.Unchanged, mat);

                var image = mat.ToImage<Gray, byte>();

                await _recognizerEngine.Train(image, streamVideo.Username, streamVideo.UserId);
            }
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        private async Task<bool> CanTrainAsync(string userId)
        {
            var user = await _trainningFaceContext.Faces
                                .Where(x => x.UserId.Equals(userId) && x.CreatedAt.Date == DateTime.Now.Date)
                                .ToListAsync();

            return user.Count < _configuration.GetValue<int>("LimitTimesTrainFace");
        }
    }
}
