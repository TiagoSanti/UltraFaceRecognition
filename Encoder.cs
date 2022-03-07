using FaceRecognitionDotNet;
using Microsoft.AI.MachineLearning;
using System.Drawing;
using Img = System.Drawing.Image;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

namespace UltraFaceRecognition
{
    public class Encoder
    {
        public ScriptEngine engine;
        public ScriptScope scope;
        public ScriptSource source;

        public Encoder()
        {
            engine = Python.CreateEngine();
            scope = engine.CreateScope();
            source = engine.CreateScriptSourceFromFile(@"D:\Documentos\PROG\Github\TiagoSanti\UltraFaceRecognition\scripts\encoder.py");
        }

        public void Execute()
        {
            source.Execute(scope);
        }

        public static void EncodeDatabaseImages()
        {
            string imageDatabaseDir = @".\database\images\";
            string encodingDatabase = @".\database\encodings\";

            if (!Directory.Exists(encodingDatabase))
            {
                Directory.CreateDirectory(encodingDatabase);
            }

            string[] peopleImagesDir = Directory.GetDirectories(imageDatabaseDir);
            if (peopleImagesDir.Length > 0)
            {
                foreach (string personImagesDir in peopleImagesDir)
                {
                    string personName = personImagesDir.Split(Path.DirectorySeparatorChar).Last();
                    string personEncodingsDir = encodingDatabase + personName;
                    string[] personEncodingsFilesFullPath = Array.Empty<string>();
                    List<string> personEncodingsFiles = new();

                    if (Directory.Exists(personEncodingsDir))
                    {
                        personEncodingsFilesFullPath = Directory.GetFiles(personEncodingsDir);
                    }
                    else
                    {
                        Directory.CreateDirectory(personEncodingsDir);
                    }

                    if (personEncodingsFilesFullPath.Length > 0)
                    {
                        foreach (string personEncodingFFP in personEncodingsFilesFullPath)
                        {
                            personEncodingsFiles.Add(Path.GetFileName(personEncodingFFP));
                        }
                    }

                    string[] personImages = Directory.GetFiles(personImagesDir);
                    if (personImages.Length > 0)
                    {
                        foreach (string personImage in personImages)
                        {
                            string imageFile = personImage.Split(Path.DirectorySeparatorChar).Last();
                            if (!CheckIfImageEncodingExists(personEncodingsFiles, personImage))
                            {
                                /*
                                var faceEncoding = EncodeImage(personImage);
                                if (faceEncoding != null)
                                {
                                    AddEncodingFileToDatabase(faceEncoding, imageFile, personName);
                                }
                                */
                            }
                        }
                    }
                }
            }
        }

        private static void AddEncodingFileToDatabase(FaceEncoding faceEncoding, string imageFile, string personName)
        {
            if (faceEncoding != null)
            {
                string imageFileWithoutExtension = Path.GetFileNameWithoutExtension(imageFile);
                string personEncodingDir = @".\database\encodings\" + personName;

                if (!Directory.Exists(personEncodingDir))
                {
                    Directory.CreateDirectory(personEncodingDir);
                }

                Helpers.SerializeEncoding(personEncodingDir + @"\" + imageFileWithoutExtension + ".encoding", faceEncoding);
            }
        }

        private static bool CheckIfImageEncodingExists(List<string> personEncodingsFiles, string personImage)
        {
            return personEncodingsFiles.Contains(Path.GetFileNameWithoutExtension(personImage) + ".encoding");
        }

        private static void EncodeImage(string personImage)
        {
            
        }
    }
}
