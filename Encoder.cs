namespace UltraFaceRecognition
{
    public class Encoder
    {
        public static void EncodeDatabase()
        {
            string laptopPythonInterpreter = @"C:\Users\TiagoSanti\AppData\Local\Programs\Python\Python39\python.exe";
            string desktopPythonInterpreter = @"C:\Users\tiago\AppData\Local\Programs\Python\Python39\python.exe";
            string projectPath = Helpers.GetProjectPath();
            string scriptPath = projectPath + "\\scripts\\database_encoder.py";
            string databasePath = projectPath + "\\database";

            using var process = Helpers.GetProcess(scriptPath, databasePath, desktopPythonInterpreter);
            Helpers.StartProcess(process);
        }
    }
}
