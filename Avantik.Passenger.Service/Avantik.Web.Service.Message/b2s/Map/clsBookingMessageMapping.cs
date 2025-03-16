using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Avantik.Web.Service.Message.b2s;
namespace Avantik.Web.Service.Message.b2s.map
{
    public static class BookingMessageMapping
    {
        #region mapping response
        public static BookingHeaderResponse MapBookingHeaderResponse(this BookingHeader header)
        {
            try
            {
                BookingHeaderResponse response = new BookingHeaderResponse();



                response.agency_code = header.agency_code;
                response.currency_rcd = header.currency_rcd;
              //  response.booking_number = header.booking_number;
                response.language_rcd = header.language_rcd;
                response.contact_name = header.contact_name;
                response.contact_email = header.contact_email;
                response.received_from = header.received_from;
             //   response.phone_search = header.phone_search;
                response.comment = header.comment;
                response.title_rcd = header.title_rcd;
                response.lastname = header.lastname;
                response.firstname = header.firstname;
                response.middlename = header.middlename;
                response.country_rcd = header.country_rcd;
               // response.group_booking_flag = header.group_booking_flag;

                return response;
            }
            catch
            {
                throw;
            }
        }
        public static IList<BookingHeaderResponse> MapBookingHeaderResponse(this IList<BookingHeader> header)
        {
            try
            {
                if (header != null && header.Count > 0)
                {
                    IList<BookingHeaderResponse> headers = new List<BookingHeaderResponse>();

                    for (int i = 0; i < header.Count; i++)
                    {
                        headers.Add(header[i].MapBookingHeaderResponse());
                    }

                    return headers;
                }
            }
            catch
            {
                throw;
            }
            return null;
        }
        
        public static FlightSegmentResponse MapBookingSegmentResponse(this FlightSegment segment)
        {
            try
            {
                FlightSegmentResponse segRes = new FlightSegmentResponse();

                segRes.booking_segment_id = segment.booking_segment_id;
               // segRes.flight_connection_id = segment.flight_connection_id;
                segRes.departure_date = segment.departure_date;
                segRes.airline_rcd = segment.airline_rcd;
                segRes.flight_number = segment.flight_number;
                segRes.origin_rcd = segment.origin_rcd;
                segRes.destination_rcd = segment.destination_rcd;
             //   segRes.od_origin_rcd = segment.od_origin_rcd;
             //   segRes.od_destination_rcd = segment.od_destination_rcd;
                segRes.booking_class_rcd = segment.booking_class_rcd;
                segRes.boarding_class_rcd = segment.boarding_class_rcd;
              //  segRes.segment_status_rcd = segment.segment_status_rcd;
                //Use for passenger count in the flight.
             //   segRes.number_of_units = segment.number_of_units;

                return segRes;
            }
            catch
            {
                throw;
            }
        }
        public static IList<FlightSegmentResponse> MapBookingSegmentsResponse(this IList<FlightSegment> segment)
        {
            try
            {
                if (segment != null && segment.Count > 0)
                {
                    IList<FlightSegmentResponse> segments = new List<FlightSegmentResponse>();

                    for (int i = 0; i < segment.Count; i++)
                    {
                        segments.Add(segment[i].MapBookingSegmentResponse());
                    }

                    return segments;
                }
            }
            catch
            {
                throw;
            }
            return null;
        }


        public static FlightSegmentResponse MapBookingSegmentSSRResponse(this BookingSegment segment)
        {
            try
            {
                FlightSegmentResponse segRes = new FlightSegmentResponse();

                segRes.departure_date = segment.departure_date;
                segRes.airline_rcd = segment.airline_rcd;
                segRes.flight_number = segment.flight_number;
                segRes.origin_rcd = segment.origin_rcd;
                segRes.destination_rcd = segment.destination_rcd;
                segRes.booking_class_rcd = segment.booking_class_rcd;

                return segRes;
            }
            catch
            {
                throw;
            }
        }
        public static IList<FlightSegmentResponse> MapBookingSegmentSSRResponse(this IList<BookingSegment> segment)
        {
            try
            {
                if (segment != null && segment.Count > 0)
                {
                    IList<FlightSegmentResponse> segments = new List<FlightSegmentResponse>();

                    for (int i = 0; i < segment.Count; i++)
                    {
                        segments.Add(segment[i].MapBookingSegmentSSRResponse());
                    }

                    return segments;
                }
            }
            catch
            {
                throw;
            }
            return null;
        }

        public static PassengerResponse MapPassengerResponse(this Passenger passenger)
        {
            try
            {
                PassengerResponse paxRes = new PassengerResponse();
                    
                        paxRes.passenger_id = passenger.passenger_id;
                        paxRes.guardian_passenger_id = passenger.guardian_passenger_id;
                        paxRes.date_of_birth = passenger.date_of_birth;
                       // paxRes.client_number = passenger.client_number;


                        paxRes.passenger_type_rcd = passenger.passenger_type_rcd;
                        paxRes.lastname = passenger.lastname;
                        paxRes.firstname = passenger.firstname;
                        paxRes.middlename = passenger.middlename;
                        paxRes.title_rcd = passenger.title_rcd;
                        paxRes.gender_type_rcd = passenger.gender_type_rcd;
                        paxRes.nationality_rcd = passenger.nationality_rcd;
                    
                    return paxRes;
            }
            catch
            {
                throw;
            }
        }
        public static IList<PassengerResponse> MapPassengersResponse(this IList<Passenger> passenger)
        {
            try
            {
                if (passenger != null && passenger.Count > 0)
                {
                    IList<PassengerResponse> passengers = new List<PassengerResponse>();

                    for (int i = 0; i < passenger.Count; i++)
                    {
                        passengers.Add(passenger[i].MapPassengerResponse());
                    }
                    return passengers;
                }
            }
            catch
            {
                throw;
            }
            return null;
        }
        public static MappingResponse MapMappingResponse(this Mapping mapping)
        {
            try
            {
                    MappingResponse mpRes = new MappingResponse();

                        mpRes.passenger_id = mapping.passenger_id;
                        mpRes.booking_segment_id = mapping.booking_segment_id;
                        mpRes.seat_number = mapping.seat_number;
                        mpRes.currency_rcd = mapping.currency_rcd;

                        return mpRes;
            }
            catch
            {
                throw;
            }
            //return null;
        }
        public static IList<MappingResponse> MapMappingsResponse(this IList<Mapping> mapping)
        {
            try
            {
                if (mapping != null && mapping.Count > 0)
                {
                    IList<MappingResponse> mappings = new List<MappingResponse>();

                    for (int i = 0; i < mapping.Count; i++)
                    {
                        mappings.Add(mapping[i].MapMappingResponse());
                    }
                    return mappings;
                }
            }
            catch
            {
                throw;
            }
            return null;
        }
        public static FeeResponse MapFeeResponse(this Fee fee)
        {
            try
            {
                    FeeResponse feRes = new FeeResponse();

                    feRes.passenger_id = fee.passenger_id;
                    feRes.booking_segment_id = fee.booking_segment_id;
                    feRes.fee_rcd = fee.fee_rcd;

                    feRes.vendor_rcd = fee.vendor_rcd;
                    feRes.currency_rcd = fee.currency_rcd;
                    feRes.comment = fee.comment;
                    feRes.fee_amount = fee.fee_amount;
                    feRes.fee_amount_incl = fee.fee_amount_incl;
                    feRes.vat_percentage = fee.vat_percentage;
                    feRes.charge_amount = fee.charge_amount;
                    feRes.charge_amount_incl = fee.charge_amount_incl;
                    feRes.number_of_units = fee.number_of_units;

                    return feRes;
            }
            catch
            {
                throw;
            }            
        }
        public static IList<FeeResponse> MapFeesResponse(this IList<Fee> fee)
        {
            try
            {
                if (fee != null && fee.Count > 0)
                {
                    IList<FeeResponse> fees = new List<FeeResponse>();
                    for (int i = 0; i < fee.Count; i++)
                    {
                        fees.Add(fee[i].MapFeeResponse());
                    }
                    return fees;
                }
            }
            catch
            {
                throw;
            }
            return null;
        }
        public static RemarkResponse MapRemarkResponse(this Remark remark)
        {
            try
            {
                RemarkResponse rkRes = new RemarkResponse();

                rkRes.remark_type_rcd = remark.remark_type_rcd;
                rkRes.remark_text = remark.remark_text;
                rkRes.nickname = remark.nickname;

                return rkRes;
            }
            catch
            {
                throw;
            }
        }
        public static IList<RemarkResponse> MapRemarksResponse(this IList<Remark> remark)
        {
            try
            {
                if (remark != null && remark.Count > 0)
                {
                    IList<RemarkResponse> remarks = new List<RemarkResponse>();

                    for (int i = 0; i < remark.Count; i++)
                    {
                        remarks.Add(remark[i].MapRemarkResponse());
                    }

                    return remarks;
                }
            }
            catch
            {
                throw;
            }
            return null;
        }
        public static ServiceResponse MapServiceResponse(this Avantik.Web.Service.Message.b2s.Service service)
        {
            try
            {
                ServiceResponse svRes = new ServiceResponse();

               // svRes.passenger_segment_service_id = service.passenger_segment_service_id;
                svRes.passenger_id = service.passenger_id;
                svRes.booking_segment_id = service.booking_segment_id;
                svRes.number_of_units = service.number_of_units;
                svRes.special_service_rcd = service.special_service_rcd;
                svRes.service_text = service.service_text;
              //  svRes.special_service_status_rcd = service.special_service_status_rcd;
              //  svRes.unit_rcd = service.unit_rcd;

                return svRes;
            }
            catch
            {
                throw;
            }
        }
        public static IList<ServiceResponse> MapServicesResponse(this IList<Avantik.Web.Service.Message.b2s.Service> service)
        {
            try
            {
                if (service != null && service.Count > 0)
                {
                    IList<ServiceResponse> services = new List<ServiceResponse>();
                    for (int i = 0; i < service.Count; i++)
                    {
                        services.Add(service[i].MapServiceResponse());
                    }

                    return services;
                }
            }
            catch
            {
                throw;
            }
            return null;
        }
        public static PaymentResponse MapPaymentResponse(this Payment payment)
        {
            try
            {
                    PaymentResponse pmRes = new PaymentResponse();

                        //If not passin use server time.
                        pmRes.payment_date_time = payment.payment_date_time;


                        if (payment.credit_card != null)
                        {
                            pmRes.credit_card = new CreditCard();
                            pmRes.credit_card.credit_card_number = payment.credit_card.credit_card_number;
                            pmRes.credit_card.name_on_card = payment.credit_card.name_on_card;
                            pmRes.credit_card.cvv_code = payment.credit_card.cvv_code;
                            pmRes.credit_card.issue_number = payment.credit_card.issue_number;
                            pmRes.credit_card.expiry_month = payment.credit_card.expiry_month;
                            pmRes.credit_card.expiry_year = payment.credit_card.expiry_year;
                            pmRes.credit_card.issue_month = payment.credit_card.issue_month;
                            pmRes.credit_card.issue_year = payment.credit_card.issue_year;
                        }

                        pmRes.payment_amount = payment.payment_amount;
                        pmRes.receive_payment_amount = payment.receive_payment_amount;
                        pmRes.payment_type_rcd = payment.payment_type_rcd;
                        pmRes.form_of_payment_rcd = payment.form_of_payment_rcd;
                        pmRes.form_of_payment_subtype_rcd = payment.form_of_payment_subtype_rcd;
                        pmRes.debit_agency_code = payment.debit_agency_code;
                        pmRes.currency_rcd = payment.currency_rcd;
                        pmRes.receive_currency_rcd = payment.receive_currency_rcd;
                        pmRes.approval_code = payment.approval_code;
                        pmRes.transaction_reference = payment.transaction_reference;
                        pmRes.payment_number = payment.payment_number;
                        pmRes.payment_reference = payment.payment_reference;
                        pmRes.payment_remark = payment.payment_remark;


                        return pmRes;
            }
            catch
            {
                throw;
            }
        }
        public static IList<PaymentResponse> MapPaymentsResponse(this IList<Payment> payment)
        {
            try
            {
                if (payment != null && payment.Count > 0)
                {
                    IList<PaymentResponse> payments = new List<PaymentResponse>();

                    for (int i = 0; i < payment.Count; i++)
                    {
                        payments.Add(payment[i].MapPaymentResponse());
                    }

                    return payments;
                }
            }
            catch
            {
                throw;
            }
            return null;
        }

        #endregion

        #region mapping save reponse  ws object
        public static BookingHeaderResponse MapBookingHeaderResponse(this  Avantik.Web.Service.Message.Booking.BookingHeader header)
        {
            BookingHeaderResponse bookingHeader = new BookingHeaderResponse();

            if (header != null)
            {
                bookingHeader.booking_id = header.BookingId;
                bookingHeader.record_locator = header.RecordLocator;
                bookingHeader.agency_code = header.AgencyCode;
                bookingHeader.currency_rcd = header.CurrencyRcd;
            //    bookingHeader.booking_number = Convert.ToString(header.BookingNumber);
                bookingHeader.language_rcd = header.LanguageRcd;
                bookingHeader.contact_name = header.ContactName;
                bookingHeader.contact_email = header.ContactEmail;
                bookingHeader.received_from = header.ReceivedFrom;
           //     bookingHeader.phone_search = header.PhoneSearch;
                bookingHeader.comment = header.Comment;
                bookingHeader.title_rcd = header.TitleRcd;
                bookingHeader.lastname = header.Lastname;
                bookingHeader.firstname = header.Firstname;
                bookingHeader.middlename = header.Middlename;
                bookingHeader.country_rcd = header.CountryRcd;

                bookingHeader.mobile_number = header.PhoneMobile;
                bookingHeader.home_number = header.PhoneHome;
                bookingHeader.business_number = header.PhoneBusiness;

            //    bookingHeader.group_booking_flag = header.GroupBookingFlag;

            }

            return bookingHeader;
        }
        public static IList<FlightSegmentResponse> MapBookingSegmentsResponse(this  IList<Avantik.Web.Service.Message.Booking.FlightSegment> objBooking)
        {
            IList<FlightSegmentResponse> objResponseList = new List<FlightSegmentResponse>();
            if (objBooking != null)
            {
                for (int i = 0; i < objBooking.Count; i++)
                {
                    objResponseList.Add(objBooking[i].MapBookingSegmentsResponse());
                }
            }
            return objResponseList;
        }
        public static FlightSegmentResponse MapBookingSegmentsResponse(this  Avantik.Web.Service.Message.Booking.FlightSegment objBookingResponse)
        {
            FlightSegmentResponse objReadResponse = new FlightSegmentResponse();

            if (objBookingResponse != null)
            {
                objReadResponse.booking_segment_id = objBookingResponse.BookingSegmentId;
              //  objReadResponse.flight_connection_id = objBookingResponse.FlightConnectionId;
                objReadResponse.flight_id = objBookingResponse.FlightId;

                objReadResponse.departure_date = objBookingResponse.DepartureDate;
                objReadResponse.departure_time = objBookingResponse.DepartureTime;
                objReadResponse.arrival_time = objBookingResponse.ArrivalTime;
                objReadResponse.airline_rcd = objBookingResponse.AirlineRcd;
                objReadResponse.flight_number = objBookingResponse.FlightNumber;
                objReadResponse.origin_rcd = objBookingResponse.OriginRcd;
                objReadResponse.destination_rcd = objBookingResponse.DestinationRcd;
             //   objReadResponse.od_origin_rcd = objBookingResponse.OdOriginRcd;
              //  objReadResponse.od_destination_rcd = objBookingResponse.OdDestinationRcd;
                objReadResponse.booking_class_rcd = objBookingResponse.BookingClassRcd;
                objReadResponse.boarding_class_rcd = objBookingResponse.BoardingClassRcd;
                objReadResponse.segment_status_rcd = objBookingResponse.SegmentStatusRcd;

                //Use for passenger count in the flight.
              //  objReadResponse.number_of_units = objBookingResponse.NumberOfUnits;

            }

            return objReadResponse;
        }
        public static IList<PassengerResponse> MapPassengersResponse(this  IList<Avantik.Web.Service.Message.Booking.Passenger> objBooking)
        {
            IList<PassengerResponse> objResponseList = new List<PassengerResponse>();
            if (objBooking != null)
            {
                for (int i = 0; i < objBooking.Count; i++)
                {
                    objResponseList.Add(objBooking[i].MapPassengersResponse());
                }
            }
            return objResponseList;
        }
        public static PassengerResponse MapPassengersResponse(this  Avantik.Web.Service.Message.Booking.Passenger objBookingResponse)
        {
            PassengerResponse objReadResponse = new PassengerResponse();

            if (objBookingResponse != null)
            {
                objReadResponse.passenger_id = objBookingResponse.PassengerId;
                //optional use only when want to defind guardian of passenger type CHD
                objReadResponse.guardian_passenger_id = objBookingResponse.GuardianPassengerId;

                objReadResponse.date_of_birth = objBookingResponse.DateOfBirth;


                objReadResponse.passenger_type_rcd = objBookingResponse.PassengerTypeRcd;
                objReadResponse.lastname = objBookingResponse.Lastname;
                objReadResponse.firstname = objBookingResponse.Firstname;
                objReadResponse.middlename = objBookingResponse.Middlename;
                objReadResponse.title_rcd = objBookingResponse.TitleRcd;
                objReadResponse.gender_type_rcd = objBookingResponse.GenderTypeRcd;
                objReadResponse.nationality_rcd = objBookingResponse.NationalityRcd;
                //Document
            }

            return objReadResponse;
        }
        public static IList<MappingResponse> MapMappingsResponse(this  IList<Avantik.Web.Service.Message.Booking.Mapping> objBooking)
        {
            IList<MappingResponse> objResponseList = new List<MappingResponse>();
            if (objBooking != null)
            {
                for (int i = 0; i < objBooking.Count; i++)
                {
                    objResponseList.Add(objBooking[i].MapMappingsResponse());
                }
            }
            return objResponseList;
        }
        public static MappingResponse MapMappingsResponse(this  Avantik.Web.Service.Message.Booking.Mapping objBookingResponse)
        {
            MappingResponse objReadResponse = new MappingResponse();

            if (objBookingResponse != null)
            {
                objReadResponse.passenger_id = objBookingResponse.PassengerId;
                objReadResponse.booking_segment_id = objBookingResponse.BookingSegmentId;

                objReadResponse.seat_number = objBookingResponse.SeatNumber;
                objReadResponse.currency_rcd = objBookingResponse.CurrencyRcd;


            }

            return objReadResponse;
        }
        public static IList<FeeResponse> MapFeesResponse(this  IList<Avantik.Web.Service.Message.Booking.Fee> objBooking)
        {
            IList<FeeResponse> objResponseList = new List<FeeResponse>();
            if (objBooking != null)
            {
                for (int i = 0; i < objBooking.Count; i++)
                {
                    objResponseList.Add(objBooking[i].MapFeesResponse());
                }
            }
            return objResponseList;
        }
        public static FeeResponse MapFeesResponse(this  Avantik.Web.Service.Message.Booking.Fee objBookingResponse)
        {
            FeeResponse objReadResponse = new FeeResponse();

            if (objBookingResponse != null)
            {
                objReadResponse.passenger_id = objBookingResponse.PassengerId;
                objReadResponse.booking_segment_id = objBookingResponse.BookingSegmentId;
             //   objReadResponse.passenger_segment_service_id = objBookingResponse.PassengerSegmentServiceId;

               objReadResponse.fee_id = objBookingResponse.BookingFeeId;

                objReadResponse.fee_rcd = objBookingResponse.FeeRcd;
              //  objReadResponse.vendor_rcd = objBookingResponse.VendorRcd;

                if (objBookingResponse.VendorRcd != null)
                {
                    objReadResponse.vendor_rcd = objBookingResponse.VendorRcd;
                }
                else
                {
                    objReadResponse.vendor_rcd =  string.Empty;
                }
                //  objReadResponse.od_origin_rcd = objBookingResponse.OdOriginRcd;
                //  objReadResponse.od_destination_rcd = objBookingResponse.OdDestinationRcd;
                objReadResponse.currency_rcd = objBookingResponse.CurrencyRcd;
                //  objReadResponse.charge_currency_rcd = objBookingResponse.ChargeCurrencyRcd;
                //  objReadResponse.unit_rcd = objBookingResponse.UnitRcd;
                // objReadResponse.mpd_number = objBookingResponse.MpdNumber;

                // objReadResponse.comment = objBookingResponse.ChangeComment;

                if (objBookingResponse.ChangeComment != null)
                {
                    objReadResponse.comment = objBookingResponse.ChangeComment;
                }
                else
                {
                    objReadResponse.comment = string.Empty;  
                }




                //  objReadResponse.external_reference = objBookingResponse.ExternalReference;

                objReadResponse.fee_amount = objBookingResponse.FeeAmount;
                objReadResponse.fee_amount_incl = objBookingResponse.FeeAmountIncl;
                objReadResponse.vat_percentage = objBookingResponse.VatPercentage;
                objReadResponse.charge_amount = objBookingResponse.ChargeAmount;
                objReadResponse.charge_amount_incl = objBookingResponse.ChargeAmountIncl;
                objReadResponse.number_of_units = objBookingResponse.NumberOfUnits;

            }

            return objReadResponse;
        }
        public static IList<RemarkResponse> MapRemarksResponse(this  IList<Avantik.Web.Service.Message.Booking.Remark> objBooking)
        {
            IList<RemarkResponse> objResponseList = new List<RemarkResponse>();
            if (objBooking != null)
            {
                for (int i = 0; i < objBooking.Count; i++)
                {
                    objResponseList.Add(objBooking[i].MapRemarksResponse());
                }
            }
            return objResponseList;
        }
        public static RemarkResponse MapRemarksResponse(this  Avantik.Web.Service.Message.Booking.Remark objBookingResponse)
        {
            RemarkResponse objReadResponse = new RemarkResponse();

            if (objBookingResponse != null)
            {

                objReadResponse.remark_type_rcd = objBookingResponse.RemarkTypeRcd;
                objReadResponse.remark_text = objBookingResponse.RemarkText;
                objReadResponse.nickname = objBookingResponse.Nickname;
            }

            return objReadResponse;
        }
        public static IList<ServiceResponse> MapServicesResponse(this  IList<Avantik.Web.Service.Message.Booking.PassengerService> objBooking)
        {
            IList<ServiceResponse> objResponseList = new List<ServiceResponse>();
            if (objBooking != null)
            {
                for (int i = 0; i < objBooking.Count; i++)
                {
                    objResponseList.Add(objBooking[i].MapServicesResponse());
                }
            }
            return objResponseList;
        }
        public static ServiceResponse MapServicesResponse(this  Avantik.Web.Service.Message.Booking.PassengerService objBookingResponse)
        {
            ServiceResponse objReadResponse = new ServiceResponse();

            if (objBookingResponse != null)
            {
              //  objReadResponse.passenger_segment_service_id = objBookingResponse.PassengerSegmentServiceId;
                objReadResponse.passenger_id = objBookingResponse.PassengerId;
                objReadResponse.booking_segment_id = objBookingResponse.BookingSegmentId;

                objReadResponse.number_of_units = objBookingResponse.NumberOfUnits;

                objReadResponse.special_service_rcd = objBookingResponse.SpecialServiceRcd;
                objReadResponse.service_text = objBookingResponse.ServiceText;
               objReadResponse.special_service_status_rcd = objBookingResponse.SpecialServiceStatusRcd;
              //  objReadResponse.unit_rcd = objBookingResponse.UnitRcd;

            }

            return objReadResponse;
        }
        public static IList<PaymentResponse> MapPaymentsResponse(this  IList<Avantik.Web.Service.Message.Booking.Payment> objBooking)
        {
            IList<PaymentResponse> objResponseList = new List<PaymentResponse>();
            if (objBooking != null)
            {
                for (int i = 0; i < objBooking.Count; i++)
                {
                    objResponseList.Add(objBooking[i].MapPaymentsResponse());
                }
            }
            return objResponseList;
        }
        public static PaymentResponse MapPaymentsResponse(this  Avantik.Web.Service.Message.Booking.Payment objBookingResponse)
        {
            PaymentResponse objReadResponse = new PaymentResponse();

            if (objBookingResponse != null)
            {
                objReadResponse.payment_date_time = objBookingResponse.PaymentDateTime;

                objReadResponse.payment_amount = objBookingResponse.PaymentAmount;
                objReadResponse.receive_payment_amount = objBookingResponse.ReceivePaymentAmount;

                //Value will be SALE or REFUND
                objReadResponse.payment_type_rcd = objBookingResponse.PaymentTypeRcd;
                objReadResponse.form_of_payment_rcd = objBookingResponse.FormOfPaymentRcd;
                objReadResponse.form_of_payment_subtype_rcd = objBookingResponse.FormOfPaymentSubtypeRcd;
                objReadResponse.debit_agency_code = objBookingResponse.DebitAgencyCode;
                objReadResponse.currency_rcd = objBookingResponse.CurrencyRcd;
                objReadResponse.receive_currency_rcd = objBookingResponse.ReceiveCurrencyRcd;
                objReadResponse.approval_code = objBookingResponse.ApprovalCode;
                objReadResponse.transaction_reference = objBookingResponse.TransactionReference;
                objReadResponse.payment_number = objBookingResponse.PaymentNumber;
                objReadResponse.payment_reference = objBookingResponse.PaymentReference;
                objReadResponse.payment_remark = objBookingResponse.PaymentRemark;

                // credit card
                objReadResponse.credit_card = new CreditCard();
                objReadResponse.credit_card.credit_card_number = objBookingResponse.DocumentNumber;
                objReadResponse.credit_card.name_on_card = objBookingResponse.NameOnCard;
                objReadResponse.credit_card.cvv_code = objBookingResponse.CvvCode;
                objReadResponse.credit_card.issue_number = objBookingResponse.IssueNumber;

                objReadResponse.credit_card.expiry_month = objBookingResponse.ExpiryMonth;
                objReadResponse.credit_card.expiry_year = objBookingResponse.ExpiryYear;
                objReadResponse.credit_card.issue_month = objBookingResponse.IssueMonth;
                objReadResponse.credit_card.issue_year = objBookingResponse.IssueYear;

                // address
                objReadResponse.credit_card.address_line1 = objBookingResponse.AddressLine1;
                objReadResponse.credit_card.address_line2 = objBookingResponse.AddressLine2;
                objReadResponse.credit_card.street = objBookingResponse.Street;
                objReadResponse.credit_card.po_box = objBookingResponse.PoBox;
                objReadResponse.credit_card.city = objBookingResponse.City;
                objReadResponse.credit_card.state = objBookingResponse.State;
                objReadResponse.credit_card.district = objBookingResponse.District;
                objReadResponse.credit_card.province = objBookingResponse.Province;
                objReadResponse.credit_card.zip_code = objBookingResponse.ZipCode;
            }
                
            return objReadResponse;
        }

        #endregion

    }
         

}
