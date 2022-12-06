using System;
using System.Diagnostics;

namespace Lab1
{
	class Program
	{
		static char TestLab1()
		{
			ResearchTeam testTeam = new ResearchTeam("Rheology of cats",
				"MIT Research Team", 42, ResearchTeam.TimeFrame.TwoYears);
			testTeam.Papers = new Paper[] {
				new Paper(
					"On the rheology of cats",
					new Person("Mark Antuan", "Fardin", new DateTime(1984, 5, 12)),
					new DateTime(2014, 7, 9)
				),
			};

			Console.WriteLine($"Shortly: {testTeam.ToShortString()}");
			Console.WriteLine($"Year: {testTeam[ResearchTeam.TimeFrame.Year]}");
			Console.WriteLine($"Two years: {testTeam[ResearchTeam.TimeFrame.TwoYears]}");
			Console.WriteLine($"Long: {testTeam[ResearchTeam.TimeFrame.Long]}");

			testTeam.OrganizationName = "Студент МИЭТ";
			testTeam.ResearchTopic = "Реология котов";
			testTeam.RegistrationNumber = 8200255;
			testTeam.ResearchDuration = ResearchTeam.TimeFrame.Long;

			Paper paper1 = new Paper(
				"Диффузия котов",
				new Person("Владимир", "Комаревцев", new DateTime(2002, 3, 4)),
				new DateTime(2022, 11, 11)
			);

			testTeam.AddPapers(paper1);

			Console.WriteLine(testTeam);
			Console.WriteLine($"Последняя публикация: {testTeam.LatestPaper}");

			Console.WriteLine();
			Console.WriteLine("Сравнение времени выполнения операций с массивами:");
			Console.Write("Введите количество строк и столбцов (разделяя их: ;, /, |): ");

			string[] inputNumbers = Console.ReadLine().Split(new char[] { ';', '/', '|' });

			int rows;
			int columns;
			if(!Int32.TryParse(inputNumbers[0], out rows))
			{
				Console.WriteLine("Ошибка: не удалось распознать число строк...");
				return Console.ReadKey().KeyChar;
			}
			if (!Int32.TryParse(inputNumbers[1], out columns))
			{
				Console.WriteLine("Ошибка: не удалось распознать число столбцов...");
				return Console.ReadKey().KeyChar;
			}
			Console.WriteLine($"Строк: {rows}, Столбцов: {columns}");

			Stopwatch watch = Stopwatch.StartNew();
			{
				Paper[] singleDimensional = new Paper[rows * columns];
				for (var i = 0; i < rows * columns; ++i)
					singleDimensional[i] = new Paper();

				watch.Start();
				for (var i = 0; i < rows * columns; ++i)
					singleDimensional[i].Title = "Monke";
				watch.Stop();
				Console.WriteLine($"Одномерный массив: {watch.ElapsedMilliseconds} мс");
			}

			{
				Paper[,] multidimensional = new Paper[rows, columns];
				for (var i = 0; i < rows; ++i)
					for (var j = 0; j < columns; ++j)
						multidimensional[i, j] = new Paper();

				watch.Start();
				for (int i = 0; i < rows; ++i)
					for (int j = 0; j < columns; ++j)
						multidimensional[i, j].Title = "Monke";
				watch.Stop();
				Console.WriteLine($"Двумерный прямоугольный массив: " +
					$"{watch.ElapsedMilliseconds} мс");
			}

			{
				Paper[][] jagged = new Paper[rows][];
				for (int i = 0; i < rows; ++i)
				{
					jagged[i] = new Paper[columns];
					for (int j = 0; j < columns; ++j)
						jagged[i][j] = new Paper();
				}
				watch.Start();
				for (int i = 0; i < rows; ++i)
					for (int j = 0; j < columns; ++j)
						jagged[i][j].Title = "Monke";
				watch.Stop();
				Console.WriteLine($"Ступенчатый массив: {watch.ElapsedMilliseconds} мс");
			}
			return Console.ReadKey().KeyChar;
		}

		static void Main()
		{
			char controlChar;
			do
			{
				Console.WriteLine(@"Тестовая программа л/р №1. Чтобы выйти введите 'e'...");
				controlChar = TestLab1();
			} while (controlChar != 'e');
			Console.WriteLine();
		}
	}
}
