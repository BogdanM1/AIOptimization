using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIOptimization.Population;
using AIOptimization.Problem;
using AIOptimization.DominationRelations;

namespace AIOptimization.Algorithms.MO
{ 
    public class MMOABCArchive:Archive
    {
        public double hypboxbound;
        public MMOABCArchive() : base() { }

        public override bool add(Solution sol, Problem.Problem problem)
        {
            int n = members.Count();
            bool samebox = false;
            int i;
            for (i = n-1; i >= 0; i--)
            {
                int dom_type = DominationRelations.DominationRelations.hypbox_dominance(sol, members[i], problem, hypboxbound, problem.minimization);
                switch (dom_type)
                {
                    case 1: { members.RemoveAt(i); break; }
                    case 2: { return false; }
                    case 3: { break; }
                    case 4: { samebox = true; break; }
                }
                if (samebox) break;
            }
            if (!samebox) members.Add(sol.copy());
            if (samebox)
            {
                bool solution_dominated = DominationRelations.DominationRelations.dominates(sol, members[i], problem.objectivesCnt, problem.minimization);
                if (solution_dominated) return false;
                bool member_dominated = DominationRelations.DominationRelations.dominates(members[i], sol, problem.objectivesCnt, problem.minimization);
                if (!member_dominated)
                {
                    double dist1 = 0, dist2 = 0;
                    int boxindex1, boxindex2;
                    for(int k=0; k<problem.objectivesCnt; k++)
                    {
                        boxindex1 = (int)Math.Floor((sol.objectivesValues[k]) / hypboxbound);
                        boxindex2 = (int)Math.Floor((members[i].objectivesValues[k]) / hypboxbound);
                        dist1 += ((sol.objectivesValues[k] - boxindex1) / hypboxbound) *
                                ((sol.objectivesValues[k] - boxindex1) / hypboxbound);
                        dist2 += ((members[i].objectivesValues[k] - boxindex2) / hypboxbound) *
                                 ((members[i].objectivesValues[k] - boxindex2) / hypboxbound);
                    }
                    if (dist2 > dist1) return false;
                }
                members.RemoveAt(i);
                members.Add(sol.copy());
            }
            return true;
        }
    }
}
