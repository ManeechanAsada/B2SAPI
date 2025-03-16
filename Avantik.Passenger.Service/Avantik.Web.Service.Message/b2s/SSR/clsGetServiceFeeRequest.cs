using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;
using Avantik.Web.Service;
using Avantik.Web.Service.Message.Booking;

namespace Avantik.Web.Service.Message.b2s
{
    [MessageContract]
    public class GetServicesRequest
    {
        [MessageHeader]
        public string Token { get; set; }

        [MessageBodyMember]
        public IList<b2s.BookingSegment> BookingSegments { get; set; }

        [MessageBodyMember]
        public string Currency { get; set; }  

      
    }
}
