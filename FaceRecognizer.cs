

namespace UltraFaceRecognition
{
    public class FaceRecognizer
    {
        public static void RecognizeFace()
        {
            string laptopPythonInterpreter = @"C:\Users\Tiago Santi\AppData\Local\Programs\Python\Python39\python.exe";
            string desktopPythonInterpreter = @"C:\Users\tiago\AppData\Local\Programs\Python\Python39\python.exe";
            string projectPath = Helpers.GetProjectPath();
            string scriptPath = projectPath + "\\scripts\\face_encoding_and_comparison.py";

            using var process = Helpers.GetProcess(scriptPath, laptopPythonInterpreter);
            Helpers.StartProcess(process);
        }
    }
}
