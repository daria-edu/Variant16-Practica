using System;
using System.Collections.Generic;
using WeatherList;

class Program
{
    private static WeatherLinkedList list = new WeatherLinkedList();

    private const string SaveFile = "weather.json";

    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        SeedList();

        bool exit = false;

        while (!exit)
        {
            PrintMenu();

            Console.Write("\nВаш вибір: ");
            string? choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        AddFirst();
                        break;

                    case "2":
                        AddLast();
                        break;

                    case "3":
                        Remove();
                        break;

                    case "4":
                        PrintTable(list);
                        break;

                    case "5":
                        IndexerGet();
                        break;

                    case "6":
                        IndexerSet();
                        break;

                    case "7":
                        PrintLength();
                        break;

                    case "8":
                        IterateManual();
                        break;

                    case "9":
                        SplitList();
                        break;

                    case "10":
                        Search();
                        break;

                    case "11":
                        Serialize();
                        break;

                    case "12":
                        Deserialize();
                        break;

                    case "0":
                        exit = true;
                        break;

                    default:
                        Console.WriteLine("Невірний пункт меню.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Помилка: " + ex.Message);
            }

            Console.WriteLine();
        }
    }

    static void PrintMenu()
    {
        Console.WriteLine("=================================");
        Console.WriteLine("ДВОЗВ'ЯЗНИЙ СПИСОК ПОГОДИ");
        Console.WriteLine("=================================");
        Console.WriteLine("1 - Додати на початок");
        Console.WriteLine("2 - Додати в кінець");
        Console.WriteLine("3 - Видалити елемент");
        Console.WriteLine("4 - Вивести список");
        Console.WriteLine("5 - Отримати елемент за індексом");
        Console.WriteLine("6 - Змінити елемент за індексом");
        Console.WriteLine("7 - Довжина списку");
        Console.WriteLine("8 - Ручна ітерація");
        Console.WriteLine("9 - Розбиття списку");
        Console.WriteLine("10 - Пошук");
        Console.WriteLine("11 - Зберегти у файл");
        Console.WriteLine("12 - Завантажити з файлу");
        Console.WriteLine("0 - Вихід");
    }

    static void SeedList()
    {
        list.AddLast(new WeatherData(WeatherType.Rainy, 12.5, true));
        list.AddLast(new WeatherData(WeatherType.Sunny, 25.0, false));
        list.AddLast(new WeatherData(WeatherType.Cloudy, 18.0, false));
        list.AddLast(new WeatherData(WeatherType.Rainy, 10.0, true));
    }

    static void AddFirst()
    {
        WeatherData data = ReadWeatherData();

        list.AddFirst(data);

        Console.WriteLine("Елемент додано.");
    }

    static void AddLast()
    {
        WeatherData data = ReadWeatherData();

        list.AddLast(data);

        Console.WriteLine("Елемент додано.");
    }

    static void Remove()
    {
        Console.Write("Введіть індекс: ");

        int index = int.Parse(Console.ReadLine()!);

        WeatherData removed = list.RemoveAt(index);

        Console.WriteLine("Видалено:");
        Console.WriteLine(removed);
    }

    static void IndexerGet()
    {
        Console.Write("Введіть індекс: ");

        int index = int.Parse(Console.ReadLine()!);

        Console.WriteLine(list[index]);
    }

    static void IndexerSet()
    {
        Console.Write("Введіть індекс: ");

        int index = int.Parse(Console.ReadLine()!);

        Console.WriteLine("Введіть нові дані:");

        list[index] = ReadWeatherData();

        Console.WriteLine("Елемент змінено.");
    }

    static void PrintLength()
    {
        Console.WriteLine("Кількість елементів: " + list.Length);
    }

    static void IterateManual()
    {
        WeatherData? current = list.IteratorReset();

        int i = 0;

        while (current != null)
        {
            Console.WriteLine(i + ": " + current);

            current = list.IteratorNext();

            i++;
        }
    }

    static void SplitList()
    {
        Console.Write("Введіть поріг температури: ");

        double threshold = ReadDouble();

        var result = list.SplitByThreshold(threshold);

        WeatherLinkedList below = result.Item1;
        WeatherLinkedList equal = result.Item2;
        WeatherLinkedList above = result.Item3;

        Console.WriteLine("\nНижче порога:");
        PrintTable(below);

        Console.WriteLine("\nДорівнює порогу:");
        PrintTable(equal);

        Console.WriteLine("\nВище порога:");
        PrintTable(above);
    }

    static void Search()
    {
        List<WeatherData> result = list.SearchRainyBelow15();

        if (result.Count == 0)
        {
            Console.WriteLine("Нічого не знайдено.");
            return;
        }

        Console.WriteLine("Знайдені елементи:");

        foreach (WeatherData item in result)
        {
            Console.WriteLine(item);
        }
    }

    static void Serialize()
    {
        list.SerializeToJson(SaveFile);

        Console.WriteLine("Список збережено.");
    }

    static void Deserialize()
    {
        list = WeatherLinkedList.DeserializeFromJson(SaveFile);

        Console.WriteLine("Список завантажено.");
    }

    static void PrintTable(WeatherLinkedList list)
    {
        if (list.Length == 0)
        {
            Console.WriteLine("Список порожній.");
            return;
        }

        Console.WriteLine();
        Console.WriteLine("№   Дані");

        int i = 0;

        foreach (WeatherData item in list)
        {
            Console.WriteLine(i + "   " + item);
            i++;
        }
    }

    static WeatherData ReadWeatherData()
    {
        Console.WriteLine("Типи погоди:");

        foreach (string name in Enum.GetNames(typeof(WeatherType)))
        {
            Console.WriteLine(name);
        }

        WeatherType weatherType;

        while (true)
        {
            Console.Write("Введіть тип погоди: ");

            string? input = Console.ReadLine();

            if (Enum.TryParse(input, true, out weatherType))
            {
                break;
            }

            Console.WriteLine("Невірний тип погоди.");
        }

        Console.Write("Введіть температуру: ");
        double temperature = ReadDouble();

        bool precipitation = ReadBool();

        return new WeatherData(
            weatherType,
            temperature,
            precipitation
        );
    }

    static double ReadDouble()
    {
        double value;

        while (true)
        {
            string? input = Console.ReadLine();

            if (double.TryParse(input, out value))
            {
                return value;
            }

            Console.Write("Помилка. Введіть число ще раз: ");
        }
    }

    static bool ReadBool()
    {
        while (true)
        {
            Console.Write("Є опади? (т/н): ");

            string? answer = Console.ReadLine();

            if (answer == null)
            {
                continue;
            }

            answer = answer.ToLower();

            if (answer == "т")
            {
                return true;
            }

            if (answer == "н")
            {
                return false;
            }

            Console.WriteLine("Введіть т або н.");
        }
    }
}