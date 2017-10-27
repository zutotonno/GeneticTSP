using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESPprogetto
{
    public class MyMutation
    {
        public static double percentage { get; set; }

        public MyMutation(double p)
        {
            percentage = p;
        }

        /*
         * Prima di effettuare la futura Selection e il Crossover sulla popolazione al tempo T+1, sulla popolazione al tempo T vengono indotte
         * delle mutazioni random con la percentuale decisa per previnire stalli sui minimi locali.
         * In questo caso la mutazione comporta uno swapping casuale tra due città nel tour.
         * @param : popolazione al tempo T
         * return : void
         */
        public void Mutate(MyPopulation p, int[] selected)
        {
            MyPopulation newPopulation = p;
            for (int i = 0; i < p.population.Count; i++)
            {
                var random = Math.Abs(Guid.NewGuid().GetHashCode());
                var r = new Random(random);
                var rand = r.NextDouble();
                if (rand < percentage)
                {
                    if (!selected.Contains(i))
                    {
                        var j = r.Next(p.population[i].lenght);
                        var swapIdx = r.Next(p.population[i].lenght);
                        var auxX = p.population[i].GetGene(swapIdx).getXValue();
                        var auxY = p.population[i].GetGene(swapIdx).getYValue();
                        var name = p.population[i].GetGene(swapIdx).getName();
                        var id = p.population[i].GetGene(swapIdx).getID();
                        Gene swapGene = new Gene(auxX, auxY, name, id);
                        Gene jGene = p.population[i].GetGene(j);
                        p.population[i].ReplaceGene(swapIdx, jGene);
                        p.population[i].ReplaceGene(j, swapGene);

                    }
                }
                p.population[i].calculateFitness();
            }
        }

        /*
          * Prima di effettuare la futura Selection e il Crossover sulla popolazione al tempo T+1, sulla popolazione al tempo T vengono indotte
          * delle mutazioni random con la percentuale decisa per previnire stalli sui minimi locali.
          * In questo caso la mutazione comporta, per ogni cromosoma e per ogni gene, uno swapping casuale tra due città nel tour.
          * @param : popolazione al tempo T
          * return : void
          */
        public void HeavyMutation(MyPopulation p, int[] selected)
        {
            MyPopulation newPopulation = p;
            for (int i = 0; i < p.population.Count; i++)
            {

                if (!selected.Contains(i))
                {
                    for (int k = 0; k < p.population[i].lenght; k++)
                    {
                        var random = Math.Abs(Guid.NewGuid().GetHashCode());
                        var r = new Random(random);
                        var rand = r.NextDouble();
                        if (rand < percentage)
                        {
                            var j = r.Next(p.population[i].lenght);
                            var swapIdx = r.Next(p.population[i].lenght);
                            var auxX = p.population[i].GetGene(swapIdx).getXValue();
                            var auxY = p.population[i].GetGene(swapIdx).getYValue();
                            var name = p.population[i].GetGene(swapIdx).getName();
                            var id = p.population[i].GetGene(swapIdx).getID();
                            Gene swapGene = new Gene(auxX, auxY, name, id);
                            Gene jGene = p.population[i].GetGene(j);
                            p.population[i].ReplaceGene(swapIdx, jGene);
                            p.population[i].ReplaceGene(j, swapGene);
                        }

                    }
                }
                p.population[i].calculateFitness();
            }
        }
    }
}
