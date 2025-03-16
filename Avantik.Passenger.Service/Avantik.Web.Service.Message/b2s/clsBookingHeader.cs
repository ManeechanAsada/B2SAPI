using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Avantik.Web.Service.Message.b2s
{
    [MessageContract]
    public class BookingHeader
    {
        [MessageBodyMember]
        public string agency_code { get; set; }
        [MessageBodyMember]
        public string currency_rcd { get; set; }
        [MessageBodyMember]
        public string language_rcd { get; set; }
        [MessageBodyMember]
        public string contact_name { get; set; }
        [MessageBodyMember]
        public string contact_email { get; set; }
        [MessageBodyMember]
        public string received_from { get; set; }
        [MessageBodyMember]
        public string comment { get; set; }
        [MessageBodyMember]
        public string title_rcd { get; set; }
        [MessageBodyMember]
        public string lastname { get; set; }
        [MessageBodyMember]
        public string firstname { get; set; }
        [MessageBodyMember]
        public string middlename { get; set; }
        [MessageBodyMember]
        public string country_rcd { get; set; }
        [MessageBodyMember]
        public string vendor_rcd { get; set; }

        [MessageBodyMember]
        public string mobile_number { get; set; }
        [MessageBodyMember]
        public string business_number { get; set; }
        [MessageBodyMember]
        public string home_number { get; set; }

         
    }
}
