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
        public void AppendToFile(string fileName)
        {
            using (StreamWriter writer = new StreamWriter(fileName, append: true))
            {
                foreach (Human person in _people)
                    WritePerson(writer, person);
            }
        }
        private void WritePerson(StreamWriter writer, Human person)
        {
            if (person is Student s)
            {
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
        public static void AddPerson(string fileName, Human person)
        {
            DataStream stream = new DataStream(new Human[] { person });
            stream.AppendToFile(fileName);
        }
        public static Human[] ReadAllFromFile(string fileName)
        {
            if (!File.Exists(fileName))
                return new Human[0];

            string text = File.ReadAllText(fileName);
            int objectCount = Regex.Matches(text, @"\b(Student|Librarian|SoftwareDeveloper)\b").Count;

            if (objectCount == 0)
                return new Human[0];

            Human[] temp = new Human[objectCount];
            int count = 0;

            using (StreamReader reader = new StreamReader(fileName))
            {
                string line;
                string currentType = null!;
                string firstName = null!, lastName = null!;
                int height = 0, weight = 0;
                string studentId = null!, passport = null!;

                while ((line = reader.ReadLine()!) != null)
                {
                    line = line.Trim();

                    if (line.StartsWith("Student")) currentType = "Student";
                    else if (line.StartsWith("Librarian")) currentType = "Librarian";
                    else if (line.StartsWith("SoftwareDeveloper")) currentType = "SoftwareDeveloper";
                    else if (line.Contains(":"))
                    {
                        string key = line.Split(':')[0].Trim();
                        string value = GetValue(line);

                        switch (key)
                        {
                            case "\"firstname\"":
                                firstName = value;
                                break;
                            case "\"lastname\"":
                                lastName = value;
                                break;
                            case "\"height\"":
                                int.TryParse(line.Split(':')[1].Trim().TrimEnd(','), out height);
                                break;
                            case "\"weight\"":
                                int.TryParse(line.Split(':')[1].Trim().TrimEnd(','), out weight);
                                break;
                            case "\"studentId\"":
                                studentId = value;
                                break;
                            case "\"passport\"":
                                passport = value;
                                break;
                        }
                    }
                    if (line == "}")
                    {
                        try
                        {
                            if (currentType == "Student")
                            {
                                string series = passport?.Substring(0, 2)!;
                                int number = 0;
                                int.TryParse(passport?.Substring(2), out number);

                                temp[count++] = new Student(firstName, lastName, height, weight, studentId, new Passport(series, number));
                            }
                            else if (currentType == "Librarian")
                                temp[count++] = new Librarian(firstName, lastName);
                            else if (currentType == "SoftwareDeveloper")
                                temp[count++] = new SoftwareDeveloper(firstName, lastName);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"Помилка при створенні об’єкта: {e.Message}");
                        }

                        currentType = firstName = lastName = studentId = passport = null!;
                        height = weight = 0;
                    }
                }
            }
            Human[] result = new Human[count];
            for (int i = 0; i < count; i++)
                result[i] = temp[i];

            return result;
        }
        public static void ClearFile(string fileName)
        {
            File.WriteAllText(fileName, string.Empty);
        }
        private static string GetValue(string line)
        {
            int start = line.IndexOf('"', line.IndexOf(':')) + 1;
            int end = line.LastIndexOf('"');
            return line.Substring(start, end - start);
        }
    }
}