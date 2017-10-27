using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
namespace ESPprogetto
{
    public partial class Form1 : Form
    {
        int seed = 1;
        int numCities = 100;
        int poison = 100000;
        string name = "berlin";
        public Form1()
        {
            InitializeComponent();

            var distanceMatrix = MatrixGenerator.Build(seed,numCities,poison); // creo la matrice delle distanze con una Factory
            //var distanceMatrix = MatrixGenerator.Build(name,numCities,poison);

            var tourLenList = new List<double>();
            var tourMatList = new List< Tuple<Matrix<double>,List<string>> >();
            var tourLenList2 = new List<double>();
            var tourMatList2 = new List<Matrix<double>>();
            var time = Vector<double>.Build.Dense(numCities);
            //Cerco i migliori percorsi partendo da ogni città usando una tecnica Greedy
            for (int i = 0; i < numCities; i++)
            {
                var greedy = new GreedyAlgorithm(distanceMatrix, numCities, poison);
                greedy.Start(i);
                var lenght = greedy.GetDistance();
                tourMatList.Add(Tuple.Create(greedy.GetCityTour(),greedy.Tour()));
                tourLenList.Add(lenght);
                time[i] = greedy.GetTime();
            }
            var minDIST = tourLenList.Min();
            var minINDEX = tourLenList.IndexOf(minDIST); //Scelgo il percorso migliore
            var bestTourTuple = tourMatList[minINDEX];
            var bestTour = bestTourTuple.Item1;
            Console.WriteLine("Avg TIME: "+time.Average() + " Total Time: "+time.Sum());
            Console.WriteLine("MIN: " + minDIST);
            label1.Text = "Greedy: " + minDIST;
            label1.Text += "\nTotal Time: " + time.Sum() +"ms";

            var populationSize = (int)Math.Round( numCities / Math.Log10(numCities)); // Formula per adattare la popolazione iniziale al numero di città
            var percentageFittest = 0.4; // la percentuale dei migliori sulla dimensione della popolazione selezionati per il Crossover
            var crossoverProbability = 1;
            var mutationProbability = 0.1;
            var genTermination = 1000;
            var stagnationTermination = 100;
            var percentageElitism = 0.05 * percentageFittest; // indica la percentuale di popolazione che va direttamente nella nuova generazione. Il resto sarà generato da Crossover11


            var fittestDim =  (int)Math.Round(populationSize * percentageFittest);
            var elitismDim = (int)Math.Round(populationSize * percentageElitism);
            var offspringDim = populationSize - elitismDim;

            TourChromosome adam = new TourChromosome(numCities, distanceMatrix.Item1,distanceMatrix.Item2,distanceMatrix.Item3);
            MyPopulation initialPopulation = new MyPopulation(populationSize, adam);
            /*Usare questo costruttore
             per indurre un bias*///MyPopulation initialPopulation = new MyPopulation(populationSize,bestTourTuple.Item1,bestTourTuple.Item2, adam); // Biased Initial Population
            MySelection selection = new MySelection(fittestDim);
            Console.WriteLine();
            MyCrossover crossover = new MyCrossover(crossoverProbability);
            MyMutation mutation = new MyMutation(mutationProbability);
            GeneticAlgorithm ga = new GeneticAlgorithm(initialPopulation,mutation,genTermination,elitismDim,offspringDim,populationSize,adam.lenght,minDIST,stagnationTermination);
            ga.Start();
            Console.WriteLine("Best Chromosome:" + ga.BestChromosome() + "Distance:"+ ga.BestChromosome().distance);
            
            TourChromosome best = ga.BestChromosome();
            label2.Text = "\nPop Size: " + populationSize;
            label2.Text += "\nFittests Size: " + fittestDim;
            label2.Text += "\nSurvivors(Elitism) Size: " + elitismDim;
            label2.Text += "\nOffspring Size: " + offspringDim;
            label2.Text += "\nGen Num: " + ga.genNUM;
            label2.Text += "\nBest Distance: " + ga.BestChromosome().distance;
            label2.Text +="\nTotal time :"+ga.totalTIME.ToString()+"ms";

            dataGridView1.ColumnCount = 3;
            dataGridView1.Columns[0].Name = "Gen";
            dataGridView1.Columns[1].Name = "Distance";
            dataGridView1.Columns[2].Name = "Fitness";

            chart1.Series.Add("cities");
            chart1.Series.Add("tour");
            chart1.Series.Add("startTour");
            chart1.Series.Add("nextCity");
            chart1.Series.Remove(chart1.Series["Series1"]);
            chart2.Series.Add("cities");
            chart2.Series.Add("tourGA");
            chart2.Series.Add("startTourGA");
            chart2.Series.Add("nextCityGA");
            chart2.Series.Remove(chart2.Series["Series1"]);
            var fitnessList = ga.fitnessList;
            chart3.Series.Add("DistanceXGen");
            chart3.Series.Add("Greedy");
            chart3.Series.Remove(chart3.Series["Series1"]);
            chart4.Series.Add("FitnessXGen");
            chart4.Series.Remove(chart4.Series["Series1"]);



            for (int i = 0; i < distanceMatrix.Item2.Count; i++)
            {
                chart1.Series["cities"].Points.AddXY(distanceMatrix.Item2[i], distanceMatrix.Item3[i]);
                chart2.Series["cities"].Points.AddXY(distanceMatrix.Item2[i], distanceMatrix.Item3[i]);
            }
            Console.WriteLine(distanceMatrix.Item2.ToString());

            
            chart1.Series["startTour"].Points.AddXY(bestTour[0, 0], bestTour[1, 0]);
            chart1.Series["nextCity"].Points.AddXY(bestTour[0, 1], bestTour[1, 1]);
            chart2.Series["startTourGA"].Points.AddXY(best.GetGene(0).getXValue(), best.GetGene(0).getYValue());
            chart2.Series["nextCityGA"].Points.AddXY(best.GetGene(1).getXValue(), best.GetGene(1).getYValue());
            for (int i = 0; i < bestTour.ColumnCount; i++)
            {
                chart1.Series["tour"].Points.AddXY(bestTour[0, i], bestTour[1, i]);
            }
            
            for(int i = 0; i < best.GetGenes().Length; i++)
            {
                chart2.Series["tourGA"].Points.AddXY(best.GetGene(i).getXValue(), best.GetGene(i).getYValue());
                Console.WriteLine("X: " + best.xPoints[i] + " Y: " + best.yPoints[i]);
            }
            chart2.Series["tourGA"].Points.AddXY(best.GetGene(0).getXValue(), best.GetGene(0).getYValue());


            for (int i = 0; i < fitnessList.Count; i++)
            {
                chart3.Series["DistanceXGen"].Points.AddXY(i, fitnessList[i]);
                chart4.Series["FitnessXGen"].Points.AddXY(i, (1/fitnessList[i]));
                dataGridView1.Rows.Add(i,fitnessList[i],1/fitnessList[i]);

            }

            chart3.Series["Greedy"].Points.AddXY(0, minDIST);
            chart3.Series["Greedy"].Points.AddXY(fitnessList.Count, minDIST);


            chart1.Series["cities"].ChartType =
                SeriesChartType.FastPoint;
            chart1.Series["cities"].Color = Color.Red;
            chart1.Series["tour"].ChartType =
                SeriesChartType.FastLine;
            chart1.Series["tour"].Color = Color.Blue;
            chart1.Series["startTour"].ChartType =
                SeriesChartType.FastPoint;
            chart1.Series["startTour"].Color = Color.Blue;
            chart1.Series["startTour"].MarkerSize = 10;
            chart1.Series["nextCity"].ChartType =
                SeriesChartType.FastPoint;
            chart1.Series["nextCity"].Color = Color.Green;
            chart1.Series["nextCity"].MarkerSize = 10;

            chart2.Series["cities"].ChartType =
                SeriesChartType.FastPoint;
            chart2.Series["cities"].Color = Color.Red;
            chart2.Series["tourGA"].ChartType =
                SeriesChartType.FastLine;
            chart2.Series["tourGA"].Color = Color.Blue;
            chart2.Series["startTourGA"].ChartType =
                SeriesChartType.FastPoint;
            chart2.Series["startTourGA"].Color = Color.Blue;
            chart2.Series["startTourGA"].MarkerSize = 10;
            chart2.Series["nextCityGA"].ChartType =
                SeriesChartType.FastPoint;
            chart2.Series["nextCityGA"].Color = Color.Green;
            chart2.Series["nextCityGA"].MarkerSize = 10;

            chart3.Series["DistanceXGen"].Color = Color.Blue;
            chart3.Series["DistanceXGen"].ChartType =
                SeriesChartType.FastLine;
            chart3.Series["Greedy"].Color = Color.Red;
            chart3.Series["Greedy"].ChartType =
                SeriesChartType.Line;
            chart3.Series["Greedy"].MarkerSize = 50;
            chart4.Series["FitnessXGen"].Color = Color.Red;
            chart4.Series["FitnessXGen"].ChartType =
                SeriesChartType.FastLine;
        }
    }
}
