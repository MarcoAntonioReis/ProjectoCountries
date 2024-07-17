using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Country_Components
{
    public class Name
    {
        /// <summary>
        /// This sting is used to store and show what the message is if no data is available
        /// </summary>
        private string NoDataMsg = "Dados não disponíveis";
        public string Common { get; set; }
        public string GetCommonString
        {
            get
            {
                if (Common != null && Common != string.Empty)
                {
                    return Common;
                }
                else
                {
                    return NoDataMsg;
                }
            }
        }


        public string Official { get; set; }
        public string GetOfficialString
        {
            get
            {
                if (Official != null && Official != string.Empty)
                {
                    return Official;
                }
                else
                {
                    return NoDataMsg;
                }
            }
        }

    }
}
