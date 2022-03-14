using System;
using System.Diagnostics;

namespace UltraFaceRecognition
{
    class PythonScript
    {
        public string LaptopPythonInterpreter = @"C:\Users\TiagoSanti\AppData\Local\Programs\Python\Python39\python.exe";
        public string DesktopPythonInterpreter = @"C:\Users\tiago\AppData\Local\Programs\Python\Python39\python.exe";
        public string[] Args;
        public Process Process;

        public PythonScript(string[] args)
        {
            Args = args;
            Process = CreateProcess();
        }

        public string PutArgs()
        {
            string args = "";

            foreach (string arg in Args)
            {
                args += arg + " ";
            }

            return args;
        }

        public Process CreateProcess()
        {
            Process process = new()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = LaptopPythonInterpreter,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    LoadUserProfile = true,
                    Arguments = PutArgs()
                },
            };

            return process;
        }

        public void StartProcess()
        {
            Process.ErrorDataReceived += new DataReceivedEventHandler((sender, e) => { Console.WriteLine(e.Data); });
            Process.OutputDataReceived += new DataReceivedEventHandler((sender, e) => { Console.WriteLine(e.Data); });
            Process.Start();

            Process.BeginErrorReadLine();
            Process.BeginOutputReadLine();
            Process.WaitForExit();
        }
    }
}
