using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIOptimization.Population;
using AIOptimization.Problem;

namespace AIOptimization.PerformanceIndicators
{
    public class Normalizer
    {
        double[] min;
        double[] max;
        IList<Solution> referent_set;

        public Normalizer(Problem.Problem problem, IList<Solution> referent_set)
        {
            min = new double[problem.objectivesCnt];
            max = new double[problem.objectivesCnt];
            this.referent_set = referent_set;
            int n = referent_set.Count(); 

            for (int i = 0; i < problem.objectivesCnt; i++)
            {
                min[i] = Double.PositiveInfinity;
                max[i] = Double.NegativeInfinity;
            }
            for (int i = 0; i < problem.objectivesCnt; i++)
            {
                for (int j = 0; j < n; j++)
                {
                   if(min[i] > referent_set[j].objectivesValues[i]) min[i] = referent_set[j].objectivesValues[i];
                   if(max[i] < referent_set[j].objectivesValues[i])  max[i] = referent_set[j].objectivesValues[i];
                }
            }
        }

        public IList<Solution> normalize(Problem.Problem problem, IList<Solution> unnormalized)
        {
            IList<Solution> normalized = new List<Solution>();
            foreach (Solution s in unnormalized) normalized.Add(s.copy());
            int n = normalized.Count();
            for (int i = 0; i < problem.objectivesCnt; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    normalized[j].objectivesValues[i] = (normalized[j].objectivesValues[i] - min[i])
                                                        / (max[i] - min[i]);
                }
            }
            return normalized;

        }


    }
}
