using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIOptimization.Population;
using AIOptimization.Problem;

namespace AIOptimization.Algorithms
{
    public abstract class Evaluator
    {
        public abstract void evaluate(Solution solution, IList<Solution> population,Problem.Problem problem);
    }
}
 