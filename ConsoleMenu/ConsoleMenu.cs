using System;
using Entities;
using Files;

namespace ConsoleMenu
{
    internal class Program
    {
        static string fileName = "people.txt";

        static void Main()
        {
            Console.WriteLine("Система управління базою даних");
            while (true)
            {
                Console.WriteLine("\n------------ МЕНЮ ------------");
                Console.WriteLine("1 | Додати особу");
                Console.WriteLine("2 | Показати всіх");
                Console.WriteLine("3 | Студенти з ідеальною вагою");
                Console.WriteLine("4 | Очистити файл");
                Console.WriteLine("\n0 | Вихід");
                Console.Write("Вибір: ");

                string choice = Console.ReadLine()!;
                switch (choice)
                {
                    case "1":
                        PersonManager.AddPersonMenu(fileName);
                        break;
                    case "2":
                        PersonManager.ShowAllMenu(fileName);
                        break;
                    case "3":
                        PersonManager.ShowIdealWeight(fileName);
                        break;
                    case "4":
                        PersonManager.ClearFileMenu(fileName);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Невірний вибір");
                        break;
                }
            }
        }
    }
}