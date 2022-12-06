using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab4
{
	class ResearchTeamsChangedEventArgs : EventArgs
	{
		public enum Revision
		{
			Remove, Replace, Property
		};

		public string CollectionName { get; set; }
		public Revision Change { get; set; }
		public string PropertyName { get; set; }

		public int RegistrationNumber { get; set; }

		public ResearchTeamsChangedEventArgs(string collectionName, Revision change,
			string propertyName, int registrationNumber)
		{
			CollectionName = collectionName;
			Change = change;
			PropertyName = propertyName;
			RegistrationNumber = registrationNumber;
		}

		public override string ToString()
		{
			return $"ResearchTeamsChanged({CollectionName}, {Change}, "
				+ $"{PropertyName}, {RegistrationNumber}";
		}
	}
}
