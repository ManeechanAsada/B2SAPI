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
    public class BookingItemsAddRequest
    {
        [MessageHeader]
        public string Token { get; set; }

        //Authentication information.
        [MessageBodyMember]
        public Guid bookingId { get; set; }
        [MessageBodyMember]
        public IList<Payment> Payments { get; set; }
        [MessageBodyMember]
        public IList<Remark> Remarks { get; set; }
        [MessageBodyMember]
        public IList<Fee> Fees { get; set; }
    }
}
