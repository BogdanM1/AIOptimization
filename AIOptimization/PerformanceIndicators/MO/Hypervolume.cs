using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIOptimization.Population;
using AIOptimization.Problem;
using AIOptimization.DominationRelations;

namespace AIOptimization.PerformanceIndicators.MO
{
    public class Hypervolume:IndicatorWithReferentSet
    {
       public bool isInverted { get; set; }
       private static bool[] hyp_Minimization; 

       public Hypervolume(Problem.Problem problem):base()
       {
           hyp_Minimization = new bool[problem.objectivesCnt];
           for (int i = 0; i < problem.objectivesCnt; i++) hyp_Minimization[i] = false;
           isInverted = true;
       }

       public Hypervolume(Problem.Problem problem, IList<Solution> referent_set):base(problem, referent_set)
       {
           hyp_Minimization = new bool[problem.objectivesCnt];
           for (int i = 0; i < problem.objectivesCnt; i++) hyp_Minimization[i] = false;
           isInverted = true;
       }

       private static void invert(int numberOfObjectivesIn,ref Solution sol)
       {
           double value; 
           for (int j = 0; j < numberOfObjectivesIn; j++)
           {
               value = sol.objectivesValues[j];
               if (value < 0.0) value = 0.0;
               else if (value > 1.0) value = 1.0;
               sol.objectivesValues[j] = 1.0 - value;
           }
        }


        private static void swap(List<Solution> population, int i, int j)
        {
             Solution temp = population[i];
             population[i] = population[j];
             population[j] = temp;
        }

        private static int filterNondominatedSet(List<Solution> population, int numberOfSolutions, int numberOfObjectivesIn)
        {
             int i = 0;
             int n = numberOfSolutions;

             while (i < n)
             {
                int j = i + 1;
                while (j < n)
                {
                    if (DominationRelations.DominationRelations.dominates(population[i], population[j], 
                        numberOfObjectivesIn, hyp_Minimization))  swap(population, j, --n);
                    else if (DominationRelations.DominationRelations.dominates(population[j], population[i],
                             numberOfObjectivesIn, hyp_Minimization))
                    {
                        swap(population, i--, --n);
                         break;
                    }
                    else
                       j++;
                }
                i++;
             }
             return n;
         }

         private static double surfaceUnchangedTo(List<Solution> population,int numberOfSolutions, int objective)
         {
              double min = population[0].objectivesValues[objective];
              for (int i = 1; i < numberOfSolutions; i++)
                    min = Math.Min(min, population[i].objectivesValues[objective]);
              return min;
          }

         private static int reduceNondominatedSet(List<Solution> population, int numberOfSolutions, int objective, double threshold)
         {
              int n = numberOfSolutions;
              for (int i = 0; i < n; i++)
              {
                  if (population[i].objectivesValues[objective] <= threshold)
                      swap(population, i, --n);
              }
              return n;
         }

         public static double calculateHypervolume(List<Solution> population,int numberOfSolutions, int numberOfObjectives)
         {
             double volume = 0.0;
             double distance = 0.0;
             int n = numberOfSolutions;
             while (n > 0)
             {
                 int numberOfNondominatedPoints = filterNondominatedSet(population, n, numberOfObjectives - 1);
                 double tempVolume = 0.0;

                 if (numberOfObjectives < 3) tempVolume = population[0].objectivesValues[0];
                 else  tempVolume = calculateHypervolume(population, numberOfNondominatedPoints, numberOfObjectives - 1);

                 double tempDistance = surfaceUnchangedTo(population, n, numberOfObjectives - 1);
                 volume += tempVolume * (tempDistance - distance);
                 distance = tempDistance;
                 n = reduceNondominatedSet(population, n, numberOfObjectives - 1, distance);
             }
             return volume;
         }

         public override double calculate(IList<Solution> calc_set, int popSize, Problem.Problem problem)
         {
             IList<Solution> calc_set_norm = normalizer.normalize(problem, calc_set);
             List<Solution> solutions = new List<Solution>();
             foreach (Solution sol in calc_set_norm)
             {
                 Solution clone = sol.copy();
                 if (isInverted) invert(problem.objectivesCnt, ref clone);
                 solutions.Add(clone);
             }
             return calculateHypervolume(solutions, solutions.Count(),problem.objectivesCnt);
         }
    }
}
