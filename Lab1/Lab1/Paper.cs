using System;

namespace Lab1
{
	class Paper
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

		public override string ToString()
		{
			return Author.ToShortString() + ". " + Title + ", "
				+ PublicationDate.ToShortDateString();
		}
	}
}
