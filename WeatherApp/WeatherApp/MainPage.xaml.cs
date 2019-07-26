using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using WeatherApp.Api;
using WeatherApp.Models.DataModels;
using WeatherApp.Models.DisplayModels;
using Xamarin.Forms;

namespace WeatherApp
{
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private string _city = "";

        private CurrentWeatherDataModel.RootObject _currentData;
        private DailyWeatherDataModel.RootObject _dailyData;

        public MainPage()
        {
            InitializeComponent();

            getDataBtn.IsEnabled = false;
            currentForcast.Opacity = 0;
            hourlyForecast.Opacity = 0;
        }

        private void CityEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            getUserInput();

            if (_city != string.Empty)
            {
                getDataBtn.IsEnabled = true;
            }
            else
            {
                getDataBtn.IsEnabled = false;
            }
        }

        async void GetDataBtn_Clicked(object sender, System.EventArgs e)
        {
            getUserInput();

            OpenWeatherApi api = new OpenWeatherApi(_city);
            try
            {
                _currentData = await api.GetCurrentWeather();
                _dailyData = await api.GetDailyWeather();

                fillInCurrentForecast();
                fillHourlyForecast();

                currentForcast.Opacity = 0;
                currentForcast.Scale = 0;

                hourlyForecast.Opacity = 0;
                hourlyForecast.Scale = 0;

                await currentForcast.FadeTo(1, 250);
                await currentForcast.ScaleTo(1, 250);

                await hourlyForecast.FadeTo(1, 250);
                await hourlyForecast.ScaleTo(1, 250);
            }
            catch (Exception ex)
            {
                if (ex.HResult == -2146233088)
                {
                    await DisplayAlert("Alert", "Incorrect input!\nPlease try again.", "Close");
                }
                else
                {
                    await DisplayAlert("Alert", "You have encountered an error!\n" + ex.Message, "Close");
                }
            }
        }

        void getUserInput()
        {
            _city = cityEntry.Text.ToString();
        }

        void fillInCurrentForecast()
        {
            new CurrentForecastDataDisplayModel(_city, cityLabel, tempLabel, pressureLabel, humidityLabel, descrLabel, tempMaxLabel, weatherIcon, _currentData, _dailyData);
        }

        void fillHourlyForecast()
        {
            List<DailyWeatherDataModel.List> dailyWeatherData = _dailyData.list;

            new DailyForecastDataDisplayModel(icon1Image, temp1Label, hr1Label, descr1Label, dailyWeatherData.ElementAt(0));
            new DailyForecastDataDisplayModel(icon2Image, temp2Label, hr2Label, descr2Label, dailyWeatherData.ElementAt(1));
            new DailyForecastDataDisplayModel(icon3Image, temp3Label, hr3Label, descr3Label, dailyWeatherData.ElementAt(2));
            new DailyForecastDataDisplayModel(icon4Image, temp4Label, hr4Label, descr4Label, dailyWeatherData.ElementAt(3));
            new DailyForecastDataDisplayModel(icon5Image, temp5Label, hr5Label, descr5Label, dailyWeatherData.ElementAt(4));
        }
    }
}
