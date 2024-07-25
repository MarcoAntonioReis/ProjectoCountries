using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelesLibrary
{/// <summary>
/// Model for web response status
/// </summary>
    public class Response
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public object Result { get; set; }
    }
}
