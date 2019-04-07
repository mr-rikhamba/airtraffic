using Acr.UserDialogs;
using AirTraffic.Mobile.Helpers;
using AirTraffic.Mobile.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace AirTraffic.Mobile.Services
{
    /// <summary>
    /// A mock implementation of IFlightService which fetches data from a json file
    /// </summary>
    public class MockFlightService : IFlightService
    {
        public async Task<List<TimetableModel>> GetAirportFlights(string iataCode, string flightType)
        {
            var response = await ReadJsonFile<TimetableModel>("Timetables.json");
            return response;
        }

        public async Task<List<CityModel>> GetCities(string countryCode)
        {
            var response = await ReadJsonFile<CityModel>("Cities.json");
            return response;
        }

        public async Task<List<AirportModel>> GetNearByAirport(double latitude, double longitude)
        {
            var response = await ReadJsonFile<AirportModel>("Airports.json");
            return response;
        }

        /// <summary>
        /// Reads data from a local text or json file and deserializes to a given type
        /// </summary>
        /// <typeparam name="T">Type required to deserialize</typeparam>
        /// <param name="fileName">Name of the file where required mock data is stored.</param>
        /// <returns></returns>
        private async Task<List<T>> ReadJsonFile<T>(string fileName)
        {
            try
            {
                if (Xamarin.Essentials.Connectivity.NetworkAccess != Xamarin.Essentials.NetworkAccess.Internet)
                {
                    UserDialogs.Instance.Toast("Limited or no network detected. Application may not run as intended.", TimeSpan.FromSeconds(10));
                    return null;
                }
                var assembly = IntrospectionExtensions.GetTypeInfo(typeof(LoadResourceText)).Assembly;
                Stream stream = assembly.GetManifestResourceStream($"AirTraffic.Mobile.MockData.{fileName}");

                using (var reader = new StreamReader(stream))
                {
                    var json = await reader.ReadToEndAsync();

                    var data = JsonConvert.DeserializeObject<List<T>>(json);
                    return data;
                }
            }
            catch (System.Exception ex)
            {

                throw new System.Exception("The data service is unavailable at the moment. Please try again later.");
            }


        }
    }
}
