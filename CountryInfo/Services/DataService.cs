using Library;
using Library.Country_Components;
using ModelesLibrary.Country_Components;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Services
{

    public class DataService
    {
        private SQLiteConnection connection;

        private SQLiteCommand command;

        public string Status;

        public DataService()
        {
            if (!Directory.Exists("Data"))
            {
                Directory.CreateDirectory("Data");
            }

            var path = @"Data\Countries.sqlite";


            try
            {
                connection = new SQLiteConnection("Data Source=" + path);
                connection.Open();

                string sqlCommand = @"CREATE TABLE IF NOT EXISTS Countries(NameCommon varchar(200),NameOfficial Varchar(200), Capital Varchar(200), Region varchar(200), SubRegion varchar(200), Population int , Gini varchar(200) ,LocalRef varchar(250),Latlng varchar(50),Area float)";
                command = new SQLiteCommand(sqlCommand, connection);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                Status = $"Algo correu mal. {ex.Message}";
            }

        }

        public async Task SaveData(List<Country> Countreis)
        {
            try
            {
                foreach (Country country in Countreis)
                {
                    //this is the default value if no data is Available
                    string LatLng = "0|0";

                    if (country.GetLatlng.Count == 2 && country.GetLatlng[0] != 0 && country.GetLatlng[1] != 0)
                    {
                        LatLng = $"{country.GetLatlng[0]}|{country.GetLatlng[1]}";
                    }


                    string sql = string.Format("Insert into Countries (NameCommon,NameOfficial,Capital,Region,SubRegion,Population,Gini,LocalRef,Latlng,Area) values('{0}','{1}','{2}','{3}','{4}',{5},'{6}','{7}','{8}',{9})", country.Name.GetOfficialString.Replace("'", "''"), country.Name.GetCommonString.Replace("'", "''"), country.GetCapitalString.FirstOrDefault().Replace("'", "''"), country.GetRegionString, country.GetSubRegionString, country.Population, country.GetGiniList.FirstOrDefault(), country.Flags.GetLocalRefString, LatLng, country.Area);

                    command = new SQLiteCommand(sql, connection);
                    await command.ExecuteNonQueryAsync();
                }
                connection.Close();
            }
            catch (Exception ex)
            {
                Status = $"Algo correu mal. {ex.Message}";
            }



        }


        public List<Country> Getdata()
        {
            List<Country> coutries = new List<Country>();

            try
            {
                string sql = "select NameCommon,NameOfficial,Capital,Region,SubRegion,Population,Gini,LocalRef,Latlng, Area from Countries";

                command = new SQLiteCommand(sql, connection);


                SQLiteDataReader reader = command.ExecuteReader();




                while (reader.Read())
                {
                    coutries.Add(new Country
                    {
                        Name = new Name
                        {
                            Common = (string)reader["NameCommon"],
                            Official = (string)reader["NameOfficial"]
                        },
                        Capital = new List<string> { (string)reader["Capital"] },
                        Region = (string)reader["Region"],
                        SubRegion = (string)reader["SubRegion"],
                        Population = (int)reader["Population"],
                        LastGine = (string)reader["Gini"],
                        Flags = new Flags { LocalRef = (string)reader["LocalRef"] },
                        Latlng = new List<double> {
                            //First part gets the Latitude witch is contain from the beginning to the char (;)
                            double.Parse( reader["Latlng"].ToString().Substring(0, reader["Latlng"].ToString().IndexOf('|'))),
                            //Second part the Longitude is contained from the char (|) to the end
                            double.Parse( reader["Latlng"].ToString().Substring(reader["Latlng"].ToString().IndexOf('|')+1))

                        },
                        Area = float.Parse(reader["Area"].ToString())



                    });
                }
                connection.Close();
                return coutries;


            }
            catch (Exception ex)
            {

                Status = $"Algo correu mal. {ex.Message}";
                return null;
            }



        }



        public void DeleteData()
        {
            try
            {
                string sql = "Delete from Countries";

                command = new SQLiteCommand(sql, connection);

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                Status = $"Algo correu mal. {ex.Message}";

            }
        }
    }
}
