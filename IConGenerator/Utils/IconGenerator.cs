using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace IConGenerator.Utils
{
    public class IconGenerator
    {
        public void CreateIcon( string path, int width, int height, string extenstion)
        {
            Image imgOriginal = Image.FromFile(path);
//            Bitmap newImage = new Bitmap(width, height);
//            using (Graphics gr = Graphics.FromImage(newImage))
//            {
//                gr.SmoothingMode = SmoothingMode.HighQuality;
//                gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
//                gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
//
//                gr.DrawImage(imgOriginal, new Rectangle(0, 0, width, height));
//                
//            }
            var newImage = ResizeImage(imgOriginal, width, height);
           
            imgOriginal.Dispose();
            // Get an ImageCodecInfo object that represents the icon codec.
            ImageCodecInfo imageCodecInfo = GetEncoderInfo(ImageFormat.Png);

            // Create an Encoder object for the Quality parameter.
            Encoder encoder = Encoder.Quality;

            // Create an EncoderParameters object. 
            EncoderParameters encoderParameters = new EncoderParameters(1);

            // Save the image as a png file with quality level.
            EncoderParameter encoderParameter = new EncoderParameter(encoder, 100);
            encoderParameters.Param[0] = encoderParameter;
            var fileName = Path.GetFileName(path);
            var filename = Path.ChangeExtension(fileName, extenstion);
            var dir = Path.GetDirectoryName(path);
            var newPath = dir +"\\"+ width.ToString()+"_" + filename;

            newImage.Save(newPath, imageCodecInfo, encoderParameters);
            newImage.Dispose();
         
        }
        private static ImageCodecInfo GetEncoderInfo(ImageFormat format)
        {
            return ImageCodecInfo.GetImageDecoders().SingleOrDefault(c => c.FormatID == format.Guid);
        }

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }


    }
}