using System;
using System.Collections.Generic;

namespace Lab5
{
	[Serializable]
	class Paper : INameAndCopy, IComparable, IComparable<Paper>,
		IComparer<Paper>
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

		public int CompareTo(object obj)
		{
			return CompareTo(obj as Paper);
		}

		public int CompareTo(Paper other)
		{
			if (other == null)
				return 1;
			return PublicationDate.CompareTo(other.PublicationDate);
		}

		public int Compare(Paper x, Paper y)
		{
			return x.Title.CompareTo(y.Title);
		}
	}
}
