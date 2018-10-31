using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIOptimization.Problem.TestProblems.MO
{
    public class ZDT3 : Problem
    {
        public ZDT3(int paramsCount, double[] min, double[] max)
            : base(2, paramsCount, min, max)
        {
            this.name = "ZDT3";
        }
        public override double[] calculateObjectives(double[] x)
        {
            double[] f = new double[objectivesCnt];
            int n = paramsCnt;
            f[0] = x[0];
            f[1] = 0; 
            double g = 0;
            double h = 0;
            double sum = 0;
            for (int i = 1; i < n; i++) sum = sum + x[i];
            g = 1 + 9.0 / (n - 1.0) * sum;
            h = 1 - Math.Sqrt(f[0] / g) - (f[0] / g) * Math.Sin(Math.PI * f[0] * 10);
            f[1] = g * h;
            return f;
        }
    }
}
