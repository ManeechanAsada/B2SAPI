using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;
using Avantik.Web.Service.Message;
using Avantik.Web.Service.Message.ManageBooking;
using Avantik.Web.Service.Message.Booking;
using Avantik.Web.Service.Message.SeatMap;
using b2s = Avantik.Web.Service.Message.b2s;
namespace Avantik.Web.Service.Contracts
{
    [ServiceContract(Namespace = "Avantik.API.SpecialService/")]
    public interface IPassengerSpecialService
    {

        [OperationContract()]
        InitializeResponse ServiceInitialize(InitializeRequest request);

        [OperationContract()]
        b2s.B2SSaveResponse SaveB2SBooking(b2s.B2SSaveRequest request);

        [OperationContract()]
        b2s.BookingItemsAddResponse BookingItemsAdd(b2s.BookingItemsAddRequest request);

        [OperationContract()]
        GetSpecialServicesResponse GetSpecialServices(b2s.GetServicesRequest request);

        [OperationContract()]
        GetFeeResponse GetFeeDefinition(GetFeeDefinitionRequest request);

    }
}
