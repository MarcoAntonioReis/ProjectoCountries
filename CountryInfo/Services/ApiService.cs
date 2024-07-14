using Library;
using ModelesLibrary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Services
{
    public class ApiService
    {

        public async Task<Response> GetResponseAsync(string urlBase, string controller)
        {
            try
            {
                var client = new HttpClient();

                client.BaseAddress = new Uri(urlBase);


                var response = await client.GetAsync(controller);

                var result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = result
                    };
                }
                var countries = JsonConvert.DeserializeObject<List<Country>>(result);

                return new Response
                {
                    IsSuccess = true,
                    Result = countries
                };
            }
            catch (Exception ex)
            {

                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message

                };
            }

        }

        /// <summary>
        /// Downloads every png avalable flag in asunc
        /// </summary>
        /// <param name="countries"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public async Task DownloadBanners(IProgress<ProgressReportModel> progress, List<Country> countries, string path)
        {
            WebClient webClient = new WebClient();
            ProgressReportModel report = new ProgressReportModel();

            CheckDirectory(path);


            foreach (Country country in countries)
            {
                if (country.Flags != null && country.Flags.Png != null)
                {
                    string filepath = @$"{path}/{country.Flags.Png.Substring(country.Flags.Png.LastIndexOf('/'))}";
                    if (filepath.EndsWith("noImg.png") || !File.Exists(filepath))
                    {
                        Uri uri = new Uri(country.Flags.Png);

                        await webClient.DownloadFileTaskAsync(uri, filepath);
                    }

                }
                report.Completed++;
                report.PercentageComplete = (report.Completed * 100) / countries.Count();
                report.Status = $"Carregadas {report.Completed} bandeiras de {countries.Count()}";
                progress.Report(report);
            }


        }

        public void CheckDirectory(string path)
        {

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

        }




    }
}
