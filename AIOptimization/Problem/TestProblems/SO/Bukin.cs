using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIOptimization.Problem.TestProblems.SO
{
    public class Bukin : Problem
    {
        public Bukin(int paramsCount, double[] min, double[] max)
            : base(1, paramsCount, min, max)
        {
            this.name = "Bukin"; 
        }
        public override double[] calculateObjectives(double[] x)
        {
            double[] f = new double[objectivesCnt];
            f[0] = 0;
            f[0] = 100 * Math.Sqrt(Math.Abs(x[1] - 0.01 * x[0] * x[0]));
            f[0] += 0.01 * Math.Abs(x[0] + 10);
            return f;
        }
    }
}
 