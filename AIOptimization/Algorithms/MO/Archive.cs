using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIOptimization.Population;

namespace AIOptimization.Algorithms.MO
{
    public abstract class Archive
    {
        public List<Solution> members;
        public int maxSize { get; set; }
         
        public Archive()
        {
            this.members = new List<Solution>();
        }

        public abstract bool add(Solution sol, Problem.Problem problem);

        public virtual void maintain()
        {
            double min;
            int n = members.Count();
            while (n > maxSize)
            {
                min = members.Min(x => x.fitnessValue);
                members.RemoveAll(x => x.fitnessValue == min);
                n = members.Count();
            }
        }

    }
}
