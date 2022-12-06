using System;

namespace Lab1
{
	class ResearchTeam
	{
		public enum TimeFrame { Undefined, Year, TwoYears, Long };

		private const string DEFAULT_RESEARCH_TOPIC = "No research topic";
		private const string DEFAULT_ORGANIZATION_NAME = "No organization name";
		private const int DEFAULT_REGISTRATION_NUMBER = -1;

		private string researchTopic;
		private string organizationName;
		private int registrationNumber;
		private TimeFrame researchDuration;
		private Paper[] papers;

		public ResearchTeam()
		{
			researchTopic = DEFAULT_RESEARCH_TOPIC;
			organizationName = DEFAULT_ORGANIZATION_NAME;
			registrationNumber = DEFAULT_REGISTRATION_NUMBER;
			researchDuration = default;
			papers = new Paper[0];
		}

		public ResearchTeam(string researchTopic, string organizationName,
			int registrationNumber, TimeFrame researchDuration)
		{
			this.researchTopic = researchTopic;
			this.organizationName = organizationName;
			this.registrationNumber = registrationNumber;
			this.researchDuration = researchDuration;

			papers = new Paper[0];
		}

		public string ResearchTopic
		{
			get => researchTopic;
			set
			{
				if (value.Length == 0)
					throw new ArgumentException("zero research topic length");
				researchTopic = value;
			}
		}

		public string OrganizationName
		{
			get => organizationName;
			set
			{
				if (value.Length == 0)
					throw new ArgumentException("zero organization name length");
				organizationName = value;
			}
		}

		public int RegistrationNumber
		{
			get => registrationNumber;
			set => registrationNumber = value;
		}

		public TimeFrame ResearchDuration
		{
			get => researchDuration;
			set => researchDuration = value;
		}

		public Paper[] Papers
		{
			get => papers;
			set
			{
				if (value == null)
					throw new ArgumentNullException();
				papers = value;
			}
		}

		public Paper LatestPaper
		{
			get
			{
				if (papers.Length == 0)
					return null;

				Paper latestPaper = papers[0];
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

		public bool this[TimeFrame researchDuration]
		{
			get => this.researchDuration == researchDuration;
		}

		public void AddPapers(params Paper[] papers)
		{
			if (papers.Length == 0)
				return;
			int oldLength = this.papers.Length;
			Array.Resize(ref this.papers, this.papers.Length + papers.Length);
			Array.Copy(papers, 0, this.papers, oldLength, papers.Length);
		}

		public override string ToString()
		{
			string result = ToShortString();
			if (papers.Length == 0)
				return result;

			result += " { ";
			foreach (Paper paper in papers)
				result += paper.ToString() + "; ";
			return result + "}";
		}

		virtual public string ToShortString()
		{
			return organizationName + "[№" + registrationNumber + "]: "
				+ researchTopic + " - " + researchDuration;
		}
	}
}
