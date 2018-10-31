using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIOptimization.Population;
using AIOptimization.Problem;

namespace AIOptimization.PerformanceIndicators.SO
{
    class PositionDiversityIndicator:PerformanceIndicator
    {
        public override double calculate(IList<Solution> population, int populationSize, Problem.Problem problem)
        {
            double val = 0;
            Normalizer norm = new Normalizer(problem, population);
            IList<Solution> population_norm = norm.normalize(problem, population);

            double[] popumean = new double[problem.paramsCnt]; 
            for (int i = 0; i < problem.paramsCnt; i++)
            {
                popumean[i] = 0.0;
                for (int j = 0; j < populationSize; j++)
                {
                    popumean[i] += population_norm[j].parameters[i];
                }
                popumean[i] /= populationSize;
            }

            double[] distance = new double[problem.paramsCnt];
            for (int i = 0; i < problem.paramsCnt; i++)
            {
                distance[i] = 0.0;
                for (int j = 0; j < populationSize; j++)
                {
                    distance[i] += (population_norm[j].parameters[i] - popumean[i])
                                    * (population_norm[j].parameters[i] - popumean[i]);
                }
                distance[i] /= populationSize;
            }
            val = distance.Sum();
            return val; 
        }
    }
}
