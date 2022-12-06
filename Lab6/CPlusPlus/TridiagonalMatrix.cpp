#include "TridiagonalMatrix.hpp"


#include <stdexcept>
#include <vector>
#include <utility>


TridiagonalMatrix::TridiagonalMatrix(size_t size)
    :upper_diagonal(size), main_diagonal(size), lower_diagonal(size)
{
    for (size_t i = 1; i < size; i++)
    {
        upper_diagonal[i] = static_cast<double>(i);
        lower_diagonal[i] = static_cast<double>(i) - 2.0;
        main_diagonal[i] = static_cast<double>(i*i) * main_diagonal[i];
    }
}


TridiagonalMatrix::TridiagonalMatrix(std::vector<double> upper_diagonal,
                                     std::vector<double> main_diagonal,
                                     std::vector<double> lower_diagonal)
{
    int upper_diagonal_size = upper_diagonal.size();

    if (upper_diagonal_size != lower_diagonal.size())
        throw std::logic_error("upper and lower diagonals' size does not match");
    if (upper_diagonal_size != main_diagonal.size() - 1)
        throw std::logic_error("upper and main diagonals' size does not match");

    this->upper_diagonal = std::move(upper_diagonal);
    this->main_diagonal = std::move(main_diagonal);
    this->lower_diagonal = std::move(lower_diagonal);
}


void TridiagonalMatrix::solve(std::vector<double>& result,
                        const std::vector<double>& constant_terms) const noexcept
{
    int n = result.size();
    std::vector<double> b(n - 1);
    std::vector<double> g(n);

    b[0] = upper_diagonal[0] / main_diagonal[0];
    g[0] = constant_terms[0] / main_diagonal[0];

    for (int i = 1; i < n - 1; ++i)
    {
        b[i] = upper_diagonal[i] / (main_diagonal[i] - b[i - 1]*lower_diagonal[i - 1]);
        g[i] = (constant_terms[i] - lower_diagonal[i - 1]*g[i - 1])
            / (main_diagonal[i] - b[i - 1]*lower_diagonal[i - 1]);
    }

    result[n - 1] = g[n - 1] = (constant_terms[n - 1] - lower_diagonal[n - 2]*g[n - 2])
            / (main_diagonal[n - 1] - b[n - 2]*lower_diagonal[n - 2]);

    for (int i = n - 2; i >= 0; --i)
        result[i] = g[i] - b[i]*result[i + 1];
}
