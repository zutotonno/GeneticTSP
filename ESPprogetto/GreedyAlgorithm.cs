using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESPprogetto
{
    class GreedyAlgorithm
    {
        Tuple<Matrix<double>,Vector<double>,Vector<double>> distanceMatrix;
        int numCities,poison;
        double dist;
        Tuple<Matrix<double>,List<string>> cityT;
        double totalTIME;
        public GreedyAlgorithm(Tuple<Matrix<double>, Vector<double>, Vector<double>> dm,int n,int p)
        {
            distanceMatrix = dm;
            numCities = n;
            poison = p;
        }
        /*
         * Partendo dal nodo next si scelgono i nodi successivi da visitare in base all'algoritmo
         * del nodo più vicino(Nearest Neighboorhood) avendo cura di avvelenare i percorsi già battuti
         * @param: nodo di partenza
         * return : void
         */
        public void Start(int next)
        {
            var startTime = System.DateTime.Now;
            var tour = new List<string>();
            double distance = 0;
            var distanceM = Matrix<double>.Build.DenseOfMatrix(distanceMatrix.Item1);
            var cityTour = Matrix<double>.Build.Dense(2, distanceM.ColumnCount + 1);

            var start = next;
            var prev = next;
            tour.Add(next.ToString());
            cityTour[0, 0] = distanceMatrix.Item2[next];
            cityTour[1, 0] = distanceMatrix.Item3[next];
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
                tour.Add(next.ToString());
                cityTour[0, i] = distanceMatrix.Item2[next];
                cityTour[1, i] = distanceMatrix.Item3[next];
                i++;
            }
            tour.Add(start.ToString());
            cityTour[0, i] = distanceMatrix.Item2[start];
            cityTour[1, i] = distanceMatrix.Item3[start];
            distance += distanceMatrix.Item1[next, start];
            cityT = Tuple.Create(cityTour,tour);
            dist = distance;
            var endTime = System.DateTime.Now.Subtract(startTime);
            totalTIME = endTime.TotalMilliseconds;
        }

        public Matrix<double> GetCityTour()
        {
            return cityT.Item1;
        }

        public List<string> Tour()
        {
            return cityT.Item2;
        }
        public double GetDistance()
        {
            return dist;
        }
        public double GetTime()
        {
            return totalTIME;
        }
    }
}
