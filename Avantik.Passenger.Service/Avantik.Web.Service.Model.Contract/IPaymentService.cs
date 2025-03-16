using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avantik.Web.Service.Entity.Flight;
using Avantik.Web.Service.Infrastructure;
using Avantik.Web.Service.Entity.Booking;
using Avantik.Web.Service.Entity.Agency;
using Avantik.Web.Service.Entity;

namespace Avantik.Web.Service.Model.Contract
{
    public interface IPaymentService
    {
        bool SavePayment(IList<Payment> payments, IList<PaymentAllocation> paymentAllocations);

      

        string ExternalPaymentAddPayment(
                                 string strBookingId,
                                 string strAgencyCode,
                                 string strFormOfPayment,
                                 string strCurrencyCode,
                                 double dAmount,
                                 string strFormOfPaymentSubtype,
                                 string strUserId,
                                 string strDocumentNumber,
                                 string strPaymentNumber,
                                 string strApprovalCode,
                                 string strRemark,
                                 string strLanguage,
                                 DateTime dtPayment,
                                 string strPaymentTransaction,
                                 string strPaymentReference,
                                 string strReceiveCurrencyCode,
                                 double dReceivePaymentAmount,
                                 IList<Avantik.Web.Service.Entity.Booking.Mapping> mappings,
                                 IList<Avantik.Web.Service.Entity.Booking.Fee> fees


           );


    }
}
