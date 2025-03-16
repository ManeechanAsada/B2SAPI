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
using Avantik.Web.Service.Message.ManageBooking;
using Avantik.Web.Service.Exception.Booking;
using Avantik.Web.Service.Message.SeatMap;
using System.Web;
using System.Web.Routing;
using System.ServiceModel;
using b2s = Avantik.Web.Service.Message.b2s;
using Avantik.Web.Service.Message.b2s.map;
using System.Linq;
using Avantik.Web.Service.Message.b2s;
using System.Collections;
using Newtonsoft.Json;
using System.Net;
using System.IO;

namespace Avantik.Web.Service
{
    [ServiceBehavior(AddressFilterMode = AddressFilterMode.Any)]
    public class PassengerSpecialService : IPassengerSpecialService
    {
        public InitializeResponse ServiceInitialize(InitializeRequest request)
        {
            InitializeResponse response = new InitializeResponse();

            // valid
            request.Valid(response);

            if (response.Success)
            {
                AuthenticationService objService = new AuthenticationService();
                response = objService.ServiceInitialize(request);
            }

            return response;
        }

        public B2SSaveResponse SaveB2SBooking(b2s.B2SSaveRequest request)
        {
            BookingService objBookingService = new BookingService();
            B2SSaveResponse response = new B2SSaveResponse();

            Avantik.Web.Service.Message.Booking.BookingSaveRequest wsSaveRequest = new Avantik.Web.Service.Message.Booking.BookingSaveRequest();

            Guid booking_id = new Guid();
            Guid userId = new Guid();
            string agencyCode = string.Empty;
            string currencyCode = string.Empty;
            string languageCode = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(request.Token))
                {
                    // valid token
                    Avantik.Web.Service.Entity.Authentication objAuthen = Avantik.Web.Service.Infrastructrue.Authentication.Authenticate(request.Token);
                    
                    if (objAuthen.ResponseSuccess)
                    {
                        //set user id  and agency code from obj authen
                        userId = objAuthen.UserId;
                        agencyCode = objAuthen.AgencyCode;
                        request.Token = request.Token;

                        // validate request
                        if (request != null)
                        {
                            //Validate input parameter.
                            if (request.BookingHeader == null)
                            {
                                response.Success = false;
                                response.Message = "BookingHeader is required";
                            }
                            else if (request.BookingSegments == null || request.BookingSegments.Count == 0)
                            {
                                response.Success = false;
                                response.Message = "BookingSegment is required";
                            }
                            else if (request.Passengers == null || request.Passengers.Count == 0)
                            {
                                response.Success = false;
                                response.Message = "Passenger is required";
                            }
                            else if (request.Mappings == null || request.Mappings.Count == 0)
                            {
                                response.Success = false;
                                response.Message = "Mappings is required";
                            }
                            else
                            {
                                //Validate information.
                                response.BookingHeader = HeaderValidation(request.BookingHeader, request.Passengers);
                                response.BookingSegments = SegmentValidation(request.BookingSegments, request.Passengers);
                                response.Passengers = PassengerValidation(request.Passengers);
                                response.Mappings = MappingValidation(request.Mappings);
                                response.Fees = FeeValidation(request.Fees);
                                response.Remarks = RemarkValidation(request.Remarks);
                                response.Services = ServiceValidation(request.Services);
                                response.Payments = PaymentValidation(request.Payments);

                                if (response.BookingSegments == null && response.Passengers == null && response.Mappings == null &&
                                    response.Fees == null && response.Remarks == null && response.Services == null && response.Payments == null 
                                    && response.BookingHeader == null)
                                {
                                    // create booking id
                                    //  booking_id = new Guid("AF194891-07C9-403B-801D-E48063732449");// Guid.NewGuid();
                                    booking_id = Guid.NewGuid();

                                    // find passenger tppe
                                    short adult = 0;
                                    short child = 0;
                                    short infant = 0;
                                    short passengerCount = 0;

                                    foreach (Avantik.Web.Service.Message.b2s.Passenger p in request.Passengers)
                                    {
                                        if (p.passenger_type_rcd == "ADULT")
                                            adult += 1;
                                        if (p.passenger_type_rcd == "CHD")
                                            child += 1;
                                        if (p.passenger_type_rcd == "INF")
                                            infant += 1;

                                        passengerCount += 1;
                                    }

                                    // Map API saveRequest to Ws request
                                    wsSaveRequest.Booking = new Web.Service.Message.Booking.BookingRequest();

                                    wsSaveRequest.Booking.Header = request.BookingHeader.FillObjectB2sRequest(booking_id, agencyCode, userId, adult, child, infant);
                                    wsSaveRequest.Booking.FlightSegments = request.BookingSegments.FillObjectB2sRequest(booking_id, userId, passengerCount);
                                    wsSaveRequest.Booking.Passengers = request.Passengers.FillObjectB2sRequest(booking_id, userId);
                                    wsSaveRequest.Booking.Fees = request.Fees.FillObjectB2sRequest(booking_id, userId, agencyCode);
                                    wsSaveRequest.Booking.Remarks = request.Remarks.FillObjectB2sRequest(booking_id, userId);
                                    wsSaveRequest.Booking.Services = request.Services.FillObjectB2sRequest(userId);
                                    wsSaveRequest.Booking.Payments = request.Payments.FillObjectB2sRequest(booking_id, agencyCode, userId);


                                    //get SSR
                                    if (wsSaveRequest.Booking.Services != null && wsSaveRequest.Booking.Services.Count > 0)
                                    {
                                        string language = request.BookingHeader.language_rcd;
                                        IList<Entity.SpecialService> objSpecialService = GetSpecialServiceList(language);

                                        if (objSpecialService != null && objSpecialService.Count > 0)
                                        {
                                            for (int j = 0; j < wsSaveRequest.Booking.Services.Count; j++)
                                            {
                                                for (int i = 0; i < objSpecialService.Count; i++)
                                                {
                                                    if (wsSaveRequest.Booking.Services[j].SpecialServiceRcd == objSpecialService[i].SpecialServiceRcd)
                                                    {
                                                        wsSaveRequest.Booking.Services[j].ServiceOnRequestFlag = objSpecialService[i].ServiceOnRequestFlag;
                                                        wsSaveRequest.Booking.Services[j].SpecialServiceStatusRcd = objSpecialService[i].ServiceOnRequestFlag == 1 ? "RQ" : "HK";
                                                        break;
                                                    }

                                                }
                                            }
                                        }
                                    }

                                    wsSaveRequest.Booking.Mappings = request.Mappings.FillObjectB2sRequest(booking_id, userId, agencyCode);
                                    wsSaveRequest.Booking.Mappings = request.Passengers.FillObjectB2sRequest(wsSaveRequest.Booking.Mappings);
                                    wsSaveRequest.Booking.Mappings = request.BookingSegments.FillObjectB2sRequest(wsSaveRequest.Booking.Mappings);

                                    wsSaveRequest.Token = request.Token;

                                    //save booking
                                    Avantik.Web.Service.Message.Booking.BookingSaveResponse wsSaveResponse = null;
                                    wsSaveResponse = objBookingService.SaveBooking(wsSaveRequest);

                                    if (wsSaveResponse != null)
                                    {
                                        if (wsSaveResponse.Success == true)
                                        {
                                            BookingReadRequest readRequest = new BookingReadRequest();
                                            readRequest.BookingId = booking_id.ToString();
                                            readRequest.Token = request.Token;
                                            //read booking
                                            Avantik.Web.Service.Message.Booking.BookingReadResponse wsReadResponse = ReadBooking(readRequest);

                                            if (wsReadResponse.Success == true)
                                            {
                                                //Set Booking Header inforamtion.
                                                response.BookingHeader = wsReadResponse.BookingResponse.Header.MapBookingHeaderResponse();

                                                // response.BookingHeader.booking_id = Guid.NewGuid();
                                                response.BookingHeader.error_code = "000";
                                                response.BookingHeader.error_message = "SUCCESS";

                                                //Fill Flight segment information.
                                                response.BookingSegments = wsReadResponse.BookingResponse.FlightSegments.MapBookingSegmentsResponse();
                                                for (int i = 0; i < response.BookingSegments.Count; i++)
                                                {
                                                    response.BookingSegments[i].error_code = "000";
                                                    response.BookingSegments[i].error_message = "SUCCESS";
                                                }

                                                //Fill passenger information.
                                                response.Passengers = wsReadResponse.BookingResponse.Passengers.MapPassengersResponse();
                                                for (int i = 0; i < response.Passengers.Count; i++)
                                                {
                                                    response.Passengers[i].error_code = "000";
                                                    response.Passengers[i].error_message = "SUCCESS";
                                                }

                                                //Fill Mapping information.
                                                response.Mappings = wsReadResponse.BookingResponse.Mappings.MapMappingsResponse();
                                                for (int i = 0; i < response.Mappings.Count; i++)
                                                {
                                                    response.Mappings[i].error_code = "000";
                                                    response.Mappings[i].error_message = "SUCCESS";
                                                }

                                                //Fill fee information.
                                                if (wsReadResponse.BookingResponse.Fees != null && wsReadResponse.BookingResponse.Fees.Count > 0)
                                                {
                                                    response.Fees = wsReadResponse.BookingResponse.Fees.MapFeesResponse();
                                                    for (int i = 0; i < response.Fees.Count; i++)
                                                    {
                                                        response.Fees[i].error_code = "000";
                                                        response.Fees[i].error_message = "SUCCESS";
                                                    }
                                                }

                                                //Fill remark information.
                                                if (request.Remarks != null && request.Remarks.Count > 0)
                                                {
                                                    response.Remarks = wsReadResponse.BookingResponse.Remarks.MapRemarksResponse();
                                                    for (int i = 0; i < response.Remarks.Count; i++)
                                                    {
                                                        response.Remarks[i].error_code = "000";
                                                        response.Remarks[i].error_message = "SUCCESS";
                                                    }
                                                }

                                                //Fill service information.
                                                if (request.Services != null && request.Services.Count > 0)
                                                {
                                                    response.Services = wsReadResponse.BookingResponse.Services.MapServicesResponse();
                                                    for (int i = 0; i < response.Services.Count; i++)
                                                    {
                                                        response.Services[i].error_code = "000";
                                                        response.Services[i].error_message = "SUCCESS";
                                                    }
                                                }

                                                //Fill payment information
                                                if (request.Payments != null && request.Payments.Count > 0)
                                                {
                                                    response.Payments = wsReadResponse.BookingResponse.Payments.MapPaymentsResponse();
                                                    for (int i = 0; i < response.Payments.Count; i++)
                                                    {
                                                        response.Payments[i].error_code = "000";
                                                        response.Payments[i].error_message = "SUCCESS";
                                                    }
                                                }

                                                response.Code = "000";
                                                response.Success = true;
                                                response.Message = "SUCCESS";

                                            }
                                            else
                                            {
                                                response.Code = "129";
                                                response.Success = false;
                                                response.Message = "Read booking failed";
                                            }
                                        }
                                        else
                                        {
                                            if (!String.IsNullOrEmpty(wsSaveResponse.Code))
                                            {
                                                response.Code = wsSaveResponse.Code;
                                            }
                                            else
                                            {
                                                response.Code = "H001";
                                            }

                                            response.Success = false;
                                            response.Message = "Save Booking Failed. [" + wsSaveResponse.Message + "]";
                                        }

                                    }
                                    else
                                    {
                                        response.Code = "H001";
                                        response.Success = false;
                                        response.Message = "Save booking is fail";
                                    }

                                }
                                else
                                {
                                    response.Success = false;
                                    response.Message = "Invalid Request parameter.";
                                }
                            }
                        }
                        else
                        {
                            response.Success = false;
                            response.Message = "Request parameter is required.";
                        }

                    }
                    else
                    {
                        response.Code = "A006";
                        response.Message = "Athenticate web service failed.";
                        response.Success = false;
                    }
                }
                else
                {
                    response.Code = "A004";
                    response.Message = "Require Token.";
                    response.Success = false;
                }
            }
            catch (ModifyBookingException mex)
            {
                response.Code = mex.ErrorCode;
                response.Message = mex.Message;
                response.Success = false;
                Logger.SaveLog("SaveBooking", DateTime.Now, DateTime.Now, mex.Message, XMLHelper.Serialize(request, false));
            }
            catch (System.Exception ex)
            {
                response.Code = "E001";
                response.Message = "System error.";
                response.Success = false;
                Logger.SaveLog("SaveBooking", DateTime.Now, DateTime.Now, ex.Message, XMLHelper.Serialize(request, false));
            }

            return response;
        }

        public BookingReadResponse ReadBooking(BookingReadRequest request)
        {
            BookingService objBookingService    = new BookingService();
            BookingReadResponse response        = new BookingReadResponse();
            string userId                       = string.Empty;
            string agencyCode                   = string.Empty;
            string currencyCode                 = string.Empty;
            string languageCode                 = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(request.Token))
                {
                    // valid token
                    Avantik.Web.Service.Entity.Authentication objAuthen = Avantik.Web.Service.Infrastructrue.Authentication.Authenticate(request.Token);
                    if (objAuthen.ResponseSuccess)
                    {
                        //set user id  and agency code from obj authen
                        userId = objAuthen.UserId.ToString();
                        agencyCode = objAuthen.AgencyCode;
                        request.Token = request.Token;

                        // validate request
                        if (string.IsNullOrEmpty(request.BookingReference) && request.BookingId == "00000000-0000-0000-0000-000000000000")
                        {
                            response.Code = "B007";
                            response.Success = false;
                            response.Message = "Booking reference is required.";
                        }
                        else if (string.IsNullOrEmpty(request.BookingReference) && string.IsNullOrEmpty(request.BookingId))
                        {
                            response.Code = "B007";
                            response.Success = false;
                            response.Message = "Booking reference is required.";
                        }
                        else
                        {
                            //read booking
                            response = objBookingService.ReadBooking(request);

                            if (response != null && response.Success == true)
                            {
                                response.Code = "000";
                                response.Message = "Success";
                                response.Success = true;
                            }
                            else
                            {
                                response.Code = "B008";
                                response.Message = "Read Booking failed.";
                                response.Success = false;
                            }
                        }
                    }
                    else
                    {
                        response.Code = "A006";
                        response.Message = "Athenticate web service failed.";
                        response.Success = false;
                    }
                }
                else
                {
                    response.Code = "A004";
                    response.Message = "Require Token.";
                    response.Success = false;
                }
            }
            catch (ModifyBookingException mex)
            {
                response.Code = mex.ErrorCode;
                response.Message = mex.Message;
                response.Success = false;
                Logger.SaveLog("ReadBooking", DateTime.Now, DateTime.Now, mex.Message,XMLHelper.Serialize(request,false) );
            }
            catch (System.Exception ex)
            {
                response.Code = "E001";
                response.Message = "System error.";
                response.Success = false;
                //Logger.Instance(Logger.LogType.Mail).WriteLog(ex, XMLHelper.JsonSerializer(typeof(GetSpecialServicesRequest), request));
                Logger.SaveLog("ReadBooking", DateTime.Now, DateTime.Now, ex.Message, XMLHelper.Serialize(request, false));
            }

            return response;
        }


        public GetSeatMapResponse GetSeatMap(GetSeatMapRequest request)
        {
            BookingService objBookingService = new BookingService();
            GetSeatMapResponse response = new GetSeatMapResponse();
            string userId = string.Empty;
            string agencyCode = string.Empty;
            string currencyCode = string.Empty;
            string languageCode = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(request.Token))
                {
                    // valid token
                    Avantik.Web.Service.Entity.Authentication objAuthen = Avantik.Web.Service.Infrastructrue.Authentication.Authenticate(request.Token);
                    
                    if (objAuthen.ResponseSuccess)
                    {
                        //set user id  and agency code from obj authen
                        userId = objAuthen.UserId.ToString();
                        agencyCode = objAuthen.AgencyCode;
                        request.Token = request.Token;

                        // validate request
                        request.Valid(response, objAuthen);
                        // no need read booking
                        // call to booking service
                        if (response.Success)
                        {
                            //get seat map
                            request = request.SeatMapRequest();
                          //  response = objBookingService.GetSeatMap(request);

                            if (response.Success  && response.SeatMaps.Count > 0)
                            {
                                response.Code = "000";
                                response.Message = "Success";
                                response.Success = true;
                            }
                        }
                    }
                    else
                    {
                        response.Code = "A006";
                        response.Message = "Athenticate web service failed.";
                        response.Success = false;
                    }
                }
                else
                {
                    response.Code = "A004";
                    response.Message = "Require Token.";
                    response.Success = false;
                }
            }
            catch (ModifyBookingException mex)
            {
                response.Code = mex.ErrorCode;
                response.Message = mex.Message;
                response.Success = false;
                Logger.SaveLog("GetSeatMap", DateTime.Now, DateTime.Now, mex.Message, XMLHelper.Serialize(request, false));
            }
            catch (System.Exception ex)
            {
                response.Code = "E001";
                response.Message = "System error.";
                response.Success = false;
               // Logger.Instance(Logger.LogType.Mail).WriteLog(ex, XMLHelper.JsonSerializer(typeof(GetSpecialServicesRequest), request));
                Logger.SaveLog("GetSeatMap", DateTime.Now, DateTime.Now, ex.Message, XMLHelper.Serialize(request, false));
            }

            return response;
        }

        public GetSpecialServicesResponse GetSpecialServices(b2s.GetServicesRequest request)
        {
            GetSpecialServicesResponse response = new GetSpecialServicesResponse();
            IList<Entity.SegmentService> segmentServicesList = null;
            IList<Entity.Booking.PassengerService> passengerServices = null;

            string userId = string.Empty;
            string agencyCode = string.Empty;
            string currencyRcd = string.Empty;
            string languageRcd = string.Empty;
            int numberOfPassenger = 0;
            int numberOfInfant = 0;
            bool bNoVat = false;
            try
            {
                if (!string.IsNullOrEmpty(request.Token))
                {
                    // valid token
                    Avantik.Web.Service.Entity.Authentication objAuthen = Avantik.Web.Service.Infrastructrue.Authentication.Authenticate(request.Token);
                    
                    if (objAuthen.ResponseSuccess)
                    {
                         // validate request
                        if (request != null)
                        {
                            //Validate input parameter.
                            if (request.BookingSegments == null || request.BookingSegments.Count == 0)
                            {
                                response.Success = false;
                                response.Message = "BookingSegment is required.";
                            }
                            else if(string.IsNullOrEmpty(request.Currency))
                            {
                                response.Success = false;
                                response.Message = "Currency is required.";
                            }
                            else
                            {
                                //Validate information.
                                response = SegmentSSRValidation(request.BookingSegments);

                                if (response.Success)
                                {
                                    userId = objAuthen.UserId.ToString();
                                    agencyCode = objAuthen.AgencyCode;
                                    request.Token = request.Token;
                                    currencyRcd = request.Currency;


                                    string baseURL = ConfigHelper.ToString("RESTURL");
                                    string apiURL = baseURL + "api/Setting/GetSpecialServices";

                                    try
                                    {
                                        IList<Avantik.Web.Service.Entity.REST.GetSSR.BookingSegment> ssrlist = new List<Avantik.Web.Service.Entity.REST.GetSSR.BookingSegment>();


                                        foreach(var s in request.BookingSegments)
                                        {
                                            var BookingSegmentsRequest = new Avantik.Web.Service.Entity.REST.GetSSR.BookingSegment();
                                            BookingSegmentsRequest.airline_rcd = s.airline_rcd;
                                            BookingSegmentsRequest.booking_class_rcd = s.booking_class_rcd;
                                            BookingSegmentsRequest.departure_date = s.departure_date;
                                            BookingSegmentsRequest.destination_rcd = s.destination_rcd;
                                            BookingSegmentsRequest.origin_rcd = s.origin_rcd;
                                            BookingSegmentsRequest.flight_number = s.flight_number;

                                            ssrlist.Add(BookingSegmentsRequest);

                                        }



                                            var GetSSRRequest = new Avantik.Web.Service.Entity.REST.GetSSR.GetSSRRequest
                                            {
                                                AgencyCode = agencyCode,
                                                Currency = currencyRcd,
                                                BookingSegments = ssrlist

                                            };

                                            var jsonContent = JsonConvert.SerializeObject(GetSSRRequest);
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
                                                        Avantik.Web.Service.Entity.REST.GetSSR.GetSpecialServicesResponse SSRResponse = JsonConvert.DeserializeObject<Avantik.Web.Service.Entity.REST.GetSSR.GetSpecialServicesResponse>(responseContent);

                                                    if (SSRResponse.ServiceFees != null && SSRResponse.ServiceFees.Count > 0)
                                                    {
                                                        // map to response
                                                        response.ServiceFees = SSRResponse.ServiceFees.ToFeeMessage();
                                                        response.Code = "000";
                                                        response.Message = "Success";
                                                        response.Success = true;
                                                    }
                                                    else
                                                    {
                                                        response.Code = "V011";
                                                        response.Message = "Get SSR not found.";
                                                        response.Success = false;
                                                    }


                                                }

                                            }
                                            }
                                        
                                    }
                                    catch (System.Exception ex)
                                    {
                                        response.Message = "Booking Read is error";
                                        response.Success = false;
                                        //Logger.Instance(Logger.LogType.Mail).WriteLog(ex, XMLHelper.JsonSerializer(typeof(BookingReadRequest), Request));
                                    }



                                    /*
                                    IList<Message.Booking.FlightSegment> fs = request.BookingSegments.FillObjectB2sRequest();

                                    //prepare Passenger Service get from system map with ssr code
                                    segmentServicesList = GetSegmentServicesnew(fs);

                                    passengerServices = GetPassengerServices();

                                    //   process get ssr from segmentfee
                                    if (passengerServices != null && passengerServices.Count > 0)
                                    {
                                        // get fee ssr
                                        IList<Entity.ServiceFee> ServiceFees = GetSSRFromSegmentFee(agencyCode, currencyRcd, languageRcd, numberOfPassenger, numberOfInfant, bNoVat, segmentServicesList, passengerServices);

                                        if (ServiceFees != null && ServiceFees.Count > 0)
                                        {
                                            // map to response
                                            response.ServiceFees = ServiceFees.ToFeeMessage();
                                            response.Code = "000";
                                            response.Message = "Success";
                                            response.Success = true;
                                        }
                                        else
                                        {
                                            response.Code = "V011";
                                            response.Message = "Get SSR not found.";
                                            response.Success = false;
                                        }

                                    }
                                    else
                                    {
                                        response.Code = "V011";
                                        response.Success = false;
                                        response.Message = "Get SSR failed.";

                                    }


                                    */

                                }

                            }
                        }
                    }
                    else
                    {
                        response.Code = "A006";
                        response.Message = "Athenticate web service failed.";
                        response.Success = false;
                    }
                }
                else
                {
                    response.Code = "A004";
                    response.Message = "Require Token.";
                    response.Success = false;
                }
            }
            catch (ModifyBookingException mex)
            {
                response.Code = mex.ErrorCode;
                response.Message = mex.Message;
                response.Success = false;
               // Logger.SaveLog("GetSpecialServices", DateTime.Now, DateTime.Now, mex.Message, request.BookingId + XMLHelper.Serialize(request.ServiceCode, false));
            }
            catch (System.Exception ex)
            {
                response.Code = "E001";
                response.Message = "System error.";
                response.Success = false;
                // Logger.Instance(Logger.LogType.Mail).WriteLog(ex, XMLHelper.JsonSerializer(typeof(GetSpecialServicesRequest), request));
               // Logger.SaveLog("GetSpecialServices", DateTime.Now, DateTime.Now, ex.Message, request.BookingId + XMLHelper.Serialize(request.ServiceCode, false));
            }
            return response;
        }
       

        public b2s.BookingItemsAddResponse BookingItemsAdd(b2s.BookingItemsAddRequest BookingItemsAddRequest)
        {
            b2s.BookingItemsAddResponse response = new b2s.BookingItemsAddResponse();
            BookingService objBookingService = new BookingService();
            string bookingReference = string.Empty;
            string userId = string.Empty;
            string agencyCode = string.Empty;
            //double bookingNumber = 0;
            //bool bWaveFee = false;
            //bool bVoid = false;
            //bool result = false;

            try
            {
                if (!string.IsNullOrEmpty(BookingItemsAddRequest.Token))
                {
                    // valid token
                    Avantik.Web.Service.Entity.Authentication objAuthen = Avantik.Web.Service.Infrastructrue.Authentication.Authenticate(BookingItemsAddRequest.Token);

                    if (objAuthen.ResponseSuccess)
                    {
                        if (BookingItemsAddRequest.bookingId.Equals(Guid.Empty) == false)
                        {
                            userId = objAuthen.UserId.ToString();
                            agencyCode = objAuthen.AgencyCode;

                            //Validate information.
                            if (BookingItemsAddRequest.Fees != null && BookingItemsAddRequest.Fees.Count > 0)
                                response.Fees = FeeValidation(BookingItemsAddRequest.Fees);

                            if (BookingItemsAddRequest.Remarks != null && BookingItemsAddRequest.Remarks.Count > 0)
                                response.Remarks = RemarkValidation(BookingItemsAddRequest.Remarks);

                            if (response.Fees == null && response.Remarks == null)
                            {

                                //read booking
                                Avantik.Web.Service.Message.Booking.BookingReadRequest wsIsFoundRequest = new Web.Service.Message.Booking.BookingReadRequest();
                                wsIsFoundRequest.BookingId = BookingItemsAddRequest.bookingId.ToString();
                                wsIsFoundRequest.Token = BookingItemsAddRequest.Token;
                                Avantik.Web.Service.Message.Booking.BookingReadResponse wsIsFoundResponse = ReadBooking(wsIsFoundRequest);

                                // Is found booking
                                if (wsIsFoundResponse.Success == true)
                                {
                                    Avantik.Web.Service.Message.Booking.BookingSaveRequest wsSaveRequest = new Web.Service.Message.Booking.BookingSaveRequest();
                                    Avantik.Web.Service.Message.Booking.BookingSaveResponse wsSaveResponse = null;
                                    Avantik.Web.Service.Message.Booking.BookingPaymentResponse responsePayment = null;

                                    // Map API saveRequest to Ws request
                                    wsSaveRequest.Booking = new Web.Service.Message.Booking.BookingRequest();

                                    Guid bookingId = BookingItemsAddRequest.bookingId;
                                    Guid user_Id = new Guid(userId);
                                    if (wsIsFoundResponse.BookingResponse.Header != null)
                                        agencyCode = wsIsFoundResponse.BookingResponse.Header.AgencyCode;

                                    wsSaveRequest.Booking.Header = wsIsFoundResponse.BookingResponse.Header;

                                    wsSaveRequest.Booking.Fees = BookingItemsAddRequest.Fees.FillObjectB2sRequest(bookingId, user_Id, agencyCode);
                                    wsSaveRequest.Booking.Remarks = BookingItemsAddRequest.Remarks.FillObjectB2sRequest(bookingId, user_Id);

                                    // call save booking for fee and remark
                                    if (wsSaveRequest.Booking.Fees != null || wsSaveRequest.Booking.Remarks != null)
                                    {
                                        wsSaveRequest.Token = BookingItemsAddRequest.Token;
                                        //save booking
                                        wsSaveResponse = objBookingService.SaveBooking(wsSaveRequest);
                                    }

                                    // payment
                                    if (BookingItemsAddRequest.Payments != null && BookingItemsAddRequest.Payments.Count > 0)
                                    {
                                        Avantik.Web.Service.Message.Booking.BookingPaymentRequest paymentRequest = new Avantik.Web.Service.Message.Booking.BookingPaymentRequest();
                                        paymentRequest.Mappings = wsIsFoundResponse.BookingResponse.Mappings;

                                        // for b2s
                                        foreach(var m in  paymentRequest.Mappings)
                                        {
                                            m.ExcludePricingFlag = 0;
                                        }

                                        paymentRequest.Fees = wsSaveRequest.Booking.Fees;

                                        if (wsIsFoundResponse.BookingResponse.Fees != null && paymentRequest.Fees != null)
                                        {
                                            paymentRequest.Fees = paymentRequest.Fees.Concat(wsIsFoundResponse.BookingResponse.Fees).ToList();
                                        }
                                        else if (wsIsFoundResponse.BookingResponse.Fees != null)
                                        {
                                            paymentRequest.Fees = wsIsFoundResponse.BookingResponse.Fees;
                                        }

                                        //go payment
                                        paymentRequest.Payments = BookingItemsAddRequest.Payments.FillObjectB2sRequest(bookingId, agencyCode, user_Id);

                                        //payment
                                        paymentRequest.CreateTicket = false;
                                        paymentRequest.Token = BookingItemsAddRequest.Token;
                                      //  responsePayment = objBookingService.BookingPayment(paymentRequest); 
                                        responsePayment = objBookingService.BookingExternalPayment(paymentRequest);
                                    }

                                        if ((wsSaveResponse != null && wsSaveResponse.Success == true) || (responsePayment != null && responsePayment.Success == true))
                                        {
                                            // read booking
                                            BookingReadRequest readRequest = new BookingReadRequest();
                                            readRequest.BookingId = BookingItemsAddRequest.bookingId.ToString();
                                            readRequest.Token = BookingItemsAddRequest.Token;

                                            Avantik.Web.Service.Message.Booking.BookingReadResponse wsReadResponse = ReadBooking(readRequest);


                                            if (wsReadResponse.Success == true)
                                            {
                                                response.booking_id = BookingItemsAddRequest.bookingId;
                                                response.record_locator = wsReadResponse.BookingResponse.Header.RecordLocator;

                                                //Fill fee information.
                                                if (BookingItemsAddRequest.Fees != null && BookingItemsAddRequest.Fees.Count > 0)
                                                {
                                                    response.Fees = wsReadResponse.BookingResponse.Fees.MapFeesResponse();
                                                    for (int i = 0; i < response.Fees.Count; i++)
                                                    {
                                                        response.Fees[i].error_code = "000";
                                                        response.Fees[i].error_message = "SUCCESS";
                                                    }
                                                }
                                                //Fill remark information.
                                                if (BookingItemsAddRequest.Remarks != null && BookingItemsAddRequest.Remarks.Count > 0)
                                                {
                                                    response.Remarks = wsReadResponse.BookingResponse.Remarks.MapRemarksResponse();
                                                    for (int i = 0; i < response.Remarks.Count; i++)
                                                    {
                                                        response.Remarks[i].error_code = "000";
                                                        response.Remarks[i].error_message = "SUCCESS";
                                                    }
                                                }

                                                //Fill Payment information.
                                                if (BookingItemsAddRequest.Payments != null && BookingItemsAddRequest.Payments.Count > 0 && responsePayment.Success == true)
                                                {
                                                    response.Payments = wsReadResponse.BookingResponse.Payments.MapPaymentsResponse();
                                                    for (int i = 0; i < response.Payments.Count; i++)
                                                    {
                                                        response.Payments[i].error_code = "000";
                                                        response.Payments[i].error_message = "SUCCESS";
                                                    }
                                                }

                                                // check return suucess
                                                bool isSuccess = true;
                                                if (BookingItemsAddRequest.Fees != null && BookingItemsAddRequest.Fees.Count > 0)
                                                {
                                                    if (response.Fees.Count > 0)
                                                    {
                                                        for (int i = 0; i < response.Fees.Count; i++)
                                                        {
                                                            if (response.Fees[i].error_code != "000")
                                                            {
                                                                isSuccess = false;
                                                                break;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        isSuccess = false;
                                                    }
                                                }

                                                if (BookingItemsAddRequest.Remarks != null && BookingItemsAddRequest.Remarks.Count > 0 && isSuccess == true)
                                                {
                                                    if (response.Remarks.Count > 0)
                                                    {
                                                        for (int i = 0; i < response.Remarks.Count; i++)
                                                        {
                                                            if (response.Remarks[i].error_code != "000")
                                                            {
                                                                isSuccess = false;
                                                                break;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        isSuccess = false;
                                                    }
                                                }


                                                if (BookingItemsAddRequest.Payments != null && BookingItemsAddRequest.Payments.Count > 0 && responsePayment.Success == true && isSuccess == true)
                                                {
                                                    if (response.Payments.Count > 0)
                                                    {
                                                        for (int i = 0; i < response.Payments.Count; i++)
                                                        {
                                                            if (response.Payments[i].error_code != "000")
                                                            {
                                                                isSuccess = false;
                                                                break;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        isSuccess = false;
                                                    }
                                                }


                                                if (isSuccess == false)
                                                {
                                                    response.Message = "Some of items are fail";
                                                    response.Success = false;
                                                }
                                                else
                                                {
                                                    response.Message = "SUCCESS";
                                                    response.Success = true;
                                                }

                                            }

                                        }
                                        else
                                        {
                                            response.Success = false;
                                            response.Message = "Add Item is fail";
                                        }

                                }
                                else
                                {
                                    response.Success = false;
                                    response.Message = "Booking not found";
                                }
                            }

                            else
                            {
                                response.Success = false;
                                response.Message = "Request parameter is required.";
                            }

                        }
                        else
                        {
                            response.Code = "B009";
                            response.Message = "Booking ID is required.";
                            response.Success = false;
                        }
                    }
                    else
                    {
                        response.Success = objAuthen.ResponseSuccess;
                        response.Message = objAuthen.ResponseMessage;
                        response.Code = objAuthen.ResponseCode;
                    }
                }
                else
                {
                    response.Code = "A004";
                    response.Message = "Require Token.";
                    response.Success = false;
                }

            }
            catch (ModifyBookingException mex)
            {
                response.Code = mex.ErrorCode;
                response.Message = mex.Message;
                response.Success = false;
            }
            catch (System.Exception ex)
            {
                response.Code = "E001";
                response.Message = "System error.";
                response.Success = false;
            }


            return response;
        }

        public GetFeeResponse GetFeeDefinition(GetFeeDefinitionRequest request)
        {
            GetFeeResponse response = new GetFeeResponse();
            IList<Message.Fee.Fee> fees = new List<Message.Fee.Fee>();
            string userId = string.Empty;
            string agencyCode = string.Empty;
            string currencyRcd = string.Empty;
            string languageRcd = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(request.Token))
                {
                    // valid token
                    Avantik.Web.Service.Entity.Authentication objAuthen = Avantik.Web.Service.Infrastructrue.Authentication.Authenticate(request.Token);

                    if (objAuthen.ResponseSuccess)
                    {
                        //set user id  and agency code from obj authen
                        userId = objAuthen.UserId.ToString();
                        agencyCode = objAuthen.AgencyCode;
                        currencyRcd = request.Currency;
                        languageRcd = objAuthen.LanguageRcd;

                        //valid
                        request.Valid(response);

                        if (response.Success)
                        {
                            // map data
                            //GetFeeRequest getFeeRequest = new GetFeeRequest();
                            //getFeeRequest = request.GetFeeDefinitionMapRequest(agencyCode, currencyRcd, languageRcd);
                            //BookingService objBookingService = new BookingService();
                            //response = objBookingService.GetFee(getFeeRequest);
                            string baseURL = ConfigHelper.ToString("RESTURL");
                            string apiURL = baseURL + "api/Setting/GetFeeDefinition";

                            try
                            {
                                var BookingRESTRequest = new Avantik.Web.Service.Entity.REST.GetFeeDefinition.GetFeeDefinitionRequest
                                {
                                    AgencyCode = agencyCode,
                                    FeeRcd = request.FeeRcd,
                                    BookingClass = request.BookingClass,
                                    FareCode = request.FareCode,
                                    Origin = request.Origin,
                                    Destination = request.Destination,
                                    LanguageRcd = languageRcd,
                                    Currency = currencyRcd

                                };

                                var jsonContent = JsonConvert.SerializeObject(BookingRESTRequest);
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
                                            Avantik.Web.Service.Entity.REST.GetFeeDefinition.GetFeeDefinitionResponse getFeeDefinitionResponse = JsonConvert.DeserializeObject<Avantik.Web.Service.Entity.REST.GetFeeDefinition.GetFeeDefinitionResponse>(responseContent);
                                            Message.Fee.Fee f = new Message.Fee.Fee();

                                            f = MapRESTMessageFeeToOrderingFee(getFeeDefinitionResponse.Fees[0]);


                                            fees.Add(f);

                                            response.Fees = fees;
                                            response.Success = true;

                                        }

                                    }
                                }

                            }
                            catch (System.Exception ex)
                            {
                                response.Message = " error";
                                response.Success = false;
                            }
                        }
                    }
                    else
                    {
                        response.Code = "A006";
                        response.Message = "Athenticate web service failed.";
                        response.Success = false;
                    }
                }
                else
                {
                    response.Code = "A004";
                    response.Message = "Require Token.";
                    response.Success = false;
                }
            }
            catch (ModifyBookingException mex)
            {
                response.Code = mex.ErrorCode;
                response.Message = mex.Message;
                response.Success = false;
            }
            catch (System.Exception ex)
            {
                response.Code = "E001";
                response.Message = "System error.";
                response.Success = false;
            }

            return response;
        }
        public Message.Fee.Fee MapRESTMessageFeeToOrderingFee(Avantik.Web.Service.Entity.REST.GetFeeDefinition.MessageFee messageFee)
        {
            return new Message.Fee.Fee
            {
                AgencyCode = messageFee.AgencyCode,
                Comment = messageFee.Comment,
                CurrencyRcd = messageFee.CurrencyRcd,
                DestinationRcd = messageFee.DestinationRcd,
                DisplayName = messageFee.DisplayName,
                FeeAmount = messageFee.FeeAmount,
                FeeAmountIncl = messageFee.FeeAmountIncl,
                FeeCategoryRcd = messageFee.FeeCategoryRcd,
                FeeId = messageFee.FeeId,
                FeeRcd = messageFee.FeeRcd,
                ManualFeeFlag = messageFee.ManualFeeFlag,
                MinimumFeeAmountFlag = messageFee.MinimumFeeAmountFlag,
                OdDestinationRcd = messageFee.OdDestinationRcd,
                OdFlag = messageFee.OdFlag,
                OdOriginRcd = messageFee.OdOriginRcd,
                OriginRcd = messageFee.OriginRcd,
                VatPercentage = messageFee.VatPercentage
            };
        }



        #region Private Method 

        private void ClearResponse(BookingResponse BookingResponse)
        {
            if (BookingResponse != null)
            {
                BookingResponse.Header = null;
                BookingResponse.Services = null;
                BookingResponse.FlightSegments = null;
                BookingResponse.Mappings = null;
                BookingResponse.Passengers = null;
                BookingResponse.Fees = null;
            }
        }

       
        private SelectedAgency SelectAgencyToDoAPI(Authentication objAuthen, Message.Booking.BookingHeader header)
        {
            SelectedAgency obj = new SelectedAgency();
            string selectedAgency = ConfigHelper.ToString("selectedAgency");

            switch (selectedAgency)
            {
                    // currency always come from booking header
                case "BookingAgency":
                    obj.UserId = objAuthen.UserId.ToString();
                    obj.AgencyCode = header.AgencyCode;
                    obj.CurrencyRcd = header.CurrencyRcd;
                    obj.LanguageRcd = header.LanguageRcd;
                    break;
                case "LoginAgency":
                    obj.UserId = objAuthen.UserId.ToString();
                    obj.AgencyCode = objAuthen.AgencyCode;
                    obj.CurrencyRcd = header.CurrencyRcd;
                    obj.LanguageRcd = objAuthen.LanguageRcd;
                    break;
                default:
                    obj.UserId = objAuthen.UserId.ToString();
                    obj.AgencyCode = objAuthen.AgencyCode;
                    obj.CurrencyRcd = header.CurrencyRcd;
                    obj.LanguageRcd = objAuthen.LanguageRcd;
                    break;
            }

            return obj;
        }
        private IList<Entity.ServiceFee> GetSSRFromSegmentFee(
                                                                string agencyCode,
                                                                string currencyCode,
                                                                string languageCode,
                                                                int numberOfPassenger,
                                                                int numberOfInfant,
                                                                bool bNoVat,
                                                                IList<Entity.SegmentService> segmentServicesList, 
                                                                IList<Entity.Booking.PassengerService> passengerServices)
                                                            {
            IFeeService objFeeService = FeeServiceFactory.CreateInstance();
            IList<Entity.ServiceFee> ServiceFees = null;

            //   process get ssr fee
            if (passengerServices != null && passengerServices.Count > 0)
            {
                // get fee ssr
                ServiceFees = objFeeService.GetSegmentFee(agencyCode,
                                                        currencyCode,
                                                        languageCode,
                                                        numberOfPassenger,
                                                        numberOfInfant,
                                                        passengerServices,
                                                        segmentServicesList,
                                                        true,
                                                        bNoVat);
            }

            return ServiceFees;
        }

        private CalculateFeesBookingResponse CalAssignlSeatFee(BookingReadResponse readResponse, IList<Entity.SeatAssign> Seats, string agencyCode, string CurrencyRcd)
        {
            BookingService objService = new BookingService();
            Booking booking = new Booking();
            booking.Header = readResponse.BookingResponse.Header.ToEntity();
            booking.Mappings = readResponse.BookingResponse.Mappings.ToListEntity();
            booking.Segments = readResponse.BookingResponse.FlightSegments.ToListEntity();
            booking.Passengers = readResponse.BookingResponse.Passengers.ToListEntity();

            //set seat to mapping
            booking.Mappings = SetSeatMapping(Seats, booking.Mappings);

            CalculateFeesSeatAssignmentRequest calSeatFeeRequest = new CalculateFeesSeatAssignmentRequest();
            CalculateFeesBookingResponse calSeatFeeResponse = new CalculateFeesBookingResponse();

            calSeatFeeRequest.AgencyCode = agencyCode;

            if (string.IsNullOrEmpty(CurrencyRcd))
                calSeatFeeRequest.Currency = readResponse.BookingResponse.Header.CurrencyRcd;
            else
                calSeatFeeRequest.Currency = CurrencyRcd;

            calSeatFeeRequest.bNovat = false;
            calSeatFeeRequest.Booking = new BookingRequest();
            calSeatFeeRequest.Booking.Header = booking.Header.ToBookingMessage();
            calSeatFeeRequest.Booking.Mappings = booking.Mappings.ToBookingMessage();
            calSeatFeeRequest.Booking.FlightSegments = booking.Segments.ToBookingMessage();
            calSeatFeeRequest.Booking.Passengers = booking.Passengers.ToBookingMessage();

          //  calSeatFeeResponse = objService.CalculateFeesSeatAssignment(calSeatFeeRequest);

            return calSeatFeeResponse;
        }

        private BookingReadResponse GetBooking(string bookingId, string token)
        {
            BookingService objService = new BookingService();
            BookingReadResponse readResponse = new BookingReadResponse();
            BookingReadRequest readRequest = new BookingReadRequest();

            readRequest.BookingId = bookingId;
            readRequest.Token = token;
            try
            {
                readResponse = objService.ReadBooking(readRequest);
            }
            catch (System.Exception ex)
            {
                Logger.SaveLog("GetBooking", DateTime.Now, DateTime.Now, ex.Message, bookingId);
            }

            return readResponse;

        }

        private List<Avantik.Web.Service.Entity.Booking.Fee> CalBagageFee(BookingReadResponse readResponse, IList<Baggage> Baggage, string agencyCode, string languageCode)
        {
            IFeeService objFeeService = FeeServiceFactory.CreateInstance();
            List<Avantik.Web.Service.Entity.Booking.Fee> BagFees = new List<Avantik.Web.Service.Entity.Booking.Fee>();
            Booking booking = new Booking();
            bool bNoVat = false;

            booking.Header = readResponse.BookingResponse.Header.ToEntity();
            booking.Mappings = readResponse.BookingResponse.Mappings.ToListEntity();

            bNoVat = Convert.ToBoolean(booking.Header.NoVatFlag == 0 ? false : true);

           

            return BagFees;
        }

        private IList<Entity.Booking.Fee> CalNameChangeFee(BookingReadResponse readResponse, IList<Message.ManageBooking.NameChange> nameChange, string agencyCode, string currencyCode, string languageCode, string userId)
        {
            Booking objBookingRequest = new Booking();
            Booking objBookingResponse = new Booking();
            NameChangeResponse response = new NameChangeResponse();
            IFeeService objFeeService = FeeServiceFactory.CreateInstance();
            bool bNoVat = false;

            // update name to mapping
            readResponse.BookingResponse.Mappings = SetMessageMapping(readResponse, nameChange);

            // update name to passenger
            readResponse.BookingResponse.Passengers = SetMessagePassenger(readResponse, nameChange);

            // map to booking obj
            objBookingRequest.Header = readResponse.BookingResponse.Header.ToEntity();
            objBookingRequest.Segments = readResponse.BookingResponse.FlightSegments.ToListEntity();
            objBookingRequest.Passengers = readResponse.BookingResponse.Passengers.ToListEntity();
            objBookingRequest.Mappings = readResponse.BookingResponse.Mappings.ToListEntity();
            objBookingRequest.Fees = readResponse.BookingResponse.Fees.ToListEntity();
            objBookingRequest.Services = readResponse.BookingResponse.Services.ToListEntity();
            objBookingRequest.Remarks = readResponse.BookingResponse.Remarks.ToListEntity();
            // set variable
            bNoVat = Convert.ToBoolean(objBookingRequest.Header.NoVatFlag == 0 ? false : true);

          

            return objBookingResponse.Fees;

        }


        private IList<Entity.SpecialService> GetSpecialServiceList(string language)
        {
            IList<Entity.SpecialService> objSSRList = null;

            if (string.IsNullOrEmpty(language))
            {
                language = "EN";
            }

            try
            {
                objSSRList = (IList<Entity.SpecialService>)HttpRuntime.Cache["SpecialServiceRef-" + language.ToUpper()];

                if (objSSRList == null || objSSRList.Count == 0)
                {
                    ISystemModelService objSystemService = SystemServiceFactory.CreateInstance();
                    objSSRList = objSystemService.GetSpecialService(string.Empty);

                    HttpRuntime.Cache.Insert("SpecialServiceRef-" + language.ToUpper(), objSSRList, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(20), System.Web.Caching.CacheItemPriority.Normal, null);
                }
            }
            catch
            {
            }

            return objSSRList;
        }

        private IList<Entity.Booking.PassengerService> GetPassengerServices()
        {
            ISystemModelService objSystemService = SystemServiceFactory.CreateInstance();

            IList<Entity.Booking.PassengerService> passengerServices = new List<Entity.Booking.PassengerService>();
            
            // read from cache
            IList<Entity.SpecialService> ssr = GetSpecialServiceList(string.Empty);


            Entity.Booking.PassengerService ps;

            for (int j = 0; j < ssr.Count; j++)
            {
                {
                    ps = new Entity.Booking.PassengerService();

                  //  if (ssr[j].SpecialServiceRcd.ToUpper() == "BLND")
                    {
                        ps.SpecialServiceRcd = ssr[j].SpecialServiceRcd.ToUpper();
                        ps.DisplayName = ssr[j].DisplayName;
                        ps.ServiceText = ssr[j].ServiceText;
                        ps.ServiceOnRequestFlag = ssr[j].ServiceOnRequestFlag;
                        ps.CutOffTime = ssr[j].CutOffTime;
                        passengerServices.Add(ps);
                    }
                }
            }
                

            return passengerServices;
        }


        private IList<Entity.SegmentService> GetSegmentServicesnew(IList<Message.Booking.FlightSegment> fs)
        {
            IList<Entity.SegmentService> segmentServicesList = null;
            segmentServicesList = new List<Entity.SegmentService>();

            for (int i = 0; i < fs.Count; i++)
            {
                Entity.SegmentService segmentService = new Entity.SegmentService();
               // segmentService.BookingSegmentId = fs[i].BookingSegmentId;
                segmentService.OriginRcd = fs[i].OriginRcd;
                segmentService.DestinationRcd = fs[i].DestinationRcd;
                segmentService.OdOriginRcd = fs[i].OdOriginRcd;
                segmentService.OdDestinationRcd = fs[i].OdDestinationRcd;
                segmentService.BookingClassRcd = fs[i].BookingClassRcd;
                segmentService.AirlineRcd = fs[i].AirlineRcd;
                segmentService.FlightNumber = fs[i].FlightNumber;
                segmentService.DepartureDate = fs[i].DepartureDate;
                //segmentService.FareCode = m[j].FareCode;

                segmentServicesList.Add(segmentService);
            }

            return segmentServicesList;
        }

        private IList<Entity.SegmentService> GetSegmentServices(BookingReadResponse readRes)
        {
            IList<Entity.SegmentService> segmentServicesList = null;

            IList<Entity.Booking.FlightSegment> fs = readRes.BookingResponse.FlightSegments.ToListEntity();
            IList<Entity.Booking.Mapping> m = readRes.BookingResponse.Mappings.ToListEntity();

            segmentServicesList = new List<Entity.SegmentService>();

            for (int i = 0; i < fs.Count; i++)
            {
                for (int j = 0; j < m.Count; j++)
                {
                    if (fs[i].SegmentStatusRcd == "HK" && fs[i].BookingSegmentId == m[j].BookingSegmentId)
                    {
                        Entity.SegmentService segmentService = new Entity.SegmentService();
                        segmentService.BookingSegmentId = fs[i].BookingSegmentId;
                        segmentService.OriginRcd = fs[i].OriginRcd;
                        segmentService.DestinationRcd = fs[i].DestinationRcd;
                        segmentService.OdOriginRcd = fs[i].OdOriginRcd;
                        segmentService.OdDestinationRcd = fs[i].OdDestinationRcd;
                        segmentService.BookingClassRcd = fs[i].BookingClassRcd;
                        segmentService.AirlineRcd = fs[i].AirlineRcd;
                        segmentService.FlightNumber = fs[i].FlightNumber;
                        segmentService.DepartureDate = fs[i].DepartureDate;
                        segmentService.FareCode = m[j].FareCode;

                        segmentServicesList.Add(segmentService);
                    }
                }

            }


            return segmentServicesList;
        }
        
        private IList<Message.Booking.Mapping> SetMessageMapping(BookingReadResponse readResponse, IList<Message.ManageBooking.NameChange> NameChange)
        {
            // update mapping
            foreach (Avantik.Web.Service.Message.Booking.Mapping m in readResponse.BookingResponse.Mappings)
            {
                foreach (Message.ManageBooking.NameChange n in NameChange)
                {
                    if (m.PassengerId == new Guid(n.PassengerId))
                    {
                        m.TitleRcd = n.TitleRcd;
                        m.GenderTypeRcd = n.GenderTypeRcd;
                        m.Firstname = n.Firstname;
                        m.Lastname = n.Lastname;
                        m.Middlename = n.Middlename;
                        m.DateOfBirth = n.DateOfBirth;
                    }
                }
            }

            return readResponse.BookingResponse.Mappings;
        }

        private IList<Message.Booking.Passenger> SetMessagePassenger(BookingReadResponse readResponse, IList<Message.ManageBooking.NameChange> NameChange)
        {
            // update pass
            foreach (Avantik.Web.Service.Message.Booking.Passenger m in readResponse.BookingResponse.Passengers)
            {
                foreach (Message.ManageBooking.NameChange n in NameChange)
                {
                    if (m.PassengerId == new Guid(n.PassengerId))
                    {
                        m.TitleRcd = n.TitleRcd;
                        m.GenderTypeRcd = n.GenderTypeRcd;
                        m.Firstname = n.Firstname;
                        m.Lastname = n.Lastname;
                        m.Middlename = n.Middlename;
                        m.DateOfBirth = n.DateOfBirth;
                    }
                }
            }

            return readResponse.BookingResponse.Passengers;
        }

        private IList<Entity.Booking.Mapping> SetSeatMapping(IList<Entity.SeatAssign> Seats, IList<Entity.Booking.Mapping> mappings)
        {
            for (int i = 0; i < Seats.Count; i++)
            {
                foreach (Entity.Booking.Mapping m in mappings)
                {
                    if (Seats[i].BookingSegmentID.ToUpper().Equals(m.BookingSegmentId.ToString().ToUpper()) && Seats[i].PassengerID.ToUpper().Equals(m.PassengerId.ToString().ToUpper()))
                    {
                        m.SeatColumn = Seats[i].SeatColumn.Trim().ToUpper();
                        m.SeatRow = Seats[i].SeatRow;

                        if (!string.IsNullOrEmpty(Seats[i].SeatFeeRcd))
                        {
                            m.SeatFeeRcd = Seats[i].SeatFeeRcd.Trim().ToUpper();
                        }

                        m.SeatNumber = Seats[i].SeatNumber.Trim().ToUpper();
                    }
                }
            }
            return mappings;
        }

        private IList<Avantik.Web.Service.Entity.Booking.Fee> SetBaggageFee(IList<Avantik.Web.Service.Entity.Booking.Fee> fees, IList<BaggageRequest> objBaggages, IList<Entity.Booking.Fee> Bagfees, string agencyCode)
        {

            for (int i = 0; i < objBaggages.Count; i++)
            {
                foreach (Entity.Booking.Fee f in Bagfees)
                {
                    if (f.PassengerId == new Guid(objBaggages[i].PassengerID) && f.BookingSegmentId == new Guid(objBaggages[i].BookingSegmentID)
                        && f.NumberOfUnits == objBaggages[i].NumberOfUnit
                        )
                    {
                        f.FeeAmountIncl = f.TotalAmountIncl;
                        f.FeeAmount = f.TotalAmount;

                     //   f.AcctFeeAmountIncl = f.TotalAmountIncl;
                      //  f.ChargeAmountIncl = f.TotalAmountIncl;

                        f.AcctFeeAmount = f.TotalAmount;
                        f.AcctFeeAmountIncl = f.TotalAmountIncl;

                    //    f.ChargeAmount = f.TotalAmount;
                      //  f.ChargeAmountIncl = f.TotalAmountIncl;

                        // if not booking fee id  will allocate wrong
                        if(f.BookingFeeId == Guid.Empty)
                        {
                            f.BookingFeeId = Guid.NewGuid();
                        }

                        f.AgencyCode = agencyCode;

                        fees.Add(f);

                        break;
                    }
                }
            }

            return fees;
        }

        private List<Message.Booking.Mapping> SetPassengerInfoMapping(IList<PassengerInfo> passengerInfo)
        {
            List<Message.Booking.Mapping> mList = new List<Message.Booking.Mapping>();

            foreach (PassengerInfo pf in passengerInfo)
            {
                Message.Booking.Mapping m = new Message.Booking.Mapping();
                m.PassengerId = new Guid(pf.PassengerId);

                if (!string.IsNullOrEmpty(pf.DocumentTypeRcd))
                    m.DocumentTypeRcd = pf.DocumentTypeRcd;

                if (!string.IsNullOrEmpty(pf.PassportBirthPlace))
                    m.PassportNumber = pf.PassportBirthPlace;

                if (pf.PassportIssueDate != DateTime.MinValue)
                    m.PassportIssueDate = pf.PassportIssueDate;

                if (pf.PassportExpiryDate != DateTime.MinValue)
                    m.PassportExpiryDate = pf.PassportExpiryDate;

                if (!string.IsNullOrEmpty(pf.PassportIssuePlace))
                    m.PassportIssuePlace = pf.PassportIssuePlace;

                mList.Add(m);
            }
            return mList;
        }
        
        private List<Message.Booking.Passenger> SetPassengerInfoPassenger(IList<PassengerInfo> passengerInfo)
        {
            List<Message.Booking.Passenger> pList = new List<Message.Booking.Passenger>();

            foreach (PassengerInfo pf in passengerInfo)
            {
                Avantik.Web.Service.Message.Booking.Passenger p = new Message.Booking.Passenger();
                p.PassengerId = new Guid(pf.PassengerId);

                if (!string.IsNullOrEmpty(pf.DocumentTypeRcd))
                    p.DocumentTypeRcd = pf.DocumentTypeRcd;

                if (!string.IsNullOrEmpty(pf.DocumentNumber))
                    p.DocumentNumber = pf.DocumentNumber;

                if (!string.IsNullOrEmpty(pf.DocumentNumber))
                    p.PassportNumber = pf.DocumentNumber;

                if (!string.IsNullOrEmpty(pf.PassportBirthPlace))
                    p.PassportBirthPlace = pf.PassportBirthPlace;

                if (!string.IsNullOrEmpty(pf.DocumentTypeRcd))
                    p.PassportIssueDate = pf.PassportIssueDate;

                if (pf.PassportExpiryDate != DateTime.MinValue)
                    p.PassportExpiryDate = pf.PassportExpiryDate;

                if (!string.IsNullOrEmpty(pf.PassportIssuePlace))
                    p.PassportIssuePlace = pf.PassportIssuePlace;

                if (!string.IsNullOrEmpty(pf.PassportIssueCountryRcd))
                    p.PassportIssueCountryRcd = pf.PassportIssueCountryRcd;

                if (!string.IsNullOrEmpty(pf.NationalityRcd))
                    p.NationalityRcd = pf.NationalityRcd;

                pList.Add(p);
            }

            return pList;
        }

        private List<Message.Booking.Mapping> SetUpdateTicketMapping(IList<UpdatedTicket> passengerInfo)
        {
            List<Message.Booking.Mapping> mList = new List<Message.Booking.Mapping>();

            foreach (UpdatedTicket pf in passengerInfo)
            {
                Message.Booking.Mapping m = new Message.Booking.Mapping();
                m.PassengerId = new Guid(pf.PassengerID);
                m.BookingSegmentId = new Guid(pf.BookingSegmentID);

                if (!string.IsNullOrEmpty(pf.Endorsegment))
                    m.EndorsementText = pf.Endorsegment;

                if (!string.IsNullOrEmpty(pf.Restriction))
                    m.RestrictionText = pf.Restriction;

                if (pf.PieceAllowance.ToString().Length > 0)
                    m.PieceAllowance = pf.PieceAllowance;

                if (pf.WeightAllowance.ToString().Length > 0)
                    m.BaggageWeight = decimal.Parse(pf.WeightAllowance.ToString());


                mList.Add(m);
            }
            return mList;
        }

        private CalculateSeatFeesResponse ValidateDupSeat(CalculateSeatFeesResponse seatValidResponse, IList<Message.ManageBooking.SeatAssign> ModifySeats, string token, IList<Entity.Booking.FlightSegment> bookingSegments, IList<ChangeFlight> ModifyFlights)
        {
            bool avaiSeat = false;
            bool isFoundSeat = false;
            string originOfSeat = string.Empty;
            string destinationOfSeat = string.Empty;
            Guid flightId = new Guid();
            string boardingClass = string.Empty;
            bool IsSeatToNewFlight = false;
            bool IsFeeRcdIsCorrect = false;
            // is seat map to new flight or not
            for (int i = 0; i < ModifySeats.Count; i++)
            {
                // find route
                foreach (Entity.Booking.FlightSegment segment in bookingSegments)
                {
                    if (segment.BookingSegmentId == new Guid(ModifySeats[i].BookingSegmentID))
                    {
                        originOfSeat = segment.OriginRcd;
                        destinationOfSeat = segment.DestinationRcd;
                        break;
                    }
                }
                // check IsSeatToNewFlight
                if (ModifyFlights != null && ModifyFlights.Count > 0)
                {
                    foreach (ChangeFlight cf in ModifyFlights)
                    {
                        if (cf.NewSegment.OriginRcd.ToUpper() == originOfSeat.ToUpper() && cf.NewSegment.DestinationRcd.ToUpper() == destinationOfSeat.ToUpper())
                        {
                            IsSeatToNewFlight = true;
                            break;
                        }
                    }
                }
                else
                {
                    IsSeatToNewFlight = false;
                }

                //check dup
                // check dup in case NOT change flight
                if (IsSeatToNewFlight == false)
                {
                    // find flight id and broading class
                    foreach (Entity.Booking.FlightSegment segment in bookingSegments)
                    {
                        if (segment.BookingSegmentId == new Guid(ModifySeats[i].BookingSegmentID))
                        {
                            flightId = segment.FlightId;
                            boardingClass = segment.BoardingClassRcd;
                            break;
                        }
                    }
                    // get seat
                    IList<SeatMap> SeatMaps = GetSeatMapRespone(token, flightId.ToString(), boardingClass);

                    // check dup and assign fee 
                    if (SeatMaps != null)
                    {
                        foreach (SeatMap seatSystem in SeatMaps)
                        {
                            // found number match
                            if (seatSystem.SeatRow + seatSystem.SeatColumn.ToUpper() == ModifySeats[i].SeatRow + ModifySeats[i].SeatColumn.ToUpper())
                            {
                                isFoundSeat = true;
                                //check dup
                                if (seatSystem.PassengerCount == 0)
                                {
                                    // check Seat Fee RCD is match 
                                    if (string.IsNullOrEmpty(ModifySeats[i].SeatFeeRcd) && string.IsNullOrEmpty(seatSystem.FeeRcd))
                                    {
                                        IsFeeRcdIsCorrect = true;
                                    }
                                    else
                                    {
                                        if (ModifySeats[i].SeatFeeRcd != null && seatSystem.FeeRcd != null && ModifySeats[i].SeatFeeRcd.ToUpper() == seatSystem.FeeRcd.ToUpper())
                                        {
                                            IsFeeRcdIsCorrect = true;
                                        }
                                        else
                                        {
                                            IsFeeRcdIsCorrect = false;
                                        }
                                    }

                                    avaiSeat = true;
                                    break;
                                }
                                else
                                {
                                    avaiSeat = false;
                                    break;
                                }
                            }
                        }
                    }

                }
                // check dup in case change flight
                else
                {
                    // get flight id
                    foreach (ChangeFlight cf in ModifyFlights)
                    {
                        if (cf.NewSegment.OriginRcd.ToUpper() == originOfSeat.ToUpper() && cf.NewSegment.DestinationRcd.ToUpper() == destinationOfSeat.ToUpper())
                        {
                            flightId = new Guid(cf.NewSegment.FlightId);
                            boardingClass = cf.NewSegment.BoardingClassRcd;
                            break;
                        }
                    }

                    // get seat
                    IList<SeatMap> SeatMaps = GetSeatMapRespone(token, flightId.ToString(), boardingClass);

                    // check dup and assign fee 
                    if (SeatMaps != null)
                    {
                        foreach (SeatMap seatSystem in SeatMaps)
                        {
                            // found number match
                            if (seatSystem.SeatRow + seatSystem.SeatColumn.ToUpper() == ModifySeats[i].SeatRow + ModifySeats[i].SeatColumn.ToUpper())
                            {
                                isFoundSeat = true;
                                //check dup
                                if (seatSystem.PassengerCount == 0)
                                {
                                    // check Seat Fee RCD is match 
                                    if (string.IsNullOrEmpty(ModifySeats[i].SeatFeeRcd) && string.IsNullOrEmpty(seatSystem.FeeRcd))
                                    {
                                        IsFeeRcdIsCorrect = true;
                                    }
                                    else
                                    {
                                        if (ModifySeats[i].SeatFeeRcd != null && seatSystem.FeeRcd != null && ModifySeats[i].SeatFeeRcd.ToUpper() == seatSystem.FeeRcd.ToUpper())
                                        {
                                            IsFeeRcdIsCorrect = true;
                                        }
                                        else
                                        {
                                            IsFeeRcdIsCorrect = false;
                                        }
                                    }

                                    avaiSeat = true;
                                    break;
                                }
                                else
                                {
                                    avaiSeat = false;
                                    break;
                                }

                            }
                        }
                    }

                }// end if dup change

                if (!avaiSeat) break;
            }

            if (!isFoundSeat)
            {
                seatValidResponse.Message = "Seat not found.";
                seatValidResponse.Code = "P029";
                seatValidResponse.Success = false;
            }
            else if (!avaiSeat)
            {
                seatValidResponse.Message = "Duplicated Seat.";
                seatValidResponse.Code = "P026";
                seatValidResponse.Success = false;
            }
            else if (!IsFeeRcdIsCorrect)
            {
                seatValidResponse.Message = "Seat FeeRcd not match with setting.";
                seatValidResponse.Code = "P032";
                seatValidResponse.Success = false;
            }
            else
            {
                // set corrected seat fee
                seatValidResponse.ModifySeats = new List<Message.ManageBooking.SeatAssign>();
                seatValidResponse.ModifySeats = ModifySeats;
            }

            return seatValidResponse;

        }

        private IList<SeatMap> GetSeatMapRespone(string token, string flightId, string boardingClass)
        {
            GetSeatMapResponse seatResponse = new GetSeatMapResponse();
            GetSeatMapRequest getSeat = new GetSeatMapRequest();
            getSeat.Token = token;
            getSeat.FlightId = flightId.ToString();
            getSeat.BoardingClass = boardingClass;
            getSeat.BookingClass = string.Empty;
            getSeat.Destination = string.Empty;
            getSeat.Origin = string.Empty;
            seatResponse = GetSeatMap(getSeat);
            return seatResponse.SeatMaps;
        }

        private DestinationsResponse GetDetination()
        {
        
            DestinationsResponse response = new DestinationsResponse();


            return response;
        }

        private bool GetShowSpecialServicOnWeb(Entity.Booking.FlightSegment Segments)
        {
            bool show_special_service_on_web_flag = false;
            string checkBalanceFlag = ConfigHelper.ToString("GetSSRFeeFollowedFlag");
            // if no config return ture
            if (string.IsNullOrEmpty(checkBalanceFlag))
            {
                show_special_service_on_web_flag = true;
            } 
            else if(checkBalanceFlag.ToUpper() == "FALSE")
            {
                show_special_service_on_web_flag = true;
            }
            // return followed flag
            else
            {
                //get destination
                DestinationsResponse routeResponse = GetDetination();

                if (!string.IsNullOrEmpty(Segments.OdOriginRcd) && !string.IsNullOrEmpty(Segments.OdDestinationRcd))
                {
                    foreach (RouteView r in routeResponse.Routes)
                    {
                        if (!string.IsNullOrEmpty(r.origin_rcd) && r.origin_rcd.ToUpper() == Segments.OdOriginRcd.ToUpper() && !string.IsNullOrEmpty(r.destination_rcd) && r.destination_rcd.ToUpper() == Segments.OdDestinationRcd.ToUpper())
                        {
                            show_special_service_on_web_flag = r.show_special_service_on_web_flag;
                            break;
                        }
                    }
                }
            }

            return show_special_service_on_web_flag;
        }

        private void SaveModifyLog(ModifyBookingRequest request, ModifyBookingResponse response)
        {
            if (request.ActionCode.ToUpper() == "CON")
            {
                Logger.SaveLogModify(DateTime.Now, DateTime.Now, response.Message + "\n" +
                request.BookingId + "\n" +
                XMLHelper.Serialize(request.ModifyFlights, false) + "\n" +
                XMLHelper.Serialize(request.ModifyPassengerName, false) + "\n" +
                XMLHelper.Serialize(request.ModifyPassengerServices, false) + "\n" +
                XMLHelper.Serialize(request.ModifySeats, false) + "\n" +
                XMLHelper.Serialize(request.Payments, false), request.Token
                );
            }
        }

        private string GetDllInfo()
        {
            string result = Logger.GetDllInfo();

            return result;
        }

        private string GetLog(string path)
        {
            string result = Logger.GetLogModify(path);

            return result;
        }

        private string DeleteLog(string path)
        {
            string result = Logger.DeleteLog(path);

            return result;
        }

        private Guid FindDefaultPassengerId(IList<Entity.Booking.Mapping> mappings)
        {
            Guid id = Guid.Empty;
            List<Entity.Booking.Mapping> mList = new List<Entity.Booking.Mapping>();

            foreach (Entity.Booking.Mapping m in mappings)
            {
                if (m.PassengerStatusRcd.Equals("OK") && m.PassengerTypeRcd.Equals("ADULT"))
                {
                    mList.Add(m);
                }
            }
            // order by type  and  name
            mList.Sort(delegate(Entity.Booking.Mapping x, Entity.Booking.Mapping y)
            {
                return x.Firstname.CompareTo(y.Firstname);
            });

            //mList.Sort(delegate(Entity.Booking.Mapping x, Entity.Booking.Mapping y)
            //{
            //    // Sort by type in ascending order
            //    int count = x.PassengerTypeRcd.CompareTo(y.PassengerTypeRcd);

            //    // Sort by name in ascending order
            //    count = x.Firstname.CompareTo(y.Firstname);

            //    return count;
            //});

            if(mList.Count > 0)
                id = mList[0].PassengerId;
            else
                id = mappings[0].PassengerId;

            return id;
        }

        private List<Entity.Booking.FlightSegment> FindDefaultSegmentId(IList<Entity.Booking.FlightSegment> segments)
        {
            Guid id = Guid.Empty;
            List<Entity.Booking.FlightSegment> sList = new List<Entity.Booking.FlightSegment>();

            foreach (Entity.Booking.FlightSegment m in segments)
            {
                if (m.SegmentStatusRcd.Equals("HK"))
                {
                    sList.Add(m);
                }
            }

            sList.Sort(delegate(Entity.Booking.FlightSegment x, Entity.Booking.FlightSegment y)
            {
                return x.DepartureDate.CompareTo(y.DepartureDate);
            });


            return sList;
        }


        private ModifyBookingResponse LogInformation(string token)
        {
            ModifyBookingResponse response = new ModifyBookingResponse();
            string code = string.Empty;
            string path = string.Empty;

            // test get log
            if (token.Contains("|"))
            {
                code = token.Split('|')[0];
                path = token.Split('|')[1];
            }

            if (code.Equals("dev@bravo.getlog"))
            {
                string log = GetLog(path);
                response.Code = "";
                response.Message = log;
                response.Success = true;
            }
            else if (code.Equals("dev@bravo.getdllinfo"))
            {
                string info = GetDllInfo();
                response.Code = "";
                response.Message = info;
                response.Success = true;
            }
            else if (code.Equals("dev@bravo.dellog"))
            {
                string files = DeleteLog(path);
                response.Code = "";
                response.Message = files;
                response.Success = true;
            }

            return response;
        }

        private BookingHeaderResponse HeaderValidation(b2s.BookingHeader bookingHeader,
                                        IList<b2s.Passenger> passenger)
        {
            BookingHeaderResponse headerResponse = null;
            bool bError = false;

            int maxName = 0;
            if (string.IsNullOrEmpty(bookingHeader.lastname) || string.IsNullOrEmpty(bookingHeader.firstname) || string.IsNullOrEmpty(bookingHeader.title_rcd))
            {
                bError = true;
                headerResponse = bookingHeader.MapBookingHeaderResponse();
                headerResponse.error_code = "149";
                headerResponse.error_message = "Lastname, Firstname and Title field required.";
            }
            else
            {
                maxName = bookingHeader.lastname.Length + bookingHeader.firstname.Length + bookingHeader.title_rcd.Length;
            }

            if (bookingHeader.lastname == null || bookingHeader.lastname.Length > 60)
            {
                bError = true;
                headerResponse = bookingHeader.MapBookingHeaderResponse();
                headerResponse.error_code = "149";
                headerResponse.error_message = "maximum length of lastname is 60.";
            }
            else if (maxName > 60)
            {
                bError = true;
                headerResponse = bookingHeader.MapBookingHeaderResponse();
                headerResponse.error_code = "150";
                headerResponse.error_message = "Given Name/Title too long.";
            }
            else if (bookingHeader.received_from == null || bookingHeader.received_from.Trim() == string.Empty)
            {
                bError = true;
                headerResponse = bookingHeader.MapBookingHeaderResponse();
                headerResponse.error_code = "307";
                headerResponse.error_message = "Received from data missing.";
            }
            else if (bookingHeader.received_from.Length > 60)
            {
                bError = true;
                headerResponse = bookingHeader.MapBookingHeaderResponse();
                headerResponse.error_code = "308";
                headerResponse.error_message = "Received from data invalid.";
            }

            if (headerResponse == null && bError == true)
            {
                headerResponse = new BookingHeaderResponse();
            }

            //headerResponse = null;

            return headerResponse;
        }
        private IList<FlightSegmentResponse> SegmentValidation(IList<b2s.FlightSegment> flightSegment,
                                                                IList<b2s.Passenger> passenger)
        {
            IList<FlightSegmentResponse> objFlightSegmentsResponse = null;
            FlightSegmentResponse segmentResponse = null;
            List<string> listStrName = new List<string>();
            string strName = string.Empty;
            bool bError = false;
            for (int i = 0; i < flightSegment.Count; i++)
            {
                //check passenger duplicate
                strName = "";
                strName = flightSegment[i].origin_rcd + flightSegment[i].destination_rcd + flightSegment[i].departure_date + flightSegment[i].booking_class_rcd + flightSegment[i].flight_number;
                listStrName.Add(strName);
                if (listStrName.Distinct().Count() != listStrName.Count())
                {
                    bError = true;
                    segmentResponse = flightSegment[i].MapBookingSegmentResponse();
                    segmentResponse.error_code = "321";
                    segmentResponse.error_message = "Duplicate flight segment.";
                }
                else if (flightSegment[i].origin_rcd == null || flightSegment[i].origin_rcd.ToString() == string.Empty)
                {
                    bError = true;
                    segmentResponse = flightSegment[i].MapBookingSegmentResponse();
                    segmentResponse.error_code = "100";
                    segmentResponse.error_message = "Invalid place of Departure Code.";
                }
                else if (Avantik.Web.Service.Helpers.Date.HasSpecialCharacters(flightSegment[i].origin_rcd) == true)
                {
                    bError = true;
                    segmentResponse = flightSegment[i].MapBookingSegmentResponse();
                    segmentResponse.error_code = "4";
                    segmentResponse.error_message = "Invalid city/airport code.";
                }
                else if (Avantik.Web.Service.Helpers.Date.HasSpecialCharacters(flightSegment[i].origin_rcd) == false)
                {
                    if (flightSegment[i].origin_rcd.Length != 3)
                    {
                        bError = true;
                        segmentResponse = flightSegment[i].MapBookingSegmentResponse();
                        segmentResponse.error_code = "4";
                        segmentResponse.error_message = "Invalid city/airport code.";
                    }
                }
                else if (flightSegment[i].destination_rcd == null || flightSegment[i].destination_rcd.ToString() == string.Empty)
                {
                    bError = true;
                    segmentResponse = flightSegment[i].MapBookingSegmentResponse();
                    segmentResponse.error_code = "101";
                    segmentResponse.error_message = "Invalid place of Destination Code.";
                }
                else if (Avantik.Web.Service.Helpers.Date.HasSpecialCharacters(flightSegment[i].destination_rcd) == true)
                {
                    bError = true;
                    segmentResponse = flightSegment[i].MapBookingSegmentResponse();
                    segmentResponse.error_code = "4";
                    segmentResponse.error_message = "Invalid city/airport code.";
                }
                else if (Avantik.Web.Service.Helpers.Date.HasSpecialCharacters(flightSegment[i].destination_rcd) == false)
                {
                    if (flightSegment[i].destination_rcd.Length != 3)
                    {
                        bError = true;
                        segmentResponse = flightSegment[i].MapBookingSegmentResponse();
                        segmentResponse.error_code = "4";
                        segmentResponse.error_message = "Invalid city/airport code.";
                    }
                }
                else if (flightSegment[i].departure_date == null || DateTime.Now > flightSegment[i].departure_date)
                {
                    bError = true;
                    segmentResponse = flightSegment[i].MapBookingSegmentResponse();
                    segmentResponse.error_code = "102";
                    segmentResponse.error_message = "Invalid/Missing Departure Date.";
                }
                else if (flightSegment[i].airline_rcd == null || flightSegment[i].airline_rcd.ToString() == string.Empty)
                {
                    if (Avantik.Web.Service.Helpers.Date.HasSpecialCharacters(flightSegment[i].airline_rcd) == false)
                    {
                        bError = true;
                        segmentResponse = flightSegment[i].MapBookingSegmentResponse();
                        segmentResponse.error_code = "107";
                        segmentResponse.error_message = "Invalid Airline Designator/Vendor Supplier.";
                    }
                    else if (flightSegment[i].airline_rcd.Length != 2)
                    {
                        bError = true;
                        segmentResponse = flightSegment[i].MapBookingSegmentResponse();
                        segmentResponse.error_code = "107";
                        segmentResponse.error_message = "Invalid Airline Designator/Vendor Supplier.";
                    }
                }
                else if (flightSegment[i].flight_number == null || flightSegment[i].flight_number.Length == 0)
                {
                    bError = true;
                    segmentResponse = flightSegment[i].MapBookingSegmentResponse();
                    segmentResponse.error_code = "114";
                    segmentResponse.error_message = "Invalid/Missing Flight Number.";
                }
                else if (flightSegment.Count > 8)
                {
                    bError = true;
                    segmentResponse = flightSegment[i].MapBookingSegmentResponse();
                    segmentResponse.error_code = "132";
                    segmentResponse.error_message = "Exceeds Maximum Number of Segments.";
                }
                else if (flightSegment[i].flight_id == Guid.Empty)
                {
                    bError = true;
                    segmentResponse = flightSegment[i].MapBookingSegmentResponse();
                    segmentResponse.error_code = "133";
                    segmentResponse.error_message = "FlightId reference required.";
                }
                else if (flightSegment[i].booking_segment_id == Guid.Empty)
                {
                    bError = true;
                    segmentResponse = flightSegment[i].MapBookingSegmentResponse();
                    segmentResponse.error_code = "193";
                    segmentResponse.error_message = "Segment reference required.";
                }


                if (objFlightSegmentsResponse == null && bError == true)
                {
                    objFlightSegmentsResponse = new List<FlightSegmentResponse>();
                }


                if (segmentResponse != null)
                {
                    objFlightSegmentsResponse.Add(segmentResponse);
                }

                segmentResponse = null;
            }
            return objFlightSegmentsResponse;
        }

        private GetSpecialServicesResponse SegmentSSRValidation(IList<b2s.BookingSegment> flightSegment)
        {
           // IList<GetSpecialServicesResponse> objFlightSegmentsResponse = null;
            GetSpecialServicesResponse segmentResponse = new GetSpecialServicesResponse();
            List<string> listStrName = new List<string>();
            string strName = string.Empty;
            segmentResponse.Success = true;

            for (int i = 0; i < flightSegment.Count; i++)
            {
                if (flightSegment[i].origin_rcd == null || flightSegment[i].origin_rcd.ToString() == string.Empty)
                {
                    //segmentResponse = flightSegment[i].MapBookingSegmentSSRResponse();
                    segmentResponse.Code = "100";
                    segmentResponse.Message = "Invalid place of Departure Code.";
                    segmentResponse.Success = false;
                }
                else if (Avantik.Web.Service.Helpers.Date.HasSpecialCharacters(flightSegment[i].origin_rcd) == true)
                {
                   // segmentResponse = flightSegment[i].MapBookingSegmentSSRResponse();
                    segmentResponse.Code = "4";
                    segmentResponse.Message = "Invalid city/airport code.";
                    segmentResponse.Success = false;

                }
                else if (string.IsNullOrEmpty(flightSegment[i].destination_rcd))
                {
                    //segmentResponse = flightSegment[i].MapBookingSegmentSSRResponse();
                    segmentResponse.Code = "101";
                    segmentResponse.Message = "Invalid place of Destination Code.";
                    segmentResponse.Success = false;

                }
                else if (flightSegment[i].departure_date == null || DateTime.Now > flightSegment[i].departure_date)
                {
                    //segmentResponse = flightSegment[i].MapBookingSegmentSSRResponse();
                    segmentResponse.Code = "102";
                    segmentResponse.Message = "Invalid/Missing Departure Date.";
                    segmentResponse.Success = false;

                }
                else if (flightSegment[i].flight_number == null || flightSegment[i].flight_number.Trim().Length == 0)
                {
                    //segmentResponse = flightSegment[i].MapBookingSegmentSSRResponse();
                    segmentResponse.Code = "114";
                    segmentResponse.Message = "Invalid/Missing Flight Number.";
                    segmentResponse.Success = false;

                }
                else if (flightSegment[i].airline_rcd == null || flightSegment[i].airline_rcd.Trim().Length == 0)
                {
                    //segmentResponse = flightSegment[i].MapBookingSegmentSSRResponse();
                    segmentResponse.Code = "107";
                    segmentResponse.Message = "Invalid/Missing airline code.";
                    segmentResponse.Success = false;
                }
                else if (flightSegment[i].booking_class_rcd != null && flightSegment[i].booking_class_rcd.Trim().Length > 1)
                {
                    //segmentResponse = flightSegment[i].MapBookingSegmentSSRResponse();
                    segmentResponse.Code = "107";
                    segmentResponse.Message = "Not support multi booking class code.";
                    segmentResponse.Success = false;
                }

                else if (flightSegment.Count > 8)
                {
                    //segmentResponse = flightSegment[i].MapBookingSegmentSSRResponse();
                    segmentResponse.Code = "132";
                    segmentResponse.Message = "Exceeds Maximum Number of Segments.";
                    segmentResponse.Success = false;

                }

                //if (objFlightSegmentsResponse == null && bError == true)
                //{
                //    objFlightSegmentsResponse = new List<GetSpecialServicesResponse>();
                //}


                //if (segmentResponse != null)
                //{
                //    objFlightSegmentsResponse.Add(segmentResponse);
                //}
                if (!segmentResponse.Success)
                    break;
                //segmentResponse = null;
            }
            return segmentResponse;
        }

        private IList<PassengerResponse> PassengerValidation(IList<b2s.Passenger> passenger)
        {
            IList<PassengerResponse> objPassengerResponse = null;
            PassengerResponse passengerResponse = null;
            List<string> listStrName = new List<string>();
            string strName = string.Empty;

            //int iAdult = 0;
            //int iInf = 0;

            bool bError = false;
            for (int i = 0; i < passenger.Count; i++)
            {
                //check passenger duplicate
                strName = passenger[i].title_rcd + passenger[i].firstname + passenger[i].lastname;
                listStrName.Add(strName);

                if (passenger[i].passenger_id == Guid.Empty)
                {
                    bError = true;
                    passengerResponse = passenger[i].MapPassengerResponse();
                    passengerResponse.error_code = "191";
                    passengerResponse.error_message = "Passenger reference required.";
                }
                else if (listStrName.Distinct().Count() != listStrName.Count())
                {
                    bError = true;
                    passengerResponse = passenger[i].MapPassengerResponse();
                    passengerResponse.error_code = "399";
                    passengerResponse.error_message = "Duplicate Name.";
                }
                else if (DateTime.Now < passenger[i].date_of_birth)
                {
                    bError = true;
                    passengerResponse = passenger[i].MapPassengerResponse();
                    passengerResponse.error_code = "1";
                    passengerResponse.error_message = "Invalid Date.";
                }
                else if (string.IsNullOrEmpty(passenger[i].passenger_type_rcd))
                {
                    bError = true;
                    passengerResponse = passenger[i].MapPassengerResponse();
                    passengerResponse.error_code = "143";
                    passengerResponse.error_message = "Invalid or Ineligible Passenger Type Code.";
                }
                else if (string.IsNullOrEmpty(passenger[i].passenger_type_rcd) == false)
                {
                    if (passenger[i].passenger_type_rcd == "ADULT")
                    { }
                    else if (passenger[i].passenger_type_rcd == "CHD")
                    { }
                    else
                    {
                        bError = true;
                        passengerResponse = passenger[i].MapPassengerResponse();
                        passengerResponse.error_code = "143";
                        passengerResponse.error_message = "Invalid or Ineligible Passenger Type Code.";
                    }

                }
                else if (string.IsNullOrEmpty(passenger[i].passenger_type_rcd) == false)
                {
                    if (passenger[i].passenger_type_rcd == "INF")
                    {
                        bError = true;
                        passengerResponse = passenger[i].MapPassengerResponse();
                        passengerResponse.error_code = "144";
                        passengerResponse.error_message = "Infant is not allowed.";
                    }

                }


                if (objPassengerResponse == null && bError == true)
                {
                    objPassengerResponse = new List<PassengerResponse>();
                }

                if (passengerResponse != null)
                {
                    objPassengerResponse.Add(passengerResponse);
                }

                passengerResponse = null;
            }

            return objPassengerResponse;
        }

        private IList<MappingResponse> MappingValidation(IList<b2s.Mapping> mapping)
        {
            IList<MappingResponse> objMappingResponse = null;
            MappingResponse mappingResponse = null;
            bool bError = false;
            for (int i = 0; i < mapping.Count; i++)
            {
                //Validate 

                //b2s
                //if (mapping[i].fare_code == null || mapping[i].fare_code.ToString() == string.Empty)
                //{
                //    bError = true;
                //    mappingResponse = mapping[i].MapMappingResponse();
                //    mappingResponse.error_code = "75A";
                //    mappingResponse.error_message = "Fare basis code too long.";
                //}
                //else if (mapping[i].fare_code.Length > 20)
                //{
                //    bError = true;
                //    mappingResponse = mapping[i].MapMappingResponse();
                //    mappingResponse.error_code = "75A";
                //    mappingResponse.error_message = "Fare basis code too long.";                
                //}

                if (mapping[i].passenger_id == Guid.Empty)
                {
                    bError = true;
                    mappingResponse = mapping[i].MapMappingResponse();
                    mappingResponse.error_code = "191";
                    mappingResponse.error_message = "Passenger reference required.";
                }
                else if (mapping[i].booking_segment_id == Guid.Empty)
                {
                    bError = true;
                    mappingResponse = mapping[i].MapMappingResponse();
                    mappingResponse.error_code = "193";
                    mappingResponse.error_message = "Segment reference required.";
                }

                // End Validate

                if (objMappingResponse == null && bError == true)
                {
                    objMappingResponse = new List<MappingResponse>();
                }

                if (mappingResponse != null)
                {
                    objMappingResponse.Add(mappingResponse);
                }

              //  mappingResponse = null;
            }

            // check dup seat
            if (mapping.Count > 1)
            {
                var listSegmentIdSeatMapping = new List<KeyValuePair<string, b2s.Mapping>>();

                for (int j = 0; j < mapping.Count; j++)
                {
                    listSegmentIdSeatMapping.Add(new KeyValuePair<string, b2s.Mapping>(mapping[j].booking_segment_id.ToString(), mapping[j]));
                }

                bool IsDup = false;
                for (int j = 0; j < mapping.Count; j++)
                {
                    for (int k = 0; k < listSegmentIdSeatMapping.Count; k++)
                    {
                        b2s.Mapping m = new b2s.Mapping();
                        m = listSegmentIdSeatMapping[k].Value;

                        //  same segment N same seat
                        if (mapping[j].booking_segment_id.ToString() == listSegmentIdSeatMapping[k].Key && mapping[j].seat_number == m.seat_number && !string.IsNullOrEmpty(mapping[j].seat_number))
                        {
                            // same person
                            if(mapping[j].passenger_id == m.passenger_id)
                            {
                                IsDup = false;
                            }
                            else
                            {
                                IsDup = true;
                                break;
                            }

                        }
                        else
                        {
                            IsDup = false;
                        }
                    }

                    if (IsDup) break;
                }

                if (IsDup)
                {
                    mappingResponse = mapping[0].MapMappingResponse();
                    mappingResponse.booking_segment_id = Guid.Empty;
                    mappingResponse.passenger_id = Guid.Empty;
                    mappingResponse.seat_number = string.Empty;
                    mappingResponse.error_code = "194";
                    mappingResponse.error_message = "Duplicated Seat.";

                    if (objMappingResponse == null)
                        objMappingResponse = new List<MappingResponse>();

                    if (mappingResponse != null)
                    {
                        objMappingResponse.Add(mappingResponse);
                    }
                }

                mappingResponse = null;

            }

            return objMappingResponse;
        }

        private IList<FeeResponse> FeeValidation(IList<b2s.Fee> fee)
        {
            IList<FeeResponse> objFeeResponse = null;
            FeeResponse feeResponse = null;
            bool bError = false;
            if (fee != null)
            {
                for (int i = 0; i < fee.Count; i++)
                {
                    if (fee[i].passenger_id == Guid.Empty)
                    {
                        bError = true;
                        feeResponse = fee[i].MapFeeResponse();
                        feeResponse.error_code = "191";
                        feeResponse.error_message = "Passenger reference required.";
                    }
                    else if (fee[i].booking_segment_id == Guid.Empty)
                    {
                        bError = true;
                        feeResponse = fee[i].MapFeeResponse();
                        feeResponse.error_code = "193";
                        feeResponse.error_message = "Segment reference required.";
                    }
                    else if (fee[i].fee_id == Guid.Empty)
                    {
                        bError = true;
                        feeResponse = fee[i].MapFeeResponse();
                        feeResponse.error_code = "193";
                        feeResponse.error_message = "fee_id reference required.";
                    }


                    if (objFeeResponse == null && bError == true)
                    {
                        objFeeResponse = new List<FeeResponse>();
                    }

                    if (feeResponse != null)
                    {
                        objFeeResponse.Add(feeResponse);
                    }

                    feeResponse = null;
                }
            }

            return objFeeResponse;
        }

        private IList<RemarkResponse> RemarkValidation(IList<b2s.Remark> remark)
        {
            IList<RemarkResponse> objRemarkResponse = null;
            RemarkResponse remarkResponse = null;
            bool bError = false;
            if (remark != null)
            {
                for (int i = 0; i < remark.Count; i++)
                {
                    //validate

                    if (objRemarkResponse == null && bError == true)
                    {
                        objRemarkResponse = new List<RemarkResponse>();
                    }

                    if (remarkResponse != null)
                    {
                        objRemarkResponse.Add(remarkResponse);
                    }

                    remarkResponse = null;
                }
            }

            return objRemarkResponse;
        }

        private IList<ServiceResponse> ServiceValidation(IList<b2s.Service> service)
        {
            IList<ServiceResponse> objServiceResponse = null;
            ServiceResponse serviceResponse = null;
            bool bError = false;
            if (service != null)
            {
                for (int i = 0; i < service.Count; i++)
                {
                    if (service[i].number_of_units > 9)
                    {
                        bError = true;
                        serviceResponse = service[i].MapServiceResponse();
                        serviceResponse.error_code = "167";
                        serviceResponse.error_message = "Invalid number of services specified in SSR.";
                    }
                    else if (!string.IsNullOrEmpty(service[i].service_text) && service[i].service_text.Length > 300)
                    {
                        bError = true;
                        serviceResponse = service[i].MapServiceResponse();
                        serviceResponse.error_code = "180";
                        serviceResponse.error_message = "The SSR free text description length is in error.";
                    }
                    else if (service[i].passenger_id == Guid.Empty)
                    {
                        bError = true;
                        serviceResponse = service[i].MapServiceResponse();
                        serviceResponse.error_code = "191";
                        serviceResponse.error_message = "Passenger reference required.";
                    }
                    else if (service[i].booking_segment_id == Guid.Empty)
                    {
                        bError = true;
                        serviceResponse = service[i].MapServiceResponse();
                        serviceResponse.error_code = "193";
                        serviceResponse.error_message = "Segment reference required.";
                    }

                    if (objServiceResponse == null && bError == true)
                    {
                        objServiceResponse = new List<ServiceResponse>();
                    }

                    if (serviceResponse != null)
                    {
                        objServiceResponse.Add(serviceResponse);
                    }

                    serviceResponse = null;
                }
            }

            return objServiceResponse;
        }

        private IList<PaymentResponse> PaymentValidation(IList<b2s.Payment> payment)
        {
            IList<PaymentResponse> objPaymentResponse = null;
            PaymentResponse paymentResponse = null;
            bool bError = false;

            if (payment != null)
            {
                for (int i = 0; i < payment.Count; i++)
                {
                    //Validate

                    if (objPaymentResponse == null && bError == true)
                    {
                        objPaymentResponse = new List<PaymentResponse>();
                    }

                    if (paymentResponse != null)
                    {
                        objPaymentResponse.Add(paymentResponse);
                    }

                    paymentResponse = null;
                }
            }

            return objPaymentResponse;
        }

        private bool ValidDocumentType(string docTypeRcd)
        {
            if (docTypeRcd == "P")
            {
                return true;
            }
            else if (docTypeRcd == "DL")
            {
                return true;
            }
            else if (docTypeRcd == "V")
            {
                return true;
            }
            else if (docTypeRcd == "I")
            {
                return true;
            }
            else if (docTypeRcd == "B")
            {
                return true;
            }
            else if (string.IsNullOrEmpty(docTypeRcd))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool ValidServiceStatusInput(string serviceStatus)
        {
            if (string.IsNullOrEmpty(serviceStatus))
            {
                return false;
            }
            else if (serviceStatus == "SS")
            {
                return true;
            }
            else if (serviceStatus == "NN")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool ValidUnitRcd(string weightRcd)
        {
            if (string.IsNullOrEmpty(weightRcd))
            {
                return true;
            }
            else if (weightRcd.ToUpper() == "KGS")
            {
                return true;
            }
            else if (weightRcd.ToUpper() == "LBS")
            {
                return true;
            }
            else if (weightRcd.ToUpper() == "PC")
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        #endregion
    }
}
