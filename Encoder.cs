using FaceRecognitionDotNet;
using System.Diagnostics;
using System.Globalization;

namespace UltraFaceRecognition
{
    public class Encoder
    {
        public static void EncodeDatabase()
        {
            string laptopPythonInterpreter = @"C:\Users\Tiago Santi\AppData\Local\Programs\Python\Python39\python.exe";
            string desktopPythonInterpreter = @"C:\Users\tiago\AppData\Local\Programs\Python\Python39\python.exe";
            string projectPath = GetProjectPath();
            string scriptPath = projectPath + "\\scripts\\DatabaseEncoder.py";
            string databasePath = projectPath + "\\database";

            using var process = new Process();
            process.StartInfo.FileName = desktopPythonInterpreter;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.LoadUserProfile = true;
            process.StartInfo.Arguments = string.Format("{0} {1}", scriptPath, databasePath);
            process.ErrorDataReceived += new DataReceivedEventHandler((sender, e) => { Console.WriteLine(e.Data); });
            process.OutputDataReceived += new DataReceivedEventHandler((sender, e) => { Console.WriteLine(e.Data); });
            process.Start();

            process.BeginErrorReadLine();
            process.BeginOutputReadLine();
            process.WaitForExit();
        }

        public static string GetProjectPath()
        {
            string current = Directory.GetCurrentDirectory();   // net6.0
            current = Directory.GetParent(current).FullName;    // Debug
            current = Directory.GetParent(current).FullName;    // x64
            current = Directory.GetParent(current).FullName;    // bin
            current = Directory.GetParent(current).FullName;    // UltraFaceRecognition
            return current;
        }
    }
}
