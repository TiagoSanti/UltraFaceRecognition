using OpenCvSharp;
using System.Drawing;
using UltraFaceDotNet;

namespace UltraFaceRecognition
{
    public class Program
    {
        public static int Main()
        {
            Camera? capture = new();
            List<Person> people = new();

            //Console.WriteLine("Starting camera");
            capture.StartCamera();

            //Console.WriteLine("Encode/Load menu");
            StartEncodings();

            //Console.WriteLine("Running recognition");
            //RunRealTimeRecognizer(capture);
            
            return 0;
        }

        private static void RunRealTimeRecognizer(Camera capture)
        {
            FaceDetector detector = new();

            while (Window.WaitKey(10) != 27)
            {
                //Console.WriteLine("Getting frame");
                Mat? mat = capture.GetFrame();
                if (mat != null)
                {
                    //Console.WriteLine("Detecting faces");
                    FaceInfo[] faceInfos = detector.DetectFacesMat(mat);

                    //Console.WriteLine("Drawing rects");
                    Drawers.DrawFacesRects(mat, faceInfos);

                    //Console.WriteLine("Showing result image");
                    capture.ShowImage(mat);
                }
            }

            capture.Close();
        }

        public static void StartEncodings()
        {
            FaceDetector detector = new();
            // enchance database images
            detector.EnchanceDatabaseImages();

            // encode database enchanced images
            Encoder.EncodeDatabaseImages();

            // load encodings
        }

        private static void EnchanceDatabaseImages()
        {
            FaceDetector detector = new();

            string imageDatabaseDir = @".\database\images\";

            string[] peopleImageDBDir = Directory.GetDirectories(imageDatabaseDir);
            foreach (string personImageDBDir in peopleImageDBDir)
            {
                string[] personImagesPath = Directory.GetFiles(personImageDBDir);
                foreach (string personImagePath in personImagesPath)
                {
                    if (!personImagePath.Contains("_cropped"))
                    {
                        FaceInfo[] faceInfos = detector.DetectFacesImagePath(personImagePath);
                        if (faceInfos != null)
                        {
                            FaceInfo faceInfo = faceInfos[0];
                            Bitmap bitmap = Helpers.CropImage(personImagePath, faceInfo);
                            Helpers.OverwriteImage(bitmap, personImagePath);
                        }
                    }
                }
            }
        }
    }
}
