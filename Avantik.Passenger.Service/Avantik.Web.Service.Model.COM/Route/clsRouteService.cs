using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using Avantik.Web.Service.Model.Contract;
using Avantik.Web.Service.Entity.Route;
using Avantik.Web.Service.COMHelper;

namespace Avantik.Web.Service.Model.COM
{
    public class RouteService : RunComplus, IRouteService
    {
        string _server = string.Empty;
        public RouteService(string server, string user, string pass, string domain)
            : base(user, pass, domain)
        {
            _server = server;
        }

        public List<Route> GetOrigins(string strLanguage, bool b2cFlag, bool b2bFlag, bool b2EFlag, bool b2SFlag, bool APIFlag)
        {
            try
            {
                List<Route> routes = new List<Route>();
               

                return routes;
            }
            catch (SystemException ex)
            {
                throw ex;
            }
            finally
            {
              
            }
        }

        public List<Route> GetDestinations(string strLanguage, bool b2cFlag, bool b2bFlag, bool b2EFlag, bool b2SFlag, bool APIFlag)
        {
            List<Route> routes = new List<Route>();
            return routes;


        }
    }
}
