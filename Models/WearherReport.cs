using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace WeatherReport.Models
{
    public class WearherReport
    {
        #region variables

        
        public int ID { get; set; }

        [RegularExpression(@"^[a-zA-Z]+[a-zA-Z""'\s-]*$", ErrorMessage = "Enter Proper city name")]
        [Required]
        public string City { get; set; }

        [Range(-100.00, 100.00, ErrorMessage = "Temparature must be between -100.00 and 100.00")]
        public string Temp { get; set; }

        #endregion

        #region private methods
        private static object Merespones(HttpResponseMessage resp)
        {
            var p = resp.Content.ReadAsStringAsync().Result;
            dynamic jsonText = JsonConvert.DeserializeObject(p);
            return jsonText;
        }
        #endregion

        #region public methods
        public Object getWeatherForcast(string city)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://api.openweathermap.org/");
            HttpResponseMessage response;
            if (!String.IsNullOrEmpty(city))
            {
                 response = client.GetAsync("data/2.5/weather?q="+city+"&APPID=2b319b8adddb087e317daea1ce4b2b24&units=imperial").Result;
            }
            else
            {
                 response = client.GetAsync("data/2.5/weather?q=Cleveland&APPID=2b319b8adddb087e317daea1ce4b2b24&units=imperial").Result;
            }
            var jsconteny = Merespones(response);
            return jsconteny;
        }
        #endregion

       
    }
}
