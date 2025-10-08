using System;
using Entities;
using Files;

namespace ConsoleMenu
{
    internal class Program
    {
        static void Main()
        {
            Human[] people = new Human[]
            {
                new Student("Олександр", "Петренко", 180, 70, "KB123456", new Passport("AB", 987654)),
                new Student("Марія", "Коваль", 165, 55, "KB789012", new Passport("AC", 345678)),
                new Librarian("Світлана", "Мельник"),
                new SoftwareDeveloper("Євген", "Романюк"),
                new Student("Іван", "Сидоренко", 175, 65, "KB456789", new Passport("AD", 111222)),
                new Student("Наталія", "Шевченко", 160, 50, "KB333444", new Passport("AE", 222333)),
                new Student("Дмитро", "Гриценко", 190, 80, "KB555666", new Passport("AF", 333444))
            };

            DataStream stream = new DataStream(people);
            string fileName = "people.txt";
            stream.WriteToFile(fileName);

            Console.WriteLine($"Дані записано у файл '{fileName}'!");
            Console.WriteLine($"Кількість студентів з ідеальною вагою: {stream.ReadFromFile(fileName)}");
            Console.WriteLine("\nСписок усіх осіб:");
            Display(people);

            Console.WriteLine("\nДемонстрація навички:");
            foreach (var person in people)
            {
                if (person is ISkill skilledPerson)
                {
                    skilledPerson.RideBike();
                    skilledPerson.RideBike();
                    Console.WriteLine($"{person.FirstName} {person.LastName} покатався {skilledPerson.RideCount} раз(и)");
                }
            }
        }
        static void Display(Human[] people)
        {
            foreach (var person in people)
            {
                if (person is Student s)
                {
                    Console.WriteLine($"Студент: {s.FirstName} {s.LastName} | {s.Height}см, {s.Weight}кг | ID: {s.StudentID} | Паспорт: {s.Passport.FullPassport}");
                }
                else
                {
                    Console.WriteLine($"Працівник: {person.GetType().Name} {person.FirstName} {person.LastName}");
                }
            }
        }
    }
}
