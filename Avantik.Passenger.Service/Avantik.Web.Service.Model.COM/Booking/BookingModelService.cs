using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Avantik.Web.Service.Model.Contract;
using Avantik.Web.Service.Infrastructure;
using Avantik.Web.Service.COMHelper;
using entity = Avantik.Web.Service.Entity.Booking;

using Avantik.Web.Service.Helpers;

using System.Net;
using Newtonsoft.Json;

namespace Avantik.Web.Service.Model.COM
{
    public class BookingModelService : RunComplus ,IBookingModelService
    {
        string _server = string.Empty;
        string _user = string.Empty;
        string _pass = string.Empty;
        string _domain = string.Empty;

        bool result = false;
        public BookingModelService(string server, string user, string pass, string domain)
            :base(user,pass,domain)
        {
            _server = server;
            _user = user;
            _pass = pass;
            _domain = domain;
        }

        public bool SaveBooking(entity.Booking booking,
                               bool createTickets,
                               bool readBooking,
                               bool readOnly,
                               bool bSetLock,
                               bool bCheckSeatAssignment,
                               bool bCheckSessionTimeOut)
        {

            string strBookingId = string.Empty;
            string strOther = string.Empty;
            string strUserId = string.Empty;
            string strAgencyCode = string.Empty;
            string strCurrency = string.Empty;
            string strIP = string.Empty;

            short iAdult = 0;
            short iChild = 0;
            short iInfant = 0;
            short iOther = 0;

            try
            {
                if (booking.Header != null)
                {
                    strBookingId = booking.Header.BookingId != null ? booking.Header.BookingId.ToString() : string.Empty;
                    strOther = string.Empty;
                    strUserId = booking.Header.CreateBy != null ? booking.Header.CreateBy.ToString() : string.Empty;
                    strAgencyCode = booking.Header.AgencyCode ?? string.Empty;
                    strCurrency = booking.Header.CurrencyRcd ?? string.Empty;
                    strIP = booking.Header.IpAddress ?? string.Empty;
                    iAdult = Convert.ToInt16(booking.Header.NumberOfAdults);
                    iChild = Convert.ToInt16(booking.Header.NumberOfChildren);
                    iInfant = Convert.ToInt16(booking.Header.NumberOfInfants);

                    booking.Header.NumberOfAdults = iAdult;
                    booking.Header.NumberOfChildren = iChild;
                    booking.Header.NumberOfInfants = iInfant;
                }
            }
            catch (System.Exception ex)
            {
            }

            //prepare request

            //add booking
            entity.Booking restBooking = new entity.Booking();
            restBooking.Header = booking.Header;
            restBooking.Segments = booking.Segments;
            restBooking.Passengers = booking.Passengers;
            restBooking.Mappings = booking.Mappings;
            restBooking.Payments = booking.Payments;
            restBooking.Fees = booking.Fees;
            restBooking.Services = booking.Services;
            restBooking.Taxs = booking.Taxs;
            restBooking.Remarks = booking.Remarks;
            restBooking.Quotes = booking.Quotes;

            // Initialize and populate the BookingSaveRequest object
            var bookingSaveRequest = new Avantik.Web.Service.Entity.Booking.REST.BookingSaveRequest
            {
                booking = restBooking,
                createTickets = true,
                readBooking = false,
                readOnly = false,
                bSetLock = false,
                bCheckSeatAssignment = true,
                bCheckSessionTimeOut = true
            };

            //call REST API
            string baseURL = ConfigHelper.ToString("RESTURL");
            //string baseURL = "https://localhost:7021/";//ConfigHelper.ToString("RESTURL");
            string apiURL = baseURL + "api/Booking/SaveBooking";

            var jsonContent = JsonConvert.SerializeObject(bookingSaveRequest);
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
                        result = true;
                    }
                }

            }
            catch (System.Exception ex)
            {

            }

            return result;
        }


      

   
      

    }
}
