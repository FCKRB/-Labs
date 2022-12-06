#pragma once

#include <vector>


class TridiagonalMatrix
{
private:
    std::vector<double> upper_diagonal;
    std::vector<double> main_diagonal;
    std::vector<double> lower_diagonal;

public:
    TridiagonalMatrix(size_t size=100U);

    TridiagonalMatrix(std::vector<double> upper_diagonal,
                      std::vector<double> main_diagonal,
                      std::vector<double> lower_diagonal);

    void solve(std::vector<double>& result,
         const std::vector<double>& constant_terms) const noexcept;
};
