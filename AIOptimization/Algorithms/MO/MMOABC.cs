using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AIOptimization.Population;
using AIOptimization.Problem;
using AIOptimization.DominationRelations;

namespace AIOptimization.Algorithms.MO
{
    public class MMOABC:epsMOABC
    {
        double hypboxbound;

        public void setParameters(int populationSize, int maxIterCount, int maxEvalCount, int limitTrials, double basicSearchOperatorProbability,
            double crossoverProbability, double hypboxbound, int maxArchSize)
        {
            base.setParameters(populationSize, maxIterCount, maxEvalCount, limitTrials, basicSearchOperatorProbability, crossoverProbability, maxArchSize);
            this.hypboxbound = hypboxbound;
        }

        public MMOABC() : base(){  this.name = "MMoabc"; }

        public override void nextIter()
        {
            // TESTIRANJE: ili base.nextIter() ili  sendOnlookerBees() za visekriterijumsku?
            // base.nextIter();
            sendOnlookerBees();
            archive.maintain();
        }

        protected override void initialize_archive()  
        { 
            archive = new MMOABCArchive(); 
            archive.maxSize = maxArchSize;
            if (problem.objectivesCnt == 1) archive.maxSize = 1;
            (archive as MMOABCArchive).hypboxbound = hypboxbound;
        }

        protected override void addToPopulation(Solution sol, int index)
        {
            if (sol.fitnessValue > population[index].fitnessValue)
            {
                population[index] = sol;
                trials[index] = 0;
                archive.add(population[index], problem);
                return;
            }
            else
            {
                trials[index]++;
            } 
            bool sol_dominated, pop_dominated;
            for(int i=0; i<populationSize; i++)
            {
                pop_dominated = DominationRelations.DominationRelations.dominates(sol, population[i], problem.objectivesCnt, problem.minimization);
                if (pop_dominated)
                {
                    population[i] = sol;
                    trials[i] = 0;
                    archive.add(population[i], problem);
                    return; 
                }
                sol_dominated = DominationRelations.DominationRelations.dominates(population[i], sol, problem.objectivesCnt, problem.minimization);
                if (sol_dominated) return;
            }
        }
    }
}
