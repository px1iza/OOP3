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
            int count = 0;

            using (StreamReader reader = new StreamReader(fileName))
            {
                string line;
                int height = 0, weight = 0;

                while ((line = reader.ReadLine()) != null)
                {
                    line = line.Trim();

                    if (line.StartsWith("\"height\""))
                        height = int.Parse(line.Split(':')[1].Trim().TrimEnd(','));

                    else if (line.StartsWith("\"weight\""))
                        weight = int.Parse(line.Split(':')[1].Trim().TrimEnd(','));

                    if (line == "}")
                    {
                        if (weight == height - 110)
                            count++;
                        height = weight = 0;
                    }
                }
            }

            return count;
        }
    }
}