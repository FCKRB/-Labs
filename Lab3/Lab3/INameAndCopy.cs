namespace Lab3
{
	interface INameAndCopy
	{
		string Name { get; set; }
		object DeepCopy();
	}
}
