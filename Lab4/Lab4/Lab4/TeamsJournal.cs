using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4
{
	class TeamsJournal
	{
		private List<TeamsJournalEntry> entries;

		public TeamsJournal()
		{
			entries = new List<TeamsJournalEntry>();
		}

		public void HandleChange(object source, ResearchTeamsChangedEventArgs args)
		{
			entries.Add(new TeamsJournalEntry(
				args.CollectionName, args.Change, args.PropertyName,
				args.RegistrationNumber
			));
		}

		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();
			string format_string = "{0,-30}|{1,-12}|{2,-20}|{3,-12}";
			builder.AppendLine(string.Format(format_string,
				"CollectionName", "Revision", "PropertyName", "Reg. number"));
			foreach (TeamsJournalEntry entry in entries)
			{
				builder.AppendLine(string.Format(format_string,
					entry.CollectionName, entry.Change, entry.PropertyName,
					entry.RegistrationNumber));
			}
				return builder.ToString();
		}
	}
}
