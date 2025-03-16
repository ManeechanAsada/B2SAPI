using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;
using Avantik.Web.Service.Message;

namespace Avantik.Web.Service.Contracts
{
    [ServiceContract(Namespace = "Avantik.API.SpecialService/")]
    public interface IAvailabilityService
    {
    }
}
