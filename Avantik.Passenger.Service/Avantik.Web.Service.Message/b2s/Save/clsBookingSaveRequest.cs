using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Avantik.Web.Service.Message.b2s
{
    [MessageContract]
    public class B2SSaveRequest
    {
        [MessageHeader]
        public string Token { get; set; }

        [MessageBodyMember]
        public BookingHeader BookingHeader { get; set; }
        [MessageBodyMember]
        public IList<FlightSegment> BookingSegments { get; set; }
        [MessageBodyMember]
        public IList<Passenger> Passengers { get; set; }
        [MessageBodyMember]
        public IList<Mapping> Mappings { get; set; }
        [MessageBodyMember]
        public IList<Fee> Fees { get; set; }
        [MessageBodyMember]
        public IList<Remark> Remarks { get; set; }
        [MessageBodyMember]
        public IList<Service> Services { get; set; }
        [MessageBodyMember]
        public IList<Payment> Payments { get; set; }

    }
}
