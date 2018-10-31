using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AIOptimization.Population;
 
namespace AIOptimization.Algorithms.SO
{ 
    public class ABCDE:ABC
    {
        public double basicSearchOperatorProbability = 0.4;
        public double crossoverProbability = 0.4;
        public SearchOperator secondarySearchOperator;

        public ABCDE(): base()
        {
            this.name = "ABCDE";
        }

        public void setParameters(int populationSize, int maxIterCount, int maxEvalCount, int limitTrials,
                                  double basicSearchOperatorProbability, double crossoverProbability)
        {
            base.setParameters(populationSize, maxIterCount, maxEvalCount, limitTrials);
            this.basicSearchOperatorProbability = basicSearchOperatorProbability;
            this.crossoverProbability = crossoverProbability;
        }

        public override void setOperatorsAndEvaluator()
        {
            base.setOperatorsAndEvaluator();
            this.secondarySearchOperator = new ABCDEOperator();
            (secondarySearchOperator as ABCDEOperator).crossOverProbab = crossoverProbability; 
           
        }

        protected virtual void beeSearch(int index, bool basic)
        {
            Solution newSol = null;
            if (basic || rGen.rnd() <= basicSearchOperatorProbability)
            {
                newSol = searchOperator.search(rGen, problem, population, populationSize, index);
            }
            else
            {
                newSol = secondarySearchOperator.search(rGen, problem, population, populationSize, index);
            }
            evaluator.evaluate(newSol, population, problem);
            evalCount++;
            addToPopulation(newSol, index);
            if (evalCount >= maxEvalCount) throw new EvaluationTermination();
        }

        protected override void sendEmployedBees()
        {
            for (int i = 0; i < populationSize; i++)
            {
                beeSearch(i, true);
            }
        }

        protected override void sendOnlookerBees()
        {
            double[] prob = calculateProbabilities();
            int i = 0, j = 0;
            while (i < populationSize)
            {
                if (rGen.rnd() <= prob[j])
                {
                    beeSearch(j, false);
                    i++;
                }
                j++;
                if (j == populationSize) j = 0;
            }
        }
    }
}
