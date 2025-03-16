using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avantik.Web.Service.Entity.Flight;
using Avantik.Web.Service.Infrastructure;
using Avantik.Web.Service.Entity.Booking;
using Avantik.Web.Service.Entity;

namespace Avantik.Web.Service.Model.Contract
{
    public interface IFeeService
    {
     
        List<ServiceFee> GetSegmentFee(string agencyCode,
                               string currencyCode,
                               string languageCode,
                               int numberOfPassenger,
                               int numberOfInfant,
                               IList<PassengerService> services,
                               IList<SegmentService> segmentService,
                               bool SpecialService,
                               bool bNovat);

    }
}
