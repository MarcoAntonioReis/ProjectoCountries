using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelesLibrary
{/// <summary>
/// todo add instructions
/// </summary>
    public class Response
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public object Result { get; set; }
    }
}
