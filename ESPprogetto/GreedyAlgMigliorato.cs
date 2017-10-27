using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESPprogetto
{
    class GreedyAlgMigliorato
    {
        Tuple<Matrix<double>, Vector<double>, Vector<double>> distanceMatrix;
        int numCities, poison;
        double dist;
        Matrix<double> cityT;
        public GreedyAlgMigliorato(Tuple<Matrix<double>, Vector<double>, Vector<double>> dm, int n, int p)
        {
            distanceMatrix = dm;
            numCities = n;
            poison = p;
        }

        public void Start(int next)
        {
            var tour = new List<string>();
            double distance = 0;
            var distanceM = Matrix<double>.Build.DenseOfMatrix(distanceMatrix.Item1);
            var cityTour = Matrix<double>.Build.Dense(2, distanceM.ColumnCount + 1);

            var start = next;
            var prev = next;
            tour.Add("X: " + distanceMatrix.Item2[next].ToString() + " Y: " + distanceMatrix.Item3[next].ToString());
            cityTour[0, 0] = distanceMatrix.Item2[next];
            cityTour[1, 0] = distanceMatrix.Item3[next];
            //Console.WriteLine("\nX: " + distanceMatrix.Item2[next].ToString() + " Y: " + distanceMatrix.Item3[next].ToString() + " dist: " + distance);
            int i = 1;
            while (!(distanceM.Row(next).Min() == poison))
            {
                prev = next;
                distance += distanceM.Row(next).Min();
                for (int j = 0; j < numCities; j++)
                {
                    distanceM[j, next] = poison;
                }
                next = distanceM.Row(next).MinimumIndex();
                tour.Add("X: " + distanceMatrix.Item2[next].ToString() + " Y: " + distanceMatrix.Item3[next].ToString());
                cityTour[0, i] = distanceMatrix.Item2[next];
                cityTour[1, i] = distanceMatrix.Item3[next];
                i++;
                //Console.WriteLine("prev:" + prev + " next:" + next);
                //Console.WriteLine("X: " + distanceMatrix.Item2[next].ToString() + " Y: " + distanceMatrix.Item3[next].ToString() + " dist: " + distance);
            }
            tour.Add("X: " + distanceMatrix.Item2[start].ToString() + " Y: " + distanceMatrix.Item3[start].ToString());
            cityTour[0, i] = distanceMatrix.Item2[start];
            cityTour[1, i] = distanceMatrix.Item3[start];
            distance += distanceMatrix.Item1[next, start];
            cityT = cityTour;
            dist = distance;
        }

        public Matrix<double> getCityTour()
        {
            return cityT;
        }
        public double getDistance()
        {
            return dist;
        }
    }
}
