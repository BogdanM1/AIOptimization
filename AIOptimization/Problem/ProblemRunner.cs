using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIOptimization.Algorithms;
using AIOptimization.ExternalScripts;
using AIOptimization.Population;
using AIOptimization.PerformanceIndicators;

namespace AIOptimization.Problem
{
    public class ProblemRunner
    {
        public static void runProblem(PopulationOptimizationAlgorithm algorithm, Problem problem, string indicatorsHeader, bool print, bool draw, string refSolutionsPath)
        {
            algorithm.setProblem(problem);
            string populationFile = null; 
            string bestSolFile = null;
            string indicatorFile = null;
            if (print)
            {
                populationFile = "population" + problem.name + ".txt";
                bestSolFile = "best" + problem.name + ".txt";
                indicatorFile = "indicators" + problem.name + ".txt";
                IndicatorPrinter indPrinter = null;
                if (indicatorsHeader != null) indPrinter = new IndicatorPrinter(indicatorFile, indicatorsHeader);
                algorithm.setPrinters(new PopulationPrinter(populationFile), new PopulationPrinter(bestSolFile),indPrinter);
            }
            algorithm.exec();
            if (draw)
            {
                string drawPopulationScript = RScripts.makeRScriptToDrawPopulation(algorithm.name, problem, populationFile, bestSolFile, refSolutionsPath);
                RScripts.runScript(drawPopulationScript);
                if (indicatorsHeader != null)
                {
                    string drawIndicatorsScript = RScripts.makeRScriptToDrawIndicators(algorithm.name, problem, indicatorFile);
                    RScripts.runScript(drawIndicatorsScript);
                }
            }

        }
    }
}
