using System;
using System.Collections.Generic;

namespace ControlMod2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            double eps = 0.001;
            double a = -1;
            double b = 1;

            Console.WriteLine("Метод дихотомии:\nМинимальное значение: " + dihot(a, b, 0.0001, function(1)).Item1 + "\nИтераций: " + dihot(a, b, 0.0001, function(1)).Item2 + "\n");
            Console.WriteLine($"Метод золотого сечения:\nМинимальное значение: {GoldenRation(a, b, eps).Item1} \nИтераций: {GoldenRation(a, b, eps).Item2}\n");
            Console.WriteLine($"Метод средней точки: {Midpoint(a, b, eps).Item1}\nИтераций: {Midpoint(a, b, eps).Item2}");
            Console.WriteLine($"Метод хорд: {Secant(a, b, eps).Item1}\nИтераций: {Secant(a, b, eps).Item2}");
            Console.WriteLine($"Метод Ньютона: {Newton(a, b, eps).Item1}\nИтераций: {Newton(a, b, eps).Item2}");

            double x = 2;
            double y = 1;

            Console.WriteLine($"Градиентный спуск/Метод наискорейшего спуска: {gradientDescent(x, y, eps).Item1} {gradientDescent(x, y, eps).Item2} \nИтераций: {gradientDescent(x, y, eps).Item3}");
            Console.WriteLine($"Метод сопряжённых градиентов: {conjugateGradients(x, y, eps).Item1} {conjugateGradients(x, y, eps).Item2} \nИтераций: {conjugateGradients(x, y, eps).Item3}");

            Zadanie3();
            Zadanie4();
            double m = 2;
            double n = 2;
            Zadanie5(m,n);
            Zadanie6(m,n);
           
        }

        static double function(double x) => Math.Pow(Math.Sin(x), 2) + Math.Pow(Math.E, x); //вар22

        static double functionDerivative(double x) => Math.Sin(2 * x) + Math.Pow(Math.E, x); //Производная функции

        static double functionDoubleDerivative(double x) => 2 * Math.Cos(2 * x) + Math.Pow(Math.E, x); //Вторая производная функции
        static (double, int) dihot(double a, double b, double eps, double f)
        {
            int k = 0;
            double c, f1, f2 = 0;

            while (Math.Abs(a - b) > eps)
            {
                k++;
                c = (a + b) / 2;
                f1 = function(c - eps);
                f2 = function(c + eps);
                if (f1 > f2)
                    a = c;
                else if (f2 > f1)
                    b = c;
                else
                {
                    a = c - eps;
                    b = c + eps;
                }
            }
            return ((a + b) / 2, k);
        }

        static (double, int) GoldenRation(double a, double b, double eps)
        {
            int k = 0;
            while (Math.Abs(b - a) > eps)
            {
                double x1 = b - (b - a) / 1.618;
                double x2 = a + (b - a) / 1.618;
                double y1 = function(x1);
                double y2 = function(x2);
                if (y1 >= y2)
                    a = x1;
                else
                    b = x2;
                k++;
            }
            return ((a + b) / 2, k);
        }


        static (double, int) Midpoint(double a, double b, double eps)
        {
            double x = (a + b) / 2;
            double a0 = a;
            double b0 = b;
            int k = 0;
            while (Math.Abs(functionDerivative(x)) > eps)
            {
                k++;
                if (functionDerivative(x) > 0)
                    b0 = x;
                else
                    a0 = x;
                x = (a0 + b0) / 2;
            }
            return (x, k);
        }
        static (double, int) Secant(double a, double b, double eps)
        {
            List<double> x = new List<double>() { a, b };
            int k = x.Count - 1;
            double tmp;
            while (Math.Abs(x[k - 1] - x[k]) > eps)
            {
                double test = Math.Abs(x[k - 1] - x[k]);
                tmp = x[k] - functionDerivative(x[k]) * (x[k - 1] - x[k]) / (functionDerivative(x[k - 1]) - functionDerivative(x[k]));
                x.Add(tmp);
                k++;
            }
            return (x[k], k);
        }
        static (double, int) Newton(double a, double b, double eps)
        {
            //гайд: https://math.stackexchange.com/questions/1900664/using-newton-raphson-method-to-approximate-the-minimum-value-of-the-function

            List<double> x = new List<double>() { 0, a };
            int k = 1;
            double tmp = 0;
            while (Math.Abs(x[k - 1] - x[k]) > eps)
            {
                tmp = x[k] - (functionDerivative(x[k])) / (functionDoubleDerivative(x[k]));
                k++;
                x.Add(tmp);
            }
            return (x[k], k);
        }

        static double function2(double x, double y) => 9 * x + 7 * y - 9 * Math.Pow(x, 2) - 3 * Math.Pow(y, 2) + 1;
        static (double, double) Gradient(double x, double y) => (x - 18 * x, 7 - 6 * y);
        static (double, double, int) gradientDescent(double x, double y, double eps)
        {
            //гайд: https://ru.wikipedia.org/wiki/Градиентный_спуск

            List<double> xList = new List<double>() { x };
            List<double> yList = new List<double>() { y };
            double tmp1 = int.MaxValue;
            double tmp2 = int.MaxValue;
            int k = 0;
            do
            {
                k++;
                tmp1 = xList[xList.Count - 1] - 0.001 * (Gradient(xList[xList.Count - 1], yList[yList.Count - 1]).Item1 * -1);
                tmp2 = yList[yList.Count - 1] - 0.001 * (Gradient(xList[xList.Count - 1], yList[yList.Count - 1]).Item2 * -1);

                xList.Add(tmp1);
                yList.Add(tmp2);
            }
            while (Math.Abs(tmp1 - xList[xList.Count - 2]) > eps);
            return (xList[xList.Count - 1], yList[yList.Count - 1], k);
        }
        static (double, double, int) conjugateGradients(double x, double y, double eps)
        {

            int k = 0;
            double x0 = x;
            List<double> xList = new List<double>() { x };
            List<double> yList = new List<double>() { y };
            double tmpX = int.MaxValue;
            double tmpY = int.MaxValue;
            double tmpDX = int.MaxValue;
            double tmpDY = int.MaxValue;
            do
            {
                double gX = Gradient(xList[xList.Count - 1], yList[yList.Count - 1]).Item1;
                double gY = Gradient(xList[xList.Count - 1], yList[yList.Count - 1]).Item2;

                double dX = -1 * gX;
                double dY = -1 * gY;

                tmpX = x + 0.001 * dX;
                tmpY = y + 0.001 * dY;
                xList.Add(tmpX);
                yList.Add(tmpY);

                k++;
            }
            while (Math.Abs(xList[xList.Count - 1] - xList[xList.Count - 2]) > eps);

            return (xList[xList.Count - 1], yList[yList.Count - 1], k);
        }
        static void Zadanie3()
        {
            int a1 = 2;
            int a2 = 3;
            int a3 = 3;

            int d1 = 1;
            int d2 = 6;
            int d3 = 7;

            int b1 = 438;
            int b2 = 747;
            int b3 = 812;

            int aC = 7000;
            int dC = 5000;

            int sum = 0;
            while (b1 >= a1 && b2 >= a2 && b3 >= a3)
            {
                b1 -= a1;
                b2 -= a2;
                b3 -= a3;
                sum += aC;
            }
            while (b1 >= d1 && b2 >= d2 && b3 >= d3)
            {
                b1 -= d1;
                b2 -= d2;
                b3 -= d3;
                sum += dC;
            }
            Console.WriteLine("Задача 3: " + sum);
        }
        static void Zadanie4()
        {
            const int rows = 4;
            const int columns = 4;
            int[,] matrix = new int[rows, columns] { {5,3,1,9 },
                                                     {3,5,4,8},
                                                     {4,7,2,7 },
                                                     {0,0,0,0 } };
            int cost = 0;
            int[] needs = new int[] { 210, 100, 100, 210 };
            int[] stacks = new int[] {110,150,70, };
            stacks = new int[] { 110, 150, 70, needs.Sum()-stacks.Sum()};
            for(int i = 0; i < rows-1;)
            {
                for(int j = 0; j < columns-1;)
                {
                    cost += matrix[i,j];
                    needs[j] -= -10;
                    stacks[i] -= 10;
                    break;
                }
                cost = matrix[0,0] + matrix[0, 1] + matrix[1, 0] + matrix[2, 0] +
                    matrix[2, 2] + matrix[3, 2] + matrix[3, 3];
                for (int n = 0; n < needs.Length;n++)
                    needs[n] = 0;
                for (int s = 0; s < stacks.Length; s++)
                    stacks[s] = 0;
                break;    
            }
            Console.WriteLine($"Задача 4: {cost}");
        }
        static void Zadanie5(double m, double n)
        {

            double func1(double m, double n) => (n+1)*2+(n+3)*3;
            double func2(double m, double n) => (n+3)*2+(n+1)*3;
            Console.WriteLine($"Функция Гомори при m={m} и n={n} равна: {func1(m, n) + func2(m, n) - 0.1 * func1(m, n) + func2(m, n)}");

        }
        static void Zadanie6(double m, double n)
        {
            double func1(double m, double n) =>  1 + (n + 1) * 3;
            double func2(double m, double n) => (m + 2) * 1 + 3;
            Console.WriteLine($"Функция Лангранджа при m={m} и n={n} равна: {func1(m, n) + func2(m, n) - 0.1 * func1(m, n) + func2(m, n)}");
        }
    }
}
