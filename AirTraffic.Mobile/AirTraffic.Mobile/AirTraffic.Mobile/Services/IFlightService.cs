using AirTraffic.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AirTraffic.Mobile.Services
{
    public interface IFlightService
    {
        Task<List<AirportModel>> GetNearByAirport(double latitude, double longitude);
        Task<List<TimetableModel>> GetAirportFlights(string iataCode, string flightType);
        Task<List<CityModel>> GetCities(string countryCode);

    }
}
