using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avantik.Web.Service.Entity.Flight;
using Avantik.Web.Service.Infrastructure;
using Avantik.Web.Service.Entity.Booking;
using Avantik.Web.Service.Entity.Agency;

namespace Avantik.Web.Service.Model.Contract
{
    public interface IBookingModelService 
    {
          bool SaveBooking(Booking booking,
                           bool createTickets,
                           bool readBooking,
                           bool readOnly,
                           bool bSetLock,
                           bool bCheckSeatAssignment,
                           bool bCheckSessionTimeOut);




    }
}
