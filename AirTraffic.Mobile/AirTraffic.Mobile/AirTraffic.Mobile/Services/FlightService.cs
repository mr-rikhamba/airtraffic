using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AirTraffic.Mobile.Models;
using Newtonsoft.Json;

namespace AirTraffic.Mobile.Services
{
    public class FlightService : IFlightService
    {
        /*
         https://aviation-edge.com/v2/public/cityDatabase?key=2e0900-309b3d&codeIso2Country=za
         http://aviation-edge.com/v2/public/timetable?key=2e0900-309b3d&iataCode=JNB&type=departure
         http://aviation-edge.com/api/public/nearby?key=2e0900-309b3d&lat=-25.8640&lng=28.0889&distance=1
             */
        public async Task<List<TimetableModel>> GetAirportFlights(string iataCode, string flightType)
        {
            var flights = await FetchRemoteData<TimetableModel>("nearby", $"iataCode={iataCode}&type={flightType}");
            return flights;
        }

        public async Task<List<CityModel>> GetCities(string countryCode)
        {
            var cities = await FetchRemoteData<CityModel>("nearby", $"&codeIso2Country={countryCode}");
            return cities;
        }

        public async Task<List<AirportModel>> GetNearByAirport(double latitude, double longitude)
        {
            var airports = await FetchRemoteData<AirportModel>("nearby", $"lat={latitude}&lng={longitude}&distance=1");
            return airports;
        }

        private async Task<List<T>> FetchRemoteData<T>(string endPoint, string parameters)
        {

            var httpCLient = new HttpClient();
            httpCLient.BaseAddress = new Uri($"https://aviation-edge.com/v2/public/");

            var action = $"{endPoint}?key=2e0900-309b3d&{parameters}";
            var response = await httpCLient.GetAsync(action);
            var stringData = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<T>>(stringData);

            return data;
        }
    }
}
