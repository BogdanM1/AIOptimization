using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIOptimization.Population;
using AIOptimization.Random;
using AIOptimization.Problem;

namespace AIOptimization.Algorithms
{
    public abstract class SearchOperator
    {
        public abstract Solution search(RandomGenerator rGen, Problem.Problem problem, IList<Solution> population, int popSize, int index);
        
    }
}
 