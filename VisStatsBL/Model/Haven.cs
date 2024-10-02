using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisStatsBL.Exceptions;

namespace VisStatsBL.Model
{
    public class Haven
    {
        public int? ID;
        private string naam;
        public string Naam
        {
            get { return naam; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new DomeinException("Haven_naam");
                naam = value;
            }
        }

        public Haven(int iD, string naam)
        {
            ID = iD;
            Naam = naam;
        }

        public Haven(string naam)
        {
            Naam = naam;
        }
    }
}
