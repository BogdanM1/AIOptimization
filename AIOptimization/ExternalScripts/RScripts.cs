using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO; 

namespace AIOptimization.ExternalScripts
{
    public static class RScripts
    {
        private static string baseDirectory = AppDomain.CurrentDomain.BaseDirectory; // directory with exe [and output files]
        private static string rscriptDrawPopulation = @"drawPopulation.R";
        private static string rscriptDrawIndicators = @"drawIndicators.R";
        public static string rscriptspath = Path.GetFullPath(Path.Combine(baseDirectory, "..\\..\\ExternalScripts\\")); // external scripts directory

        private static void loadFunctions(StreamWriter sw)
        {
            sw.WriteLine("source(\"" + rscriptspath.Replace("\\", "/") + "supportingStructures.R\")");
            sw.WriteLine("source(\"" + rscriptspath.Replace("\\","/") + "drawingFunctions.R\")");
        }

        public static string makeRScriptToDrawPopulation(string algorithmName, Problem.Problem problem, string fOutPopulation, string fOutBestSolutions, string drawRefSolutionsPath)
        {
            string rscript = rscriptspath+rscriptDrawPopulation;
            StreamWriter sw = new StreamWriter(rscript);
            loadFunctions(sw);
            sw.WriteLine("drawPopulation(\"" + fOutPopulation + "\",\"" + fOutBestSolutions + "\",");
            if (drawRefSolutionsPath!=null) 
                 sw.WriteLine("paste(\""+ drawRefSolutionsPath.Replace("\\","/") +"\",\"" + problem.name + ".pf\",sep=''),");
            else sw.WriteLine("NULL,");
            sw.WriteLine("all_iterations,paste(\"" + problem.name + " -\",\""+algorithmName+"\") ,\"" 
                         + problem.name + algorithmName + ".html\",");
            sw.WriteLine("" + problem.paramsCnt + "," + problem.objectivesCnt + ")");
            sw.Close();
            return rscript;
        }

        public static string makeRScriptToDrawIndicators(string algorithmName, Problem.Problem problem, string fOutIndicators)
        {
            string rscript = rscriptspath + rscriptDrawIndicators;
            StreamWriter sw = new StreamWriter(rscript);
            loadFunctions(sw); 
            sw.WriteLine("drawIndicators(\"" + fOutIndicators + "\",paste(\"" + problem.name + " \",\""+algorithmName+"\"),");
            sw.WriteLine("\"ind" + problem.name + algorithmName + ".html\",skip_first_iteration)");
            sw.Close();
            return rscript;
        }

        public static void runScript(string rscript)
        {
            ProcessStartInfo processInfo;
            Process process;
            string command = @"rscript " + rscript;
            processInfo = new ProcessStartInfo("cmd.exe", "/c " + command);
            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = false;
            processInfo.WorkingDirectory = baseDirectory;        
            processInfo.RedirectStandardError = true;
            processInfo.RedirectStandardOutput = true;
            process = Process.Start(processInfo);
            process.WaitForExit();
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            Console.WriteLine("output>>" + (String.IsNullOrEmpty(output) ? "(none)" : output));
            Console.WriteLine("error>>" + (String.IsNullOrEmpty(error) ? "(none)" : error));
            process.Close();
        }
    }
}
