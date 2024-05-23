namespace adaptweb_6.Models
{
    public class WeatherData
    {
        public Main Main { get; set; }
        public Wind Wind { get; set; }
        public Sys Sys { get; set; }
    }
    public class Main
    {
        public float Temp { get; set; }
        public float Humidity { get; set; }
    }

    public class Wind
    {
        public float Speed { get; set; }
        public float Deg { get; set; }
    }

    public class Sys
    {
        public string Country { get; set; }
        public string Name { get; set; }
    }
}
