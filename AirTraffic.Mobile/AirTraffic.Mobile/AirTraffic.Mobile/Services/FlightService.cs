using Acr.UserDialogs;
using AirTraffic.Mobile.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AirTraffic.Mobile.Services
{
    /// <summary>
    /// Implementation of IFlightService which gets data from Aviation Edge.
    /// </summary>
    public class FlightService : IFlightService
    {
        /*
         https://aviation-edge.com/v2/public/cityDatabase?key=2e0900-309b3d&codeIso2Country=za
         http://aviation-edge.com/v2/public/timetable?key=2e0900-309b3d&iataCode=JNB&type=departure
         http://aviation-edge.com/api/public/nearby?key=2e0900-309b3d&lat=-25.8640&lng=28.0889&distance=1
             */
        #region Constants
        private const string AirportError = "There are no airports within your current proximity. Please try again later.";
        private const string TimetableError = "The selected airport has no asscociated flights at the moment. Please try again later.";
        private const string CityError = "An error occurred while fetching cities. Please try again later.";
        #endregion
        #region IFlightService Implementation
        public async Task<List<TimetableModel>> GetAirportFlights(string iataCode, string flightType)
        {
            var flights = await FetchRemoteData<TimetableModel>("timetable", $"iataCode={iataCode}&type={flightType}", TimetableError);
            return flights ?? new List<TimetableModel>();
        }

        public async Task<List<CityModel>> GetCities(string countryCode)
        {
            var cities = await FetchRemoteData<CityModel>("cityDatabase", $"&codeIso2Country={countryCode}", CityError);
            return cities ?? new List<CityModel>();
        }

        public async Task<List<AirportModel>> GetNearByAirport(double latitude, double longitude)
        {
            var airports = await FetchRemoteData<AirportModel>("nearby", $"lat={latitude}&lng={longitude}&distance=100", TimetableError);
            return airports ?? new List<AirportModel>();
        }
        #endregion
        #region Internal Methods

        private async Task<List<T>> FetchRemoteData<T>(string endPoint, string parameters, string errorMsg)
        {

            if (Xamarin.Essentials.Connectivity.NetworkAccess != Xamarin.Essentials.NetworkAccess.Internet)
            {
                UserDialogs.Instance.Toast("Limited or no network detected. Application may not run as intended.", TimeSpan.FromSeconds(10));
                return null;
            }
            string stringData = string.Empty;

            try
            {
                UserDialogs.Instance.ShowLoading();
                var httpCLient = new HttpClient();
                httpCLient.BaseAddress = new Uri($"https://aviation-edge.com/v2/public/");

                var action = $"{endPoint}?key=2e0900-309b3d&{parameters}";
                var response = await httpCLient.GetAsync(action);
                stringData = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<List<T>>(stringData);
                UserDialogs.Instance.HideLoading();

                return data;
            }
            catch (Exception)
            {
                UserDialogs.Instance.HideLoading();
                if (!string.IsNullOrWhiteSpace(stringData) && stringData.Contains("No Record Found"))
                {
                    UserDialogs.Instance.Alert(errorMsg, "Error");
                }
                else
                {
                    UserDialogs.Instance.Alert("An unexpected error occurred. Please try again later.", "Error");
                }
                return default(List<T>);

            }
        }
        #endregion
    }
}
