using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;
using Avantik.Web.Service;
using Avantik.Web.Service.Message.Booking;

namespace Avantik.Web.Service.Message
{
    [MessageContract]
    public class GetSpecialServicesRequest
    {
        [MessageHeader]
        public string Token { get; set; }

        [MessageBodyMember]
        IList<b2s.FlightSegment> FlightSegments { get; set; }  
      
        [MessageBodyMember]
        public string ServiceCode { set; get; }

    }
}
