using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adaptweb_5
{
    public class WeatherForecast
    {
        public List<WeatherDataForecast> Data { get; set; }
    }

    public class WeatherData
    {
        public string City { get; set; }
        public string Country { get; set; }
        public double Temperature { get; set; }
        public string WeatherDescription { get; set; }
    }

    public class WeatherDataForecast
    {
        public DateTime DateTime { get; set; }
        public string CityName { get; set; }
        public string CountryCode { get; set; }
        public double Temperature { get; set; }
        public string WeatherDescription { get; set; }
    }
}
