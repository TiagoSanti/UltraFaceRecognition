namespace UltraFaceRecognition
{
    class Encoder
    {
        public static void EncodeDatabaseImages()
        {
            string imageDatabaseDir = @".\database\images\";
            string encodingDatabase = @".\database\encodings\";
            Person person;

            if (!Directory.Exists(encodingDatabase))
            {
                Directory.CreateDirectory(encodingDatabase);
            }
            else
            {
                string[] peopleDir = Directory.GetDirectories(encodingDatabase);
                if (peopleDir.Length > 0)
                {

                }
            }
        }
    }
}
