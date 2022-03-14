using OpenCvSharp;

namespace UltraFaceRecognition
{
    public class FaceRecognizer
    {
        public static async void CallPythonAsync()
        {
            string projectPath = Helpers.GetProjectPath();
            string scriptPath = projectPath + "\\scripts\\face_encoding_and_comparison.py";
            string databasePath = projectPath + "\\database";
            string tempPath = projectPath + "\\temp";
            string[] args = {scriptPath, databasePath, tempPath};

            PythonScript script = new(args);
            await Task.Run(() => script.StartProcess());
        }
    }
}
