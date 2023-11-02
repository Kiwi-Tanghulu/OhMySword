using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Random
    {
        private static System.Random random = new System.Random();

        public static int Range(int min, int max)
        {
            return min + (random.Next() % (max - min));
        }
    }
}
