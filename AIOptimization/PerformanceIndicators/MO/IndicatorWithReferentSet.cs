using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIOptimization.Population;
using AIOptimization.Problem;

namespace AIOptimization.PerformanceIndicators
{
    public abstract class IndicatorWithReferentSet:PerformanceIndicator
    {
        protected Normalizer normalizer;
        protected IList<Solution> referent_set_norm;

        public IndicatorWithReferentSet()
        {
            normalizer = null; 
            referent_set_norm = null;
        }

        public IndicatorWithReferentSet(Problem.Problem problem, IList<Solution> referent_set)
        {
            normalizer = new Normalizer(problem, referent_set);
            referent_set_norm = normalizer.normalize(problem, referent_set);
        }

        public void setReferentSet(Problem.Problem problem, IList<Solution> referent_set)
        {
            this.normalizer = new Normalizer(problem, referent_set);
            referent_set_norm = normalizer.normalize(problem, referent_set);
        }
    }
}
