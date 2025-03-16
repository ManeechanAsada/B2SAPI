﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;
using Avantik.Web.Service.Message.Booking;
using Avantik.Web.Service.Contracts;
using Avantik.Web.Service.Message;
using Avantik.Web.Service.Message.Fee;
using Avantik.Web.Service.Message.SeatMap;

namespace Avantik.Web.Service.Proxy
{
    public class BookingServiceProxy : ClientBase<IBookingService>, IBookingService
    {
    }
}
