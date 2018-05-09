using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherReport.Models
{
    public class WeatherCityValidaton
    {
        [Required(ErrorMessage = "Please enter valid City")]
        public string City { get; set; }
    }
}
