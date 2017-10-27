using Encog.Util.CSV;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Drawing;

namespace ESPprogetto
{
    internal class MatrixGenerator
    {
        public static Tuple<Matrix<double>,Vector<double>,Vector<double>> Build(int seed,int w,int poison)
        {
            var r = new Random();
            var xCities = Vector<double>.Build.Dense(w, (i) => ((double)r.NextDouble())*seed);
            var yCities = Vector<double>.Build.Dense(w, (i) => ((double)r.NextDouble())*seed);
            var matrixDistance = Matrix<double>.Build.Dense(w, w, poison);
            String sourceCity = "source" + w + ".csv";
            var csv = new ReadCSV(sourceCity, false, ' ');
            csv.Next();
            {
                var line = new String[w];
                for (int j = 0; j < (line.Length); j++)
                {
                    xCities[j] = double.Parse(csv.Get(j))*seed;
                }
            }
            while (csv.Next())
            {
                var line = new String[w];
                for (int j = 0; j < (line.Length); j++)
                {
                    yCities[j] = double.Parse(csv.Get(j))*seed;
                }
            }

            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    if (i != j)
                    {
                        var xDist = Math.Pow((xCities[i] - xCities[j]), 2);
                        var yDist = Math.Pow((yCities[i] - yCities[j]), 2);
                        matrixDistance[i, j] = Math.Sqrt(xDist + yDist);
                    }
                }
            }
            var distances = new Tuple<Matrix<double>, Vector<double>, Vector<double>>(
                matrixDistance,
                xCities,
                yCities
                );
            return distances;
        }
        public static Tuple<Matrix<double>, Vector<double>, Vector<double>> Build(string name, int w, int poison)
        {
            var r = new Random();
            var xCities = Vector<double>.Build.Dense(w, 0);
            var yCities = Vector<double>.Build.Dense(w, 0);
            var matrixDistance = Matrix<double>.Build.Dense(w, w, poison);
            String sourceCity = name + w + ".csv";
            var seed = 1.0;
            if (name.Equals("berlin"))
                seed = 0.1;
            var csv = new ReadCSV(sourceCity, false, ' ');
            int k = 0;
            while (csv.Next())
            {
                var line = new String[w];
                xCities[k] = double.Parse(csv.Get(1))*seed;
                yCities[k] = double.Parse(csv.Get(2))*seed;
                Console.WriteLine(xCities[k] + " " + yCities[k]);
                k++;

            }

            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    if (i != j)
                    {
                        var xDist = Math.Pow((xCities[i] - xCities[j]), 2);
                        var yDist = Math.Pow((yCities[i] - yCities[j]), 2);
                        matrixDistance[i, j] = Math.Sqrt(xDist + yDist);
                    }
                }
            }
            var distances = new Tuple<Matrix<double>, Vector<double>, Vector<double>>(
                matrixDistance,
                xCities,
                yCities
                );
            return distances;
        }
    }
}