using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
 
namespace AIOptimization.Random
{
    public class RandomGenerator
    {
        System.Random random;

        public RandomGenerator()
        {
            random = new System.Random();
        }

        public RandomGenerator(int seed)
        {
            random = new System.Random(seed);
        }

        public void setSeed(int seed)
        {
            random = new System.Random(seed);
        }

        public double rnd()
        {
            return random.NextDouble();
        }

        public double rnd(double min, double max)
        {
            double value = min + random.NextDouble() * (max - min);
            return value;
        }

        public double rnd(double max)
        {
            double value = random.NextDouble() * max;
            return value;
        }

        public int rnd(int min, int max)
        {
            return random.Next(min, max+1);
        }

        public int rnd( int max)
        {
            return random.Next(max+1);
        }

        public int rndExcl(int max, int excluding)
        {
            int value = 0;
            if (max == 0) return 0;
            value = random.Next(max);
            if (value >= excluding) value++;
            return value;
        }

        public int rndExcl(int min, int max, int excluding)
        {
            int value = 0;
            if (min == max) return min;
            value = min + random.Next(max - min);
            if (value >= excluding) value++;
            return value;
        }

        public int rndExcl(int max, IList<int> excluding)
        {
            int value = 0;
            if (max == 0) return 0;
            value = random.Next(max + 1 - excluding.Count());
            foreach (int ex in excluding)
            {
                if (value < ex) break;
                value++;
            }
            return value;
        }

        public int rndExcl(int min, int max, IList<int> excluding)
        {
            int value = 0;
            if (min == max) return min;
            value = min + random.Next(max - min + 1 - excluding.Count());
            foreach (int ex in excluding)
            {
                if (value < ex) break;
                value++;
            }
            return value;
        }
    }
}
