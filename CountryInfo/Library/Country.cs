using Library.Country_Components;
using ModelesLibrary.Country_Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class Country
    {
        public Name Name { get; set; }
        public List<string> Capital { get; set; }

        public string Region { get; set; }

        public string SubRegion { get; set; }

        public int Population { get; set; }

        /// <summary>
        /// May not exist in so countries 
        /// </summary>
        public Gini Gini { get; set; }

        public Flags Flags { get; set; }



    }
}
