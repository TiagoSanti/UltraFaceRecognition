using OpenCvSharp;
using UltraFaceDotNet;

namespace UltraFaceRecognition
{
    public class Program
    {
        public static void Main()
        {
            FaceDetector detector = new();

            ClearTemp();
            var watch = System.Diagnostics.Stopwatch.StartNew();
            //DatabaseEncodings(detector);
            watch.Stop();
            Console.WriteLine("Time to run DatabaseEncodings(): " + watch.Elapsed.TotalSeconds.ToString());

            RunRealTimeRecognizer();
        }

        public static void RunRealTimeRecognizer()
        {
            FaceDetector detector = new();
            FaceRecognizer faceRecognizer = new();
            Camera capture = new();
            capture.StartCamera();
            faceRecognizer.RunPython();

            var watch = System.Diagnostics.Stopwatch.StartNew();
            while (Window.WaitKey(10) != 27)
            {
                using Mat? mat = capture.GetFrame();
                if (mat != null)
                {
                    FaceInfo[] faceInfos = detector.DetectFacesMat(mat);
                    if (faceInfos.Length > 0)
                    {
                        IEnumerable<FaceInfo> validFaceInfos =  from faceInfo in faceInfos
                                                                where faceInfo.Score > 0.9
                                                                select faceInfo;
                        List<Mat> croppedMats = Helpers.BitmapsToMats(Helpers.CropImageFromMat(mat, faceInfos));
                        if (watch.Elapsed.TotalSeconds > 5)
                        {
                            Helpers.SaveTempImage(croppedMats);
                            watch.Restart();
                        }
                        //faceRecognizer.RunPython();
                        Drawers.DrawFacesRects(mat, faceInfos);
                    }

                    capture.ShowImage(mat);
                }
            }
            watch.Stop();
            ClearTemp();
            Camera.Close();
        }

        public static void ClearTemp()
        {
            string tempPath = Helpers.GetProjectPath() + "\\temp";
            var tempImages = Directory.GetFiles(tempPath);
            foreach (var image in tempImages)
            {
                if (Directory.Exists(image))
                {
                    Directory.Delete(image);
                }
            }
        }

        public static void DatabaseEncodings(FaceDetector detector)
        {
            detector.EnchanceDatabaseImages();
            Encoder.EncodeDatabase();
        }
    }
}
