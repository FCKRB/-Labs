﻿using System;
using System.Collections.Generic;

namespace Lab2
{
	class Person : INameAndCopy, IEquatable<Person>
	{
		private const string DEFAULT_NAME = "-";
		private const string DEFAULT_SURNAME = "-";

		private string name;
		private string surname;
		private DateTime birthday;

		public Person()
		{
			Name = DEFAULT_NAME;
			Surname = DEFAULT_SURNAME;
			Birthday = DateTime.Now;
		}

		public Person(string name, string surname, DateTime birthday)
		{
			Name = name;
			Surname = surname;
			Birthday = birthday;
		}

		public virtual string Name
		{
			get => name;
			set
			{
				if (value.Length == 0)
					throw new ArgumentException("zero name length");
				if (!char.IsUpper(value[0]))
					throw new ArgumentException("name must start with a capital letter");
				name = value;
			}
		}

		public string Surname
		{
			get => surname;
			set
			{
				if (value.Length == 0)
					throw new ArgumentException("zero surname length");
				if (!char.IsUpper(value[0]))
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

		public virtual object DeepCopy()
		{
			return new Person(name, surname, birthday);
		}

		public static bool operator==(Person lhs, Person rhs)
		{
			return Equals(lhs, rhs);
		}

		public static bool operator!=(Person lhs, Person rhs)
		{
			return !Equals(lhs, rhs);
		}

		virtual public string ToShortString()
		{
			return $"{name} {surname}";
		}

		public override string ToString()
		{
			return $"{ToShortString()} ({birthday.ToShortDateString()})";
		}

		public bool Equals(Person person)
		{
			if (person == null)
				return false;
			if (ReferenceEquals(this, person))
				return true;

			return name == person.name
				   && surname == person.surname
					&& birthday == person.birthday;
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as Person);
		}

		//Generated by Visual Studio
		public override int GetHashCode()
		{
			int hashCode = 1880212840;
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(name);
			hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(surname);
			hashCode = hashCode * -1521134295 + birthday.GetHashCode();
			return hashCode;
		}
	}
}