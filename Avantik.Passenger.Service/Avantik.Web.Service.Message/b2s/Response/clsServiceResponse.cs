using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Avantik.Web.Service.Message.b2s
{
    [MessageContract]
    public class ServiceResponse : Service
    {
        [MessageBodyMember]
        public string error_code { get; set; }
        [MessageBodyMember]
        public string error_message { get; set; }
        [MessageBodyMember]
        public string special_service_status_rcd { get; set; }

        
    }
}
