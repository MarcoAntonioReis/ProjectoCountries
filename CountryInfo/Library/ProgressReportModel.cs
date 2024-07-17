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
        /// <summary>
        /// Total percentange of the task comnpletion.
        /// </summary>
        public int PercentageComplete { get; set; } = 0;

        /// <summary>
        /// Total completed taskes
        /// </summary>
        public int Completed { get; set; } = 0;

        /// <summary>
        /// Can be used to send a string describing the status of task
        /// </summary>
        public string Status { get; set; }


    }
}
