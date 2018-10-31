using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIOptimization.Problem.TestProblems.MO
{
    public class Schaffer2 : Problem
    {
        public Schaffer2(int paramsCount, double[] min, double[] max)
            : base(2, paramsCount, min, max)
        {
            this.name = "Schaffer2"; 
        }
        public override double[] calculateObjectives(double[] x)
        {
            double[] f = new double[objectivesCnt];
            int n = paramsCnt;
            if (x[0] < 1) f[0] = -x[0];
            if (1 < x[0] && x[0] <= 3) f[0] = x[0] - 2;
            if (3 < x[0] && x[0] <= 4) f[0] = 4 - x[0];
            if (x[0] > 4) f[0] = x[0] - 4;
             
            f[1] = (x[0] - 5) * (x[0] - 5);
            return f;
        }
    }
}
