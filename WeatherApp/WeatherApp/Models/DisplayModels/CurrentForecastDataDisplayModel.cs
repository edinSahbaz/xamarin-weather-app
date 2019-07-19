using System;
using System.Linq;
using WeatherApp.Models.DataModels;
using Xamarin.Forms;

namespace WeatherApp.Models.DisplayModels
{
    public class CurrentForecastDataDisplayModel
    {
        private DailyWeatherDataModel.RootObject _dailyData { get; set; }
        public CurrentForecastDataDisplayModel(string city, Label cityLabel, Label tempLabel, 
            Label pressureLabel, Label humidityLabel, Label descrLabel, Label tempMaxLabel, Image weatherIcon, 
            CurrentWeatherDataModel.RootObject currentData, DailyWeatherDataModel.RootObject dailyData)
        {
            _dailyData = dailyData;

            cityLabel.Text = city + "," + currentData.sys.country;

            tempLabel.Text = Math.Round(Convert.ToDecimal(currentData.main.temp)).ToString() + "°C";

            tempMaxLabel.Text = getMaxTemp().ToString() + "°C";

            pressureLabel.Text = currentData.main.pressure / 1000 + "Bar";

            humidityLabel.Text = currentData.main.humidity + "%";

            descrLabel.Text = currentData.weather.First().main;

            weatherIcon.Source = "http://openweathermap.org/img/w/" + currentData.weather.First().icon + ".png";
        }

        int getMaxTemp()
        {
            var data = _dailyData.list.ToArray();

            int[] temperatures = new int[5];
            for (int i = 0; i < 5; i++)
            {
                temperatures[i] = Convert.ToInt32(Math.Ceiling((decimal)data[i].main.temp));
            }

            int maxTemp = temperatures.Max();

            return maxTemp;
        }
    }
}
