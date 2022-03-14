namespace UltraFaceRecognition
{
    public class FaceRecognizer
    {
        private readonly PythonScript PyScript;
        public bool IsStarted = false;
        public string[] Args;
        public Task PyTask;

        public FaceRecognizer()
        {
            IsStarted = false;
            Args = CreatePythonScript();
            PyScript = new PythonScript(Args);
            PyTask = new(PyScript.StartProcess);
        }

        private static string[] CreatePythonScript()
        {
            string projectPath = Helpers.GetProjectPath();
            string scriptPath = projectPath + "\\scripts\\face_encoding_and_comparison.py";
            string databasePath = projectPath + "\\database";
            string tempPath = projectPath + "\\temp";
            string[] args = { scriptPath, databasePath, tempPath };

            return args;
        }

        public async void RunPythonAsync()
        {
            if (!IsStarted)
            {
                Console.WriteLine("PyTask starting..");
                ReloadPyTask();
                PyTask.Start();
                IsStarted = true;
            }
            if (PyTask.IsCompleted)
            {
                Console.WriteLine("PyTask completed..");
                IsStarted = false;
                PyTask.Dispose();
            }
            else
            {
                Console.WriteLine("PyTask still running..");
            }
        }

        private void ReloadPyTask()
        {
            PyTask = new(PyScript.StartProcess);
        }

        public void Close()
        {
            PyScript.Process.Close();
        }
    }
}
