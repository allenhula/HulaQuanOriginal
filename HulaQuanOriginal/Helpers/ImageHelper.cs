using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;

namespace HulaQuanOriginal.Helpers
{
    public class ImageHelper
    {
        public static void ResizeAndSaveImage(Image image, int maxWidth, int maxHeight, string filePath)
        {
            var newImage = ResizeImage(image, maxWidth, maxHeight, false);
            using (newImage)
            {
                SaveImage(newImage, filePath);
            }
        }

        public static Bitmap ResizeImage(Image image, int maxWidth, int maxHeight, bool lockAspectRatio = true)
        {
            var newWidth = maxWidth;
            var newHeight = maxHeight;

            if (lockAspectRatio)
            {
                var originalWidth = image.Width;
                var originalHeight = image.Height;

                var ratioX = (float)(maxWidth / originalWidth);
                var ratioY = (float)(maxHeight / originalHeight);
                var ratio = Math.Min(ratioX, ratioY);

                newWidth = (int)(originalWidth * ratio);
                newHeight = (int)(originalHeight * ratio);
            }

            var newImage = new Bitmap(newWidth, newHeight, PixelFormat.Format24bppRgb);

            using (Graphics graphics = Graphics.FromImage(newImage))
            {
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);
            }

            return newImage;
        }

        /// <summary>
        /// Save image to target location with 100 quality and JPEG format
        /// </summary>
        /// <param name="image">image to save<</param>
        /// <param name="filePath">location to save</param>
        public static void SaveImage(Bitmap image, string filePath)
        {
            SaveImage(image, filePath, ImageFormat.Jpeg, 100);
        }

        /// <summary>
        /// Save image to target location with quality
        /// </summary>
        /// <param name="image">image to save</param>
        /// <param name="filePath">location to save</param>
        /// <param name="imageFormat">Image format to save</param>
        /// <param name="quality">image quality to save, should be in the range [0..100]</param>
        public static void SaveImage(Bitmap image, string filePath, ImageFormat imageFormat, long quality)
        {
            ImageCodecInfo jpgInfo = ImageCodecInfo.GetImageEncoders().SingleOrDefault(codecInfo => codecInfo.FormatID == imageFormat.Guid);
            using (EncoderParameters encParams = new EncoderParameters(1))
            {
                encParams.Param[0] = new EncoderParameter(Encoder.Quality, quality);
                image.Save(filePath, jpgInfo, encParams);
            }
        }
    }
}