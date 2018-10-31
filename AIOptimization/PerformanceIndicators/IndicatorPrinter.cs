using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AIOptimization.PerformanceIndicators
{
    public class IndicatorPrinter
    {
        StreamWriter sw;
        public String outputFile;
        public IndicatorPrinter(String file, String header)
        {
            this.outputFile = file;
            sw = new StreamWriter(outputFile);
            sw.WriteLine("Iteration," + header);
        }

        public void print(int iter, IList<double> values)
        { 
            int count = values.Count();
            sw.Write("    {0},   ", iter);
            for (int i = 0; i < count - 1;i++ ) sw.Write("{0:0.0000},    ", values[i]);
            sw.WriteLine("{0:0.0000}", values[count-1]);
            sw.Flush();
        }

        public void StopPrinter()
        {
            sw.Close();
        }
    }
}
