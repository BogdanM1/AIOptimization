using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AIOptimization.Problem;
using AIOptimization.Problem.TestProblems;
using AIOptimization.Problem.TestProblems.SO;
using AIOptimization.Population;
using AIOptimization.Random;
using AIOptimization.Algorithms;
using AIOptimization.Algorithms.SO;
using AIOptimization.PerformanceIndicators;
using AIOptimization.PerformanceIndicators.SO;

using AIOptimization.ExternalScripts;
using AIOptimization.Algorithms.MO;
 

namespace AIOptimization
{
    class Program
    {
        static void Main(string[] args)
        { 

           /**/ ABC algo1 = new ABC();
           algo1.setParameters(100, Int32.MaxValue, 10000, 10);
           algo1.setRandomGenerator(new RandomGenerator());
           runBenchmarks.runSO(algo1, true, true); /**/

           /** ABCDE algo2 = new ABCDE();
           algo2.setParameters(100, Int32.MaxValue, 10000, 10, 0.7, 0.6);
           algo2.setRandomGenerator(new RandomGenerator());
           runBenchmarks.runSO(algo2, true, true); /**/

           /** GBA algo3 = new GBA();
           algo3.setParameters(100, 100, 30000, 5, 0.01);
           algo3.setRandomGenerator(new RandomGenerator());
           runBenchmarks.runSO(algo3, true, true);/**/

           /** epsMOABC algo4 = new epsMOABC();
           algo4.setParameters(100, Int32.MaxValue, 40000, 10, 0.4, 0.4, 100);
           algo4.setRandomGenerator(new RandomGenerator());
           runBenchmarks.runMO(algo4, true, true);/**/

           /** MMOABC algo5 = new MMOABC();
           algo5.setParameters(100, Int32.MaxValue, 10000, 10, 0.7, 0.6, 0.0075, 400);
           algo5.setRandomGenerator(new RandomGenerator());
           runBenchmarks.runMO(algo5, true, true);/**/

        }
    }
}
