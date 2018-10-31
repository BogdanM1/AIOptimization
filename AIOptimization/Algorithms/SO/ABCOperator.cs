using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIOptimization.Population;
using AIOptimization.Random; 

namespace AIOptimization.Algorithms.SO
{
    public class ABCOperator:SearchOperator
    {
        public override Solution search(RandomGenerator rGen, Problem.Problem problem, IList<Solution> population, int popSize, int index)
        {
            Solution sol = population[index].copy();
            int neighIndex = rGen.rndExcl(popSize-1, index);
            Solution neighbour = population[neighIndex];
            int dimension = rGen.rnd(sol.paramCount - 1);
            sol.parameters[dimension] += rGen.rnd(-1.0, 1.0) *
                                        (sol.parameters[dimension] - neighbour.parameters[dimension]);
            sol.constraint(problem, dimension);
            return sol;
        }
    }
}
