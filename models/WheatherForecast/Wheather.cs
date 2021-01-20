using System.Collections.Generic;

namespace wheather_api.models.WheatherForecast
{
    public class WheatherForecast
    {
        public List<Wheather> List {get; set;}
    }
    public class Wheather
    {
        public int dt {get; set;}
        public Main main {get; set;}
        public Wind wind {get; set;}
        public Clouds clouds {get; set;}
    }

    public class Main
    {
        public double temp {get; set;}
        public double temp_min {get; set;}
        public double temp_max {get; set;}
    }

    public class Wind
    {
        public double speed {get; set;}
    }

    public class Clouds
    {
        public int all {get; set;}
    }
}