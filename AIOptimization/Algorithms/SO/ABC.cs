using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIOptimization.Population; 

namespace AIOptimization.Algorithms.SO
{
    public class ABC:PopulationOptimizationAlgorithm
    {
        protected int limitTrials;
        protected int[] trials;
         
        public ABC(): base()  
        {
            this.name = "ABC";
        }

        public void setParameters(int populationSize, int maxIterCount, int maxEvalCount, int limitTrials)
        {
            base.setParameters(populationSize, maxIterCount, maxEvalCount);
            this.limitTrials = limitTrials;
        }

        public override void init()
        {
            base.init();
            this.trials = new int[populationSize];
        }

        public override void nextIter()
        {
            sendEmployedBees();
            sendOnlookerBees();
            sendScoutBees();
        }

        protected virtual void beeSearch(int index)
        {
            Solution newSol = searchOperator.search(rGen, problem, population, populationSize, index);
            evaluator.evaluate(newSol, population, problem);
            evalCount++;
            addToPopulation(newSol, index);
            if (evalCount >= maxEvalCount) throw new EvaluationTermination();
        }

        protected virtual void addToPopulation(Solution sol, int index)
        {
            if (sol.fitnessValue > population[index].fitnessValue)
            {
                population[index] = sol;
                trials[index] = 0;
            }
            else
            {
                trials[index]++;
            }        
        }

        protected virtual void sendEmployedBees()
        {
            for (int i = 0; i < populationSize; i++)
            {
                beeSearch(i);
            }
        }

        protected virtual double[] calculateProbabilities()
        {
            double maxfitness = population[0].fitnessValue;
            for (int i = 1; i < populationSize; i++)
            {
                if (population[i].fitnessValue > maxfitness)
                {
                    maxfitness = population[i].fitnessValue;
                }
            }
            double[] prob = new double[populationSize];
            for (int i = 1; i < populationSize; i++)
            {
                prob[i] = (0.9 * (population[i].fitnessValue / maxfitness)) + 0.1;
            }  
            return prob;
        }

        protected virtual void sendOnlookerBees()
        {
            double[] prob = calculateProbabilities();
            int i = 0, j = 0;  
            while (i < populationSize)
            {
                if (rGen.rnd() <= prob[j])
                {
                    beeSearch(j);
                    i++;
                }
                j++;
                if (j == populationSize) j = 0;
            } 
        }

        protected virtual void sendScoutBees()
        {
            int maxTrials = 0; 
            int index     = -1;
            for (int i = 0; i < populationSize; i++)
            { 
                if(trials[i]>maxTrials)
                {
                    index = i;
                    maxTrials = trials[i];
                }
            }

            if (maxTrials >= limitTrials)
            {
                population[index].generate(rGen, problem.min, problem.max);
                evaluator.evaluate(population[index], population, problem);
                evalCount++;
                trials[index] = 0;
                if (evalCount >= maxEvalCount) throw new EvaluationTermination();
            }

        }

        public override void setOperatorsAndEvaluator()
        {
            this.searchOperator = new ABCOperator();
            this.evaluator = new SOEvaluator();
        }
    }
}
