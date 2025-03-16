using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Avantik.Web.Service.Message.b2s
{
    [MessageContract]
    public class BookingSegment
    {
        [MessageBodyMember]
        public DateTime departure_date { get; set; }
        [MessageBodyMember]
        public string airline_rcd { get; set; }
        [MessageBodyMember]
        public string flight_number { get; set; }
        [MessageBodyMember]
        public string origin_rcd { get; set; }
        [MessageBodyMember]
        public string destination_rcd { get; set; }
        [MessageBodyMember]
        public string booking_class_rcd { get; set; }
    }
}
