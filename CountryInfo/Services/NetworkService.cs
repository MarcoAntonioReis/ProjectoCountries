using ModelesLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class NetworkService
    {
        public Response CheckConnection()
        {
            var client = new WebClient();

            try
            {
                using (client.OpenRead("http://clients3.google.com/generate_204"))
                {
                    return new Response
                    {
                  
                        IsSuccess=true,
                        Message = "Sucesso"
                    };

                }
            }
            catch (Exception)
            {

                return new Response
                {
                    IsSuccess = false,
                    Message = "Configure a sua ligação a Internet."
                };
            }
        }
    }
}
