using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
namespace Avantik.Web.Service.Message.b2s
{
    [MessageContract]
    public class Mapping
    {
        [MessageBodyMember]
        public Guid passenger_id { get; set; }
        [MessageBodyMember]
        public Guid booking_segment_id { get; set; }
        [MessageBodyMember]
        public string seat_number { get; set; }
        [MessageBodyMember]
        public string currency_rcd { get; set; }
        //[MessageBodyMember]
        //public string endorsement_text { get; set; }
        //[MessageBodyMember]
        //public string restriction_text { get; set; }
       // [MessageBodyMember]
       // public decimal net_total { get; set; }
    }
}
