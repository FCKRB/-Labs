using System;

namespace Lab1
{
	class Person
	{
		private const string DEFAULT_NAME = "-";
		private const string DEFAULT_SURNAME = "-";

		private string name;
		private string surname;
		private DateTime birthday;

		public Person()
		{
			name = DEFAULT_NAME;
			surname = DEFAULT_SURNAME;
			birthday = DateTime.Now;
		}

		public Person(string name, string surname, DateTime birthday)
		{
			this.name = name;
			this.surname = surname;
			this.birthday = birthday;
		}

		public string Name
		{
			get => name;
			set
			{
				if (value.Length > 0)
					throw new ArgumentException("zero name length");
				if (Char.IsUpper(value[0]))
					throw new ArgumentException("name must start with a capital letter");
				name = value;
			}
		}

		public string Surname
		{
			get => surname;
			set
			{
				if (value.Length > 0)
					throw new ArgumentException("zero surname length");
				if (Char.IsUpper(value[0]))
					throw new ArgumentException("surname must start with a capital letter");
				surname = value;
			}
		}

		public DateTime Birthday
		{
			get => birthday;
			set
			{
				if (birthday > DateTime.Now)
					throw new ArgumentException("future date");
				birthday = value;
			}
		}

		public int BirthYear
		{
			get => birthday.Year;
			set
			{
				if (value > DateTime.Now.Year)
					throw new ArgumentException("future date");
				birthday = new DateTime(value, birthday.Month, birthday.Day);
			}
		}

		public override string ToString()
		{
			return ToShortString() + " (" + birthday.ToShortDateString() + ")";
		}

		virtual public string ToShortString()
		{
			return name + " " + surname;
		}
	}
}
