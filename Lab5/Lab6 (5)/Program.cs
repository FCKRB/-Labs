using System;
using System.IO;
using static Lab5.ResearchTeam;

namespace Lab5
{
    class Program
    {
        static void Main(string[] args)
        {
            ResearchTeam researchTeam = new ResearchTeam();

            {
                Person leadResearcher = new Person("Сергей", "Шапокляк", new DateTime(2020, 5, 5));
                Person subleadResearcher = new Person("Пётр", "Совок", new DateTime(2019, 6, 17));
                Person badResearcher = new Person("Саша", "Александр", new DateTime(2110, 9, 2));

                researchTeam.AddMembers(leadResearcher, subleadResearcher, badResearcher);

                researchTeam.AddPapers(new Paper[] {
                    new Paper("Я не умею программировать", leadResearcher, new DateTime(2020, 5, 5)),
                    new Paper("Без названия №1", subleadResearcher, new DateTime(2019, 6, 17)),
                    new Paper("Последняя ли 'я' буква?", leadResearcher, new DateTime(2110, 9, 2)),
                });
            }

            ResearchTeam teamCopy = (ResearchTeam) researchTeam.DeepCopy();

            Console.WriteLine($"Оригинал: {researchTeam}.");
            Console.WriteLine();

            Console.WriteLine($"Копия (MemoryStream): {teamCopy}.");
            Console.WriteLine();

            Console.WriteLine($"Они равны? {researchTeam == teamCopy}.");
            Console.WriteLine();

            Console.Write("Введите название файла, в который будет сохранён объект: ");
            string filename = Console.ReadLine();
            if (!File.Exists(filename))
            {
                Console.WriteLine("Файла с таким именем не существует. Он будет создан...");
            }
            else
            {
                researchTeam.Load(filename);
                Console.WriteLine($"После загрузки: {researchTeam}.");
            }

            Console.WriteLine();

            researchTeam.AddPaperFromConsole();
            Console.WriteLine();

            Console.WriteLine($"После добавления публикации: {researchTeam}.");

            Console.WriteLine();
            researchTeam.Save(filename);
            Console.WriteLine($"Объект сохранён в {filename}...");
            Console.WriteLine();

            while (true)
            {
                Load(filename, researchTeam);
                Console.WriteLine($"Объект загружен из {filename}...");
                Console.WriteLine();

                researchTeam.AddPaperFromConsole();
                Console.WriteLine();

                Save(filename, researchTeam);
                Console.WriteLine($"Объект сохранён в {filename}...");
                Console.WriteLine();

                Console.WriteLine($"Результат: {researchTeam}");
                Console.WriteLine();

                Console.Write("Для продолжения ввода публикаций нажмите кнопку 'C': ");
                if (Console.ReadKey().Key != ConsoleKey.C) break;
                Console.WriteLine();
            }
        }
    }
}
