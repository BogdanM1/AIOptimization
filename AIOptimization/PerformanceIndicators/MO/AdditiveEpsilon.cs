using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIOptimization.Population;
using AIOptimization.Problem;

namespace AIOptimization.PerformanceIndicators.MO
{
    public class AdditiveEpsilon:IndicatorWithReferentSet
    { 
        public AdditiveEpsilon():base(){}

        public AdditiveEpsilon(Problem.Problem problem, IList<Solution> referent_set):base(problem, referent_set){}

        public static double  calculateEpsilon(double[] objectives_values1, double[] objectives_values2, int numberOfObjectivesIn)
        {
            double e;
            double rez1 = 0, rez2 = 0;
            rez1 = objectives_values1[0];
            rez2 = objectives_values2[0];
            e = rez1 - rez2;
            for (int i = 1; i < numberOfObjectivesIn; i++)
            {
                rez1 = objectives_values1[i];
                rez2 = objectives_values2[i];
                if (rez1 - rez2 > e) e = rez1 - rez2;
            }
            return e;
        }

        public static double calculateEpsilon(double[] objectives_values1, IList<Solution> calc_set, int numberOfObjectivesIn)
        {
            double epsilon = 0;
            double min = Double.PositiveInfinity;
            foreach (Solution sol in calc_set)
            {
                epsilon = calculateEpsilon(sol.objectivesValues, objectives_values1, numberOfObjectivesIn);
                if (epsilon < min) min = epsilon;
            }
            return min;
        }


        public override double calculate(IList<Solution> calc_set, int popSize, Problem.Problem problem)
        {
            double max = Double.NegativeInfinity;
            double epsilon;
            IList<Solution> calc_set_norm = normalizer.normalize(problem, calc_set);

            foreach (Solution ref_sol in referent_set_norm)
            {
                epsilon = calculateEpsilon(ref_sol.objectivesValues,calc_set_norm, problem.objectivesCnt);
                if (epsilon > max) max = epsilon;
            }
            return max;
        }
    }
}
