using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Avantik.Web.Service.Message.b2s
{
    [MessageContract]
    public class CreditCard
    {
        [MessageBodyMember]
        public string credit_card_number { get; set; }
        [MessageBodyMember]
        public string name_on_card { get; set; }
        [MessageBodyMember]
        public string cvv_code { get; set; }
        [MessageBodyMember]
        public string issue_number { get; set; }
        [MessageBodyMember]
        public int expiry_month { get; set; }
        [MessageBodyMember]
        public int expiry_year { get; set; }
        [MessageBodyMember]
        public int issue_month { get; set; }
        [MessageBodyMember]
        public int issue_year { get; set; }

        [MessageBodyMember]
        public string address_line1 { get; set; }
        [MessageBodyMember]
        public string address_line2 { get; set; }
        [MessageBodyMember]
        public string street { get; set; }
        [MessageBodyMember]
        public string po_box { get; set; }
        [MessageBodyMember]
        public string city { get; set; }
        [MessageBodyMember]
        public string state { get; set; }
        [MessageBodyMember]
        public string district { get; set; }
        [MessageBodyMember]
        public string province { get; set; }
        [MessageBodyMember]
        public string zip_code { get; set; }

    }
}
