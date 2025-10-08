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
        public Human[] ReadFromFile(string fileName)
        {
            if (!File.Exists(fileName))
            {
                return new Human[0];
            }

            string[] lines = File.ReadAllLines(fileName);
            Human[] people = new Human[0];

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].Trim();

                if (string.IsNullOrWhiteSpace(line) || line == "{" || line == "}")
                {
                    continue;
                }
                string[] header = line.Split(' ');
                string type = header[0];

                string firstName = "", lastName = "", studentID = "", passportSeries = "";
                int height = 0, weight = 0, passportNumber = 0;

                i++;
                while (i < lines.Length && lines[i].Trim() != "}")
                {
                    string dataLine = lines[i].Trim();

                    if (dataLine == "{" || string.IsNullOrWhiteSpace(dataLine))
                    {
                        i++;
                        continue;
                    }
                    dataLine = dataLine.Replace("\"", "").Replace(",", "");
                    string[] parts = dataLine.Split(new[] { ':' }, 2);

                    if (parts.Length < 2)
                    {
                        i++;
                        continue;
                    }

                    string key = parts[0].Trim();
                    string value = parts[1].Trim();

                    switch (key)
                    {
                        case "firstname":
                            firstName = value;
                            break;
                        case "lastname":
                            lastName = value;
                            break;
                        case "height":
                            int.TryParse(value, out height);
                            break;
                        case "weight":
                            int.TryParse(value, out weight);
                            break;
                        case "studentId":
                            studentID = value;
                            break;
                        case "passport":
                            if (value.Length >= 8)
                            {
                                passportSeries = value.Substring(0, 2);
                                int.TryParse(value.Substring(2), out passportNumber);
                            }
                            break;
                    }
                    i++;
                }

                Human newPerson = null;
                if (type == "Student")
                {
                    newPerson = new Student(firstName, lastName, height, weight, studentID, new Passport(passportSeries, passportNumber));
                }
                else if (type == "Librarian")
                {
                    newPerson = new Librarian(firstName, lastName);
                }
                else if (type == "SoftwareDeveloper")
                {
                    newPerson = new SoftwareDeveloper(firstName, lastName);
                }

                if (newPerson != null)
                {
                    // Додаємо створену людину в масив
                    Array.Resize(ref people, people.Length + 1);
                    people[people.Length - 1] = newPerson;
                }
            }
            return people;
        }
        public int CountIdealWeightStudents(string fileName)
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