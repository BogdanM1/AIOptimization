using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIOptimization.Population; 

namespace AIOptimization.DominationRelations 
{
    public class DominationRelations
    {
        public static bool dominates(Solution solution1, Solution solution2, int numberOfObjectives, bool[] minimization)
        {

            Func<double, double, bool> comparatorBetter;
            Func<double, double, bool> comparatorWorse;
            bool betterInAnyObjective = false;
            bool worseInAnyObjective = false;

            for (int i = 0; i < numberOfObjectives; i++)
            {
                if (worseInAnyObjective)
                {
                    break;
                }

                if (!minimization[i])
                {
                    comparatorBetter = (x1, x2) => { return (x1 > x2); };
                    comparatorWorse = (x1, x2) => { return (x1 < x2); };
                }
                else
                {
                    comparatorBetter = (x1, x2) => { return (x1 < x2); };
                    comparatorWorse = (x1, x2) => { return (x1 > x2); };
                }

                if (comparatorBetter(solution1.objectivesValues[i], solution2.objectivesValues[i]))
                {
                    betterInAnyObjective = true;
                }
                else if (comparatorWorse(solution1.objectivesValues[i], solution2.objectivesValues[i]))
                {
                    worseInAnyObjective = true;
                }
            }

            return !worseInAnyObjective && betterInAnyObjective;
        }
        /*  
            1 if sol1 dominates sol2
            2 if sol2 dominates sol1
            3 if sol1 and sol2 are non-dominated and sol1!=sol2 (b_index unequal)
            4 if sol1 and sol2 are non-dominated and sol1=sol2 	*/
        public static int hypbox_dominance(Solution solution1, Solution solution2, Problem.Problem problem, double hypboxbound, bool[] minimization)
        {
            int boxindex1, boxindex2;
            bool first_dominates = false;
            bool second_dominates = false;
            Func<int, int, bool> comparatorBetter;

            for (int i = 0; i < problem.objectivesCnt; i++)
            {
                if (!minimization[i])
                    comparatorBetter = (x1, x2) => { return (x1 > x2); };
                else
                    comparatorBetter = (x1, x2) => { return (x1 < x2); };
                boxindex1 = (int)Math.Floor((solution1.objectivesValues[i]) / hypboxbound);
                boxindex2 = (int)Math.Floor((solution2.objectivesValues[i]) / hypboxbound);
                if (comparatorBetter(boxindex1, boxindex2)) first_dominates = true;
                if (comparatorBetter(boxindex2, boxindex1)) second_dominates = true;
            }
            if (first_dominates && !second_dominates) return 1;
            if (!first_dominates && second_dominates) return 2;
            if (first_dominates && second_dominates)  return 3;
            return 4;
        }
    }
}
