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
            List<Person> people = new();
            Encoder encoder = new();


            //Console.WriteLine("Starting camera");
            //capture.StartCamera();

            //Console.WriteLine("Start python encoder instance");
            Encoder.Test();

            //Console.WriteLine("Encode/Load menu");
            //people = StartEncodings(detector, people);

            //Console.WriteLine("Running recognition");
            //RunRealTimeRecognizer(capture, people);
            
            return 0;
        }

        public static void RunRealTimeRecognizer(Camera capture, List<Person> people)
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

            Camera.Close();
        }

        public static List<Person> StartEncodings(FaceDetector detector, List<Person> people)
        {
            // enchance database images
            detector.EnchanceDatabaseImages();

            // encode database enchanced images
            Encoder.EncodeDatabaseImages();

            // load encodings

            return people;
        }

        public void test()
        {

        }
    }
}
