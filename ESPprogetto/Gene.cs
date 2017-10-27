using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESPprogetto
{
    public class Gene
    {
        double xCoord;
        double yCoord;
        string name;
        int id;
        public Gene(double x, double y, string n, int id)
        {
            xCoord = x;
            yCoord = y;
            name = n;
            this.id = id;
        }
        public override string ToString()
        {
            return name+"-> ";
        }

        public double getXValue()
        {
            return xCoord;
        }
        public double getYValue()
        {
            return yCoord;
        }
        public int getID()
        {
            return id;
        }
        public string getName()
        {
            return name;
        }
        public void Replace(double x,double y, string n)
        {
            xCoord = x;
            yCoord = y;
            name = n;
        }
    }
}
