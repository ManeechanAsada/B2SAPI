using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avantik.Web.Service.Entity.REST.Token
{
    public class TravelAgentLogonRequest
    {
        public string AgencyCode { get; set; }
        public string AgentLogon { get; set; }
        public string AgentPassword { get; set; }
    }

    public class TravelAgentLogonResponse : ResponseBase
    {
        public Agency.Agent AgentResponse { get; set; }

    }
    public class ResponseBase
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Code { get; set; }
    }
}
