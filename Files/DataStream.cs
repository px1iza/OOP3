using System;
using System.IO;
using Entities;
using System.Text.RegularExpressions;

namespace Files
{
    public class DataStream
    {
        private Human[] _people;

        public DataStream(Human[] people)
        {
            _people = people;
        }

        public void WriteToFile(string fileName)
        {
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                foreach (Human person in _people)
                {
                    if (person is Student s)
                    {
                        if (!s.IsValidStudentID() || !s.Passport.IsValidPassport())
                            throw new Exception("Некоректні дані студента!");

                        writer.WriteLine($"Student {s.FirstName}{s.LastName}");
                        writer.WriteLine("{");
                        writer.WriteLine($"\"firstname\": \"{s.FirstName}\",");
                        writer.WriteLine($"\"lastname\": \"{s.LastName}\",");
                        writer.WriteLine($"\"height\": {s.Height},");
                        writer.WriteLine($"\"weight\": {s.Weight},");
                        writer.WriteLine($"\"studentId\": \"{s.StudentID}\",");
                        writer.WriteLine($"\"passport\": \"{s.Passport.FullPassport}\"");
                        writer.WriteLine("}");
                    }
                    else if (person is Librarian lib)
                    {
                        writer.WriteLine($"Librarian {lib.FirstName}{lib.LastName}");
                        writer.WriteLine("{");
                        writer.WriteLine($"\"firstname\": \"{lib.FirstName}\",");
                        writer.WriteLine($"\"lastname\": \"{lib.LastName}\"");
                        writer.WriteLine("}");
                    }
                    else if (person is SoftwareDeveloper dev)
                    {
                        writer.WriteLine($"SoftwareDeveloper {dev.FirstName}{dev.LastName}");
                        writer.WriteLine("{");
                        writer.WriteLine($"\"firstname\": \"{dev.FirstName}\",");
                        writer.WriteLine($"\"lastname\": \"{dev.LastName}\"");
                        writer.WriteLine("}");
                    }
                }
            }
        }
        public int ReadFromFile(string fileName)
        {
            string fileContent = File.ReadAllText(fileName);
            int count = 0;
            Regex studentBlockRegex = new Regex(@"Student.*?\{(.*?)\}", RegexOptions.Singleline);

            Regex heightRegex = new Regex(@"""height""\s*:\s*(\d+)");
            Regex weightRegex = new Regex(@"""weight""\s*:\s*(\d+)");

            MatchCollection studentMatches = studentBlockRegex.Matches(fileContent);

            foreach (Match studentMatch in studentMatches)
            {
                string blockContent = studentMatch.Groups[1].Value;

                Match heightMatch = heightRegex.Match(blockContent);
                Match weightMatch = weightRegex.Match(blockContent);

                if (heightMatch.Success && weightMatch.Success)
                {
                    int.TryParse(heightMatch.Groups[1].Value, out int height);
                    int.TryParse(weightMatch.Groups[1].Value, out int weight);

                    if (weight > 0 && height > 0 && weight == height - 110)
                    {
                        count++;
                    }
                }
            }
            return count;
        }
    }
}