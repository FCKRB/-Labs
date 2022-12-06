using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lab5
{
    [Serializable]
    class TimeItem
    {
        private int matrixDimension;
        private int repetitionCount;

        public TimeSpan CppExecutionTime { get; set; }
        public TimeSpan CsExecutionTime { get; set; }

        public int MatrixDimension
        {
            get { return matrixDimension; }
            set
            {
                if (value < 0) throw new ArgumentException("negative matrix dimension");
                matrixDimension = value;
            }
        }

        public int RepetitionCount
        {
            get { return repetitionCount; }
            set
            {
                if (value < 0) throw new ArgumentException("negative repetition count");
                repetitionCount = value;
            }
        }

        public double CsToCppExecutionTime
        {
            get { return (double)CsExecutionTime.Ticks / CppExecutionTime.Ticks; }
        }

        public TimeItem(int matrixDimension,
                        int repetitionCount,
                        TimeSpan cppExecutionTime,
                        TimeSpan csExecutionTime)
        {
            MatrixDimension = matrixDimension;
            RepetitionCount = repetitionCount;
            CppExecutionTime = cppExecutionTime;
            CsExecutionTime = csExecutionTime;
        }

        public override string ToString()
        {
            return $"{nameof(TimeItem)}({MatrixDimension}, {RepetitionCount})[{CsToCppExecutionTime}]";
        }
    }
}
