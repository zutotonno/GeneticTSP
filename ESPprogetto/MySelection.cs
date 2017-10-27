using System;
using System.Collections.Generic;
using System.Linq;

namespace ESPprogetto
{
    public class MySelection
    {
        public static int fittestDim { get; set; }

        public MySelection(int dim)
        {
            fittestDim = dim;
        }
        /*
         * Dato un cromosoma X la RouletteSelection sceglie il cromosoma X 
         * come parte della futura generazione al tempo T+1 in base a quanto
         * la sua fitness incide sulla fitness totale della popolazione al tempo T.
         * Con questo tipo di selection un cromosoma con alta fitness ha alte probabilità
         * di venire scelto più volte come sopravvisuto.
         * @param pop: popolazione al tempo T
         * @return : indici degli individui sopravvisuti
         */
        public static int[] RouletteSelection(MyPopulation pop)
        {

            double weight_sum = 0;
            double dist_sum = 0;
            for (int i = 0; i < pop.size; i++)
            {
                weight_sum += pop.population[i].fitness;
                dist_sum += pop.population[i].distance;
            }
            var survivors = new int[fittestDim];
            // Ordino la popolazione in base alla fitness
            var sorted = pop.population.OrderByDescending(x => x.fitness);
            int idx = 0;
            foreach (TourChromosome t in sorted)
            {
                pop.population[idx] = t;
                idx++;
            }
            //</Fine Sorting>

            for (int j = 0; j < fittestDim; j++)
            {
                var random = Math.Abs(Guid.NewGuid().GetHashCode());
                var r = new Random(random);
                var rand = r.Next(pop.size);
                double value = Math.Abs(weight_sum/rand);
                int i = 0;
                for (i = 0; i < pop.size; i++)
                {
                    value -= pop.population[i].fitness;
                    if (value <= 0)
                    {
                        survivors[j] = i;
                        break;
                    }
                }
            }
            survivors.OrderBy(x => x);
            int[] sortedCopy = survivors.OrderBy(i => i).ToArray();
            return sortedCopy;
        }
    }
}