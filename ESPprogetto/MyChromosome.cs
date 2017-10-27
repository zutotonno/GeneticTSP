using GALibrary;
using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ESPprogetto
{
    class MyChromosome : ChromosomeBase
    {
        Matrix<double> distanceMatrix;
        Vector<double> xPoints;
        Vector<double> yPoints;
        Gene[] genes;
        public MyChromosome(int lenght) : base(lenght) 
        {

            genes = new Gene[lenght];
        }

        public void setChromosome(Matrix<double> distance, Vector<double> xp, Vector<double> yp)
        {
            distanceMatrix = distance;
            xPoints = xp;
            yPoints = yp;
            CreateGenes();
        }
        public override double Fitness { get => base.Fitness; set => CalculateFitness(); }

        private double CalculateFitness()
        {
            foreach(Gene g in GetGenes())
            {
                var x = g.getXValue();
                var y = g.getYValue();
            }
            return 1.0;
        }

        public override Gene GenerateGene(int idx)
        {
            Gene g = new Gene(xPoints[idx], yPoints[idx],("citta "+idx),idx);
            return g;
        }


    }
}
