using System;

namespace Lab2
{
	class Paper : INameAndCopy
	{
		private const string DEFAULT_TITLE = "No title";

		public string Title { get; set; }
		public Person Author { get; set; }
		public DateTime PublicationDate { get; set; }

		public Paper()
		{
			Title = DEFAULT_TITLE;
			Author = new Person();
			PublicationDate = DateTime.Now;
		}

		public Paper(string title, Person author, DateTime publicationDate)
		{
			Title = title;
			Author = author;
			PublicationDate = publicationDate;
		}

		public virtual string Name
		{
			get => Title;
			set => Title = value;
		}

		public virtual object DeepCopy()
		{
			return new Paper(Title, (Person)Author.DeepCopy(), PublicationDate);
		}

		public override string ToString()
		{
			return $"{Author.ToShortString()}. {Title}, " +
				PublicationDate.ToShortDateString();
		}
	}
}
