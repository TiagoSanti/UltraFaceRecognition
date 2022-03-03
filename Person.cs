using FaceRecognitionDotNet;

namespace UltraFaceRecognition
{
    class Person
    {
        public string Name { get; set; }
        public List<FaceEncoding>? FaceEncodings;

        public Person(string name)
        {
            Name = name;
        }
    }
}
