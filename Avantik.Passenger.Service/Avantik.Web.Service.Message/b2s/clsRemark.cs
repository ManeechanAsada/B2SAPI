using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Avantik.Web.Service.Message.b2s
{
    [MessageContract]
    public class Remark
    {
        [MessageBodyMember]
        public string remark_type_rcd { get; set; }
        [MessageBodyMember]
        public string remark_text { get; set; }
        [MessageBodyMember]
        public string nickname { get; set; }
    }
}
