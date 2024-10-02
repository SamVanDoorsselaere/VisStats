using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisStatsBL.Exceptions;

namespace VisStatsBL.Model
{
    public class VisStatsDataRecord
    {
        private int jaar; 
        public int Jaar 
        { 
            get { return jaar; }
            set
            {
                if (value < 2000 || value > 2100)
                    throw new DomeinException("Jaar niet correct");
                jaar = value;
            }
        }
        private int maand;
        public int Maand
        {
            get { return maand; }
            set
            {
                if (value < 1 || value > 12)
                    throw new DomeinException("Maand niet correct");
                maand = value;
            }
        }
        private double gewicht;
        public double Gewicht
        {
            get { return gewicht; }
            set
            {
                if (value < 0.0)
                    throw new DomeinException("Gewicht niet correct");
                gewicht = value;
            }
        }
        private double waarde;
        public double Waarde
        {
            get { return waarde; }
            set
            {
                if (value < 0.0)
                    throw new DomeinException("Waarde niet correct");
                waarde = value;
            }
        }
        private Vissoort soort;
        public Vissoort Soort
        {
            get { return soort; }
            set
            {
                if (value == null)
                    throw new DomeinException("Soort is null");
                soort = value;
            }
        }

    }
}
