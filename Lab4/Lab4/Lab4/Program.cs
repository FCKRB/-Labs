using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Lab4.ResearchTeam;

namespace Lab4
{
	class Program
	{
		static void Main(string[] args)
		{
			ResearchTeamCollection<string> first = new ResearchTeamCollection<string>(
				(team) => team.ToString());
			first.CollectionName = "Первая коллекция";

			ResearchTeamCollection<string> second = new ResearchTeamCollection<string>(
				(team) => team.ToString());
			second.CollectionName = "Вторая коллекция";

			TeamsJournal teamsJournal = new TeamsJournal();
			first.ResearchTeamsChanged += teamsJournal.HandleChange;
			second.ResearchTeamsChanged += teamsJournal.HandleChange;

			ResearchTeam researchTeam0 = new ResearchTeam("Скучные исследования",
				"Скучные люди", 10, TimeFrame.Year);
			ResearchTeam researchTeam1 = new ResearchTeam("Черепашьи бега",
				"Терпеливые зоологи", 11, TimeFrame.TwoYears);
			ResearchTeam researchTeam2 = new ResearchTeam("Старение бананов",
				"Перспективные исследования", 12, TimeFrame.Long);
			ResearchTeam researchTeam3 = new ResearchTeam();

			first.AddResearchTeams(researchTeam0, researchTeam1);
			second.AddResearchTeams(researchTeam2, researchTeam3);

			researchTeam0.ResearchTopic = "[Данные удалены]";
			researchTeam2.ResearchTopic = "[Данные удалены]";
			researchTeam1.ResearchDuration = TimeFrame.Long;
			researchTeam3.ResearchDuration = TimeFrame.TwoYears;

			first.Remove(researchTeam1);
			researchTeam1.ResearchTopic = "[Данные удалены]";
			second.Remove(researchTeam3);
			researchTeam3.ResearchTopic = "[Данные удалены]";

			first.Replace(researchTeam0, researchTeam1);
			second.Replace(researchTeam2, researchTeam3);
			researchTeam0.ResearchDuration = TimeFrame.Long;
			researchTeam2.ResearchDuration = TimeFrame.TwoYears;

			Console.WriteLine(teamsJournal);
			Console.ReadKey();
		}
	}
}
