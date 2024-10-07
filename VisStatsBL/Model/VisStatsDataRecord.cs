using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
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
            set { if ((value < 2000) || (value > 2100)) 
                    throw new DomeinException("jaar niet correct");
                jaar = value; 
            } 
        }
        private int maand;
        public int Maand 
        { 
            get { return maand; } 
            set { if ((value > 12) || (value < 1)) 
                    throw new DomeinException("maand niet correct"); 
                maand = value; 
            } 
        }
        private double gewicht;
        public double Gewicht 
        { get { return gewicht; } 
            set { if (value < 0) 
                    throw new DomeinException("gewicht niet correct"); 
                gewicht = value; 
            } 
        }
        private double waarde;
        public double Waarde 
        { 
            get { return waarde; } 
            set { if (value < 0) 
                    throw new DomeinException("waarde niet correct"); 
                waarde = value; 
            } 
        }
        private Haven haven;
        public Haven Haven 
        { 
            get { return haven; } 
            set { if (value == null) 
                    throw new DomeinException("haven niet correct"); 
                haven = value; 
            } 
        }
        private Vissoort vissoort;
        public Vissoort Vissoort 
        {
            get { return vissoort; } 
            set { if (value == null) 
                    throw new DomeinException("vissoort niet correct"); 
                vissoort = value; 
            } 
        }

        public VisStatsDataRecord(int jaar, int maand, double gewicht, double waarde, Haven haven, Vissoort vissoort)
        {
            Jaar = jaar;
            Maand = maand;
            Gewicht = gewicht;
            Waarde = waarde;
            Haven = haven;
            Vissoort = vissoort;
        }
    }
}
