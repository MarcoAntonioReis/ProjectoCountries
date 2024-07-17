using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Country_Components
{
    ///TODO place Read Only properties to addle data not found
    public class Flags
    {

        public string Png { get; set; }
        public string Svg { get; set; }
        public string Alt { get; set; }

        /// <summary>
        /// This is a property that is intendent from the API data received, used to store the locations of images locally
        /// </summary>
        public string LocalRef { get; set; }

        public string GetLocalRefString
        {
            get
            {
                if (LocalRef != null && LocalRef != string.Empty)
                {
                    return LocalRef;
                }
                else
                {
                    // because its the Environment it will get the fiscal address were the main program is running
                    string workingDirectory = Environment.CurrentDirectory;

                    string FlagsPath = $"{Directory.GetParent(workingDirectory).Parent.Parent.FullName}/Flags";

                    return @$"{FlagsPath}\noImg.png";
                }
            }
        }





    }
}
