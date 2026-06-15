using System;

namespace WeatherList
{
    public enum WeatherType
    {
        Sunny,
        Cloudy,
        Rainy,
        Snowy,
        Foggy,
        Stormy
    }

    public class WeatherData
    {
        private WeatherType weatherType;
        private double temperature;
        private bool hasPrecipitation;

        public WeatherType WeatherType
        {
            get { return weatherType; }
            set { weatherType = value; }
        }

        public double Temperature
        {
            get { return temperature; }
            set
            {
                if (value < -100 || value > 100)
                {
                    throw new ArgumentOutOfRangeException(
                        "Температура повинна бути від -100 до 100");
                }

                temperature = value;
            }
        }

        public bool HasPrecipitation
        {
            get { return hasPrecipitation; }
            set { hasPrecipitation = value; }
        }

        public WeatherData(
            WeatherType weatherType,
            double temperature,
            bool hasPrecipitation)
        {
            WeatherType = weatherType;
            Temperature = temperature;
            HasPrecipitation = hasPrecipitation;
        }

        public override string ToString()
        {
            return $"{WeatherType,-10} | {Temperature,6:F1} | {(HasPrecipitation ? "Так" : "Ні")}";
        }
    }

    public class WeatherNode
    {
        public WeatherData Data { get; set; }

        public WeatherNode? Next { get; set; }

        public WeatherNode? Prev { get; set; }

        public WeatherNode(WeatherData data)
        {
            Data = data;
            Next = null;
            Prev = null;
        }
    }
}