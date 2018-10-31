using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIOptimization.Population;
using AIOptimization.Problem;

namespace AIOptimization.Algorithms.SO
{
    class SOEvaluator:Evaluator 
    {
        public override void evaluate(Solution solution, IList<Solution> population, Problem.Problem problem)
        {
            solution.objectivesValues = problem.calculateObjectives(solution.parameters);
            double val = solution.objectivesValues[0];
            if (val >= 0)
            {
                val = 1 / (val + 1);
            }
            else
            {
                val = 1 + Math.Abs(val);
            }
            solution.fitnessValue = val;
        }
    }
}
