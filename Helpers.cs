using FaceRecognitionDotNet;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System.Drawing;
using Img = System.Drawing.Image;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UltraFaceDotNet;

namespace UltraFaceRecognition
{
    class Helpers
    {
        #region Converters

        public static Bitmap MatToBitmap(Mat mat)
        {
            return BitmapConverter.ToBitmap(mat);
        }

        public static Mat BitmapToMat(Bitmap bitmap)
        {
            return BitmapConverter.ToMat(bitmap);
        }

        #endregion

        #region Serizalization

        public static void SerializeEncoding(string fileName, FaceEncoding encoding)
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(fileName, FileMode.Create);

            formatter.Serialize(stream, encoding);
            stream.Close();
        }

        public static FaceEncoding? DeserializeEncoding(string fileName)
        {
            if (File.Exists(fileName))
            {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(fileName, FileMode.Open);
                FaceEncoding encoding = (FaceEncoding)formatter.Deserialize(stream);
                stream.Close();

                return encoding;
            }

            return null;
        }

        internal static Rect FaceRect(FaceInfo faceInfo)
        {
            throw new NotImplementedException();
        }

        #endregion

        public static Bitmap CropImage(string imageFilePath, FaceInfo faceInfo)
        {
            Bitmap bitmap = Img.FromFile(imageFilePath) as Bitmap;

            int X1 = (int)faceInfo.X1;
            int Y1 = (int)faceInfo.Y1;
            int width = (int)(faceInfo.X2 - faceInfo.X1);
            int height = (int)(faceInfo.Y2 - faceInfo.Y1);

            Rectangle faceRectangle = new Rectangle(X1, Y1, width, height);

            Bitmap cropped = new Bitmap(faceRectangle.Width, faceRectangle.Height);
            using Graphics g = Graphics.FromImage(cropped);
            g.DrawImage(bitmap, -faceRectangle.X, -faceRectangle.Y);

            return cropped;
        }

        internal static void OverwriteImage(Bitmap bitmap, string personImagePath)
        {
            string fileName = Path.GetFileNameWithoutExtension(personImagePath);

            File.Delete(personImagePath);
            bitmap.Save(fileName, System.Drawing.Imaging.ImageFormat.Jpeg);
        }
    }
}
