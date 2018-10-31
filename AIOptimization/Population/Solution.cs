using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIOptimization.Random;

namespace AIOptimization.Population
{
    public class Solution:IComparable<Solution>
    {
        public double[] objectivesValues {get; set;}
        public double[] parameters       { get; set;}
        public int objectivesCount, paramCount;
        public double fitnessValue;
        public int ID;

        public Solution(int objectivesCount, int paramCount)
        {
            this.objectivesCount = objectivesCount; 
            this.paramCount = paramCount;
            objectivesValues = new double[objectivesCount];
            parameters = new double[paramCount];
        }

        public void generate(RandomGenerator rGen, double[] min, double[] max)
        {
            for (int i = 0; i < paramCount; i++)
                parameters[i] = rGen.rnd(min[i], max[i]); 
        }

        public virtual Solution copy()
        {
            Solution sol = new Solution(objectivesCount, paramCount);
            sol.fitnessValue = this.fitnessValue;
            sol.ID = ID;
            for (int i = 0; i < paramCount; i++) 
                sol.parameters[i] = this.parameters[i];
            for (int i = 0; i < objectivesCount; i++) 
                sol.objectivesValues[i] = this.objectivesValues[i];
            return sol;
        }

        public void constraint(Problem.Problem problem, int dim)
        {
            if (parameters[dim] > problem.max[dim]) parameters[dim] = problem.max[dim];
            if (parameters[dim] < problem.min[dim]) parameters[dim] = problem.min[dim];
        }

        public void constraint(Problem.Problem problem)
        {
            for (int i = 0; i < problem.paramsCnt; i++) constraint(problem, i);
        }


        public int CompareTo(Solution other)
        {
            if (other.fitnessValue > this.fitnessValue) return 1;
            if (other.fitnessValue < this.fitnessValue) return -1;
            return 0;
        }
    }
}
