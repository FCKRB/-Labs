using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4
{
	class AuthorComparer : IComparer<Paper>
	{
		public int Compare(Paper x, Paper y)
		{
			int result = x.Author.Surname.CompareTo(y.Author.Surname);
			if (result == 0)
				result = x.Author.Name.CompareTo(y.Author.Name);
			return result;
		}
	}
}
