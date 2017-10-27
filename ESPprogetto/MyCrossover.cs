using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;

namespace ESPprogetto
{
    internal class MyCrossover
    {
        public static double probability { get; set; }
        public MyCrossover(double prob)
        {
            probability = prob;
        }


        /* Dopo la Selection che estrae una sotto popolazione di individui con alta fitness ,restituendone
         * l'indice nella popolazione attuale, vengono scelti in questa sottopopolazione due individui che 
         * andranno incrociati con un opportuna funzione di crossover per ottenere un cromosoma figlio
         * @param survivors : array di indici dei sopravvisuti
         * @return : indici di due genitori
         */
        public static Tuple<int,int> Cross(int[] survivors)
        {
            var selected = false;
            int g1=0, g2=0;
            while (!selected)
            {
                var random = Math.Abs(Guid.NewGuid().GetHashCode());
                var r = new Random(random);
                var rand = r.NextDouble();
                if (rand < probability)
                {
                    var r1 = r.Next(survivors.Length);
                    var r2 = r.Next(survivors.Length);
                    g1 = survivors[r1];
                    g2 = survivors[r2];
                    selected = true;
                }

            }
            return Tuple.Create(g1, g2);
        }


        /*
         * Questo crossover va a fare una ricerca di minimo locale nello spazio di soluzioni offerte 
         * dai due genitori indicati da idx1 e idx2. Il cromosoma figlio è costruito gene per gene
         * valutando quale sottosequenza di geni sia quella con la più alta fitness(minor distanza)
         * @param : Popolazione al tempo T, indice Genitore1, indice Genitore2
         * return : Cromosoma figlio
         */


        public static TourChromosome SCXcrossover(MyPopulation p, int idx1, int idx2)
        {
            var g1 = p.population[idx1];
            var g2 = p.population[idx2];
            var chromosomeSon = new TourChromosome(g1.lenght, g1.distanceMatrix, g1.xPoints, g1.yPoints);
            int numTour = 0;
            var random = Math.Abs(Guid.NewGuid().GetHashCode());
            var r = new Random(random);
            var rand = r.Next(g1.lenght);
            var start = g1.GetGene(rand).getID();
            chromosomeSon.ReplaceGene(numTour, g1.GetGene(rand));
            numTour=1;
            var listTour = new List<int>();
            listTour.Add(start);
            while (numTour<g1.lenght)
            {
                var next1 = searchNext(start, g1.GetGenes());
                var next2 = searchNext(start, g2.GetGenes());
                while(listTour.Contains(next1) && listTour.Contains(next2)) 
                {
                    start++;
                    if (start == g1.lenght)
                        start = 0;
                    next1 = searchNext(start, g1.GetGenes());
                    next2 = searchNext(start, g2.GetGenes());
                }
                if (listTour.Contains(next1) && !listTour.Contains(next2))
                {
                    chromosomeSon.ReplaceGene(numTour, g2.GetGeneByID(next2));
                    start = next2;
                }
                else if (listTour.Contains(next2) && !listTour.Contains(next1)) // Caso analogo a sopra
                {
                    chromosomeSon.ReplaceGene(numTour, g1.GetGeneByID(next1));
                    start = next1;
                }
                else 
                {
                    var cost1 = g1.distanceMatrix[start, next1];
                    var cost2 = g2.distanceMatrix[start, next2];

                    if (cost1 < cost2)
                    {
                        chromosomeSon.ReplaceGene(numTour, g1.GetGeneByID(next1));
                        start = next1;
                    }
                    else
                    {
                        chromosomeSon.ReplaceGene(numTour, g2.GetGeneByID(next2));
                        start = next2;
                    }
                }
                numTour++;
                listTour.Add(start);
            }
            return chromosomeSon;

        }


        /*
         * E' la funzione che dato un indice e un array di geni trova l'indice del gene
         * che ha come ID l'indice ricercato
         * @param: Indice da cercare,Array geni
         * return : indice cercato
         */


        private static int searchNext(int start,Gene[] genes)
        {
            bool found = false;
            int i = 0;
            while (!found && i<genes.Length)
            {
                if (genes[i].getID() == start)
                    found = true;
                i++;
            }
            if (found)
            {
                if (i == genes.Length)
                    i = 0;
            }
            return genes[i].getID();
        }


        /*
        * Il più semplice degli Ordered Crossovers: Dati due genitori, si sceglie un punto random sul primo o sul secondo genitore
        * e il figlio avrà la prima parte uguale a quella del genitore1 e la restante viene presa dal genitore 2 evitando geni già presi dal genitore1
        * @param : Popolazione al tempo T, indice Genitore1, indice Genitore2
        * return : Cromosoma figlio
        */
        public static TourChromosome OrderedBasedCrossover(MyPopulation p, int idx1, int idx2)
        {

            var g1 = p.population[idx1];
            var g2 = p.population[idx2];
            var random = Math.Abs(Guid.NewGuid().GetHashCode());
            var r = new Random(random);
            var rand = r.Next(g1.lenght);
            List<int> selected = new List<int>();
            var chromosomeSon = new TourChromosome(g1.lenght, g1.distanceMatrix,g1.xPoints,g1.yPoints);
            int i = 0;
            for (i = 0; i < rand; i++)
            {
                Gene aux = g1.GetGene(i);
                chromosomeSon.ReplaceGene(i, aux);
                selected.Add(aux.getID());
            }
            while (i < g1.lenght)
            {
                for(int j = 0; j < g2.lenght; j++)
                {
                    if (!selected.Contains(g2.GetGene(j).getID())){
                        Gene aux = g2.GetGene(j);
                        chromosomeSon.ReplaceGene(i, aux);
                        selected.Add(aux.getID());
                        i++;
                    }
                }
            }
            return chromosomeSon;
        }

    }
}