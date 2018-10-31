using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIOptimization.Problem.TestProblems.SO
{
    public class Rosenbrock : Problem
    {
        public Rosenbrock(int paramsCount, double[] min, double[] max)
            : base(1, paramsCount, min, max)
        {
            this.name = "Rosenbrock"; 
        }
        public override double[] calculateObjectives(double[] x)
        {
            double[] f = new double[objectivesCnt];
            int n = paramsCnt;
            for (int i = 0; i < (n - 1); i++)
            {
                f[0] = f[0] + 100 * (x[i + 1] - x[i] * x[i]) * (x[i + 1] - x[i] * x[i]) + (x[i] - 1) * (x[i] - 1);
            }
            return f;
        }
    }
}
 