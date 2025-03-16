using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Avantik.Web.Service.Message.b2s
{
    [MessageContract]
    public class BookingHeaderResponse : BookingHeader
    {
        [MessageBodyMember]
        public string error_code { get; set; }
        [MessageBodyMember]
        public string error_message { get; set; }

        //Response property
        [MessageBodyMember]
        public Guid booking_id { get; set; }
        [MessageBodyMember]
        public string record_locator { get; set; }
    }
}
