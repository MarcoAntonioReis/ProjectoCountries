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


        ///TODO place Read Only properties to addle data not found

        private string NoDataMsg = "Dados não disponíveis";

        public Name Name { get; set; }

        
        ///////////////////////////////////////////

        public List<string> Capital { get; set; }

        public List<string> GetCapitalString
        {
            get
            {
                if (Region != null && Region != string.Empty)
                {
                    return Capital;
                }
                else
                {
                    List<string> temp = new List<string>();
                    temp.Add(NoDataMsg);
                    return temp;
                }
            }
        }

        //////////////////////////////////////////////////////////


        public string Region { get; set; }

        public string GetRegionString
        {
            get
            {
                if (Region != null && Region != string.Empty)
                {
                    return Region;
                }
                else
                {
                    return NoDataMsg;
                }
            }
        }

        ////////////////////////////////////////////////////////////

        public string SubRegion { get; set; }
        public string GetSubRegionString
        {
            get
            {
                if (SubRegion != null && SubRegion != string.Empty)
                {
                    return SubRegion;
                }
                else
                {
                    return NoDataMsg;
                }
            }
        }

        ////////////////////////////////////////////////////////////

        public int Population { get; set; }

        public string GetPopulationString
        {
            get
            {
                if (Population != 0)
                {
                    return Population.ToString();
                }
                else
                {
                    return NoDataMsg;
                }
            }
        }
        ////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// May not exist in so countries 
        /// </summary>
        public Gini Gini { get; set; }

        public Flags Flags { get; set; }



    }
}
