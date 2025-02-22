﻿using OpenCvSharp;
using OpenCvSharp.Extensions;
using System.Drawing;
using Img = System.Drawing.Image;
using UltraFaceDotNet;

namespace UltraFaceRecognition
{
    public class Helpers
    {
        #region Converters
        public static Bitmap MatToBitmap(Mat mat)
        {
            try
            {
                return BitmapConverter.ToBitmap(mat);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public static List<Bitmap> MatsToBitmaps(List<Mat> mats)
        {
            List<Bitmap> bitmaps = new();
            foreach (Mat mat in mats)
            {
                bitmaps.Add(BitmapConverter.ToBitmap(mat));
            }
            return bitmaps;
        }

        public static List<Mat> BitmapsToMats(List<Bitmap> bitmaps)
        {
            List<Mat> mats = new();
            foreach (Bitmap bitmap in bitmaps)
            {
                mats.Add(BitmapConverter.ToMat(bitmap));
            }
            return mats;
        }

        #endregion

        #region Image Manager
        public static Bitmap CropImageFromPath(string imageFilePath, FaceInfo faceInfo)
        {
            using Bitmap bitmap = Img.FromFile(imageFilePath) as Bitmap;

            int X1 = (int)faceInfo.X1;
            int Y1 = (int)faceInfo.Y1;
            int width = (int)(faceInfo.X2 - faceInfo.X1);
            int height = (int)(faceInfo.Y2 - faceInfo.Y1);

            Rectangle faceRectangle = new(X1, Y1, width, height);

            Bitmap cropped = new(faceRectangle.Width, faceRectangle.Height);
            using Graphics g = Graphics.FromImage(cropped);
            g.DrawImage(bitmap, -faceRectangle.X, -faceRectangle.Y);

            return cropped;
        }

        public static List<Bitmap> CropImageFromMat(Mat mat, FaceInfo[] faceInfos)
        {
            using Bitmap bitmap = MatToBitmap(mat);
            List<Bitmap> croppeds = new();

            foreach (FaceInfo faceInfo in faceInfos)
            {
                int X1 = (int)faceInfo.X1;
                int Y1 = (int)faceInfo.Y1;
                int width = (int)(faceInfo.X2 - faceInfo.X1);
                int height = (int)(faceInfo.Y2 - faceInfo.Y1);

                Rectangle faceRectangle = new(X1, Y1, width, height);

                Bitmap cropped = new(faceRectangle.Width, faceRectangle.Height);
                using Graphics g = Graphics.FromImage(cropped);
                g.DrawImage(bitmap, -faceRectangle.X, -faceRectangle.Y);

                croppeds.Add(cropped);
            }

            return croppeds;
        }

        internal static void OverwriteImage(Bitmap bitmap, string personImagePath)
        {
            File.Delete(personImagePath);
            bitmap.Save(personImagePath+"_cropped.png", System.Drawing.Imaging.ImageFormat.Png);
        }

        public static void SaveTempImage(List<Mat> mats)
        {
            string tempPath = GetProjectPath() + "\\temp\\";
            if (!Directory.Exists(tempPath))
            {
                Directory.CreateDirectory(tempPath);
            }

            int i = 0;
            foreach (Mat mat in mats)
            {
                var fileName = "person_"+i;
                Cv2.ImWrite(tempPath + fileName + ".png", mat);
                i++;
            }
        }
        #endregion

        #region Path Manager
        public static string GetProjectPath()
        {
            string current = Directory.GetCurrentDirectory();   // \net6.0
            current = Directory.GetParent(current).FullName;    // \Debug
            current = Directory.GetParent(current).FullName;    // \x64
            current = Directory.GetParent(current).FullName;    // \bin
            current = Directory.GetParent(current).FullName;    // \UltraFaceRecognition
            return current;
        }

        public static bool ThereIsTemp()
        {
            string tempPath = GetProjectPath() + "\\temp";
            string[] tempImages = Directory.GetFiles(tempPath);

            return tempImages.Length > 0;
        }

        public static void ClearTemp()
        {
            if (ThereIsTemp())
            {
                string[] files = Directory.GetFiles(GetProjectPath() + "\\temp");
                foreach (string file in files)
                {
                    File.Delete(file);
                }
            }
        }
        #endregion
    }
}