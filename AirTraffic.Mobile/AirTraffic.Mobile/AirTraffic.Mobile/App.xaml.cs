using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AirTraffic.Mobile.Views;
using AirTraffic.Mobile.Models;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace AirTraffic.Mobile
{
    public partial class App : Application
    {
        public NavigationPage NavigationPage { get;  set; }
        public static AirportModel CurrentAirport { get;  set; }

        public App()
        {
            InitializeComponent();

            MainPage = new MapPage();
            (Application.Current as App).NavigationPage = new NavigationPage(new MapPage());
            Application.Current.MainPage = NavigationPage;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
