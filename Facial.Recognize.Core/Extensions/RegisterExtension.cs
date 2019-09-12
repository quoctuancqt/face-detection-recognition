namespace Facial.Recognize.Core.Extensions
{
    using Emgu.CV;
    using Emgu.CV.Face;
    using Facial.Recognize.Core.Data;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.IO;

    public static class RegisterExtension
    {
        public static IServiceCollection RegisterRecognize(this IServiceCollection services)
        {
            services.AddSingleton(provider => new EigenFaceRecognizer(4, 800));

            services.AddSingleton(provider => new CascadeClassifier(Path.GetFullPath($"{AppDomain.CurrentDomain.BaseDirectory}opencv/haarcascade_frontalface_default.xml")));

            services.AddSingleton<IRecognizerEngine, RecognizerEngine>();

            services.AddScoped<IDataStoreAccess, DataStoreAccess>();

            return services;
        }
    }
}
