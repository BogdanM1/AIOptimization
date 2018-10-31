using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIOptimization.Problem.TestProblems.SO
{
    public class Ackley:Problem
    {
        public Ackley(int paramsCount, double[] min, double[] max)
            : base(1,paramsCount,min,max)
        {
            this.name = "Ackley";
        }
        public override double[] calculateObjectives(double[] parameters)
        {
            double[] f = new double[objectivesCnt];
            int n = paramsCnt;
            double pi = Math.PI;
            f[0] = -20 * Math.Exp(-0.2 * Math.Sqrt(0.5 * parameters.Sum(t => t * t)))
                    - Math.Exp(1.0 / n * parameters.Sum(t => Math.Cos(2 * pi * t)))
                     + Math.E + 20;
            return f;
        }
    } 
}
