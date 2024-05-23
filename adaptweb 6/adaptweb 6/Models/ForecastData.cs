namespace adaptweb_6.Models
{
    public class ForecastData
    {
        public List<Forecast> List { get; set; }
        public string Name { get; set; }    
    }
    public class Forecast
    {
        public Main Main { get; set; }
        public Wind Wind { get; set; }
        public Sys Sys { get; set; }
        public string dt_txt { get; set; }
    }
}
