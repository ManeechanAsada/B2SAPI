using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Avantik.Web.Service.Message.b2s
{
    [MessageContract]
    public class BookingItemsAddResponse : ResponseBase
    {
        [MessageBodyMember]
        public string record_locator { get; set; }
        [MessageBodyMember]
        public Guid booking_id { get; set; }
        [MessageBodyMember]
        public IList<PaymentResponse> Payments { get; set; }
        [MessageBodyMember]
        public IList<RemarkResponse> Remarks { get; set; }
        [MessageBodyMember]
        public IList<FeeResponse> Fees { get; set; }
    }
}
