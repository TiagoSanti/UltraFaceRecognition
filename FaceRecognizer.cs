namespace UltraFaceRecognition
{
    public class FaceRecognizer
    {
        public PythonScript PyScript;
        public string[] Args;
        public Task PyTask;

        public FaceRecognizer()
        {
            Args = CreatePythonScript();
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

        public void RunPython()
        {
            if (PyScript == null)
            {
                PyScript = new PythonScript(Args);
                PyTask = new(PyScript.StartProcess);
            }
            if (PyTask.Status == TaskStatus.Created)
            {
                Console.WriteLine("PyTask starting..");
                ReloadPyTask();
                PyTask.Start();
            }
            else if (PyTask.Status == TaskStatus.Running)
            {
                //Console.WriteLine("PyTask still running..");
            }
            else if (PyTask.Status == TaskStatus.RanToCompletion)
            {
                Console.WriteLine("PyTask completed..");
                PyTask.Dispose();
                ReloadPyTask();
            }
            else if (PyTask.Status == TaskStatus.Faulted)
            {
                Console.WriteLine("PyTask got an exception");
                PyTask.Dispose();
            }
            else if (PyTask.Status == TaskStatus.Canceled)
            {
                Console.WriteLine("PyTask's been cancelled");
                PyTask.Dispose();
            }
            else
            {
                Console.WriteLine(PyTask.Status.ToString());
            }
        }

        private void ReloadPyTask()
        {
            PyScript = new PythonScript(Args);
            PyTask = new(PyScript.StartProcess);
        }
    }
}
