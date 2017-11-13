using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ESPprogetto
{
    internal class GeneticAlgorithm
    {
        private int chromosomeLenght;
        private int elitismDim;
        private int offspringDim;
        private int populationSize;
        private int genTermination;
        private MyPopulation initialPopulation;
        private MyMutation mutation;
        private double minDist;
        private double BestDistance;
        private double LastDistance;
        private int FitnessStagnation;
        private int stagnationTermination;
        private int sogliaStagnation;
        public double totalTIME { get; private set; }
        public List<double> distanceList { get; private set; }
        public int genNUM { get; private set; }
        public GeneticAlgorithm(MyPopulation initialPopulation, MyMutation mutation, int genTermination, int elitismDim, int offspringDim, int populationSize, int chromoL, double minDist, int stagnation)
        {
            this.initialPopulation = initialPopulation;
            this.mutation = mutation;
            this.genTermination = genTermination;
            this.elitismDim = elitismDim;
            this.offspringDim = offspringDim;
            this.populationSize = populationSize;
            this.chromosomeLenght = chromoL;
            this.minDist = minDist;
            distanceList = new List<double>();
            stagnationTermination = stagnation;
            sogliaStagnation = (int)Math.Round(stagnationTermination * 0.05);
        }

        public void Start()
        {
            var startTime = System.DateTime.Now;
            BestChromosome().calculateFitness();
            BestDistance= BestChromosome().distance;
            distanceList.Add(BestDistance);
            var selected = MySelection.RouletteSelection(initialPopulation);
            genNUM = 0;
            FitnessStagnation = 0;
            var LocalStagnation = 0;
            while (FitnessStagnation < stagnationTermination) // (genNUM < genTermination)//(BestFitness>minDist)//
            {

                
                selected = MySelection.RouletteSelection(initialPopulation);
                MyPopulation newPopulation = new MyPopulation(populationSize);
                TrueReinsertion(selected, newPopulation, initialPopulation);
                CrossOver(selected, newPopulation, initialPopulation);
                mutation.Mutate(newPopulation, selected);
                initialPopulation.population.Clear();
                foreach (TourChromosome t in newPopulation.population)
                {
                    TourChromosome aux = new TourChromosome(chromosomeLenght, t.distanceMatrix, t.xPoints, t.yPoints);
                    for (int i = 0; i < aux.GetGenes().Length; i++)
                    {
                        aux.ReplaceGene(i, t.GetGene(i));
                    }
                    aux.calculateFitness();
                    initialPopulation.population.Add(aux);
                }
                LastDistance = BestDistance;
                BestDistance = BestChromosome(initialPopulation).distance;
                distanceList.Add(BestDistance);
                if (LastDistance == BestDistance)
                {
                    LocalStagnation++;
                    FitnessStagnation++;
                }
                else 
                {
                    FitnessStagnation=0;
                    if (elitismDim < selected.Length - 1)
                    {
                        elitismDim++;
                        offspringDim--;
                    }
                }
                if (LocalStagnation > sogliaStagnation)
                {
                    MyMutation.percentage += 0.01;
                    mutation.HeavyMutation(initialPopulation, selected);
                    if (elitismDim > 1)
                    {
                        elitismDim--;
                        offspringDim++;
                    }
                    LocalStagnation = 0;
                }
                genNUM++;
            }
            var endTime = System.DateTime.Now.Subtract(startTime);
            totalTIME = endTime.TotalMilliseconds;
        }


        public TourChromosome BestChromosome()
        {
            var sorted = initialPopulation.population.OrderByDescending(x => x.fitness);
            return sorted.First();
        }

        private void Reinsertion(int[] selected, MyPopulation newPopulation, MyPopulation initialPop)
        {

            for (int j = 0; j < elitismDim; j++)
            {
                var random = Math.Abs(Guid.NewGuid().GetHashCode());
                var r = new Random(random);
                var randIDX = r.Next(selected.Length);
                var rand = selected[randIDX];
                TourChromosome aux = new TourChromosome(chromosomeLenght, initialPopulation.population[rand].distanceMatrix, initialPopulation.population[rand].xPoints, initialPopulation.population[rand].yPoints);
                for (int i = 0; i < initialPopulation.population[rand].lenght; i++)
                {
                    aux.ReplaceGene(i, initialPopulation.population[rand].GetGene(i));
                }
                newPopulation.population.Add(aux);
            }

        }

        private void TrueReinsertion(int[] selected, MyPopulation newPopulation, MyPopulation initialPop)
        {
            int best = 0;
            for (int j = 0; j < elitismDim; j++)
            {
                TourChromosome aux = new TourChromosome(chromosomeLenght, initialPopulation.population[best].distanceMatrix, initialPopulation.population[best].xPoints, initialPopulation.population[best].yPoints);
                for (int i = 0; i < initialPopulation.population[best].lenght; i++)
                {
                    aux.ReplaceGene(i, initialPopulation.population[best].GetGene(i));
                }
                newPopulation.population.Add(aux);
                best++;
            }

        }

        private void CrossOver(int[] selected, MyPopulation newPop, MyPopulation initialPop)
        {
            for (int j = 0; j < offspringDim; j++)
            {

                var genTuple = MyCrossover.Cross(selected);
                var g1 = genTuple.Item1;
                var g2 = genTuple.Item2;
                TourChromosome son = MyCrossover.SCXcrossover(initialPop, g1, g2);//MyCrossover.OrderedBasedCrossover(initialPop, g1, g2);
                newPop.population.Add(son);
            }
        }

        public TourChromosome BestChromosome(MyPopulation pop)
        {
            var sorted = pop.population.OrderByDescending(x => x.fitness);
            return sorted.First();
        }
    }
}