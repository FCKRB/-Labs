using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3
{
	delegate KeyValuePair<TKey, TValue> ElementGenerator<TKey, TValue>(
		int param);

	class TestCollections<TKey, TValue>
	{
		private List<TKey> keyList;
		private List<string> stringList;
		private Dictionary<TKey, TValue> keyDictionary;
		private Dictionary<string, TValue> stringDictionary;
		private ElementGenerator<TKey, TValue> elementGenerator;

		public TestCollections(int elementsCount,
			ElementGenerator<TKey, TValue> elementGenerator)
		{
			if (elementsCount <= 0)
				throw new ArgumentException("elements quantity must be positive");

			this.elementGenerator = elementGenerator;
			keyList = new List<TKey>(elementsCount);
			stringList = new List<string>(elementsCount);
			keyDictionary = new Dictionary<TKey, TValue>(elementsCount);
			stringDictionary = new Dictionary<string, TValue>(elementsCount);
			for (int i = 0; i < elementsCount; ++i)
			{
				KeyValuePair<TKey, TValue> pair = elementGenerator(i);
				keyList.Add(pair.Key);
				stringList.Add(pair.Key.ToString());
				keyDictionary.Add(pair.Key, pair.Value);
				stringDictionary.Add(pair.Key.ToString(), pair.Value);
			}
		}

		private void test(TKey key, TValue value)
		{
			Stopwatch watch = new Stopwatch();

			watch.Start();
			keyList.Contains(key);
			watch.Stop();
			Console.WriteLine($"Key list: {watch.ElapsedMilliseconds}ms");

			string stringKey = key.ToString();
			watch.Start();
			stringList.Contains(stringKey);
			watch.Stop();
			Console.WriteLine($"String list: {watch.ElapsedMilliseconds}ms");

			watch.Start();
			keyDictionary.ContainsKey(key);
			watch.Stop();
			Console.WriteLine($"Key dictionary: key {watch.ElapsedMilliseconds}ms");

			watch.Start();
			keyDictionary.ContainsValue(value);
			watch.Stop();
			Console.WriteLine($"\tvalue {watch.ElapsedMilliseconds}ms");

			watch.Start();
			stringDictionary.ContainsKey(stringKey);
			watch.Stop();
			Console.WriteLine($"String dictionary: key {watch.ElapsedMilliseconds}ms");

			watch.Start();
			stringDictionary.ContainsValue(value);
			watch.Stop();
			Console.WriteLine($"\tvalue {watch.ElapsedMilliseconds}ms");
		}

		public void testFirst()
		{
			TKey key = keyList.First();
			TValue value = keyDictionary[key];
			test(key, value);
		}

		public void testLast()
		{
			TKey key = keyList.Last();
			TValue value = keyDictionary[key];
			test(key, value);
		}

		public void testCentral()
		{
			TKey key = keyList[keyList.Count / 2];
			TValue value = keyDictionary[key];
			test(key, value);
		}

		public void testAbsent()
		{
			KeyValuePair<TKey, TValue> absentPair =
				elementGenerator(keyList.Count + 1);
			test(absentPair.Key, absentPair.Value);
		}
	}
}
