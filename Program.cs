using OpenCvSharp;
using UltraFaceDotNet;

namespace UltraFaceRecognition
{
    public class Program
    {
        public static void Main()
        {
            FaceDetector detector = new();


            var watch = System.Diagnostics.Stopwatch.StartNew();
            DatabaseEncodings(detector);
            watch.Stop();
            Console.WriteLine("Time to run DatabaseEncodings(): " + watch.Elapsed.TotalSeconds.ToString());

            RunRealTimeRecognizer();
        }

        public static void RunRealTimeRecognizer()
        {
            FaceDetector detector = new();
            Camera capture = new();
            capture.StartCamera();

            while (Window.WaitKey(10) != 27)
            {
                using Mat? mat = capture.GetFrame();
                if (mat != null)
                {
                    FaceInfo[] faceInfos = detector.DetectFacesMat(mat);
                    Mat croppedMat = Helpers.BitmapToMat(Helpers.CropImageFromMat(mat, faceInfos[0]));
                    Helpers.SaveTempImage(croppedMat);
                    FaceRecognizer.CallPythonAsync();
                    Drawers.DrawFacesRects(mat, faceInfos);

                    capture.ShowImage(mat);
                }
            }

            Camera.Close();
        }

        public static void DatabaseEncodings(FaceDetector detector)
        {
            detector.EnchanceDatabaseImages();
            Encoder.EncodeDatabase();
        }
    }
}
