using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIOptimization.Problem.TestProblems.MO
{
    public class Kursawe : Problem
    {
        public Kursawe(int paramsCount, double[] min, double[] max)
            : base(2, paramsCount, min, max)
        {
            this.name = "Kursawe"; 
        }
        public override double[] calculateObjectives(double[] x)
        {
            double[] f = new double[objectivesCnt]; 
            int n = paramsCnt;
            f[0] = 0;
            f[1] = 0;
            for (int i = 0; i < n - 1; i++)
                f[0] += -10 * Math.Exp(-0.2 * Math.Sqrt(x[i] * x[i] + x[i + 1] * x[i + 1]));
            for (int i = 0; i < n; i++)
                f[1] += Math.Pow(Math.Abs(x[i]), 0.8) + 5 * Math.Sin(x[i] * x[i] * x[i]);/**/
            return f;
        }
    }
}
