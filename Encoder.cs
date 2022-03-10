using FaceRecognitionDotNet;
using System.Diagnostics;
using System.Globalization;

namespace UltraFaceRecognition
{
    public class Encoder
    {
        public static void Test()
        {
            Dictionary<string, string> paths = new();
            paths.Add("laptopPythonInterpreter", @"C:\Users\Tiago Santi\AppData\Local\Programs\Python\Python39\python.exe");
            paths.Add("laptopScript", "\"C:\\Users\\Tiago Santi\\Documents\\GitHub\\UltraFaceRecognition\\scripts\\encoder.py\"");
            paths.Add("laptopImageTest", @"C:\code\UltraFaceRecognition\scripts\3.jpg_cropped.png");
            paths.Add("laptopImageTest2", "3.jpg_cropped.png");
            paths.Add("desktopPythonInterpreter", @"C:\Users\tiago\AppData\Local\Programs\Python\Python39\python.exe");
            paths.Add("desktopScript", "\"D:\\Documentos\\PROG\\Github\\TiagoSanti\\UltraFaceRecognition\\scripts\\encoder.py\"");
            paths.Add("desktopImageTest", @"D:\Documentos\PROG\Github\TiagoSanti\UltraFaceRecognition\scripts\2.jpg_cropped.png");

            using (var process = new Process())
            {
                process.StartInfo.FileName = paths["desktopPythonInterpreter"];
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.LoadUserProfile = true;
                process.StartInfo.Arguments = string.Format("{0} {1}", paths["desktopScript"], paths["desktopImageTest"]);
                string? error = null;
                process.ErrorDataReceived += new DataReceivedEventHandler((sender, e) => { error += e.Data; });
                process.Start();

                process.BeginErrorReadLine();
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                Console.WriteLine(error);
                Console.WriteLine(output);
                Console.WriteLine("---------------------------------------------");
                string[] outputSplit = output.Split(',');
                outputSplit = outputSplit.Take(outputSplit.Length - 1).ToArray();
                foreach (string unit in outputSplit)
                {
                    Console.WriteLine(unit);
                }
                
                double[] doubleOutput = new double[outputSplit.Length];
                for (int i = 0; i < doubleOutput.Length; i++)
                {
                    doubleOutput[i] = double.Parse(outputSplit[i], CultureInfo.InvariantCulture);
                }

                Console.WriteLine("-------------- double array ------------------");
                foreach (double unit in doubleOutput)
                {
                    Console.WriteLine(unit);
                }
            }
        }

        static void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Console.WriteLine(e.Data);
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
