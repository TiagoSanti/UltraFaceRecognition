using OpenCvSharp;
using UltraFaceDotNet;

namespace UltraFaceRecognition
{
    public class Program
    {
        public static int Main()
        {
            Camera? capture = new();
            FaceDetector detector = new();

            //Console.WriteLine("Starting camera");
            //capture.StartCamera();

            //Console.WriteLine("Encode/Load menu");
            DatabaseEncodings(detector);

            //Console.WriteLine("Running recognition");
            RunRealTimeRecognizer(capture);
            
            return 0;
        }

        public static void RunRealTimeRecognizer(Camera capture)
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

                    //FaceRecognizer.RecognizeFace();

                    //Console.WriteLine("Showing result image");
                    capture.ShowImage(mat);
                }
            }

            Camera.Close();
        }

        public static void DatabaseEncodings(FaceDetector detector)
        {
            // enchance database images
            detector.EnchanceDatabaseImages();

            // encode database enchanced images
            Encoder.EncodeDatabase();
        }
    }
}
