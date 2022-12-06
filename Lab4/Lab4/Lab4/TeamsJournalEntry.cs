using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Lab4.ResearchTeamsChangedEventArgs;

namespace Lab4
{
	class TeamsJournalEntry
	{
		public string CollectionName { get; set; }
		public Revision Change { get; set; }
		public string PropertyName { get; set; }

		public int RegistrationNumber { get; set; }

		public TeamsJournalEntry(string collectionName, Revision change,
			string propertyName, int registrationNumber)
		{
			CollectionName = collectionName;
			Change = change;
			PropertyName = propertyName;
			RegistrationNumber = registrationNumber;
		}

		public override string ToString()
		{
			return $"TeamsJournalEntry({CollectionName}, {Change}, "
				+ $"{PropertyName}, {RegistrationNumber})";
		}
	}
}
