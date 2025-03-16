using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ServiceModel;

namespace Avantik.Web.Service.Message.b2s
{
    [MessageContract]
    public class Fee
    {
        [MessageBodyMember]
        public Guid fee_id { get; set; }
        [MessageBodyMember]
        public Guid passenger_id { get; set; }
        [MessageBodyMember]
        public Guid booking_segment_id { get; set; }
        [MessageBodyMember]
        public string fee_rcd { get; set; }
        [MessageBodyMember]
        public string vendor_rcd { get; set; }
        [MessageBodyMember]
        public string currency_rcd { get; set; }

        [MessageBodyMember]
        public string comment { get; set; }
        [MessageBodyMember]
        public decimal fee_amount { get; set; }
        [MessageBodyMember]
        public decimal fee_amount_incl { get; set; }
        [MessageBodyMember]
        public decimal vat_percentage { get; set; }
        [MessageBodyMember]
        public decimal charge_amount { get; set; }
        [MessageBodyMember]
        public decimal charge_amount_incl { get; set; }
        [MessageBodyMember]
        public decimal number_of_units { get; set; }

        [MessageBodyMember]
        public string origin_rcd { get; set; }
        [MessageBodyMember]
        public string destination_rcd { get; set; }
        
    }
}
