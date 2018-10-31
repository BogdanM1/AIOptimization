using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIOptimization.Problem.TestProblems.MO
{
    public class Fonseca : Problem
    {
        public Fonseca(int paramsCount, double[] min, double[] max)
            : base(2, paramsCount, min, max)
        { 
            this.name = "Fonseca"; 
        }
        public override double[] calculateObjectives(double[] parameters)
        {
            double[] f = new double[objectivesCnt];
            double x = parameters[0];
            double y = parameters[1];
            f[0] = 1.0 - Math.Exp(-(x - 1) * (x - 1) -
                   (y + 1.0) * (y + 1.0));

            f[1] = 1.0 - Math.Exp(-(x + 1) * (x + 1) -
                   (y - 1.0) * (y - 1.0));
            return f;
        }
    }
}
