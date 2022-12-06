using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Text;
using System.Diagnostics;

namespace Lab5
{
    class TimesList : IEnumerable<TimeItem>
    {
        private List<TimeItem> timesList;

        public TimesList()
        {
            timesList = new List<TimeItem>();
        }

        public void Add(params TimeItem[] timeItem)
        {
            if (timeItem is null) throw new NullReferenceException("parameter is null");
            if (timeItem.Any((_timeItem) => _timeItem is null))
                throw new ArgumentException("item is null");
            timesList.AddRange(timeItem);
        }

        public static void SaveToFile(string filename, TimesList source)
        {
            using (Stream stream = new FileStream(filename, FileMode.Create, FileAccess.Write))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, source.timesList);
            }
        }

        public static void LoadFromFile(string filename, TimesList target)
        {
            using (Stream stream = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                var formatter = new BinaryFormatter();
                target.timesList = (List<TimeItem>)formatter.Deserialize(stream);
            }
        }

        public IEnumerator<TimeItem> GetEnumerator()
        {
            return timesList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public string TableString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            string rowFormatting = "|{0,-8}|{1,-16}|{2,-16}|{3,-11:000000}|{4,-12:000000}|{5,-8:0.000E+0}|";
            string header = string.Format(rowFormatting, "TimeItem", "Matrix dimension",
                "Repetition count", "C# time, ns", "C++ time, ns", "C#/C++");

            stringBuilder.AppendLine(new string('_', header.Length));
            stringBuilder.AppendFormat(header);
            stringBuilder.AppendLine();

            int itemIndex = 0;
            foreach (TimeItem item in timesList)
            {
                long nanosecondsPerTick = 1_000_000_000L / Stopwatch.Frequency;
                long csTime = item.CsExecutionTime.Ticks * nanosecondsPerTick;
                long cppTime = item.CppExecutionTime.Ticks * nanosecondsPerTick;

                stringBuilder.AppendFormat(rowFormatting, itemIndex, item.MatrixDimension,
                    item.RepetitionCount, csTime, cppTime, item.CsToCppExecutionTime);
                stringBuilder.AppendLine();
                itemIndex += 1;
            }

            stringBuilder.Append(new string('-', header.Length));

            return stringBuilder.ToString();
        }
    }
}
