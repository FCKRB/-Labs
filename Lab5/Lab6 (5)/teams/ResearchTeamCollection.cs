using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Lab5.ResearchTeam;
using static Lab5.ResearchTeamsChangedEventArgs;


namespace Lab5
{
	delegate TKey KeySelector<TKey>(ResearchTeam researchTeam);

	class ResearchTeamCollection<TKey>
	{
		private Dictionary<TKey, ResearchTeam> collection;
		readonly private KeySelector<TKey> keySelector;

		public event EventHandler<ResearchTeamsChangedEventArgs> ResearchTeamsChanged;

		public string CollectionName { get; set; }

		public ResearchTeamCollection(KeySelector<TKey> keySelector)
		{
			this.keySelector = keySelector;
			collection = new Dictionary<TKey, ResearchTeam>();
		}

		public DateTime LatestPaperDate
		{
			get
			{
				if (collection.Count == 0)
					return new DateTime();

				Paper latestPaper = new Paper("-", new Person(), new DateTime());

				foreach (TKey key in collection.Keys)
				{
					ResearchTeam researchTeam = collection[key];
					try
					{
						Paper paper = researchTeam.Papers.Max();
						if (paper != null && paper.CompareTo(latestPaper) > 0)
							latestPaper = paper;
					}
					catch (ArgumentNullException) { }
				}
				return latestPaper.PublicationDate;
			}
		}

		public IEnumerable<KeyValuePair<TKey, ResearchTeam>> TimeFrameGroup(
			TimeFrame timeFrame)
		{
			return collection.Where(
				(pair) => pair.Value.ResearchDuration == timeFrame);
		}

		public IEnumerable<IGrouping<TimeFrame, KeyValuePair<TKey, ResearchTeam>>>
			groupByResearchDuration(TimeFrame timeFrame)
		{
			return collection.GroupBy((pair) => pair.Value.ResearchDuration)
				.Where((group) =>
					group.First().Value.ResearchDuration == timeFrame);
		}

		private void HandleResearchTeamPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			ResearchTeamsChanged.Invoke(this, new ResearchTeamsChangedEventArgs(
				CollectionName, Revision.Property, args.PropertyName, -1));
		}

		public void AddDefaults()
		{
			ResearchTeam default_team = new ResearchTeam();
			collection[keySelector(default_team)] = default_team;
			default_team.PropertyChanged += HandleResearchTeamPropertyChanged;
		}

		public void AddResearchTeams(params ResearchTeam[] teams)
		{
			if (teams == null)
				throw new ArgumentNullException();

			foreach (ResearchTeam team in teams)
			{
				if (team == null)
					throw new ArgumentNullException();
				collection[keySelector(team)] = team;
				team.PropertyChanged += HandleResearchTeamPropertyChanged;
			}
		}

		public bool Remove(ResearchTeam researchTeam)
		{
			foreach (var pair in collection)
			{
				if (pair.Value.Equals(researchTeam))
				{
					collection.Remove(pair.Key);
					pair.Value.PropertyChanged -= HandleResearchTeamPropertyChanged;

					ResearchTeamsChanged.Invoke(collection,
						new ResearchTeamsChangedEventArgs(CollectionName,
							Revision.Remove, "", researchTeam.RegistrationNumber));
					return true;
				}
			}
			return false;
		}

		public bool Replace(ResearchTeam oldValue, ResearchTeam newValue)
		{
			foreach (var pair in collection)
			{
				if (pair.Value.Equals(oldValue))
				{
					collection.Remove(pair.Key);
					pair.Value.PropertyChanged -= HandleResearchTeamPropertyChanged;

					collection.Add(pair.Key, newValue);
					newValue.PropertyChanged += HandleResearchTeamPropertyChanged;

					ResearchTeamsChanged.Invoke(collection,
						new ResearchTeamsChangedEventArgs(CollectionName,
							Revision.Replace, "", oldValue.RegistrationNumber));
					return true;
				}
			}
			return false;
		}

		public string ToShortString()
		{
			StringBuilder builder = new StringBuilder();
			foreach (TKey key in collection.Keys)
			{
				ResearchTeam team = collection[key];
				builder.AppendFormat("{}\n\tMembers count: {}\n\tPublications count: {}",
					team.ToShortString(), team.Members.Count, team.Papers.Count);
			}
			return builder.ToString();
		}

		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();
			foreach (TKey key in collection.Keys)
				builder.AppendLine(collection[key].ToString());
			return builder.ToString();
		}
	}
}
