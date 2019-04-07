using AirTraffic.Mobile.Helpers;
using AirTraffic.Mobile.Models;
using AirTraffic.Mobile.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace AirTraffic.Mobile.ViewModels
{
    public class MapViewModel : BaseViewModel
    {


        #region Properties
        AirportModel _selectedPin;
        public AirportModel SelectedPin
        {
            get { return _selectedPin; }
            set { SetProperty(ref _selectedPin, value); }
        }
        bool _showInfoWindow;
        public bool ShowInfoWindow
        {
            get { return _showInfoWindow; }
            set { SetProperty(ref _showInfoWindow, value); }
        }
        public ObservableCollection<TimetableModel> Timetables { get; set; }
        public ObservableCollection<AirportModel> Airports { get; set; }
        #endregion

        #region Commands
        public ICommand GetTimeTableCommand { get; }
        public ICommand ClowWindowCommand { get; }
        public ICommand ViewFlightsCommand { get; }
        #endregion

        public Position Position { get; set; }
        public static object FeedItems { get; internal set; }

        public MapViewModel()
        {
            Title = "Airports";
            Timetables = new ObservableCollection<TimetableModel>();
            Airports = new ObservableCollection<AirportModel>();
            Device.BeginInvokeOnMainThread(async () =>
            {

                var request = new GeolocationRequest(GeolocationAccuracy.Low);
                request.Timeout = TimeSpan.FromSeconds(10);
                var location = await Geolocation.GetLocationAsync(request);
                if (location != null)
                {
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                    MessagingCenter.Send<string, Position>("MapUpdate", "MoveTo", new Position(location.Latitude, location.Longitude));
                    Position = new Position(location.Latitude, location.Longitude);
                    var airports = await FlightService.GetNearByAirport(Position.Latitude, Position.Longitude);
                    airports.ForEach(item => Airports.Add(item));
                    MessagingCenter.Send<string, List<AirportModel>>("MapUpdate", "PushPins", airports);

                }


            });
            GetTimeTableCommand = new Command<AirportModel>(async (airportModel) =>
            {
                var timetables = await FlightService.GetAirportFlights(airportModel.codeIataAirport, Constants.FlightTypeDeparture);
                timetables.ForEach(item => Timetables.Add(item));
            });
            ClowWindowCommand = new Command(() =>
            {
                ShowInfoWindow = false;
            });
            ViewFlightsCommand = new Command(async () =>
            {
                App.CurrentAirport = SelectedPin;
                await (Application.Current as App).NavigationPage.PushAsync(new TimetablePage());
            });
        }

    }
}
