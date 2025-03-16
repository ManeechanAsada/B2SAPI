using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avantik.Web.Service.Entity.REST.GetFeeDefinition
{
    public class GetFeeDefinitionRequest
    {
        public string AgencyCode { get; set; }
        public string FeeRcd { set; get; }
        public string BookingClass { set; get; }
        public string FareCode { set; get; }
        public string Origin { set; get; }
        public string Destination { set; get; }
        public string LanguageRcd { set; get; }
        public string Currency { set; get; }
    }

}
