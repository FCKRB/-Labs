using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
	class Program
	{
		static KeyValuePair<Team, ResearchTeam> GeneratePair(int param)
		{
			++param;
			Team team = new Team($"Organization {param}", param);
			ResearchTeam researchTeam = new ResearchTeam(
				$"Topic {param}", team.OrganizationName, team.RegistrationNumber,
				(ResearchTeam.TimeFrame)((param % 3) + 1));
			return new KeyValuePair<Team, ResearchTeam>(team, researchTeam);
		}

		static void Main(string[] args)
		{
			Console.Write("Enter elements count: ");
			string elementsCountString = Console.ReadLine();
			int elementsCount = -1;
			while (!int.TryParse(elementsCountString, out elementsCount)
				|| elementsCount <= 0)
			{
				Console.WriteLine("Error: incorrect input... Try again.");
				Console.Write("Enter elements count: ");
				elementsCountString = Console.ReadLine();
			}

			TestCollections<Team, ResearchTeam> testCollections =
				new TestCollections<Team, ResearchTeam>(elementsCount, GeneratePair);
			Console.WriteLine("Finding the first element:");
			testCollections.testFirst();
			Console.WriteLine();

			Console.WriteLine("Finding the last element:");
			testCollections.testLast();
			Console.WriteLine();

			Console.WriteLine("Finding the central element:");
			testCollections.testCentral();
			Console.WriteLine();

			Console.WriteLine("Finding an absent element:");
			testCollections.testAbsent();
			Console.WriteLine();

			ResearchTeam mietResearchTeam = new ResearchTeam(
				"Размышления над апельсином", "Ромашки", 42, ResearchTeam.TimeFrame.TwoYears);

			{
				Person leadResearcher = new Person("Сергей", "Шпалов", new DateTime(2020, 5, 5));
				Person subleadResearcher = new Person("Пётр", "Совок", new DateTime(2019, 6, 17));
				Person badResearcher = new Person("Саша", "Александр", new DateTime(2110, 9, 2));

				mietResearchTeam.AddMembers(leadResearcher, subleadResearcher, badResearcher);

				mietResearchTeam.AddPapers(new Paper[] {
					new Paper("Без названия №1", leadResearcher, new DateTime(2020, 5, 5)),
					new Paper("Без названия №2", subleadResearcher, new DateTime(2019, 6, 17)),
					new Paper("А - не буква", leadResearcher, new DateTime(2110, 9, 2)),
					new Paper("Я - тоже", badResearcher, new DateTime(2021, 4, 28)),
				});
			}

			mietResearchTeam.SortPapersByDate();
			Console.WriteLine("Публикации, отсортированные по дате публикации:");
			foreach (Paper paper in mietResearchTeam.Papers)
				Console.WriteLine($"\t{paper}");

			mietResearchTeam.SortPapersByTitle();
			Console.WriteLine("Публикации, отсортированные по названию:");
			foreach (Paper paper in mietResearchTeam.Papers)
				Console.WriteLine($"\t{paper}");

			mietResearchTeam.SortPapersByAuthor();
			Console.WriteLine("Публикации, отсортированные по фамилии автора:");
			foreach (Paper paper in mietResearchTeam.Papers)
				Console.WriteLine($"\t{paper}");

			ResearchTeamCollection<string> researchTeamCollection =
				new ResearchTeamCollection<string>((team) => team.Team.ToString());
			researchTeamCollection.AddDefaults();

			{
				ResearchTeam researchTeam = new ResearchTeam("Чем бы заняться?",
					"Исследователи бесполезного", 2, ResearchTeam.TimeFrame.Long);
				Person majorPhilosopher = new Person("Антон", "Молочник", new DateTime(1991, 2, 23));
				Person minorPhilosopher = new Person("Павел", "Енот", new DateTime(1998, 9, 12));
				researchTeam.AddMembers(majorPhilosopher, minorPhilosopher);

				researchTeam.AddPapers(
					new Paper("Сколько гусениц нужно, чтобы вкрутить лампочку?",
						majorPhilosopher, new DateTime(2012, 5, 6)),
					new Paper("А если замотать кота изолентой?",
						minorPhilosopher, new DateTime(2021, 10, 1))
				);

				ResearchTeam anotherResearchTeam = (ResearchTeam) researchTeam.DeepCopy();
				anotherResearchTeam.ResearchDuration = ResearchTeam.TimeFrame.TwoYears;

				researchTeamCollection.AddResearchTeams(researchTeam, mietResearchTeam,
					new ResearchTeam("Пока не выбрали", "Пока не выбрали", 3, ResearchTeam.TimeFrame.TwoYears)
				);
			}

			Console.WriteLine("Дата последней публикации: " +
				researchTeamCollection.LatestPaperDate);

			Console.WriteLine("Команды, работающие два года:");
			foreach (var pair in researchTeamCollection.TimeFrameGroup(ResearchTeam.TimeFrame.TwoYears))
				foreach (Person member in pair.Value.Members)
					Console.WriteLine($"\t{member}");

			Console.WriteLine("Группа команд, работающих два года:");
			foreach (var pair in researchTeamCollection.groupByResearchDuration(
				ResearchTeam.TimeFrame.TwoYears))
			{
				foreach (var group in pair)
					Console.WriteLine($"\t{group.Value}");
			}

			Console.WriteLine("Группа команд, работающих много лет:");
			foreach (var pair in researchTeamCollection.groupByResearchDuration(
				ResearchTeam.TimeFrame.Long))
			{
				foreach (var group in pair)
					Console.WriteLine($"\t{group.Value}");
			}

			Console.WriteLine("Группа команд, работающих один год:");
			foreach (var pair in researchTeamCollection.groupByResearchDuration(
							ResearchTeam.TimeFrame.Year))
			{
				foreach (var group in pair)
					Console.WriteLine($"\t{group.Value}");
			}

			Console.ReadKey();
		}
	}
}
