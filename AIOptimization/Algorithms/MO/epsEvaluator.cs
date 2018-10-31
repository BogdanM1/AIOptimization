using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIOptimization.Problem;
using AIOptimization.Population;
using AIOptimization.PerformanceIndicators.MO;

namespace AIOptimization.Algorithms.MO
{
    public class epsEvaluator:Evaluator
    { 
        public override void evaluate(Solution solution, IList<Solution> population, Problem.Problem problem)
        {
            int popSize = population.Count();
            solution.objectivesValues = problem.calculateObjectives(solution.parameters);
            double val = 0;
            val = - AdditiveEpsilon.calculateEpsilon(solution.objectivesValues, population, problem.objectivesCnt);
            if (val < 0)
                val = 1 + Math.Abs(val);
            else val = 1 / (val + 1);
            solution.fitnessValue = val;
        }
    }
}
