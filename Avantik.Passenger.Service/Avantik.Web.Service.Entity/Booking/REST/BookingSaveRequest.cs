using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using entity = Avantik.Web.Service.Entity.Booking;

namespace Avantik.Web.Service.Entity.Booking.REST
{
    public class BookingSaveRequest
    {
        #region BookingSave
        public entity.Booking booking { get; set; }
        public bool createTickets { get; set; } = false;
        public bool readBooking { get; set; } = false;
        public bool readOnly { get; set; } = false;
        public bool bSetLock { get; set; } = false;
        public bool bCheckSeatAssignment { get; set; } = false;
        public bool bCheckSessionTimeOut { get; set; } = false;
        #endregion
    }
}
