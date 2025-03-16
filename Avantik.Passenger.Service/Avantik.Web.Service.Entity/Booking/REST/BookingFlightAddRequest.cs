using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using entity = Avantik.Web.Service.Entity.Booking;

namespace Avantik.Web.Service.Entity.Booking.REST
{
    public class BookingFlightAddRequest
    {
        #region BookFlight
        public string AgencyCode { get; set; }
        public string Currency { get; set; }
        public IList<Flight> Flight { get; set; }
        public string BookingId { get; set; }
        public short Adults { get; set; }
        public short Children { get; set; }
        public short Infants { get; set; }
        public short Others { get; set; }
        public string StrOthers { get; set; }
        public string UserId { get; set; }
        public string StrIpAddress { get; set; }
        public string StrLanguageCode { get; set; }
        public bool BNoVat { get; set; }
        #endregion

    }

    public class BookingPaymentRequest
    {
        public IList<Mapping> Mappings { get; set; }
        public IList<Fee> Fees { get; set; }
        public IList<Payment> Payments { get; set; }
        public bool CreateTicket { get; set; }
    }

    public class BookingPaymentResponse : ResponseBase
    {

    }

    public class BookingExternalPaymentResponse : ResponseBase
    {
        public string Result { get; set; }
    }

}
