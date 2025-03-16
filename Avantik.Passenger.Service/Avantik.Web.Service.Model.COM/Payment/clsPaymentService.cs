using Avantik.Web.Service.Model.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Avantik.Web.Service.Entity.Booking;
using Avantik.Web.Service.Entity;

using Avantik.Web.Service.Helpers;
using Newtonsoft.Json;
using System.Net;
using System.IO;

namespace Avantik.Web.Service.Model.COM
{
    public class PaymentService : RunComplus, IPaymentService
    {
        string _server = string.Empty;
        string _user = string.Empty;
        string _pass = string.Empty;
        string _domain = string.Empty;
        public PaymentService(string server, string user, string pass, string domain)
            : base(user, pass, domain)
        {
            _server = server;
            _user = user;
            _pass = pass;
            _domain = domain;
        }
        public string ExternalPaymentAddPayment(
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

     )
        {

            //create payment
            IList<Payment> payments = new List<Payment>();
            Payment payment = new Payment();
            payment.BookingPaymentId = Guid.NewGuid();
            payment.BookingId = new Guid(strBookingId);
            // payment.BookingSegmentId = mappings[0].BookingSegmentId;
            payment.AgencyCode = strAgencyCode;
            payment.FormOfPaymentRcd = !string.IsNullOrEmpty(strFormOfPayment) ? strFormOfPayment : ""; //strFormOfPayment; 
            payment.FormOfPaymentSubtypeRcd = !string.IsNullOrEmpty(strFormOfPaymentSubtype) ? strFormOfPaymentSubtype : "";
            payment.CurrencyRcd = strCurrencyCode;
            payment.ReceiveCurrencyRcd = strReceiveCurrencyCode;
            payment.PaymentAmount = Convert.ToDecimal(dAmount);
            payment.ReceivePaymentAmount = Convert.ToDecimal(dReceivePaymentAmount);

            payment.UpdateBy = new Guid(strUserId);
            payment.CreateBy = new Guid(strUserId);
            payment.CreateDateTime = DateTime.Now;
            payment.UpdateDateTime = DateTime.Now;
            payment.PaymentBy = new Guid(strUserId);

            payment.DocumentNumber = strDocumentNumber;
            payment.PaymentNumber = strPaymentNumber;
            payment.ApprovalCode = strApprovalCode;
            payment.PaymentRemark = strRemark;
            payment.TransactionReference = strPaymentTransaction;
            payment.PaymentReference = strPaymentReference;
            payment.PaymentDateTime = dtPayment;

            payments.Add(payment);


            IList<Avantik.Web.Service.Entity.Booking.Fee> fees1 = new List<Avantik.Web.Service.Entity.Booking.Fee>();

            if (fees != null && fees.Count > 0)
            {
                fees1 = fees;
            }
            else
            {
                fees1 = null;
            }



            // Initialize and populate the BookingSaveRequest object
            var paymentRequest = new Avantik.Web.Service.Entity.Booking.REST.BookingPaymentRequest
            {
                Mappings = mappings,
                Fees = fees1,
                Payments = payments,
                CreateTicket = true

            };

            //call REST API
            string baseURL = ConfigHelper.ToString("RESTURL");
            //string baseURL = "https://localhost:7021/";//ConfigHelper.ToString("RESTURL");
            string apiURL = baseURL + "api/Booking/BookingExternalPayment";

            var jsonContent = JsonConvert.SerializeObject(paymentRequest);
            var content = System.Text.Encoding.UTF8.GetBytes(jsonContent);

            var requestUri = new Uri(apiURL);

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUri);

            var userlogon = string.Format("{0}:{1}", "DLXAPI", "dlxapi");
            byte[] bytes = Encoding.UTF8.GetBytes(userlogon);
            string base64String = Convert.ToBase64String(bytes);
            httpWebRequest.Headers["Authorization"] = "Basic " + base64String;


            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.ContentLength = content.Length;


            try
            {
                using (System.IO.Stream requestStream = httpWebRequest.GetRequestStream())
                {
                    requestStream.Write(content, 0, content.Length);
                }
                using (HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    if (httpResponse.StatusCode == HttpStatusCode.OK)
                    {
                        using (StreamReader reader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            var responseContent = reader.ReadToEnd();
                            Avantik.Web.Service.Entity.Booking.REST.BookingPaymentResponse re = JsonConvert.DeserializeObject<Avantik.Web.Service.Entity.Booking.REST.BookingPaymentResponse>(responseContent);

                        }

                    }
                }

            }
            catch (System.Exception ex)
            {
            }

            return strBookingId;
        }

        public bool SavePayment(IList<Payment> payments, IList<PaymentAllocation> paymentAllocations)
        {
          

            return true;

        }





     
    }
}
