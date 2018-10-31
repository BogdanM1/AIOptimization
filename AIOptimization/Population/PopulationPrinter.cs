using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AIOptimization.Population
{
        public class PopulationPrinter
        {
            StreamWriter sw;
            public string outputFile; 
            public PopulationPrinter(string outputFile)
            {
                this.outputFile = outputFile;
                sw = new StreamWriter(outputFile);
            }
             
            public void create_headers(Problem.Problem problem)
            {
                string header = "Iteration,";
                for (int i = 0; i < problem.paramsCnt; i++)
                {
                    header += String.Format("X{0},", i + 1);
                }
                for (int i = 0; i < problem.objectivesCnt; i++)
                {
                    header += String.Format("Y{0},", i + 1);

                }
                header += "ID, Fitness";
                sw.WriteLine(header);
            }

            public void printPopulation(int iter, IList<Solution> population, Problem.Problem problem)
            {
                int popSize = population.Count();
                for (int j = 0; j < popSize; j++)
                {
                    sw.Write("    {0},   ", iter);
                    for (int i = 0; i < problem.paramsCnt; i++)
                        sw.Write("{0:0.0000},    ", population[j].parameters[i]);
                                 
                    for (int l = 0; l < problem.objectivesCnt; l++)
                        sw.Write("{0:0.0000},    ", population[j].objectivesValues[l]);
                   
                        sw.Write("{0},    ", population[j].ID);
                        sw.Write("{0:0.0000}\n", population[j].fitnessValue);   
                }
                sw.Flush();
            }

            public void StopPrinter()
            {
                sw.Close();
            }

        }
 }

