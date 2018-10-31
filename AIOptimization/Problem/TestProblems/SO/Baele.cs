using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIOptimization.Problem.TestProblems.SO
{
    public class Baele : Problem
    {
        public Baele(int paramsCount, double[] min, double[] max)
            : base(1, paramsCount, min, max)
        {
            this.name = "Baele"; 
        }
        public override double[] calculateObjectives(double[] x)
        {
            double[] f = new double[objectivesCnt];
            f[0] = 0;
            f[0] = (1.5 - x[0] + x[0] * x[1]) * (1.5 - x[0] + x[0] * x[1]);
            f[0] += (2.25 - x[0] + x[0] * x[1] * x[1]) * (2.25 - x[0] + x[0] * x[1] * x[1]);
            f[0] += (2.625 - x[0] + x[0] * x[1] * x[1] * x[1]) * (2.625 - x[0] + x[0] * x[1] * x[1] * x[1]);
            return f;
        }
    }
}
 