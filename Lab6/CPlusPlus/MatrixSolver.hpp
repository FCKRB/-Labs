#pragma once


#include <cstdint>


extern "C"
{
    //Returns nanoseconds time of specified dimension matrix solvation
    //for parameter number of times.
    //
    //Returns negative result and puts error message to cerr if fatal error occurs.
    __declspec(dllexport) int64_t __cdecl solve_repeatedly(int dimension,
                                                           int repeat_count);

    //Returns nanoseconds solvation time of matrix constructed from parameters.
    //
    //Returns negative result and puts error message to cerr if fatal error occurs.
    __declspec(dllexport) int64_t __cdecl solve(int dimension,
                                                double upper_diagonal[],
                                                double main_diagonal[],
                                                double lower_diagonal[],
                                                double result[],
                                                double constant_terms[]);
};
