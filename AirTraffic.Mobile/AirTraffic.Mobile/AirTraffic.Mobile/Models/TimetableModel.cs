using System;

namespace AirTraffic.Mobile.Models
{
    public class TimetableModel
    {
        public string type { get; set; }
        public string status { get; set; }
        public Departure departure { get; set; }
        public Arrival arrival { get; set; }
        public Airline airline { get; set; }
        public Flight flight { get; set; }
        public Codeshared codeshared { get; set; }
        public string StatusImage { get { return status == "Departed" ? "Red_dot_x1.png" : "green_dot_x1.png"; }  }
    }

    public class Departure
    {
        public string iataCode { get; set; }
        public string icaoCode { get; set; }
        public string terminal { get; set; }
        public string gate { get; set; }
        public DateTime scheduledTime { get; set; }
        public DateTime estimatedTime { get; set; }
        public DateTime actualTime { get; set; }
        public DateTime estimatedRunway { get; set; }
        public DateTime actualRunway { get; set; }
        public int delay { get; set; }

        public string SimpleTime { get { return scheduledTime.ToString("HH:mm"); } }

        public string City { get; internal set; }
    }

    public class Arrival
    {
        public string iataCode { get; set; }
        public string icaoCode { get; set; }
        public DateTime scheduledTime { get; set; }
        public DateTime estimatedTime { get; set; }
        public DateTime actualTime { get; set; }
        public string baggage { get; set; }
        public DateTime estimatedRunway { get; set; }
        public DateTime actualRunway { get; set; }
        public int delay { get; set; }
        public string terminal { get; set; }
        public string gate { get; set; }
        public string City { get;  set; }
    }

    public class Airline
    {
        public string name { get; set; }
        public string iataCode { get; set; }
        public string icaoCode { get; set; }
    }

    public class Flight
    {
        public string number { get; set; }
        public string iataNumber { get; set; }
        public string icaoNumber { get; set; }
    }

    public class Codeshared
    {
        public Airline1 airline { get; set; }
        public Flight1 flight { get; set; }
    }

    public class Airline1
    {
        public string name { get; set; }
        public string iataCode { get; set; }
        public string icaoCode { get; set; }
    }

    public class Flight1
    {
        public string number { get; set; }
        public string iataNumber { get; set; }
        public string icaoNumber { get; set; }
    }

}
