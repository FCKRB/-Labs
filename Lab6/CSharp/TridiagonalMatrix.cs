using System;
using System.Linq;
using System.Text;

namespace Lab5
{
    class TridiagonalMatrix
    {
        private readonly double[] upperDiagonal;
        private readonly double[] mainDiagonal;
        private readonly double[] lowerDiagonal;

        public TridiagonalMatrix(int dimension)
        {
            upperDiagonal = new double[dimension - 1];
            mainDiagonal = new double[dimension];
            lowerDiagonal = new double[dimension - 1];

            for (int i = 0; i < dimension - 1; ++i)
            {
                upperDiagonal[i] = 1E3;
                mainDiagonal[i] = 1E15;
                lowerDiagonal[i] = 1E3;
            }
            mainDiagonal[dimension - 1] = 1E15;
        }

        public TridiagonalMatrix(double[] upperDiagonal, double[] mainDiagonal, double[] lowerDiagonal)
        {
            int upperDiagonalDimension = upperDiagonal.Length;
            if (upperDiagonalDimension != lowerDiagonal.Length)
                throw new ArgumentException("upper and lower diagonals' size does not match");
            if (upperDiagonalDimension != mainDiagonal.Length - 1)
                throw new ArgumentException("upper and main diagonals' size does not match");

            this.upperDiagonal = upperDiagonal;
            this.mainDiagonal = mainDiagonal;
            this.lowerDiagonal = lowerDiagonal;
        }

        public void Solve(double[] result, double[] constantTerms)
        {
            int n = result.Length;
            double[] b = new double[n - 1];
            double[] g = new double[n];

            b[0] = upperDiagonal[0] / mainDiagonal[0];
            g[0] = constantTerms[0] / mainDiagonal[0];

            for (int i = 1; i < n - 1; ++i)
            {
                b[i] = upperDiagonal[i] / (mainDiagonal[i] - b[i - 1] * lowerDiagonal[i - 1]);
                g[i] = (constantTerms[i] - lowerDiagonal[i - 1] * g[i - 1])
                    / (mainDiagonal[i] - b[i - 1] * lowerDiagonal[i - 1]);
            }

            result[n - 1] = g[n - 1] = (constantTerms[n - 1] - lowerDiagonal[n - 2] * g[n - 2])
                    / (mainDiagonal[n - 1] - b[n - 2] * lowerDiagonal[n - 2]);

            for (int i = n - 2; i >= 0; --i)
                result[i] = g[i] - b[i] * result[i + 1];
        }

        public string ToString(double[] constantTerms)
        {
            if (constantTerms.Length != mainDiagonal.Length)
                throw new ArgumentException("constant terms and matrix dimensions do not match");

            StringBuilder stringBuilder = new StringBuilder();
            int dimension = mainDiagonal.Count();
            const string number_formatting = "{0:+0.000E+000;-0.000E+000;}";

            stringBuilder.Append(string.Format(number_formatting + " ", mainDiagonal[0]));
            stringBuilder.Append(string.Format(number_formatting + " ", lowerDiagonal[0]));

            for (int i = 2; i < dimension; ++i)
                stringBuilder.Append(string.Format(number_formatting + " ", 0));

            stringBuilder.AppendLine(string.Format("| " + number_formatting, constantTerms[0]));

            for (int i = 1; i < dimension - 1; ++i)
            {
                for (int j = 0; j < i - 1; ++j)
                    stringBuilder.Append(string.Format(number_formatting + " ", 0));

                stringBuilder.Append(string.Format(number_formatting + " ", upperDiagonal[i - 1]));
                stringBuilder.Append(string.Format(number_formatting + " ", mainDiagonal[i]));
                stringBuilder.Append(string.Format(number_formatting + " ", lowerDiagonal[i]));

                for (int j = (i - 1) + 3; j < dimension; ++j)
                    stringBuilder.Append(string.Format(number_formatting + " ", 0));

                stringBuilder.AppendLine(string.Format("| " + number_formatting, constantTerms[i]));
            }

            for (int j = 0; j < dimension - 2; ++j)
                stringBuilder.Append(string.Format(number_formatting + " ", 0));

            stringBuilder.Append(string.Format(number_formatting + " ", upperDiagonal[dimension - 2]));
            stringBuilder.Append(string.Format(number_formatting + " ", mainDiagonal[dimension - 1]));
            stringBuilder.Append(string.Format("| " + number_formatting, constantTerms[dimension - 1]));

            return stringBuilder.ToString();
        }
    }
}
