using Emgu.CV;
namespace Facial.Recognize.Core.Utils
{
    using Emgu.CV.CvEnum;
    using Emgu.CV.Structure;
    using Emgu.CV.Util;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;

    public class ImageHelper
    {
        public static Mat ConvertByteToMat(byte[] bytes)
        {
            var mat = new Mat();

            CvInvoke.Imdecode(bytes, ImreadModes.Unchanged, mat);

            return mat;
        }

        public static byte[] ConvertMatToByte(Mat mat)
        {
            var bytes = new byte[mat.Rows * mat.Cols];
            mat.GetData().CopyTo(bytes, 0);
            return bytes;
        }

        public static byte[] Bitmap2Byte(Bitmap bitmap)
        {
            using (Stream stream1 = new MemoryStream())
            {
                bitmap.Save(stream1, ImageFormat.Jpeg);
                byte[] arr = new byte[stream1.Length];
                stream1.Position = 0;
                stream1.Read(arr, 0, (int)stream1.Length);
                stream1.Close();
                return arr;
            }
        }

        public static Bitmap Byte2Bitmap(byte[] bytes)
        {
            byte[] bytelist = bytes;
            Bitmap bitmap;
            using (MemoryStream ms1 = new MemoryStream(bytelist))
            {
                bitmap = (Bitmap)Image.FromStream(ms1);
                ms1.Close();
            }
            return bitmap;
        }

        public static Mat[] ConvertImageToMat<TColor, TDepth>(Image<TColor, TDepth>[] src)
            where TColor : struct, IColor where TDepth : new()
        {
            var result = new Mat[src.Length];

            for (var i = 0; i < src.Length; i++)
            {
                result[i] = src[i].Mat;
            }

            return result;
        }

        public static byte[,,] CreateMultiDimensionalBytes(byte[] bytes, int width, int height, int numberOfChannels)
        {
            int oneChannelLength = bytes.Length / numberOfChannels;
            byte[,,] multiDimensionalBytes = new byte[height, width, numberOfChannels];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    for (int k = 0; k < numberOfChannels; k++)
                    {
                        multiDimensionalBytes[i, j, k] = bytes[i * height + j * numberOfChannels + k];
                    }
                }
            }

            return multiDimensionalBytes;
        }

        public static string GetMimeTypeFromImage(Bitmap image)
        {
            var format = image.RawFormat;
            var codec = ImageCodecInfo.GetImageDecoders().FirstOrDefault(c => c.FormatID == format.Guid);

            if (codec != null)
            {
                return codec.MimeType;
            }
            else
            {
                return "image/png";
            }
        }

        public static string GetExtensionFromImage(Bitmap image)
        {
            var format = image.RawFormat;
            var codec = ImageCodecInfo.GetImageDecoders().FirstOrDefault(c => c.FormatID == format.Guid);

            if (codec != null)
            {
                return codec.FilenameExtension;
            }
            else
            {
                return ".png";
            }
        }

        public static Bitmap Base64ToImage(string base64String)
        {
            // Convert Base64 String to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            var ms = new MemoryStream(imageBytes, 0, imageBytes.Length);

            // Convert byte[] to Image
            ms.Write(imageBytes, 0, imageBytes.Length);
            var image = new Bitmap(Image.FromStream(ms, true));

            return image;
        }

        public static string ImageToBase64(Bitmap image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, ImageFormat.Png);

                byte[] imageBytes = ms.ToArray();
                string base64String = Convert.ToBase64String(imageBytes);

                return base64String;
            }
        }
    }
}
