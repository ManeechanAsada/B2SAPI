using Avantik.Web.Service.Model.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avantik.Web.Service.Entity.Agency;
using Avantik.Web.Service.COMHelper;
using System.Runtime.InteropServices;

using Avantik.Web.Service.Exception.Booking;


namespace Avantik.Web.Service.Model.COM
{
    public class AgencyService : RunComplus, IAgencyService
    {
        string _server = string.Empty;
        public AgencyService(string server, string user, string pass, string domain)
            :base(user,pass,domain)
        {
            _server = server;
        }

        public Agent GetAgencySessionProfile(string strAgencyCode, string strUserId)
        {
            Agent agent = null; 
          
            return agent;
        }

        public Agent TravelAgentLogon(string agencyCode, string agentLogon, string agentPassword)
        {

            Agent agent = null;
        

            return agent;
        }
       


    }
}
