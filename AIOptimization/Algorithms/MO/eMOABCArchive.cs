using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIOptimization.Population;
using AIOptimization.Problem;
using AIOptimization.PerformanceIndicators.MO;

namespace AIOptimization.Algorithms.MO
{
    public class eMOABCArchive:Archive
    {
        public eMOABCArchive():base() { } 
         
        public override bool add(Solution sol, Problem.Problem problem)
        {
            int n = members.Count(); 
            if (n < maxSize)
            {
                members.Add(sol.copy());
                return true;
            }

            for (int i = n - 1; i >= 0; i--)
            {
                 if (DominationRelations.DominationRelations.dominates(sol, members[i],
                      problem.objectivesCnt, problem.minimization))
                 {
                     members.RemoveAt(i);
                     n--;
                 }
                 else if (DominationRelations.DominationRelations.dominates(members[i], sol,
                        problem.objectivesCnt, problem.minimization)) return false;
             }
             members.Add(sol.copy());
             return true;
        }
    }
}
