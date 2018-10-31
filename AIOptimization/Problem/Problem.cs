using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIOptimization.Population;

namespace AIOptimization.Problem
{
    public abstract class Problem
    {
        public double[] min        { get; protected set; }
        public double[] max        { get; protected set; }
        public int paramsCnt       { get; protected set; }
        public int objectivesCnt   { get; protected set; }
        public string name;
        public bool[] minimization;

        public Problem() { }
        public Problem(int objectivesCnt, int paramsCnt, double [] min, double[] max) 
        { 
            this.objectivesCnt = objectivesCnt;
            this.paramsCnt = paramsCnt;
            this.min = new double[paramsCnt];
            this.max = new double[paramsCnt];
            this.minimization = new bool[objectivesCnt];
            for (int i = 0; i < paramsCnt; i++)
            {
                this.min[i] = min[i];
                this.max[i] = max[i];
            }
            for (int i = 0; i < objectivesCnt; i++) minimization[i] = true;
        }

        public abstract double[] calculateObjectives(double[] parameters);
    }
}
