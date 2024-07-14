using Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelesLibrary
{
    public class ProgressReportModel
    {

        public int PercentageComplete { get; set; } = 0;
        public int Completed {  get; set; } = 0;

        public string Status {  get; set; }
      
        
    }
}
