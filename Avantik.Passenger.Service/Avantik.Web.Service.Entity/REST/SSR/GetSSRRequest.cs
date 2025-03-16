using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avantik.Web.Service.Entity.REST.GetSSR
{   
    public class GetSSRRequest
    {
        public string AgencyCode { get; set; }
        public IList<BookingSegment> BookingSegments { get; set; }
        public string Currency { get; set; }
    }

    public class BookingSegment
    {
        public DateTime departure_date { get; set; }
        public string airline_rcd { get; set; }
        public string flight_number { get; set; }
        public string origin_rcd { get; set; }
        public string destination_rcd { get; set; }
        public string booking_class_rcd { get; set; }
    }

    public class GetSpecialServicesResponse : ResponseBase
    {
        public IList<ServiceFee> ServiceFees { get; set; }

    }

}
