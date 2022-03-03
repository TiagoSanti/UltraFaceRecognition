﻿using OpenCvSharp;
using System.Drawing;
using UltraFaceDotNet;

namespace UltraFaceRecognition
{
    class FaceDetector
    {
        public UltraFace? ultraFace;
        public string binPath = @".\ncnn\data\version-RFB\RFB-320.bin";
        public string paramPath = @".\ncnn\data\version-RFB\RFB-320.param";
        public UltraFaceParameter? parameter;

        public FaceDetector()
        {
            parameter = new UltraFaceParameter
            {
                BinFilePath = binPath,
                ParamFilePath = paramPath,
                InputWidth = 320,
                InputLength = 240,
                NumThread = 1,
                ScoreThreshold = 0.7f
            };

            ultraFace = UltraFace.Create(parameter);
        }

        public FaceInfo[] DetectFacesMat(Mat mat)
        {
            using var inMat = NcnnDotNet.Mat.FromPixels(mat.Data, NcnnDotNet.PixelType.Bgr2Rgb, mat.Cols, mat.Rows);
            
            return ultraFace.Detect(inMat).ToArray();
        }

        public FaceInfo[] DetectFacesImagePath(string imagePath)
        {
            using var frame = Cv2.ImRead(imagePath);
            using var inMat = NcnnDotNet.Mat.FromPixels(frame.Data, NcnnDotNet.PixelType.Bgr2Rgb, frame.Cols, frame.Rows);

            return ultraFace.Detect(inMat).ToArray();
        }

        internal void EnchanceDatabaseImages()
        {
            string imageDatabaseDir = @".\database\images\";

            string[] peopleImageDBDir = Directory.GetDirectories(imageDatabaseDir);
            foreach (string personImageDBDir in peopleImageDBDir)
            {
                string[] personImagesPath = Directory.GetFiles(personImageDBDir);
                foreach (string personImagePath in personImagesPath)
                {
                    if (!personImagePath.Contains("_cropped"))
                    {
                        FaceInfo[] faceInfos = DetectFacesImagePath(personImagePath);
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
