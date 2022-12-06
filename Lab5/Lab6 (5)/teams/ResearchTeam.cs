using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

//Methods added in 5th work start with line 308.

namespace Lab5
{
	[Serializable]
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
			StringBuilder resultBuilder = new StringBuilder(ToShortString());

			if (members.Count > 0)
			{
				resultBuilder.Append(". ");
				for (int i = 0; i < members.Count - 1; ++i)
					resultBuilder.Append($"{(members[i]).ToShortString()}, ");
				resultBuilder.Append($"{(members[members.Count - 1]).ToShortString()}. ");
			}

			if (papers.Count > 0)
			{
				resultBuilder.Append(" { ");
				foreach (Paper paper in papers)
					resultBuilder.Append($"{paper}; ");
				resultBuilder.Append("}");
			}

			return resultBuilder.ToString();
		}

		public bool Equals(ResearchTeam team)
		{
			if (team == null) return false;
			if (ReferenceEquals(this, team)) return true;

			return OrganizationName == team.OrganizationName
				&& RegistrationNumber == team.RegistrationNumber;
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

		public override object DeepCopy()
		{
			using (var memoryStream = new MemoryStream())
			{
				IFormatter formatter = new BinaryFormatter();
				formatter.Serialize(memoryStream, this);
				memoryStream.Position = 0;
				return formatter.Deserialize(memoryStream);
			}
		}

		public bool Save(string filename)
		{
            FileStream fileStream = File.Open(filename, FileMode.OpenOrCreate);
			try
			{
				var formatter = new BinaryFormatter();
				formatter.Serialize(fileStream, this);
				return true;
			}
			catch (Exception e)
			{
				Console.Error.WriteLine(e.Message);
				return false;
			}
			finally { fileStream.Close(); }
		}

		public bool Load(string filename)
		{
			if (!File.Exists(filename))
				return false;

			FileStream fileStream = null;
			try
			{
				fileStream = File.Open(filename, FileMode.Open);
				var formatter = new BinaryFormatter();
				ResearchTeam copy = (ResearchTeam) formatter.Deserialize(fileStream);

				OrganizationName = copy.OrganizationName;
				RegistrationNumber = copy.RegistrationNumber;

				ResearchTopic = copy.ResearchTopic;
				ResearchDuration = copy.researchDuration;
				Members = copy.Members;
				Papers = copy.Papers;

				return true;
			}
			catch (Exception e)
			{
				Console.Error.WriteLine(e.Message);
				return false;
			}
			finally { if (!fileStream.Equals(null)) fileStream.Close(); }
		}

		public bool AddPaperFromConsole()
		{
			Console.WriteLine("Введите публикацию (<название> <автор: <имя> <фамилия> <дата рождения: число.месяц.год>> <дата публикации: число.месяц.год>).");

			char[] delimiters = new char[] { ' ', '-', '_' };
			Console.Write("Разделяйте значения следующими символами: ");
			foreach (char delimiter in delimiters)
				Console.Write($"'{delimiter}' ");
			Console.WriteLine(".");

			var cultureInfo = new CultureInfo("ru-RU");
			string[] consoleArgs = Console.ReadLine().Split();
			if (consoleArgs.Length < 5)
            {
				Console.WriteLine("Ошибка: некорретный ввод.");
				return false;
			}

			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < consoleArgs.Length - 4; ++i)
			{
				stringBuilder.Append(consoleArgs[i]);
				stringBuilder.Append(' ');
			}

			string title = stringBuilder.ToString();
			string name = consoleArgs[consoleArgs.Length - 4];
			string surname = consoleArgs[consoleArgs.Length - 3];
			DateTime birthday;
			DateTime publicationDate;
			try
			{
				birthday = DateTime.Parse(consoleArgs[consoleArgs.Length - 2], cultureInfo);
				publicationDate = DateTime.Parse(consoleArgs[consoleArgs.Length - 1], cultureInfo);
			}
			catch (Exception e) when (e is ArgumentNullException || e is FormatException)
			{
				Console.WriteLine("Ошибка: некорректная дата.");
				return false;
			}

			Person author;
			try { author = new Person(name, surname, birthday); }
			catch (Exception)
            {
				Console.WriteLine("Ошибка: некорректно введены данные автора...");
				return false;
			}

			try
			{
				Paper paper = new Paper(title, author, publicationDate);
				papers.Add(paper);

				return true;
			}
			catch (Exception)
			{
				Console.WriteLine("Ошибка: некорректный ввод");
				return false;
			}
		}

		public static bool Save(string filename, ResearchTeam team)
		{
			return team.Save(filename);
		}

		public static bool Load(string filename, ResearchTeam team)
		{
			return team.Load(filename);
		}
	}
}
