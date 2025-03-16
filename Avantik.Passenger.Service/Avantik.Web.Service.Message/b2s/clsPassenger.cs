using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Avantik.Web.Service.Message.b2s
{
    [MessageContract]
    public class Passenger
    {
        [MessageBodyMember]
        public Guid passenger_id { get; set; }
        [MessageBodyMember]
        public DateTime date_of_birth { get; set; }
        [MessageBodyMember]
        public string passenger_type_rcd { get; set; }
        [MessageBodyMember]
        public string lastname { get; set; }
        [MessageBodyMember]
        public string firstname { get; set; }
        [MessageBodyMember]
        public string middlename { get; set; }
        [MessageBodyMember]
        public string title_rcd { get; set; }
        [MessageBodyMember]
        public string gender_type_rcd { get; set; }
        [MessageBodyMember]
        public string nationality_rcd { get; set; }
        [MessageBodyMember]
        public Guid guardian_passenger_id { get; set; }
        
    }
}
