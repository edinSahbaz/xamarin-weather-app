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

            currentForecast.Opacity = 0;
            hourlyForecast.Scale = 0;
        }

        private void CityEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            GetUserInput();

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
            GetUserInput();

            OpenWeatherApi api = new OpenWeatherApi(_city);
            try
            {
                _currentData = await api.GetCurrentWeather();
                _dailyData = await api.GetDailyWeather();

                FillInCurrentForecast();
                FillHourlyForecast();

                await LoadAnimations();
            }
            catch (Exception ex)
            {
                ExceptionHandling(ex);
            }
        }

        void ExceptionHandling(Exception ex)
        {
            if (ex.HResult == -2146233088)
            {
                DisplayAlert("Alert", "Incorrect input!\nPlease try again.", "Close");
            }
            else
            {
                DisplayAlert("Alert", "You have encountered an error!\n" + ex.Message, "Close");
            }
        }

        async Task LoadAnimations()
        {
            currentForecast.Opacity = 0;
            hourlyForecast.Scale = 0;

            await Task.WhenAll(
                currentForecast.FadeTo(1, 300),
                hourlyForecast.ScaleTo(1, 300)
                );
        }

        void GetUserInput()
        {
            _city = cityEntry.Text.ToString();
        }

        void FillInCurrentForecast()
        {
            new CurrentForecastDataDisplayModel(_city, cityLabel, tempLabel, pressureLabel, humidityLabel, descrLabel, tempMaxLabel, weatherIcon, _currentData, _dailyData);
        }

        void FillHourlyForecast()
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
