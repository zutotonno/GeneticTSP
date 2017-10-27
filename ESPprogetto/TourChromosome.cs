using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESPprogetto
{
    public class TourChromosome
    {
        public Matrix<double> distanceMatrix { get; }
        public Vector<double> xPoints { get; set; }
        public Vector<double> yPoints { get; set; }
        public string name { get; set; }
        public double fitness { get; set; }
        public double distance { get; private set; }
        Gene[] genes;
        public int lenght { get; }
        public TourChromosome(int lenght, Matrix<double> distance, Vector<double> xp, Vector<double> yp)
        {
            genes = new Gene[lenght];
            distanceMatrix = distance;
            xPoints = xp;
            yPoints = yp;
            this.lenght = lenght;
            CreateGenes();
        }

        public void CreateGenes()
        {
            distance = 0;
            for (int i = 0; i < lenght; i++)
            {
                genes[i] = GenerateGene(i);
            }
            
        }

        public void calculateFitness()
        {
            distance = 0;
            for (int i = 0; i < lenght - 1; i++)
            {
                var from = genes[i].getID();
                var to = genes[i + 1].getID();
                distance += distanceMatrix[from, to];
            }
            var last = genes[lenght - 1].getID();
            var first = genes[0].getID();
            distance += distanceMatrix[first, last];
            fitness = 1/distance; 
        }

        private Gene GenerateGene(int i)
        {
            Gene g = new Gene(xPoints[i], yPoints[i], ("citta " + i), i);
            return g;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < lenght; i++)
                sb.Append(genes[i].ToString() + " -> ");
            return sb.ToString();
        }

        public Gene GetGene(int idx)
        {
            return genes[idx];
        }

        public Gene GetGeneByID(int id)
        {
            int i = 0;
            bool found = false;
            while (!found)
            {
                if (genes[i].getID() == id)
                    found = true;
                i++;
            }
            i--;
            return genes[i];
        }

        public void ReplaceGene(int idx, Gene g)
        {
            genes[idx] = g;
        }



        public Gene[] GetGenes()
        {
            return genes;
        }

    }
}
