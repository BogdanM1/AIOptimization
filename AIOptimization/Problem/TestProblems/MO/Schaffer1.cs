using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIOptimization.Problem.TestProblems.MO
{
    public class Schaffer1 : Problem
    {
        public Schaffer1(int paramsCount, double[] min, double[] max)
            : base(2, paramsCount, min, max)
        {
            this.name = "Schaffer"; 
        }
        public override double[] calculateObjectives(double[] x)
        {
            double[] f = new double[objectivesCnt];
            int n = paramsCnt; 
            f[0] = x[0] * x[0];
            f[1] = (x[0] - 2) * (x[0] - 2);
            return f;
        }
    }
}
