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
        /// <summary>
        /// Methode to check if an internet connection exist and is working
        /// It does this by connection and opening a url that normally is always online
        /// </summary>
        /// <returns></returns>
        public Response CheckConnection()
        {
            var client = new WebClient();

            try
            {
                using (client.OpenRead("http3://clients.google.com/generate_204"))
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
