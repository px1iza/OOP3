using System;
using Entities;
using Files;

namespace ConsoleMenu
{
    internal class Program
    {
        static Human[] InputPeople()
        {
            Console.Write("Скільки осіб ви хочете додати? ");
            int n = int.Parse(Console.ReadLine()!);
            Human[] people = new Human[n];

            for (int i = 0; i < n; i++)
            {
                Console.WriteLine($"\n--- Введення даних для особи №{i + 1} ---");
                Console.WriteLine("Оберіть тип: 1 - Студент, 2 - Бібліотекар, 3 - Розробник");
                string type = Console.ReadLine()!;

                Console.Write("Ім'я: ");
                string firstName = Console.ReadLine();
                Console.Write("Прізвище: ");
                string lastName = Console.ReadLine();

                if (type == "1")
                {
                    Console.Write("Зріст (см): ");
                    int height = int.Parse(Console.ReadLine());
                    Console.Write("Вага (кг): ");
                    int weight = int.Parse(Console.ReadLine());
                    Console.Write("Студентський квиток (напр. KB123456): ");
                    string studentId = Console.ReadLine();
                    Console.Write("Серія паспорта (2 літери): ");
                    string passportSeries = Console.ReadLine();
                    Console.Write("Номер паспорта (6 цифр): ");
                    int passportNumber = int.Parse(Console.ReadLine());

                    people[i] = new Student(firstName, lastName, height, weight, studentId, new Passport(passportSeries, passportNumber));
                }
                else if (type == "2")
                {
                    people[i] = new Librarian(firstName, lastName);
                }
                else
                {
                    people[i] = new SoftwareDeveloper(firstName, lastName);
                }
            }
            return people;
        }

        static void Main()
        {
            Human[] peopleFromInput = InputPeople();

            DataStream stream = new DataStream(peopleFromInput);
            string fileName = "people.txt";
            stream.WriteToFile(fileName);
            Console.WriteLine($"\nДані успішно записано у файл '{fileName}'!");

            Console.WriteLine("\n--- Читання даних з файлу ---");
            Human[] peopleFromFile = stream.ReadFromFile(fileName);
            Display(peopleFromFile);

            Console.WriteLine($"\nКількість студентів з ідеальною вагою: {stream.CountIdealWeightStudents(fileName)}");

            Console.WriteLine("\n--- Демонстрація навички ---");
            foreach (var person in peopleFromFile)
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