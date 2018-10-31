using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using AIOptimization.Population;
using AIOptimization.Algorithms;
using AIOptimization.Algorithms.MO;
using AIOptimization.PerformanceIndicators;
using AIOptimization.PerformanceIndicators.SO;
using AIOptimization.PerformanceIndicators.MO;
using AIOptimization.Problem.TestProblems.SO;
using AIOptimization.Problem.TestProblems.MO;

namespace AIOptimization.Problem.TestProblems
{ 
    public class runBenchmarks:ProblemRunner
    {
        public static string refSolutionsPath = null; 
       // public static string refSolutionsPath = @"C:\Users\Bogdan\Desktop\optimization\MOEAFramework-Parallel\pf\";

        static IList<Solution> readRefFront(string filepath, int objcnt, int parcnt)
        {
            IList<Solution> ref_front = new List<Solution>();
            String[] lines = File.ReadAllLines(filepath);
            foreach (String line in lines)
            {
                string[] split_line = line.Split(' ').Where(x => !string.IsNullOrEmpty(x)).ToArray();
                Solution sol = new Solution(objcnt,parcnt);
                for (int i = 0; i < objcnt; i++)
                {
                    sol.objectivesValues[i] = Convert.ToDouble(split_line[i]);
                }
                ref_front.Add(sol);
            }
            return ref_front;
        }


        public static void runSO(PopulationOptimizationAlgorithm algorithm, bool print, bool draw)
        {
             Ackley     ackley     = new Ackley(2, new double[] { -5, -5 }, new double[] { 5, 5 });
             Sphere     sphere     = new Sphere(2, new double[] { -5, -5 }, new double[] { 5, 5 });
             Baele      baele      = new Baele(2, new double[] { -4.5, -4.5 }, new double[] { 4.5, 4.5 });
             Rastrigin  rastrigin  = new Rastrigin(2, new double[] { -5, -5 }, new double[] { 5, 5 });
             Rosenbrock rosenbrock = new Rosenbrock(2, new double[] { -5, -5 }, new double[] { 5, 5 });
             IList<Problem> problems = new List<Problem>() { ackley,/* sphere, baele, rastrigin, rosenbrock*/};
             foreach (Problem problem in problems)
             {
                 List<PerformanceIndicator> performanceIndicators = new List<PerformanceIndicator>() { new PositionDiversityIndicator() };
                 Console.WriteLine("{0}:", problem.name);
                 algorithm.setPerformanceIndicators(performanceIndicators);
                 runProblem(algorithm, problem, "Position diversity", print, draw, null);
             }
        }

        public static void runMO(PopulationOptimizationAlgorithm algorithm, bool print, bool draw)
        {
             Kursawe kursawe     = new Kursawe(3, new double[] { -5, -5, -5 }, new double[] { 5, 5, 5 });
             Schaffer1 schaffer1 =  new Schaffer1(1, new double[] { -5}, new double[] {5});
             Schaffer2 schaffer2 =  new Schaffer2(1, new double[] { -5}, new double[] {5});
             ZDT1 zdt1           = new ZDT1(3, new double[] { 0, 0, 0 }, new double[] { 1, 1, 1 });
             ZDT3 zdt3           = new ZDT3(3, new double[] { 0, 0, 0 }, new double[] { 1, 1, 1 });
             IList<Problem> problems   = new List<Problem>() { kursawe /*, schaffer1, schaffer2, zdt1, zdt3*/ };
             Hypervolume hypervolGlob  = null;
             AdditiveEpsilon epsGlobal = null;
             foreach (Problem problem in problems)
             {
                 double globalHyp = 0, globalEps = 0;
                 if (refSolutionsPath != null)
                 {
                        IList<Solution> refSolutions = readRefFront(refSolutionsPath + problem.name + ".pf", problem.objectivesCnt, problem.paramsCnt);
                        hypervolGlob = new Hypervolume(problem, refSolutions);
                        epsGlobal = new AdditiveEpsilon(problem, refSolutions);
                        globalHyp = hypervolGlob.calculate(refSolutions, algorithm.populationSize, problem);
                        globalEps = epsGlobal.calculate(refSolutions, algorithm.populationSize, problem);
                        Console.WriteLine("eps global: {0} hyp global: {1}", globalEps, globalHyp);
                 }
                 Hypervolume hypIter = new Hypervolume(problem);
                 List<PerformanceIndicator> performanceIndicators = new List<PerformanceIndicator>() { hypIter, hypervolGlob, epsGlobal };
                 int[] performance_update_set = new int[] {1, 0, 0};
                 algorithm.setPerformanceIndicators(performanceIndicators, performance_update_set);
                 Console.WriteLine("{0}:", problem.name);
                 string indicatorsHeader = " Hypervolume (iter)";
                 if (hypervolGlob != null) indicatorsHeader += ", Hypervolume (global)";
                 if(epsGlobal     != null) indicatorsHeader += ", Epsilon (global)";
                 runProblem(algorithm, problem, indicatorsHeader, print, draw, refSolutionsPath);
            }
        }

        public static void run(PopulationOptimizationAlgorithm algorithm, bool print, bool draw)
        {
            runSO(algorithm, print, draw);
            runMO(algorithm, print, draw);
        }
    }
}
