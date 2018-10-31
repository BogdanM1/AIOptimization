using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIOptimization.Problem.TestProblems.SO
{
    public class Rastrigin : Problem
    {
        public Rastrigin(int paramsCount, double[] min, double[] max)
            : base(1, paramsCount, min, max)
        {
            this.name = "Rastrigin"; 
        }
        public override double[] calculateObjectives(double[] parameters)
        {
            double[] f = new double[objectivesCnt];
            int n = paramsCnt;
            double pi = Math.PI;
            f[0] = 10 * n + parameters.Sum(t => (t * t - 10 * Math.Cos(2 * pi * t)));
            return f;
        }
    }
}
 