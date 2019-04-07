using AirTraffic.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AirTraffic.Mobile.Services
{
    /// <summary>
    /// An interface which defines a service which can be used to gather flight informations for different airports
    /// </summary>
    public interface IFlightService
    {
        /// <summary>
        /// Fetch airports based on a given location
        /// </summary>
        /// <param name="latitude">Latitude of geoposition</param>
        /// <param name="longitude">Longitude of geoposition</param>
        /// <returns></returns>
        Task<List<AirportModel>> GetNearByAirport(double latitude, double longitude);
        /// <summary>
        /// Fetch flights for a specific airport
        /// </summary>
        /// <param name="iataCode">Code used to determine which aiport to fetch the flight timetable for</param>
        /// <param name="flightType">Determines whether to fetch arrivals or departures</param>
        /// <returns></returns>
        Task<List<TimetableModel>> GetAirportFlights(string iataCode, string flightType);
        /// <summary>
        /// Get cities by country code
        /// </summary>
        /// <param name="countryCode">Country code</param>
        /// <returns></returns>
        Task<List<CityModel>> GetCities(string countryCode);

    }
}
