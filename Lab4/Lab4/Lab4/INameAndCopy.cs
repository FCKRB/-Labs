namespace Lab4
{
	interface INameAndCopy
	{
		string Name { get; set; }
		object DeepCopy();
	}
}
