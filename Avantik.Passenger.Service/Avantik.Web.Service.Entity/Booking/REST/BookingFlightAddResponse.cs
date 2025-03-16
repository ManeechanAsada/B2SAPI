using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avantik.Web.Service.Entity.Booking.REST
{

    public class BookingFlightAddResponse : ResponseBase
    {
        public Booking Booking { get; set; }
    }

    public class Booking
    {
        public BookingHeader Header { get; set; }
        public IList<FlightSegment> Segments { get; set; }
        public IList<Passenger> Passengers { get; set; }
        public IList<Remark> Remarks { get; set; }
        public IList<Payment> Payments { get; set; }
        public IList<Mapping> Mappings { get; set; }
        public IList<PassengerService> Services { get; set; }
        public IList<Tax> Taxs { get; set; }
        public IList<Fee> Fees { get; set; }
        public IList<Quote> Quotes { get; set; }
    }


}
