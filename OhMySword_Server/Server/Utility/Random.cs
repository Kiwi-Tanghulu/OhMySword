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

        public static float Range(float min, float max)
        {
            return min + (random.Next() % (max - min));
        }

        public static Vector3 InCircle(float radius)
        {
            float angle = Range(0, 360f) * DEFINE.Deg2Rad;
            return (new Vector3(MathF.Cos(angle), MathF.Sin(angle), 0f) * radius);
        }
    }
}
