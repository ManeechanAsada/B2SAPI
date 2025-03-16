using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Avantik.Web.Service.Message;
using Avantik.Web.Service.Contracts;
using Avantik.Web.Service.Helpers;
using Avantik.Web.Service.Entity;
using Avantik.Web.Service.Model;
using Avantik.Web.Service.Model.Contract;
using Avantik.Web.Service.Exception.Flight;
using Avantik.Web.Service.Message.Booking;
using Avantik.Web.Service.Model.Booking;
using Avantik.Web.Service.Extension;
using Avantik.Web.Service.Entity.Booking;
using Avantik.Web.Service.Entity.Agency;
using Avantik.Web.Service.Message.Fee;
using Avantik.Web.Service.Message.SeatMap;
using Avantik.Web.Service.Exception.Booking;
using System.Web;
using System.ServiceModel;
using Newtonsoft.Json;
using System.Net;
using System.IO;

namespace Avantik.Web.Service
{
   [ServiceBehavior(AddressFilterMode = AddressFilterMode.Any)]
    public class BookingService : IBookingService
    {
        public BookingSaveResponse SaveBooking(BookingSaveRequest Request)
        {
            IBookingModelService objBookingService = BookingServiceFactory.CreateInstance();
            BookingSaveResponse response = new BookingSaveResponse();
            BookingPaymentResponse paymentResponse = null;
            Booking booking = new Booking();
            bool result = false;
            bool createTickets = false;
            bool readBooking = false;
            bool readOnly = false;
            bool bSetLock = false;
            bool bCheckSeatAssignment = true;
            bool bCheckSessionTimeOut = true;
            Guid userId = Guid.Empty; 
            string agencyCode = string.Empty;

            try
            {
                // valid token
                Avantik.Web.Service.Entity.Authentication objAuthen = Infrastructrue.Authentication.Authenticate(Request.Token);
                if (objAuthen.ResponseSuccess == false)
                {
                    response.Success = objAuthen.ResponseSuccess;
                    response.Message = objAuthen.ResponseMessage;
                    response.Code = objAuthen.ResponseCode;
                    return response;
                }
                else
                {
                    // prepare data
                    userId = objAuthen.UserId;
                    agencyCode = objAuthen.AgencyCode;

                    //Header
                    if (Request.Booking.Header != null)
                    {
                        Request.Booking.Header.UpdateBy = userId;
                        Request.Booking.Header.CreateBy = userId;
                        Request.Booking.Header.AgencyCode = agencyCode;
                    }

                    //Segment
                    if (Request.Booking.FlightSegments != null && Request.Booking.FlightSegments.Count > 0)
                    {
                        foreach (Message.Booking.FlightSegment obj in Request.Booking.FlightSegments)
                        {
                            obj.CreateBy = userId;
                            obj.UpdateBy = userId;
                        }
                    }
                    //passenger
                    if (Request.Booking.Passengers != null && Request.Booking.Passengers.Count > 0)
                    {
                        foreach (Message.Booking.Passenger obj in Request.Booking.Passengers)
                        {
                            obj.CreateBy = userId;
                            obj.UpdateBy = userId;
                        }
                    }
                    //mapping
                    if (Request.Booking.Mappings != null && Request.Booking.Mappings.Count > 0)
                    {
                        foreach (Message.Booking.Mapping obj in Request.Booking.Mappings)
                        {
                            obj.CreateBy = userId;
                            obj.UpdateBy = userId;
                            obj.AgencyCode = agencyCode;
                        }
                    }
                    //fee
                    if (Request.Booking.Fees != null && Request.Booking.Fees.Count > 0)
                    {
                        foreach (Message.Booking.Fee obj in Request.Booking.Fees)
                        {
                            obj.CreateBy = userId;
                            obj.UpdateBy = userId;
                            obj.AgencyCode = agencyCode;
                        }
                    }
                    //remark
                    if (Request.Booking.Remarks != null && Request.Booking.Remarks.Count > 0)
                    {
                        foreach (Message.Booking.Remark obj in Request.Booking.Remarks)
                        {
                            obj.CreateBy = userId;
                            obj.UpdateBy = userId;
                        }
                    }
                    //service
                    if (Request.Booking.Services != null && Request.Booking.Services.Count > 0)
                    {
                        foreach (Message.Booking.PassengerService obj in Request.Booking.Services)
                        {
                            obj.CreateBy = userId;
                            obj.UpdateBy = userId;
                        }
                    }

                }

                // map from message to entity
                if (Request != null)
                {
                    booking.Header = Request.Booking.Header.ToEntity();

                    booking.Segments = Request.Booking.FlightSegments.ToListEntity();
                    booking.Passengers = Request.Booking.Passengers.ToListEntity();
                    booking.Mappings = Request.Booking.Mappings.ToListEntity();
                    booking.Payments = Request.Booking.Payments.ToListEntity();
                    booking.Fees = Request.Booking.Fees.ToListEntity();
                    booking.Remarks = Request.Booking.Remarks.ToListEntity();
                    booking.Services = Request.Booking.Services.ToListEntity();
                   // booking.Taxs = Request.Booking.Taxs.ToListEntity();

                    //set flag agency
                    //b2s
                    //if (booking.Header != null)
                    //{
                    //    Agent agent = new Agent();
                    //    IAgencyService objAgencyService = AgencyServiceFactory.CreateInstance();

                    //    agent = objAgencyService.GetAgencySessionProfile(agencyCode, string.Empty);

                    //    if (agent != null)
                    //    {
                    //        //set booking source
                    //        booking.SetBookingSource(agent.OwnAgencyFlag, agent.WebAgencyFlag);
                    //    }
                    //}
                }
                    
                result = objBookingService.SaveBooking(booking,
                                                       createTickets,
                                                       readBooking,
                                                       readOnly,
                                                       bSetLock,
                                                       bCheckSeatAssignment,
                                                       bCheckSessionTimeOut);

                if (result == true && booking.Payments != null && booking.Payments.Count > 0)
                {
                    BookingPaymentRequest payRequest = new BookingPaymentRequest();
                    payRequest.Mappings = Request.Booking.Mappings;
                    payRequest.Payments = Request.Booking.Payments;
                    payRequest.Fees = Request.Booking.Fees;
                    payRequest.Token = Request.Token;

                    paymentResponse = BookingPayment(payRequest);

                    if (paymentResponse.Success == false) result = false;
                }

                if (result == true)
                {
                    response.Code = "000";
                    response.Success = true;
                    response.Message = "Success request.";
                }
                else
                {
                    response.Success = false;
                    response.Message = "No Booking Save";
                }
            }
            catch (BookingSaveException ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                response.Code = ex.ErrorCode;
            }
            catch (System.Exception ex)
            {
                response.Success = false;
                response.Message = "Booking Save error.";
            }

            return response;
        }

        public BookingReadResponse ReadBooking(BookingReadRequest Request)
        {
            // IBookingModelService objBookingService = BookingServiceFactory.CreateInstance();
            BookingReadResponse response = new BookingReadResponse();
            //  string apiURL = "https://localhost:7021/api/Booking/ReadBooking";
            string baseURL = ConfigHelper.ToString("RESTURL");
            string apiURL = baseURL + "api/Booking/ReadBooking";

            try
            {
                if (!String.IsNullOrEmpty(Request.BookingId))
                {
                    var BookingRESTRequest = new Avantik.Web.Service.BookingRead.BookingReadRequest
                    {
                        booking_id = new Guid(Request.BookingId),
                        bReadHeader = true,
                        bReadPassenger = true,
                        bReadSegment = true,
                        bReadMapping = true,
                        bReadPayment = true,
                        bReadTax = true,
                        bReadFee = true,
                        bReadRemark = true,
                        bReadOd = true,
                        bReadService = true
                    };

                    var jsonContent = JsonConvert.SerializeObject(BookingRESTRequest);
                    var content = System.Text.Encoding.UTF8.GetBytes(jsonContent);

                    var requestUri = new Uri(apiURL);

                    HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUri);

                    var userlogon = string.Format("{0}:{1}", "DLXAPI", "dlxapi");
                    byte[] bytes = Encoding.UTF8.GetBytes(userlogon);
                    string base64String = Convert.ToBase64String(bytes);


                    httpWebRequest.Headers["Authorization"] = "Basic " + base64String;
                    //  httpWebRequest.Headers["AnotherHeader"] = "HeaderValue";

                    httpWebRequest.Method = "POST";
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.ContentLength = content.Length;

                    using (Stream requestStream = httpWebRequest.GetRequestStream())
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
                                Avantik.Web.Service.BookingRead.BookingRead bookingRead = JsonConvert.DeserializeObject<Avantik.Web.Service.BookingRead.BookingRead>(responseContent);

                                BookingResponse bookingResponse = new BookingResponse();

                                bookingResponse.Header = bookingRead.Header.ToBookingMessage();
                                bookingResponse.FlightSegments = bookingRead.Segments.ToBookingMessage();
                                bookingResponse.Passengers = bookingRead.Passengers.ToBookingMessage();
                                bookingResponse.Mappings = bookingRead.Mappings.ToBookingMessage();
                                bookingResponse.Fees = bookingRead.Fees.ToBookingMessage();
                                bookingResponse.Services = bookingRead.Services.ToBookingMessage();
                                bookingResponse.Payments = bookingRead.Payments.ToBookingMessage();
                                bookingResponse.Remarks = bookingRead.Remarks.ToBookingMessage();

                                bookingResponse.BookingId = bookingResponse.Header.BookingId.ToString();
                                bookingResponse.RecordLocator = bookingResponse.Header.RecordLocator;

                                response.BookingResponse = new BookingResponse();
                                response.BookingResponse.Header = bookingResponse.Header;
                                response.BookingResponse.FlightSegments = bookingResponse.FlightSegments;
                                response.BookingResponse.Passengers = bookingResponse.Passengers;
                                response.BookingResponse.Mappings = bookingResponse.Mappings;
                                response.BookingResponse.Payments = bookingResponse.Payments;
                                response.BookingResponse.Remarks = bookingResponse.Remarks;
                                response.BookingResponse.Fees = bookingResponse.Fees;
                                response.BookingResponse.Services = bookingResponse.Services;

                                response.Success = true;
                                response.BookingResponse = bookingResponse;
                            }

                        }
                    }
                }
                else
                {
                    response.Message = "Fail";
                    response.Success = false;
                }
            }
            catch (System.Exception ex)
            {
                response.Message = "Booking Read is error";
                response.Success = false;
                Logger.Instance(Logger.LogType.Mail).WriteLog(ex, XMLHelper.JsonSerializer(typeof(BookingReadRequest), Request));
            }

            return response;
        }

    
        public BookingPaymentResponse BookingExternalPayment(BookingPaymentRequest Request)
        {
            IPaymentService objPaymentService = PaymentServiceFactory.CreateInstance();
            BookingPaymentResponse response = new BookingPaymentResponse();

            string strLanguage = "EN";
            string resultPayment = string.Empty; ;

            string strBookingId = Request.Payments[0].BookingId.ToString();
            string strUserId = Request.Payments[0].PaymentBy.ToString();
            double dPaymentAmount = decimal.ToDouble(Request.Payments[0].PaymentAmount);
            double dReceivePaymentAmount = decimal.ToDouble(Request.Payments[0].ReceivePaymentAmount);

            Request.CreateTicket = true;

            // get outstanding balance
            BookingService objService = new BookingService();
            BookingReadResponse readResponse = new BookingReadResponse();
            BookingReadRequest readRequest = new BookingReadRequest();
            readRequest.BookingId = Request.Payments[0].BookingId.ToString();
            Booking booking = new Booking();
            // decimal CalOutStandingBalance = 0;
            //bool bNotYetIssuedTicket = false;

            try
            {
                //read booking
                readResponse = objService.ReadBooking(readRequest);

                if (readResponse.Success)
                {
                    booking.Header = readResponse.BookingResponse.Header.ToEntity();
                    booking.Segments = readResponse.BookingResponse.FlightSegments.ToListEntity();
                    booking.Passengers = readResponse.BookingResponse.Passengers.ToListEntity();
                    booking.Mappings = readResponse.BookingResponse.Mappings.ToListEntity();
                    booking.Services = readResponse.BookingResponse.Services.ToListEntity();
                    booking.Fees = readResponse.BookingResponse.Fees.ToListEntity();

                }

                if (dPaymentAmount != 0)
                {
                    resultPayment = objPaymentService.ExternalPaymentAddPayment(
                                                                 strBookingId,
                                                                 Request.Payments[0].AgencyCode,
                                                                 Request.Payments[0].FormOfPaymentRcd,
                                                                 Request.Payments[0].CurrencyRcd,
                                                                 dPaymentAmount,
                                                                 Request.Payments[0].FormOfPaymentSubtypeRcd,
                                                                 strUserId,
                                                                 Request.Payments[0].DocumentNumber,
                                                                 Request.Payments[0].PaymentNumber,
                                                                 Request.Payments[0].ApprovalCode,
                                                                 Request.Payments[0].PaymentRemark,
                                                                 strLanguage,
                                                                 Request.Payments[0].PaymentDateTime,
                                                                 Request.Payments[0].TransactionReference,
                                                                 Request.Payments[0].PaymentReference,
                                                                 Request.Payments[0].ReceiveCurrencyRcd,
                                                                 dReceivePaymentAmount,
                                                                 booking.Mappings,
                                                                 booking.Fees
                                                                 );
                }


                if (resultPayment != null && resultPayment != string.Empty)
                {
                    if (resultPayment.Contains(strBookingId))
                    {
                        response.Message = "Success";
                        response.Success = true;
                        response.Code = "000";
                    }
                    else
                    {
                        if (resultPayment.Contains("404"))
                        {
                            response.Message = "Agency Code is invalid.";
                            response.Success = false;
                            response.Code = "404";
                        }
                        if (resultPayment.Contains("411"))
                        {
                            response.Message = "Currency Code is invalid.";
                            response.Success = false;
                            response.Code = "411";
                        }
                        if (resultPayment.Contains("412"))
                        {
                            response.Message = "Payment Amount does not match outstanding Balance (Summary).";
                            response.Success = false;
                            response.Code = "412";
                        }
                        if (resultPayment.Contains("413"))
                        {
                            response.Message = "Booking is not balanced after Payment.";
                            response.Success = false;
                            response.Code = "413";
                        }
                        if (resultPayment.Contains("999"))
                        {
                            response.Message = "Payment not processed.";
                            response.Success = false;
                            response.Code = "999";
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                response.Message = "Save payment is error";
                response.Success = false;
                Logger.Instance(Logger.LogType.Mail).WriteLog(ex, XMLHelper.JsonSerializer(typeof(BookingPaymentRequest), Request));
            }

            return response;
        }

        public BookingPaymentResponse BookingPayment(BookingPaymentRequest Request)
        {
            IPaymentService objPaymentService = PaymentServiceFactory.CreateInstance();
            BookingPaymentResponse response = new BookingPaymentResponse();

            Booking booking = null;
            bool resultPayment = false;
            Guid userId = new Guid();
            IList<PaymentAllocation> paymentAllocations = null;

            try
            {
                // valid token
                Avantik.Web.Service.Entity.Authentication objAuthen = Avantik.Web.Service.Infrastructrue.Authentication.Authenticate(Request.Token);
                if (objAuthen.ResponseSuccess == false)
                {
                    response.Success = objAuthen.ResponseSuccess;
                    response.Message = objAuthen.ResponseMessage;
                    response.Code = objAuthen.ResponseCode;
                    return response;
                }
                else
                {
                    userId = objAuthen.UserId;

                    //payment
                    if (Request.Payments != null && Request.Payments.Count > 0)
                    {
                        foreach (Message.Booking.Payment obj in Request.Payments)
                        {
                            obj.CreateBy = userId;
                            obj.UpdateBy = userId;
                        }
                    }

                    //payment
                    if (Request.Fees != null && Request.Fees.Count > 0)
                    {
                        foreach (Message.Booking.Fee obj in Request.Fees)
                        {
                            obj.CreateBy = userId;
                            obj.UpdateBy = userId;
                        }
                    }

                }

                booking = new Booking();
                booking.Payments = Request.Payments.ToListEntity();
                booking.Mappings = Request.Mappings.ToListEntity();
                booking.Fees = Request.Fees.ToListEntity();
                paymentAllocations = booking.CreateAllocation();

                if (paymentAllocations != null)
                    resultPayment = objPaymentService.SavePayment(booking.Payments, paymentAllocations);
               
                if(resultPayment == true)
                {
                    response.Message = "Success";
                    response.Success = true;
                }
                else
                {
                    response.Message = "Fail";
                    response.Success = false;
                }
            }
            catch (System.Exception ex)
            {
                response.Message = "Save payment error.";
                response.Success = false;
                //Logger.Instance(Logger.LogType.Mail).WriteLog(ex, XMLHelper.JsonSerializer(typeof(BookingPaymentRequest), Request));
            }

            return response;
        }


        public AgencySessionProfileResponse GetAgencySessionProfile(AgencySessionProfileRequest Request)
        {
            IAgencyService objAgencyService = AgencyServiceFactory.CreateInstance();
            AgencySessionProfileResponse response = new AgencySessionProfileResponse();
            Agent agent = new Agent();

            try
            {
                agent = objAgencyService.GetAgencySessionProfile(Request.AgencyCode, Request.UserId);

                if (agent != null)
                {
                    response.AgentResponse = agent.ToAgentLogonMessage();
                    response.Message = "Success";
                    response.Success = true;
                }
                else
                {
                    response.Message = "Fail";
                    response.Success = false;
                }
            }
            catch
            {
                response.Message = "Get Agency Profile error.";
                response.Success = false;
            }

            return response;
        }

        public TravelAgentLogonResponse TravelAgentLogon(TravelAgentLogonRequest Request)
        {
            //  IAgencyService objAgencyService = AgencyServiceFactory.CreateInstance();
            TravelAgentLogonResponse response = new TravelAgentLogonResponse();
            Agent agent = new Agent();

            try
            {
                // call api


                Avantik.Web.Service.Entity.REST.Token.TravelAgentLogonRequest rq = new Avantik.Web.Service.Entity.REST.Token.TravelAgentLogonRequest
                {
                    AgentLogon = Request.AgentLogon,
                    AgencyCode = Request.AgencyCode,
                    AgentPassword = Request.AgentPassword
                };

                //call REST API
                string baseURL = ConfigHelper.ToString("RESTURL");
                //string baseURL = "https://localhost:7021/";//ConfigHelper.ToString("RESTURL");
                string apiURL = baseURL + "api/System/TravelAgentLogon";

                var jsonContent = JsonConvert.SerializeObject(rq);
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
                            Avantik.Web.Service.Entity.REST.Token.TravelAgentLogonResponse rs = JsonConvert.DeserializeObject<Avantik.Web.Service.Entity.REST.Token.TravelAgentLogonResponse>(responseContent);
                            agent = rs.AgentResponse;
                        }

                    }
                }


                if (agent != null && agent.Users != null)
                {
                    response.AgentResponse = agent.ToAgentLogonMessage();
                    response.Message = "Success";
                    response.Success = true;
                }
                else
                {
                    response.Message = "Fail";
                    response.Success = false;
                }
            }
            catch (System.Exception ex)
            {
                response.Message = "Agent logon error.";
                response.Success = false;
            }

            return response;
        }
       

    }
}
