using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIOptimization.Random;
using AIOptimization.Population;
using AIOptimization.Problem;
 

namespace AIOptimization.Algorithms.SO
{
    public class ABCDEOperator:SearchOperator
    {
        
        public double crossOverProbab = 0.75;

        void DEcrossover(RandomGenerator rGen, ref Solution sol1, Solution sol2, Problem.Problem problem)
        {
            int randomDimension = rGen.rnd(problem.paramsCnt - 1);
            for (int i = 0; i < problem.paramsCnt; i++)
            {
                if (rGen.rnd() >= crossOverProbab && i != randomDimension)
                {
                    sol1.parameters[i] = sol2.parameters[i];
                }
            }        
        }


        public override Solution search(RandomGenerator rGen, Problem.Problem problem, IList<Solution> population, int popSize, int index)
        {
            int baseSolutionIndex = rGen.rndExcl(popSize - 1, index);
            Solution sol = population[baseSolutionIndex].copy();
            double F = rGen.rnd(0.5, 1.0);
            List<int> excluding = new List<int>();
            excluding.Add(index);
            excluding.Add(baseSolutionIndex);
            excluding.Sort();
            int rnd1 = rGen.rndExcl(popSize - 1, excluding);
            excluding.Add(rnd1);
            excluding.Sort();
            int rnd2 = rGen.rndExcl(popSize - 1, excluding);
            for (int i = 0; i < problem.paramsCnt; i++)
            {
                sol.parameters[i] += F * (population[rnd1].parameters[i] - population[rnd2].parameters[i]); 
            }
            DEcrossover(rGen, ref sol, population[index], problem);
            sol.constraint(problem);
            return sol;
        }
    }
}
