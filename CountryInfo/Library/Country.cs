using Library.Country_Components;
using ModelesLibrary.Country_Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
                if (Capital != null && Capital.FirstOrDefault() != string.Empty)
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

        /// <summary>
        /// last Gini is used hen data is loaded locally
        /// </summary>
        public string LastGine { get; set; }
        public List<string> GetGiniList
        {
            get
            {
                if (Gini != null)
                {
                    List<string> temp = new List<string>();
                    if (Gini._1992 != null)
                    {
                        temp.Add($"Gini 1992: {Gini._1992}");
                    }
                    if (Gini._1998 != null)
                    {
                        temp.Add($"Gini 1998: {Gini._1998}");
                    }
                    if (Gini._1999 != null)
                    {
                        temp.Add($"Gini 1999: {Gini._1999}");
                    }
                    if (Gini._2003 != null)
                    {
                        temp.Add($"Gini 2003: {Gini._2003}");
                    }
                    if (Gini._2004 != null)
                    {
                        temp.Add($"Gini 2004: {Gini._2004}");
                    }
                    if (Gini._2004 != null)
                    {
                        temp.Add($"Gini 2004: {Gini._2004}");
                    }
                    if (Gini._2005 != null)
                    {
                        temp.Add($"Gini 2005: {Gini._2005}");
                    }
                    if (Gini._2006 != null)
                    {
                        temp.Add($"Gini 2006: {Gini._2006}");
                    }
                    if (Gini._2008 != null)
                    {
                        temp.Add($"Gini 2008: {Gini._2008}");
                    }
                    if (Gini._2009 != null)
                    {
                        temp.Add($"Gini 2009: {Gini._2009}");
                    }
                    if (Gini._2010 != null)
                    {
                        temp.Add($"Gini 2010: {Gini._2010}");
                    }
                    if (Gini._2011 != null)
                    {
                        temp.Add($"Gini 2011: {Gini._2011}");
                    }
                    if (Gini._2012 != null)
                    {
                        temp.Add($"Gini 2012: {Gini._2012}");
                    }
                    if (Gini._2013 != null)
                    {
                        temp.Add($"Gini 2013: {Gini._2013}");
                    }
                    if (Gini._2014 != null)
                    {
                        temp.Add($"Gini 2014: {Gini._2014}");
                    }
                    if (Gini._2015 != null)
                    {
                        temp.Add($"Gini 2015: {Gini._2015}");
                    }
                    if (Gini._2016 != null)
                    {
                        temp.Add($"Gini 2016: {Gini._2016}");
                    }
                    if (Gini._2017 != 0)
                    {
                        temp.Add($"Gini 2017: {Gini._2017}");
                    }
                    if (Gini._2018 != null)
                    {
                        temp.Add($"Gini 2018: {Gini._2018}");
                    }
                    if (Gini._2019 != null)
                    {
                        temp.Add($"Gini 2019: {Gini._2019}");
                    }


                    return temp;
                }
                else
                {
                    List<string> temp = new List<string>();
                    temp.Add(NoDataMsg);
                    return temp;
                }
            }
        }

        public Flags Flags { get; set; }



    }
}
