using AForge.Imaging.Filters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using YU.Core.Log;

namespace YU.Core.Utils
{
    public static class ImageUtils
    {

        /// <summary>
        /// 获取Url中的Image
        /// </summary>
        /// <param name="imgUrl"></param>
        /// <returns></returns>
        public static Image ImageFromWebTest(string imgUrl, CookieContainer cookie = null)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(imgUrl);
                if (cookie != null)
                    request.CookieContainer = cookie;
                using (WebResponse response = request.GetResponse())
                {
                    Image img = Image.FromStream(response.GetResponseStream());
                    return img;
                }
            }
            catch (Exception ex)
            {
                Logger.Error(string.Format("远程获取图片失败，失败Url：{0}", imgUrl), ex);
                return null;
            }
        }

        public static Image GetImage(string path)
        {
            FileStream fs = new FileStream(path, System.IO.FileMode.Open);
            Image result = Image.FromStream(fs);
            fs.Close();
            return result;
        }


        /// <summary>
        /// Convert Image to Byte[]
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static byte[] ImageToBytes(Image image)
        {
            ImageFormat format = image.RawFormat;
            using (MemoryStream ms = new MemoryStream())
            {
                if (format.Equals(ImageFormat.Jpeg))
                {
                    image.Save(ms, ImageFormat.Jpeg);
                }
                else if (format.Equals(ImageFormat.Png))
                {
                    image.Save(ms, ImageFormat.Png);
                }
                else if (format.Equals(ImageFormat.Bmp))
                {
                    image.Save(ms, ImageFormat.Bmp);
                }
                else if (format.Equals(ImageFormat.Gif))
                {
                    image.Save(ms, ImageFormat.Gif);
                }
                else if (format.Equals(ImageFormat.Icon))
                {
                    image.Save(ms, ImageFormat.Icon);
                }
                else
                {
                    image.Save(ms, ImageFormat.Bmp);
                }
                byte[] buffer = new byte[ms.Length];
                //Image.Save()会改变MemoryStream的Position，需要重新Seek到Begin
                ms.Seek(0, SeekOrigin.Begin);
                ms.Read(buffer, 0, buffer.Length);
                return buffer;
            }
        }

        /// <summary>
        /// Convert Byte[] to Image
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static Image BytesToImage(byte[] buffer)
        {
            MemoryStream ms = new MemoryStream(buffer);
            Image image = System.Drawing.Image.FromStream(ms);
            return image;
        }

        /// <summary>
        /// Convert Byte[] to a picture and Store it in file
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static string CreateImageFromBytes(string fileName, byte[] buffer)
        {
            string file = fileName;
            Image image = BytesToImage(buffer);
            ImageFormat format = image.RawFormat;
            if (format.Equals(ImageFormat.Jpeg))
            {
                file += ".jpeg";
            }
            else if (format.Equals(ImageFormat.Png))
            {
                file += ".png";
            }
            else if (format.Equals(ImageFormat.Bmp))
            {
                file += ".bmp";
            }
            else if (format.Equals(ImageFormat.Gif))
            {
                file += ".gif";
            }
            else if (format.Equals(ImageFormat.Icon))
            {
                file += ".icon";
            }
            System.IO.FileInfo info = new System.IO.FileInfo(file);
            System.IO.Directory.CreateDirectory(info.Directory.FullName);
            File.WriteAllBytes(file, buffer);
            return file;
        }


        public static Bitmap GetOrcImage(Bitmap bmp)
        {
            try
            {
                // from AForge's sample code
                if (bmp.PixelFormat == PixelFormat.Format16bppGrayScale || Bitmap.GetPixelFormatSize(bmp.PixelFormat) > 32)
                    throw new NotSupportedException("Unsupported image format");

                var img = AForge.Imaging.Image.Clone(bmp, PixelFormat.Format24bppRgb);

                img = new Grayscale(0.2125, 0.7154, 0.0721).Apply(img);
                img = new Threshold(50).Apply(img);

                ConservativeSmoothing filter = new ConservativeSmoothing();
                // apply the filter
                filter.ApplyInPlace(img);

                //BilateralSmoothing filter = new BilateralSmoothing();
                //filter.KernelSize = 3;
                //filter.SpatialFactor = 10;
                //filter.ColorFactor = 60;
                //filter.ColorPower = 0.5;
                //// apply the filter
                //img = filter.Apply(img);

                //img = new BlobsFiltering(1, 1, img.Width, img.Height).Apply(img);

                //// create filter
                //Median filter = new Median();
                //// apply the filter
                //filter.ApplyInPlace(image)

                return img;
            }
            finally
            {
                bmp.Dispose();
            }
        }

    }
}
