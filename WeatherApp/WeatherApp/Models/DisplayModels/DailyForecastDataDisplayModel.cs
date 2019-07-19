using System;
using System.Linq;
using WeatherApp.Models.DataModels;
using Xamarin.Forms;

namespace WeatherApp.Models.DisplayModels
{
    public class DailyForecastDataDisplayModel
    {
        public DailyForecastDataDisplayModel(Image weatherIcon, Label tempLabel, 
            Label hrLabel, Label descrLabel, DailyWeatherDataModel.List hourData)
        {
            weatherIcon.Source = "http://openweathermap.org/img/w/" + hourData.weather.First().icon + ".png";

            tempLabel.Text = Math.Ceiling(Convert.ToDecimal(hourData.main.temp)).ToString() + "°C";

            hrLabel.Text = hourData.dt_txt.Substring(10, 6) + " h";

            descrLabel.Text = hourData.weather.First().main;
        }
    }
}
