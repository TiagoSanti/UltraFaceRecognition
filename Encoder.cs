using FaceRecognitionDotNet;
using System.Diagnostics;
using System.Globalization;

namespace UltraFaceRecognition
{
    public class Encoder
    {
        public static void EncodeDatabase()
        {
            string laptopPythonInterpreter = @"C:\Users\Tiago Santi\AppData\Local\Programs\Python\Python39\python.exe"
            string desktopPythonInterpreter = @"C:\Users\tiago\AppData\Local\Programs\Python\Python39\python.exe"

            using var process = new Process();
            process.StartInfo.FileName = laptopPythonInterpreter;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.LoadUserProfile = true;
            // TODO: adicionar caminho genérico para script
            // process.StartInfo.Arguments = string.Format("{0} {1}", PYTHONSCRIPTPATH);
            string? error = null;
            process.ErrorDataReceived += new DataReceivedEventHandler((sender, e) => { error += e.Data; });
            process.Start();

            process.BeginErrorReadLine();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
        }
    }
}
