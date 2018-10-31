using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using AIOptimization.Problem;
using AIOptimization.Population; 

namespace AIOptimization.Algorithms.SO
{
    public class GBA:PopulationOptimizationAlgorithm
    {
        protected double[] fgn_radius;
        protected bool set_radius;
        protected double   fgn_radius_tmp; 
        protected int ngroups;

        
        protected double scout_bees_K_param { get; set; }
        protected int[] group_sizes { get; set; }
        protected int[] recruited_bees_per_group { get; set; }
        protected double[] A_param { get; set; }
        protected double[] B_param { get; set; }
        protected double[][] neighbourhood { get; set; }
        protected int rnd_group_size { get; set; }

        protected Solution[] sorted_population;

        public GBA():base()
        {
            this.name = "GBA";
        }

        public void setParameters(int populationSize, int maxIterCount, int maxEvalCount, int ngroups, double fgn_radius)
        {
            base.setParameters(populationSize, maxIterCount, maxEvalCount);
            this.ngroups = ngroups;
            this.fgn_radius_tmp = fgn_radius;
            set_radius = true;
        }

        public void setParameters(int populationSize, int maxIterCount, int maxEvalCount, int ngroups, double[] fgn_radius)
        {
            base.setParameters(populationSize, maxIterCount, maxEvalCount);
            this.ngroups = ngroups;
            set_radius = false;
            int n = fgn_radius.Count();
            this.fgn_radius = new double[n];
            for (int i = 0; i < n; i++ )
                this.fgn_radius[i] = fgn_radius[i];
        }

        public override void setOperatorsAndEvaluator()
        {
            this.evaluator = new SOEvaluator();
            this.searchOperator = new GBAOperator();
        }

        public override void init()
        {
            base.init();
            if (set_radius)
            {
                this.fgn_radius = new double[problem.paramsCnt];
                for (int i = 0; i < problem.paramsCnt; i++) fgn_radius[i] = fgn_radius_tmp;
            }
            sorted_population = population.OrderByDescending(b => b.fitnessValue).ToArray();
            int scout_bees_K_param_denominator = (ngroups + 1) * (ngroups + 1)
                                                   * (ngroups + 1) - 1;

            scout_bees_K_param = 3.0 * populationSize / scout_bees_K_param_denominator;
            group_sizes = new int[ngroups];
            recruited_bees_per_group = new int[ngroups];
            neighbourhood = new double[ngroups][];
            A_param = new double[problem.paramsCnt];
            B_param = new double[problem.paramsCnt];
            for (int i = 0; i < problem.paramsCnt; i++)
            {
                double value = problem.max[i] - problem.min[i];
                A_param[i] = (value / 2 - fgn_radius[i])
                            / (ngroups * ngroups - 1);
                B_param[i] = fgn_radius[i] - A_param[i];
            }

            for (int i = 0; i < ngroups; i++)
            {
                neighbourhood[i] = new double[ngroups];
                group_sizes[i] = (int)Math.Floor(scout_bees_K_param * (i + 1) * (i + 1));
                if (group_sizes[i] <= 0) group_sizes[i] = 1;
                recruited_bees_per_group[i] = (ngroups - i) * (ngroups - i);
                for (int j = 0; j < problem.paramsCnt; j++)
                {
                    neighbourhood[i][j] = A_param[j] * (i + 1) * (i + 1) + B_param[j];
                }
            }
            rnd_group_size = populationSize - group_sizes.Sum();
            if (rnd_group_size < 0) rnd_group_size = 0;
            (searchOperator as GBAOperator).setNeighbourhoodSize(neighbourhood, ngroups, problem.paramsCnt);
            print_parameters();
        }

        public override void nextIter()
        {
            int bee_index = -1;
            for (int groupIndex = 0; groupIndex < ngroups; groupIndex++)
            {
                (searchOperator as GBAOperator).group_index = groupIndex;
                for (int beegroupIndex = 0; beegroupIndex < group_sizes[groupIndex]; beegroupIndex++)
                {
                    bee_index++;
                    for (int recruitedbeeIndex = 0; recruitedbeeIndex < recruited_bees_per_group[groupIndex]; recruitedbeeIndex++)
                    {
                        Solution solution = searchOperator.search(rGen, problem, sorted_population, populationSize, bee_index);
                        evaluator.evaluate(solution, sorted_population, problem);
                        evalCount++;
                        if (evalCount >= maxEvalCount) throw new EvaluationTermination();
                        if (solution.fitnessValue > sorted_population[bee_index].fitnessValue)
                        {
                            sorted_population[bee_index] = solution;
                        }
                    }
                }
            }

            for (int rnd_groupbeeindex = 0; rnd_groupbeeindex < rnd_group_size; rnd_groupbeeindex++)
            {
                bee_index++;
                sorted_population[bee_index].generate(rGen, problem.min, problem.max);
                evalCount++;
                if (evalCount >= maxEvalCount) throw new EvaluationTermination();
                evaluator.evaluate(sorted_population[bee_index],sorted_population, problem);
            }

            sorted_population = sorted_population.OrderByDescending(b => b.fitnessValue).ToArray();
        }

        public override IList<Solution> bestSolutions()
        {
            return new List<Solution> {sorted_population[0]};
        }

        public void print_parameters()
        {
            StreamWriter sw = new StreamWriter("GroupedBeeParameters.txt");
            sw.WriteLine("group sizes:");
            for (int i = 0; i < ngroups; i++) sw.WriteLine("{0} ", group_sizes[i]);
            sw.WriteLine();
            sw.WriteLine("recruited bees per group:");
            for (int i = 0; i < ngroups; i++) sw.WriteLine("{0} ", recruited_bees_per_group[i]);
            sw.WriteLine();
            sw.WriteLine("rnd_group_size:");
            sw.WriteLine("{0} ", rnd_group_size);
            sw.WriteLine();
            sw.WriteLine(" neighbourhood");
            for (int i = 0; i < ngroups; i++)
            {
                for (int j = 0; j < problem.paramsCnt; j++)
                    sw.WriteLine("{0} ", neighbourhood[i][j]);
                sw.WriteLine();
            }
            sw.WriteLine();
            sw.Close();
        }
    }
}
