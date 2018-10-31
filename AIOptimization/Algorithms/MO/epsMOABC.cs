using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIOptimization.Population;
using AIOptimization.Algorithms.SO;

namespace AIOptimization.Algorithms.MO
{
    public class epsMOABC:ABCDE
    {
        protected Archive archive;
        protected int maxArchSize;

        public epsMOABC(): base()
        {
            this.name = "epsMoabc";
        }

        public override void setOperatorsAndEvaluator()
        {
            this.evaluator = new epsEvaluator();
            this.searchOperator = new ABCOperator();
            this.secondarySearchOperator = new ABCDEOperator();
            (secondarySearchOperator as ABCDEOperator).crossOverProbab = crossoverProbability;

        }

        public void setParameters(int populationSize, int maxIterCount, int maxEvalCount, int limitTrials, double basicSearchOperatorProbability,
            double crossoverProbability, int maxArchSize)
        {
            base.setParameters(populationSize, maxIterCount, maxEvalCount, limitTrials, basicSearchOperatorProbability,crossoverProbability);
            this.maxArchSize = maxArchSize;
        }

        protected virtual void initialize_archive()
        {
            archive = new eMOABCArchive(); 
            archive.maxSize = maxArchSize;
            if (problem.objectivesCnt == 1) archive.maxSize = 1;
        }

        public override void init()
        {
            initialize_archive();
            for (int i = 0; i < populationSize; i++)
            {
                this.population[i] = new Solution(problem.objectivesCnt, problem.paramsCnt);
                this.population[i].generate(rGen, problem.min, problem.max);
                this.population[i].ID = i + 1;
            }
            for (int i = 0; i < populationSize; i++)
            {
                evaluator.evaluate(population[i], population, problem);
                evalCount++;
                archive.add(population[i], problem);
            }
            this.trials = new int[populationSize];
        }

       /* protected override double[] calculateProbabilities()
        {
            int[] indices = new int[populationSize];
            List<Solution> sorted = population.OrderByDescending(x => x.fitnessValue).ToList();
            for (int i = 0; i < populationSize; i++) indices[i] = sorted[i].ID - 1;
            int[] ranks = new int[populationSize];
            int rank = 1;
            ranks[indices[0]] = rank;
            for (int i = 1; i < populationSize; i++)
            {
                if (sorted[i - 1].fitnessValue != sorted[i].fitnessValue)
                    rank++;
                ranks[indices[i]] = rank;
            }
            double sum = ranks.Sum(x => 1.0 / x);
            double[] probab = new double[populationSize];
            for (int i = 0; i < populationSize; i++)
            {
                probab[i] = 0.9*(1.0 / ranks[i]) / sum + 0.1;
            }
            return probab; 
        }*/

        public override void nextIter()
        {
            base.nextIter();  
            archive.maintain();
        }

        protected override void addToPopulation(Solution sol, int index)
        {
            if (sol.fitnessValue > population[index].fitnessValue)
            {
                population[index] = sol;
                trials[index] = 0;
                archive.add(population[index], problem);
            }
            else
            {
                trials[index]++;
            }        
        }

        public override IList<Solution> bestSolutions()
        {
            return archive.members;
        }
    }
}
