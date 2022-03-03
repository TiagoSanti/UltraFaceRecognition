using FaceRecognitionDotNet;
using OpenCvSharp;

namespace UltraFaceRecognition
{
    class FaceRecognizer
    {
        public static void RecognizeFaces(List<FaceEncoding> faceEncodings, Location[] faceLocations, List<Person> people, Mat mat)
        {
            var index = 0;

            while (index < faceEncodings.Count)
            {
                FaceEncoding encoding = faceEncodings[index];
                Location faceLocation = faceLocations[index];

                double bestAvgDistance = 1;
                Person? bestAvgMatchPerson = null;

                double minDistance = 1;
                Person? minDistancePerson = null;

                foreach (Person person in people)
                {
                    IEnumerable<double> distances = FaceRecognition.FaceDistances(person.FaceEncodings, encoding);

                    double avgPersonDistance = distances.Average();
                    double minPersonDistance = distances.Min();

                    if (avgPersonDistance < bestAvgDistance)
                    {
                        bestAvgDistance = avgPersonDistance;
                        bestAvgMatchPerson = person;
                    }
                    if (minPersonDistance < minDistance)
                    {
                        minDistance = minPersonDistance;
                        minDistancePerson = person;
                    }
                }

                if (bestAvgMatchPerson != null && minDistancePerson != null)
                {
                    if (bestAvgMatchPerson.Equals(minDistancePerson))
                    {
                        Console.WriteLine("Best match distance person: " +
                        bestAvgMatchPerson.Name +
                        "\nWith average: " + bestAvgDistance +
                        "\nAnd minimal: " + minDistance +
                        "\n--------------------------------------------------");
                    }
                    else
                    {
                        Console.WriteLine("Best average distance match person: " +
                        bestAvgMatchPerson.Name +
                        "\nWith average: " + bestAvgDistance +
                        "\nAnd best minimal distance match person: " +
                        minDistancePerson.Name +
                        "\n--------------------------------------------------");
                    }
                }

                if (bestAvgMatchPerson != null)
                {
                    Drawers.DrawName(mat, bestAvgMatchPerson, faceLocation);
                }

                index++;
            }
        }
    }
}
