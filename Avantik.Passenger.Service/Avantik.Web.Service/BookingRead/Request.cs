using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avantik.Web.Service.BookingRead
{
    public class BookingReadRequest
    {
        public Guid booking_id { get; set; }

        public bool bReadHeader { get; set; }
        public bool bReadSegment { get; set; }
        public bool bReadPassenger { get; set; }
        public bool bReadRemark { get; set; }
        public bool bReadPayment { get; set; }
        public bool bReadMapping { get; set; }
        public bool bReadService { get; set; }
        public bool bReadTax { get; set; }
        public bool bReadFee { get; set; }
        public bool bReadOd { get; set; }
    }
}
