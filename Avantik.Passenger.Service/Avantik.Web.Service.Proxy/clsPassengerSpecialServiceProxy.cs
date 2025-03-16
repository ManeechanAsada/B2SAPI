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
using Avantik.Web.Service.Message.ManageBooking;
using Avantik.Web.Service.Message.SeatMap;
using b2s = Avantik.Web.Service.Message.b2s;
namespace Avantik.Web.Service.Proxy
{
    public class PassengerSpecialServiceProxy : ClientBase<IPassengerSpecialService>, IPassengerSpecialService
    {
        public InitializeResponse ServiceInitialize(InitializeRequest request)
        {
            return base.Channel.ServiceInitialize(request);
        }
        public b2s.B2SSaveResponse SaveB2SBooking(b2s.B2SSaveRequest request)
        {
            return base.Channel.SaveB2SBooking(request);
        }
        public b2s.BookingItemsAddResponse BookingItemsAdd(b2s.BookingItemsAddRequest request)
        {
            return base.Channel.BookingItemsAdd(request);
        }
        public GetSpecialServicesResponse GetSpecialServices(b2s.GetServicesRequest request)
        {
            return base.Channel.GetSpecialServices(request);
        }
        public GetFeeResponse GetFeeDefinition(GetFeeDefinitionRequest request)
        {
            return base.Channel.GetFeeDefinition(request);
        }

    }
}
