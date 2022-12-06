using System;

namespace Lab2
{
	class Program
	{
		static void Main()
		{
			{
				Team testTeam = new Team("MIT Research player", 667677);
				Team testTeamClone = new Team("MIT Research player", 667677);
				Console.WriteLine($"Участник MIT: {testTeam}");
				Console.WriteLine($"Её клон: {testTeamClone}");
				Console.WriteLine($"Сравнение ссылок: {testTeam == testTeamClone}");
				Console.WriteLine($"Сравнение значений: {testTeam.Equals(testTeamClone)}");
				Console.WriteLine();

				Console.WriteLine("Устроим диверсию (*^ ^*)");
				try
				{
					testTeam.RegistrationNumber = -12;
					Console.WriteLine("Мы в системе, всё отлично!");
				}
				catch (ArgumentException e)
				{
					Console.WriteLine($"Не получилось -_-");
					Console.WriteLine($@"'{e.Message}' - ответили оттуда...");
				}
				Console.WriteLine();
			}

			ResearchTeam mietResearchTeam = new ResearchTeam(
				"Реология кошек", "Исследователь МИЭТ",
				8200255, ResearchTeam.TimeFrame.Year
			);

			Person founder = new Person("Mark Antuan", "Fardin", new DateTime(1984, 5, 12));
			Person leadResearcher = new Person("Владимир", "Комаревцев", new DateTime(2002, 6, 6));

			mietResearchTeam.AddMembers(new Person[] {
				founder,
				leadResearcher,
				new Person("Владимир", "Комаревцев", new DateTime(2002, 5, 5)),
				new Person("Владимир", "Комаревцев", new DateTime(2002, 4, 4)),
			});

			mietResearchTeam.AddPapers(new Paper[] {
				new Paper("On the rheology of cats", founder, new DateTime(2014, 7, 9)),
				new Paper("Действительно ли коты - жидкость?", leadResearcher, new DateTime(2021, 4, 21)),
				new Paper("Диффузия котов в стеке", leadResearcher, new DateTime(2021, 4, 26)),
				new Paper("Зависимость потока частиц котов от цвета их шерсти", leadResearcher, new DateTime(2021, 4, 28)),
			});

			Console.WriteLine($"Лучшый исследователь в мире: {mietResearchTeam}");
			Console.WriteLine($"Сокращённо: {mietResearchTeam.Team}");

			{
				Console.WriteLine("*Клонируем её*");
				ResearchTeam mietResearchTeamClone = (ResearchTeam) mietResearchTeam.DeepCopy();

				((Paper)mietResearchTeam.Papers[0]).Name = "[Данные удалены]";
				mietResearchTeam.Team.Name = "[Данные удалены]";
				mietResearchTeam.AddMembers(
					new Person("Владимир", "Комаревцев", new DateTime(2002, 3, 3)));
				mietResearchTeam.AddPapers(
					new Paper("Опровержение теории о жидкостном происхождении котов",
						leadResearcher, new DateTime(2021, 9, 16))
				);
				Console.WriteLine($"Оригинал: {mietResearchTeam}");
				Console.WriteLine($"Фальшивка: {mietResearchTeamClone}");
			}

			Console.WriteLine("Дармоед:");
			foreach (Person person in mietResearchTeam.Freeloaders())
				Console.WriteLine($"\t{person}");

			Console.WriteLine("Последние публикации за 2 года:");
			foreach (Paper paper in mietResearchTeam.LastPapers(2))
				Console.WriteLine($"\t{paper}");

			Console.WriteLine("Не дармоед:");
			foreach (Person person in mietResearchTeam)
				Console.WriteLine($"\t{person}");

			Console.WriteLine("Старичок:");
			foreach (Person person in mietResearchTeam.VeteranMembers())
				Console.WriteLine($"\t{person}");

			Console.WriteLine("Публикации за последний год:");
			foreach (Paper paper in mietResearchTeam.LastYearPapers())
				Console.WriteLine($"\t{paper}");

			Console.ReadKey();
		}
	}
}
