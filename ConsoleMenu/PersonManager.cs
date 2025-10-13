using System;
using Entities;
using Files;
namespace ConsoleMenu
{
    internal static class PersonManager
    {
        public static void AddPersonMenu(string fileName)
        {
            Console.WriteLine("\n1-Студент, 2-Бібліотекар, 3-Розробник");
            string choice = Console.ReadLine()!;
            Console.Write("Ім'я: ");
            string firstName = Console.ReadLine()!;
            Console.Write("Прізвище: ");
            string lastName = Console.ReadLine()!;

            Human person = null!;

            if (choice == "1")
            {
                Console.Write("Зріст: ");
                if (!int.TryParse(Console.ReadLine(), out int height))
                {
                    Console.WriteLine("Зріст має бути числом!");
                    return;
                }
                Console.Write("Вага: ");
                if (!int.TryParse(Console.ReadLine(), out int weight))
                {
                    Console.WriteLine("Вага має бути числом!");
                    return;
                }
                Console.Write("Студентський (KB123456): ");
                string studentID = Console.ReadLine()!.ToUpper();

                Console.Write("Серія паспорту (AB): ");
                string series = Console.ReadLine()!.ToUpper();

                Console.Write("Номер паспорту (6 цифр): ");
                if (!int.TryParse(Console.ReadLine(), out int number))
                {
                    Console.WriteLine("Номер паспорту має бути числом!");
                    return;
                }
                var passport = new Passport(series, number);
                var student = new Student(firstName, lastName, height, weight, studentID, passport);

                if (!student.IsValidStudentID() || !passport.IsValidPassport())
                {
                    Console.WriteLine("Некоректні дані студента! Запис не виконано.");
                    return;
                }
                person = student;

            }
            else if (choice == "2")
            {
                person = new Librarian(firstName, lastName);
                ((ISkill)person).RideBike();
            }
            else if (choice == "3")
            {
                person = new SoftwareDeveloper(firstName, lastName);
                ((ISkill)person).RideBike();
                ((ISkill)person).RideBike();
                ((ISkill)person).RideBike();
            }
            if (person != null)
            {
                DataStream.AddPerson(fileName, person);
                Console.WriteLine("Дані додано!");

                if (person is ISkill skilledPerson)
                    Console.WriteLine($"{person.FirstName} {person.LastName} покатався на велосипеді {skilledPerson.RideCount} разів");
            }
        }
        public static void ShowAllMenu(string fileName)
        {
            Human[] people = DataStream.ReadAllFromFile(fileName);

            if (people.Length == 0)
            {
                Console.WriteLine("\nФайл порожній");
                return;
            }

            Console.WriteLine($"\nВсього осіб: {people.Length}");
            foreach (var person in people)
            {
                Console.WriteLine("---");
                if (person is Student s)
                    Console.WriteLine($"Студент: {s.FirstName} {s.LastName} | {s.Height}см, {s.Weight}кг | ID: {s.StudentID}");
                else if (person is Librarian lib)
                    Console.WriteLine($"Бібліотекар: {lib.FirstName} {lib.LastName}");
                else if (person is SoftwareDeveloper dev)
                    Console.WriteLine($"Розробник: {dev.FirstName} {dev.LastName}");
            }
        }
        public static void ShowIdealWeight(string fileName)
        {
            Human[] all = DataStream.ReadAllFromFile(fileName);
            int count = 0;
            foreach (var p in all)
            {
                if (p is Student s && s.Weight == s.Height - 110)
                {
                    count++;
                    Console.WriteLine($"- {s.FirstName} {s.LastName} ({s.Height}см, {s.Weight}кг)");
                }
            }
            Console.WriteLine($"\nСтудентів з ідеальною вагою: {count}");
        }
        public static void ClearFileMenu(string fileName)
        {
            Console.Write("\nВидалити всі дані? (y/n): ");
            if (Console.ReadLine() == "y")
            {
                DataStream.ClearFile(fileName);
                Console.WriteLine("Очищено!");
            }
        }
    }
}