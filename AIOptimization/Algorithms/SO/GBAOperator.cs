using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AIOptimization.Random;
using AIOptimization.Problem;
using AIOptimization.Population; 
namespace AIOptimization.Algorithms.SO
{
    public class GBAOperator:SearchOperator
    {
        protected double[][] neighbourhood;
        public int group_index;

        public void setNeighbourhoodSize(double[][] neighbourhood, int m, int n)
        {
            this.neighbourhood = new double[m][];
            for(int i=0; i<m; i++)
            {
                this.neighbourhood[i] = new double[n];
                for (int j = 0; j < n; j++)
                {
                    this.neighbourhood[i][j] = neighbourhood[i][j];
                }
            }
        }

        public override Solution search(RandomGenerator rGen, Problem.Problem problem, IList<Solution> population, int popSize, int index)
        {
            Solution sol = population[index].copy();
            for (int i = 0; i < problem.paramsCnt; i++)
            {
                sol.parameters[i] = (sol.parameters[i] - neighbourhood[group_index][i])
                                     + 2 * neighbourhood[group_index][i] * rGen.rnd(1, problem.paramsCnt);
            }
            sol.constraint(problem);
            return sol;
        }
    }
}
