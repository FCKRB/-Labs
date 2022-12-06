using System;
using System.Collections;
using System.Collections.Generic;

namespace Lab3
{
	class ResearchTeamEnumerator : IEnumerator<Person>
	{
		private Person[] members;
		private int position = -1;

		public ResearchTeamEnumerator(Person[] members)
		{
			this.members = members ?? throw new ArgumentNullException();
		}

		public Person Current
		{
			get
			{
				if (position < 0 || position >= members.Length)
					throw new InvalidOperationException();
				return members[position];
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return Current;
			}
		}

		public void Dispose()
		{
			position = -2;
			members = null;
		}

		public bool MoveNext()
		{
			++position;
			return position < members.Length;
		}

		public void Reset()
		{
			position = -1;
		}
	}
}
