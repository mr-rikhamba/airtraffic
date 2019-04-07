using AirTraffic.Mobile.Helpers;
using AirTraffic.Mobile.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace AirTraffic.Mobile.ViewModels
{
    public class TimetableViewModel : BaseViewModel
    {


        #region Properties
        AirportModel _currentAirport = App.CurrentAirport;
        public AirportModel CurrentAirport
        {
            get { return _currentAirport; }
            set { SetProperty(ref _currentAirport, value); }
        }
        bool _isDeparture;
        public bool IsDeparture
        {
            get { return _isDeparture; }
            set { SetProperty(ref _isDeparture, value); }
        }
        public ObservableCollection<TimetableModel> Timetables { get; set; }
        public ObservableCollection<AirportModel> Airports { get; set; }
        public ObservableCollection<CityModel> Cities { get; set; }
        #endregion

        #region Commands
        public ICommand ToggleFlightTypeCommand { get; }
        public ICommand RefreshTimetableCommand { get; }
        public ICommand RefreshCitiesCommand { get; }
        #endregion


        public TimetableViewModel()
        {
            Title = "Airports";
            Timetables = new ObservableCollection<TimetableModel>();
            Airports = new ObservableCollection<AirportModel>();
            Cities = new ObservableCollection<CityModel>();
            #region Command implementations

            ToggleFlightTypeCommand = new Command(async () =>
            {
                IsDeparture = !IsDeparture;
            });
            RefreshTimetableCommand = new Command(async () =>
            {
                await GetFlights();
            });
            RefreshCitiesCommand = new Command(async () =>
            {
                await GetCities();
            });
            #endregion
            #region Manual Executions of commands in order to initialize cities and timetables based on current airport.
            RefreshCitiesCommand.Execute(null);
            RefreshTimetableCommand.Execute(null); 
            #endregion
        }
        #region Internal actions

        private async Task GetCities()
        {
            var cities = await FlightService.GetCities(CurrentAirport.codeIso2Country);
            cities.ForEach(item => Cities.Add(item));
        }
        private async Task GetFlights()
        {
            var timetables = await FlightService.GetAirportFlights(CurrentAirport.codeIataAirport, Constants.FlightTypeDeparture);
            timetables.ForEach(item =>
            {
                item.arrival.City = Cities.FirstOrDefault(c => c.codeIataCity == item.arrival.iataCode)?.nameCity ?? "City not found";
                item.departure.City = Cities.FirstOrDefault(c => c.codeIataCity == item.departure.iataCode)?.nameCity ?? "City not found";
                Timetables.Add(item);
            });
        } 
        #endregion

    }
}
