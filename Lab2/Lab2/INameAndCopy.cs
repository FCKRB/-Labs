namespace Lab2
{
	interface INameAndCopy
	{
		string Name { get; set; }
		object DeepCopy();
	}
}
