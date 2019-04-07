using AirTraffic.Mobile.Models;
using AirTraffic.Mobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;

namespace AirTraffic.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : ContentPage
    {
        public MapViewModel MapViewModel { get; set; }
        public MapPage()
        {
            InitializeComponent();
            MapViewModel = (MapViewModel)BindingContext;

            MessagingCenter.Subscribe<string, Position>("MapUpdate", "MoveTo", (sender, pos) => {
           
                var pin = new Pin()
                {
                    Type = PinType.Place,
                    Label = "Your current location",
                    Position = new Position(pos.Latitude, pos.Longitude),
                    Icon = BitmapDescriptorFactory.DefaultMarker(Color.Blue),
                    Tag = "current_location",
                    IsVisible = true
                };
                map.Pins.Add(pin);
                map.MoveToRegion(MapSpan.FromCenterAndRadius(pos, Xamarin.Forms.GoogleMaps.Distance.FromMeters(5000)));
            });


            MessagingCenter.Subscribe<string, List<AirportModel>>("MapUpdate", "PushPins", (sender, pins) => {

                pins.ForEach((pinModel) =>
                {
                    try
                    {
                        var lat = Double.Parse(pinModel.latitudeAirport);
                        var lon = Double.Parse(pinModel.longitudeAirport);
                        var pos = new Position(lat, lon);
                        var pin = new Pin()
                        {
                            Type = PinType.Place,
                            Label = $"{pinModel.nameAirport}({pinModel.codeIataAirport}), {pinModel.nameCountry}",
                            Position = pos,
                            Icon = BitmapDescriptorFactory.DefaultMarker(Color.Red),
                            Tag = pinModel.codeIataAirport,
                            IsVisible = true
                        };
                        map.Pins.Add(pin);
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                });
            });

            map.PinClicked += Map_PinClicked;
        }


        private void Map_PinClicked(object sender, PinClickedEventArgs e)
        {
            if (e.Pin.Tag == typeof(string))
            {
                return;
            }
            MapViewModel.SelectedPin = MapViewModel.Airports.FirstOrDefault(c => c.codeIataAirport == (string)e.Pin.Tag);
            MapViewModel.ShowInfoWindow = true;
        }
    }
}