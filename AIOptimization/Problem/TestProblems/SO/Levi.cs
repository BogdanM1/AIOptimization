using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIOptimization.Problem.TestProblems.SO
{
    public class Levi : Problem
    {
        public Levi(int paramsCount, double[] min, double[] max)
            : base(1, paramsCount, min, max)
        {
            this.name = "Levi"; 
        }
        public override double[] calculateObjectives(double[] x)
        {
            double[] f = new double[objectivesCnt];
            double sin3x = Math.Sin(3 * Math.PI * x[0]);
            sin3x = sin3x * sin3x;
            double sin3y = Math.Sin(3 * Math.PI * x[1]);
            sin3y = sin3y * sin3y;
            double sin2y = Math.Sin(2 * Math.PI * x[1]);
            sin2y = sin2y * sin2y;
            f[0] = sin3x + (x[0] - 1) * (x[0] - 1) * (1 + sin3y);
            f[0] += (x[1] - 1) * (x[1] - 1) * (1 + sin2y);
            return f;
        }
    }
} 
