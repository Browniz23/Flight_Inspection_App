using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flight_Inspection_App
{
    public class Line
    {
        public double a, b;

        public Line(double a, double b)
        {
            this.a = a;
            this.b = b;
        }
    }
    static public class probabilityLib
    {
        static double avg(double[] x, int size)
        {
            double sum = 0;
            for (int i = 0; i < size; i++)
                sum += x[i];
            return sum / size;
        }

        static double var(double[] x, int size)
        {
            double average = avg(x, size);
            double var = 0;

            for (int i = 0; i < size; i++)
            {
                var += (x[i] - average) * (x[i] - average);
            }

            return (var / size);
        }


        static double cov(double[] x, double[] y, int size)
        {
            double avg_x = avg(x, size);
            double avg_y = avg(y, size);
            double cov = 0;
            for (int i = 0; i < size; i++)
            {
                cov += ((x[i] - avg_x) * (y[i] - avg_y)) / size;
            }

            return cov;
        }

        public static Line linearReg(double[] x, double[] y)
        {
            int size = x.Length;
            double a = cov(x, y, size) / var(x, size);
            double b = avg(y, size) - a * avg(x, size);
            return new Line(a, b);
        }

        public static double pearson(double[] x, double[] y)
        {
            int size = x.Length;
            double covariance = cov(x, y, size);
            double var_x = Math.Sqrt(var(x, size));
            double var_y = Math.Sqrt(var(y, size));
            return covariance / (var_x * var_y);
        }


    }
}
