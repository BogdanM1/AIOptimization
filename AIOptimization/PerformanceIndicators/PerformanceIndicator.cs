using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIOptimization.Population;
using AIOptimization.Problem;

namespace AIOptimization.PerformanceIndicators
{
    public abstract class PerformanceIndicator
    {

        public abstract double calculate(IList<Solution> population, int populationSize ,Problem.Problem problem);

    }
}
 