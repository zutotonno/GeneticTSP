using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESPprogetto
{

    public class MyPopulation
    {
        public int size { get; }
        public List<TourChromosome> population { get; set; }
        public MyPopulation(int size, TourChromosome adam)
        {
            this.size = size;
            population = new List<TourChromosome>(size);
            createPopulation(adam);

        }

        public MyPopulation(int size)
        {
            population = new List<TourChromosome>(size);

            this.size = size;
        }

        public MyPopulation(int size, Matrix<double> cityTour ,List<string> optPop, TourChromosome adam)
        {
            this.size = size;
            population = new List<TourChromosome>(size);
            createOPTPopulation(cityTour,optPop, adam);
        }

        private void createOPTPopulation(Matrix<double> cityTour,List<string> optPop, TourChromosome adam)
        {
            int i = 0;
            Vector<double> xP = cityTour.Row(0);
            Vector<double> yP = cityTour.Row(1);
            var chromoAUX = new TourChromosome(adam.lenght, adam.distanceMatrix, xP, yP);
            for (int j = 0; j < adam.lenght; j++)
            {
                Gene aux = new Gene(cityTour[0, j], cityTour[1, j], optPop[j], int.Parse(optPop[j]));
                chromoAUX.ReplaceGene(j, aux);
            }
            var percentageCross = 0.3;
            var stopCopy = (int)Math.Round(size * percentageCross);
            for (i = 0; i < stopCopy; i++)
            {
                chromoAUX.calculateFitness();
                population.Add(chromoAUX);

            }
            MyCrossover myc = new MyCrossover(1);
            while (i < size)
            {
                var shuffled = FisherYateShuffle(chromoAUX);
                shuffled.calculateFitness();
                population.Add(shuffled);
                i++;
            }
        }



        private void createPopulation(TourChromosome adam)
        {
            for (int i = 0; i < size; i++)
            {
                var shuffled = FisherYateShuffle(adam);
                population.Add(shuffled);
            }
        }



        private TourChromosome FisherYateShuffle(TourChromosome adam)
        {
            var chromoAUX = new TourChromosome(adam.lenght, adam.distanceMatrix, adam.xPoints, adam.yPoints);

            for (int i = adam.lenght - 1; i >= 0; i--)
            {
                var random = Math.Abs(Guid.NewGuid().GetHashCode());
                var r = new Random(random);
                var j = r.Next(i);
                Gene aux = chromoAUX.GetGene(i);
                chromoAUX.ReplaceGene(i, chromoAUX.GetGene(j));
                chromoAUX.ReplaceGene(j, aux);
            }
            return chromoAUX;
        }
    }
}
