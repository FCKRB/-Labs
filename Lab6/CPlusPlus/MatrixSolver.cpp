#include "MatrixSolver.hpp"


#include <vector>
#include <chrono>
#include <iostream>
#include <cstdint>
#include <algorithm>

#include "TridiagonalMatrix.hpp"


int64_t __cdecl solve_repeatedly(int dimension, int repetition_count)
{
    using namespace std::chrono;

    if (dimension <= 0)
    {
        std::cerr << "Fatal error in solve_repeatedly: dimension is negative or equal to 0..." << std::endl;
        return -1;
    }

    if (repetition_count <= 0)
    {
        std::cerr << "Fatal error in solve_repeatedly: repeat count is negative or equal to 0..." << std::endl;
        return -1;
    }

    TridiagonalMatrix matrix(dimension);
    std::vector<double> result(dimension);
    std::vector<double> constant_terms(dimension, 5.0);

    auto start_time = high_resolution_clock::now();
    for (int i = 0; i < repetition_count; ++i)
        matrix.solve(result, constant_terms);
    auto end_time = high_resolution_clock::now();

    return static_cast<int64_t>(duration_cast<nanoseconds>(end_time - start_time).count());
}


int64_t __cdecl solve(int dimension,
                      double upper_diagonal[],
                      double main_diagonal[],
                      double lower_diagonal[],
                      double result[],
                      double constant_terms[])
{
    using namespace std::chrono;

    if (dimension <= 0)
    {
        std::cerr << "Fatal error in solve_repeatedly: dimension is negative or equal to 0..." << std::endl;
        return -1;
    }

    TridiagonalMatrix matrix(
        std::vector<double>(upper_diagonal, upper_diagonal + (dimension - 1)),
        std::vector<double>(main_diagonal, main_diagonal + dimension),
        std::vector<double>(lower_diagonal, lower_diagonal + (dimension - 1))
    );
    std::vector<double> _result(dimension);

    matrix.solve(_result, std::vector<double>(constant_terms, constant_terms + dimension));
    std::copy(_result.cbegin(), _result.cend(), result);

    return 0;
}
