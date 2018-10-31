using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AIOptimization.Problem.TestProblems.SO
{
    public class Sphere:Problem
    {
        public Sphere(int paramsCount,double[] min, double[] max)
            : base(1,paramsCount,min,max)
        {
            this.name = "Sphere"; 
        }
        public override double[] calculateObjectives(double[] parameters)
        {
            double rez = parameters.Sum(t => t * t);
            return new double[]{rez};
        }
    }
}
 