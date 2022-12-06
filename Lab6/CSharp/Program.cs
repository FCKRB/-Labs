using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace Lab5
{
    class Program
    {
        [DllImport("CPlusPlus.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern long solve_repeatedly(int dimension, int repetitionCount);

        [DllImport("CPlusPlus.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern long solve(int dimension,
                                         double[] upperDiagonal,
                                         double[] mainDiagonal,
                                         double[] lowerDiagonal,
                                         double[] result,
                                         double[] constantTerms);

        static void Main(string[] args)
        {
            {
                const int DIMENSION = 3;
                double[] upperDiagonal = new double[DIMENSION - 1] { 5.33, 7.74, };
                double[] mainDiagonal = new double[DIMENSION] { 100, 101, 100, };
                double[] lowerDiagonal = new double[DIMENSION - 1] { -2.32, 1.41, };
                double[] constantTerms = new double[DIMENSION] { 15.21, 11.11, 0.81, };
                double[] result = new double[DIMENSION];

                TridiagonalMatrix matrix = new TridiagonalMatrix(upperDiagonal, mainDiagonal, lowerDiagonal);

                Console.WriteLine("1. Проверить, что метод решения системы линейных уравнений работает правильно.");
                Console.WriteLine("Исходная матрица: ");
                Console.WriteLine(matrix.ToString(constantTerms));

                matrix.Solve(result, constantTerms);

                Console.Write("Решение: ");
                for (int i = 0; i < DIMENSION; ++i)
                    Console.Write(string.Format("{0:+000.00;-000.00} ", result[i]));
                Console.WriteLine();
                Console.WriteLine();

                Console.WriteLine($"2. Передать в код C++ данные, определяющие матрицу и правую часть. Вывести полученное решение.");
                solve(DIMENSION, upperDiagonal, mainDiagonal, lowerDiagonal, result, constantTerms);
                Console.Write("Решение: ");
                for (int i = 0; i < DIMENSION; ++i)
                    Console.Write(string.Format("{0:+000.00;-000.00} ", result[i]));
                Console.WriteLine();
                Console.WriteLine();
            }

            Console.Write("3. Введите имя файла с данными типа TimeItem: ");

            string filename = Console.ReadLine();
            while (filename.Length == 0)
            {
                Console.WriteLine("Ошибка: ничего не введено");
                Console.WriteLine("Введите имя файла с данными типа TimeItem: ");
                filename = Console.ReadLine();
            }

            TimesList timesList = new TimesList();
            if (!File.Exists(filename))
            {
                Console.WriteLine("Файла не существует. Он будет создан.");
                try
                {
                    using (File.Create(filename))
                    {
                        Console.WriteLine("Файл успешно создан");
                    }
                }
                catch (IOException e)
                {
                    Console.WriteLine("Ошибка: невозможно создать файл...");
                    Console.WriteLine("Причина: " + e.Message);
                }
            }
            else
            {
                try
                {
                    TimesList.LoadFromFile(filename, timesList);
                    Console.WriteLine("Данные успешно загружены...");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Ошибка: невозможно загрузить данные из файла...");
                    Console.WriteLine("Причина: " + e.Message);
                }
            }            

            Console.WriteLine("Текущие значения: ");
            Console.WriteLine(timesList.TableString());
            Console.WriteLine();

            Console.WriteLine("4. Приглашение ввести порядок матрицы и число повторов или завершить работу приложения.");
            while (true)
            {
                Console.WriteLine("Для создания новой записи, введите через пробел порядок матрицы и число повторов.");
                Console.WriteLine("Чтобы завершить работу приложения, введите exit или quit.");

                string command = Console.ReadLine();
                if (command.Equals("quit") || command.Equals("exit"))
                {
                    Console.WriteLine();
                    break;
                }

                string[] parameters = command.Split();
                if (parameters.Length != 2)
                {
                    Console.WriteLine("Ошибка: некорректный ввод...");
                    Console.WriteLine();
                    continue;
                }

                if (!int.TryParse(parameters[0], out int dimension))
                {
                    Console.WriteLine($"Ошибка: некорректно введён порядок матрицы: {parameters[0]}...");
                    Console.WriteLine();
                    continue;
                }
                if (dimension <= 0)
                {
                    Console.WriteLine($"Ошибка: порядок матрицы меньше или равен нулю...");
                    Console.WriteLine();
                    continue;
                }

                if (!int.TryParse(parameters[1], out int repetitionCount))
                {
                    Console.WriteLine($"Ошибка: некорректно введено число повторов: {parameters[1]}...");
                    Console.WriteLine();
                    continue;
                }
                if (repetitionCount <= 0)
                {
                    Console.WriteLine($"Ошибка: число повторов меньше или равно нулю...");
                    Console.WriteLine();
                    continue;
                }

                try
                {
                    TridiagonalMatrix matrix = new TridiagonalMatrix(dimension);
                    double[] result = new double[dimension];
                    double[] constantTerms = new double[dimension];
                    for (int i = 0; i < dimension; ++i)
                        constantTerms[i] = 15.5;

                    Stopwatch watch = new Stopwatch();
                    watch.Start();
                    for (int i = 0; i < repetitionCount; ++i)
                        matrix.Solve(result, constantTerms);
                    watch.Stop();
                    TimeSpan cSharpSolvationTime = new TimeSpan(watch.ElapsedTicks);

                    long cppResultNanoseconds = solve_repeatedly(dimension, repetitionCount);
                    if (cppResultNanoseconds < 0)
                    {
                        Console.WriteLine("Ошибка: импортированная функция вернула код ошибки...");
                        Console.WriteLine();
                        continue;
                    }

                    long nanosecPerTick = 1_000_000_000L / Stopwatch.Frequency;
                    TimeSpan cPlusPlusSolvationTime = new TimeSpan(cppResultNanoseconds / nanosecPerTick);

                    timesList.Add(new TimeItem(dimension, repetitionCount,
                        cPlusPlusSolvationTime, cSharpSolvationTime));
                    Console.WriteLine("Запись успешно добавлена...");
                }
                catch (OutOfMemoryException)
                {
                    Console.WriteLine("Ошибка: слишком большой порядок. Невозможно создать матрицу...");
                }
                Console.WriteLine();
            }

            Console.WriteLine("Текущие значения: ");
            Console.WriteLine(timesList.TableString());
            Console.WriteLine();

            try
            {
                TimesList.SaveToFile(filename, timesList);
                Console.WriteLine("Данные успешно сохранены...");
            }
            catch (Exception e)
            {
                Console.WriteLine("Ошибка: невозможно сохранить данные в файл...");
                Console.WriteLine("Причина: " + e.Message);
            }

            Console.WriteLine("Для продолжения нажмите любую клавишу...");
            Console.ReadKey();
        }
    }
}
