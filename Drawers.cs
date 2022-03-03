using FaceRecognitionDotNet;
using OpenCvSharp;
using UltraFaceDotNet;

namespace UltraFaceRecognition
{
    class Drawers
    {
        public static void DrawFacesRects(Mat mat, FaceInfo[] faceInfos)
        {
            foreach (FaceInfo faceInfo in faceInfos)
            {
                OpenCvSharp.Point pt1 = new(faceInfo.X1, faceInfo.Y1);
                OpenCvSharp.Point pt2 = new(faceInfo.X2, faceInfo.Y2);

                DrawRect(mat, pt1, pt2);
            }
        }
        public static void DrawRect(Mat mat, OpenCvSharp.Point pt1, OpenCvSharp.Point pt2)
        {
            Cv2.Rectangle(mat, pt1, pt2, Scalar.Red,2);
        }

        public static void DrawName(Mat mat, Person person, Location faceLocation)
        {
            Cv2.Rectangle(mat,
                new OpenCvSharp.Point(faceLocation.Left, faceLocation.Bottom),
                new OpenCvSharp.Point(faceLocation.Right, faceLocation.Bottom + 20),
                Scalar.Red,
                -1);
            mat.PutText(person.Name, new OpenCvSharp.Point(faceLocation.Left + 3, faceLocation.Bottom + 15), fontFace: HersheyFonts.HersheyDuplex, fontScale: 0.5, color: Scalar.White);
        }
    }
}
