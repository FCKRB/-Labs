using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace Lab4
{
	class ResearchTeam : Team, IEnumerable<Person>,
		INotifyPropertyChanged
	{
		public enum TimeFrame { Undefined, Year, TwoYears, Long, };

		private const string DEFAULT_RESEARCH_TOPIC = "No research topic";

		private string researchTopic;
		private TimeFrame researchDuration;
		private List<Person> members;
		private List<Paper> papers;

		public event PropertyChangedEventHandler PropertyChanged;

		public ResearchTeam()
		{
			researchTopic = DEFAULT_RESEARCH_TOPIC;
			researchDuration = default;
			members = new List<Person>();
			papers = new List<Paper>();
		}

		public ResearchTeam(string researchTopic, string organizationName,
			int registrationNumber, TimeFrame researchDuration)
				: base(organizationName, registrationNumber)
		{
			ResearchTopic = researchTopic;
			ResearchDuration = researchDuration;
			members = new List<Person>();
			papers = new List<Paper>();
		}

		public string ResearchTopic
		{
			get => researchTopic;
			set
			{
				if (value.Length == 0)
					throw new ArgumentException("zero research topic length");
				researchTopic = value;

				if (PropertyChanged != null)
					PropertyChanged.Invoke(this,
						new PropertyChangedEventArgs("ResearchTopic"));
			}
		}

		public TimeFrame ResearchDuration
		{
			get => researchDuration;
			set
			{
				researchDuration = value;

				if (PropertyChanged != null)
					PropertyChanged.Invoke(this,
						new PropertyChangedEventArgs("ResearchDuration"));
			}
		}

		public List<Person> Members
		{
			get => members;
			set
			{
				if (value == null)
					throw new ArgumentNullException();
				foreach (Person person in value)
					if (person == null)
						throw new ArgumentNullException("null Person argument");
				members = value;
			}
		}

		public List<Paper> Papers
		{
			get => papers;
			set
			{
				if (value == null)
					throw new ArgumentNullException();
				foreach (Paper paper in value)
					if (paper == null)
						throw new ArgumentNullException("null Paper argument");
				papers = value;
			}
		}

		public Paper LatestPaper
		{
			get
			{
				if (papers.Count == 0)
					return null;

				Paper latestPaper = (Paper) papers[0];
				foreach (Paper paper in papers)
				{
					if (paper.PublicationDate.CompareTo(
						latestPaper.PublicationDate) > 0)
					{
						latestPaper = paper;
					}
				}
				return latestPaper;
			}
		}

		public Team Team
		{
			get => (Team) base.DeepCopy();
			set
			{
				OrganizationName = value.OrganizationName;
				RegistrationNumber = value.RegistrationNumber;
			}
		}

		public override object DeepCopy()
		{
			Team team = (Team) base.DeepCopy();
			ResearchTeam result = new ResearchTeam(researchTopic,
				team.OrganizationName, team.RegistrationNumber,
				researchDuration);

			result.members.Capacity = members.Capacity;
			foreach (Person person in members)
				result.members.Add((Person) person.DeepCopy());

			result.papers.Capacity = papers.Capacity;
			foreach (Paper paper in papers)
				result.papers.Add((Paper) paper.DeepCopy());

			return result;
		}

		public IEnumerator<Person> GetEnumerator()
		{
			List<Person> authors = new List<Person>();
			foreach (Person member in members)
			{
				if (NumberOfPublications(member) > 0)
					authors.Add(member);
			}
			return new ResearchTeamEnumerator(authors.ToArray());
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public IEnumerable<Person> Freeloaders()
		{
			foreach (Person member in members)
			{
				if (NumberOfPublications(member) == 0)
					yield return member;
			}
		}

		public IEnumerable<Paper> LastPapers(int years)
		{
			int currentYear = DateTime.Now.Year;
			foreach (Paper paper in papers)
			{
				if (currentYear - paper.PublicationDate.Year < years)
					yield return paper;
			}
		}

		public IEnumerable<Person> VeteranMembers()
		{
			foreach (Person member in members)
			{
				if (NumberOfPublications(member) >= 2)
					yield return member;
			}
		}

		public IEnumerable<Paper> LastYearPapers()
		{
			int currentYear = DateTime.Now.Year;
			foreach (Paper paper in papers)
			{
				if (currentYear - paper.PublicationDate.Year <= 1)
					yield return paper;
			}
		}

		private int NumberOfPublications(Person member)
		{
			int publicationsCount = 0;
			foreach (Paper paper in papers)
			{
				if (member == paper.Author)
					++publicationsCount;
			}
			return publicationsCount;
		}

		public void AddPapers(params Paper[] papers)
		{
			if (papers == null)
				throw new ArgumentNullException();

			foreach (Paper paper in papers)
				if (paper == null)
					throw new ArgumentNullException();

			this.papers.AddRange(papers);
		}

		public void AddMembers(params Person[] members)
		{
			if (members == null)
				throw new ArgumentNullException();

			foreach (Person member in members)
				if (member == null)
					throw new ArgumentNullException();

			this.members.AddRange(members);
		}

		public void SortPapersByDate()
		{
			papers.Sort();
		}

		public void SortPapersByTitle()
		{
			papers.Sort(new Paper());
		}

		public void SortPapersByAuthor()
		{
			papers.Sort(new AuthorComparer());
		}

		public static bool operator ==(ResearchTeam lhs, ResearchTeam rhs)
		{
			return Equals(lhs, rhs);
		}

		public static bool operator !=(ResearchTeam lhs, ResearchTeam rhs)
		{
			return !Equals(lhs, rhs);
		}

		public virtual string ToShortString()
		{
			return base.ToString() + $": {researchTopic} ({researchDuration})";
		}

		public bool this[TimeFrame researchDuration]
		{
			get => this.researchDuration == researchDuration;
		}

		public override string ToString()
		{
			string result = ToShortString();

			if (members.Count > 0)
			{
				result += ". ";
				for (int i = 0; i < members.Count - 1; ++i)
					result += $"{((Person)members[i]).ToShortString()}, ";
				result += $"{((Person)members[members.Count - 1]).ToShortString()}. ";
			}

			if (papers.Count == 0)
				return result;

			result += " { ";
			foreach (Paper paper in papers)
				result += $"{paper}; ";
			return result + "}";
		}

		public bool Equals(ResearchTeam team)
		{
			if (team == null)
				return false;
			if (ReferenceEquals(this, team))
				return true;

			return organizationName == team.organizationName &&
				   registrationNumber == team.registrationNumber;
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as ResearchTeam);
		}

		//Generated by Visual Studio
		public override int GetHashCode()
		{
			int hashCode = 2083773863;
			hashCode = hashCode * -1521134295 + base.GetHashCode();
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ResearchTopic);
			hashCode = hashCode * -1521134295 + ResearchDuration.GetHashCode();
			hashCode = hashCode * -1521134295 + EqualityComparer<List<Person>>.Default.GetHashCode(Members);
			hashCode = hashCode * -1521134295 + EqualityComparer<List<Paper>>.Default.GetHashCode(Papers);
			hashCode = hashCode * -1521134295 + EqualityComparer<Team>.Default.GetHashCode(Team);
			return hashCode;
		}
	}
}
