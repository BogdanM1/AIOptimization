using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIOptimization.Population;
using AIOptimization.Random;
using AIOptimization.Problem;
using AIOptimization.PerformanceIndicators;
using AIOptimization.PerformanceIndicators.MO;
using AIOptimization.PerformanceIndicators.SO;
using AIOptimization.ExternalScripts;

namespace AIOptimization.Algorithms
{
    public class EvaluationTermination : Exception { }
   
    public abstract class PopulationOptimizationAlgorithm
    {
        protected RandomGenerator rGen;
        protected IList<Solution> population;
        protected SearchOperator searchOperator; 
        protected Evaluator evaluator;

        public int populationSize { get; protected set; }
        protected int maxIterCount, iterCount;
        protected int maxEvalCount, evalCount;

        protected Problem.Problem problem;
        protected PopulationPrinter all_printer;
        protected PopulationPrinter best_printer;

        protected IList<PerformanceIndicator> indicators;
        protected IndicatorPrinter ind_printer;
        protected int[] update_referent_ind;

        public string name { get; protected set; }

        public PopulationOptimizationAlgorithm()
        {
            evalCount = 0;
            iterCount = 0;
            indicators = null;
        }

        public virtual void setProblem(Problem.Problem problem)
        {
            this.problem = problem;
        }

        public virtual void setRandomGenerator(RandomGenerator rGen)
        {
            this.rGen = rGen;
        }

        public virtual void setPerformanceIndicators(IList<PerformanceIndicator> indicators)
        {
            this.indicators = indicators;
            this.update_referent_ind = new int[indicators.Count()];
        }

        public virtual void setPerformanceIndicators(IList<PerformanceIndicator> indicators, int[] update_referent)
        {
            this.indicators = indicators.Where(x => x != null).ToList();
            int n = this.indicators.Count();
            this.update_referent_ind = new int[n];
            for (int i = 0; i < n; i++) this.update_referent_ind[i] = update_referent[i];
        }

        public void setPrinters(PopulationPrinter all_printer, PopulationPrinter best_printer, IndicatorPrinter ind_printer)
        {
            this.all_printer  = all_printer;
            this.best_printer = best_printer;
            this.ind_printer = ind_printer;
        }

        public abstract void setOperatorsAndEvaluator();
        
        public void setParameters(int populationSize, int maxIterCount, int maxEvalCount)
        {
            this.populationSize = populationSize;
            this.maxIterCount = maxIterCount;
            this.maxEvalCount = maxEvalCount;
            this.population = new Solution[populationSize];
        }
         
        public virtual void init()
        {
            for (int i = 0; i < populationSize; i++)
            {
                this.population[i] = new Solution(problem.objectivesCnt, problem.paramsCnt);
                this.population[i].generate(rGen, problem.min, problem.max);
                evaluator.evaluate(population[i], population, problem);
                evalCount++;
                this.population[i].ID = i+1;
            }
        }

        public virtual void exec()
        {
            evalCount = 0;
            iterCount = 0;
            setOperatorsAndEvaluator();
            init();
            IList<Solution> result = bestSolutions().ToArray();
            create_headers();
            printPopulationAndResults(result);
            Console.WriteLine("Iterations, Evaluations");
            while (evalCount < maxEvalCount && iterCount < maxIterCount)
            {
                iterCount++;
                updateIndicators(result);
                try{ nextIter(); } catch (EvaluationTermination) {  }
                result = bestSolutions().ToArray();
                Console.WriteLine("{0},\t{1}", iterCount, evalCount);
                printPopulationAndResults(result);
                calculateIndicators(result);
            }
            stopPrinters();

        }

        private void create_headers()
        {
            if (all_printer != null)  all_printer.create_headers(problem); 
            if (best_printer != null) best_printer.create_headers(problem);       
        }

        private void printPopulationAndResults(IList<Solution> result)
        {
            if (all_printer != null) all_printer.printPopulation(iterCount, population, problem);
            if (best_printer != null) best_printer.printPopulation(iterCount, result, problem);        
        }

        private void stopPrinters()
        {
            if (all_printer != null) all_printer.StopPrinter();
            if (best_printer != null) best_printer.StopPrinter();
            if (ind_printer != null) ind_printer.StopPrinter();        
        }

        private void updateIndicators(IList<Solution> result)
        {
            if (indicators == null) return;
            for (int ind = 0; ind < indicators.Count(); ind++)
            {
                if (update_referent_ind[ind] == 1) (indicators[ind] as IndicatorWithReferentSet).setReferentSet(problem, result);
            }
        }

        private void calculateIndicators(IList<Solution> result)
        {
            if (indicators != null)
            {
                double[] indicator_values = new double[indicators.Count()];
                int i = 0;
                foreach (PerformanceIndicator pInd in indicators)
                {
                    if (pInd is IndicatorWithReferentSet)
                    {
                        indicator_values[i] = pInd.calculate(result, populationSize, problem);
                    }
                    else
                    {
                        indicator_values[i] = pInd.calculate(population, populationSize, problem);
                    }
                    i++;
                }
                if (ind_printer != null) ind_printer.print(iterCount, indicator_values);
            }        
        }
        
        public virtual IList<Solution> bestSolutions()
        {
            Solution best = population[0];
            double bestNectar = best.fitnessValue;
            for (int i = 1; i < populationSize; i++)
            {
                if (population[i].fitnessValue > bestNectar)
                {
                    bestNectar = population[i].fitnessValue;
                    best = population[i];
                }
            }
            Solution[] result = new Solution[] { best };
            return result;
        }

        public abstract void nextIter();
    }
}
