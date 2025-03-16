using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avantik.Web.Service.Entity.REST.GetFeeDefinition
{
    public class GetFeeDefinitionResponse : ResponseBase
    {
        public IList<MessageFee> Fees { get; set; }
    }
    public class MessageFee
    {
        public string AgencyCode { get; set; }
        public string Comment { get; set; }
        public string CurrencyRcd { get; set; }
        public string DestinationRcd { get; set; }
        public string DisplayName { get; set; }
        public decimal FeeAmount { get; set; }
        public decimal FeeAmountIncl { get; set; }
        public string FeeCategoryRcd { get; set; }
        public Guid FeeId { get; set; }
        public string FeeRcd { get; set; }
        public byte ManualFeeFlag { get; set; }
        public byte MinimumFeeAmountFlag { get; set; }
        public string OdDestinationRcd { get; set; }
        public byte OdFlag { get; set; }
        public string OdOriginRcd { get; set; }
        public string OriginRcd { get; set; }
        public decimal VatPercentage { get; set; }
    }
}
