using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Inspection_App
{
    public class Line
    {
        public float a, b;

        public Line(float a, float b)
        {
            this.a = a;
            this.b = b;
        }
    }
    static public class probabilityLib
    {
        static float avg(float[] x, int size)
        {
            float sum = 0;
            for (int i = 0; i < size; i++)
                sum += x[i];
            return sum / size;
        }

        static float var(float[] x, int size)
        {
            float average = avg(x, size);
            float var = 0;

            for (int i = 0; i < size; i++)
            {
                var += (x[i] - average) * (x[i] - average);
            }

            return (var / size);
        }


        static float cov(float[] x, float[] y, int size)
        {
            float avg_x = avg(x, size);
            float avg_y = avg(y, size);
            float cov = 0;
            for (int i = 0; i < size; i++)
            {
                cov += ((x[i] - avg_x) * (y[i] - avg_y)) / size;
            }

            return cov;
        }

        public static Line linearReg(float[] x, float[] y)
        {
            int size = x.Length;
            float a = cov(x, y, size) / var(x, size);
            float b = avg(y, size) - a * avg(x, size);
            return new Line(a, b);
        }

        public static double pearson(float[] x, float[] y)
        {
            int size = x.Length;
            float covariance = cov(x, y, size);
            double var_x = Math.Sqrt(var(x, size));
            double var_y = Math.Sqrt(var(y, size));
            return covariance / (var_x * var_y);
        }


    }
}
