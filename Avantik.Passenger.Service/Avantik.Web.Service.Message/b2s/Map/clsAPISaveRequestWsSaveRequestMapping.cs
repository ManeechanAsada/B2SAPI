using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Avantik.Web.Service;
using Avantik.Web.Service.Message.Booking;
using b2s = Avantik.Web.Service.Message.b2s;

namespace Avantik.Web.Service.Message.b2s.map
{
    public static class APISaveRequestWsSaveRequestMapping
    {
        public static Avantik.Web.Service.Message.Booking.BookingHeader FillObjectB2sRequest(this  b2s.BookingHeader header, Guid bookingId,string agencyCode,Guid userId,short a,short c, short inf)
        {
            Avantik.Web.Service.Message.Booking.BookingHeader bookingheader = null;

            if (header != null)
            {
                bookingheader =new Avantik.Web.Service.Message.Booking.BookingHeader();

                bookingheader.BookingId = bookingId;

                if (header.agency_code == null || header.agency_code.ToString() == string.Empty)
                {
                    bookingheader.AgencyCode = agencyCode;
                }
                else
                {
                    bookingheader.AgencyCode = header.agency_code;
                }

                bookingheader.CurrencyRcd = header.currency_rcd;

                bookingheader.LanguageRcd = header.language_rcd;
                bookingheader.ContactName = header.contact_name;
                bookingheader.ContactEmail = header.contact_email;
                bookingheader.ReceivedFrom = header.received_from;
                bookingheader.PhoneMobile = header.mobile_number;
                bookingheader.PhoneHome = header.home_number;
                bookingheader.PhoneBusiness = header.business_number;

                bookingheader.Comment = header.comment;
                bookingheader.TitleRcd = header.title_rcd;
                bookingheader.Lastname = header.lastname;
                bookingheader.Firstname = header.firstname;
                bookingheader.Middlename = header.middlename;
                bookingheader.CountryRcd = header.country_rcd;

                bookingheader.CreateBy = userId;
                bookingheader.CreateDateTime=   DateTime.Now;
                bookingheader.UpdateBy = userId;
                bookingheader.UpdateDateTime = DateTime.Now;

                //b2s
                if(string.IsNullOrEmpty(header.vendor_rcd))
                    bookingheader.VendorRcd = "EDW";
                else
                    bookingheader.VendorRcd = header.vendor_rcd;

                bookingheader.BookingSourceRcd = "B2S";


                bookingheader.NumberOfAdults = a;
                bookingheader.NumberOfChildren = c;
                bookingheader.NumberOfInfants = inf;

                // end b2s

                    //bookingheader.AddressLine1 = header.address_line1;
                    //bookingheader.AddressLine2 = header.address_line2;
                    //bookingheader.Street = header.street;
                    //bookingheader.PoBox = header.po_box;
                    //bookingheader.City = header.city;
                    //bookingheader.State = header.state;
                    //bookingheader.District = header.district;
                    //bookingheader.Province = header.province;
                    //bookingheader.ZipCode = header.zip_code;
                    //bookingheader.CountryRcd = header.country_rcd;
                    //bookingheader.PhoneMobile = header.phone_mobile;
                    //bookingheader.PhoneHome = header.phone_home;
                    //bookingheader.PhoneBusiness = header.phone_business;
                    //bookingheader.PhoneFax = header.phone_fax;
            }

            return bookingheader;
        }

        public static IList<Avantik.Web.Service.Message.Booking.FlightSegment> FillObjectB2sRequest(this  IList<FlightSegment> objMessage, Guid bookingId, Guid userId, short passengerCount)
        {
            IList<Avantik.Web.Service.Message.Booking.FlightSegment> segmentList = null;
            if (objMessage != null)
            {
                segmentList = new List<Avantik.Web.Service.Message.Booking.FlightSegment>();
                for (int i = 0; i < objMessage.Count; i++)
                {
                    segmentList.Add(objMessage[i].FillObjectB2sRequest(bookingId, userId, passengerCount));
                }
            }
            return segmentList;
        }

        public static Avantik.Web.Service.Message.Booking.FlightSegment FillObjectB2sRequest(this  FlightSegment segment, Guid bookingId, Guid userId, short passengerCount)
        {
            Avantik.Web.Service.Message.Booking.FlightSegment flightSegment = null;

            if (segment != null)
            {
                flightSegment = new Avantik.Web.Service.Message.Booking.FlightSegment();
                flightSegment.BookingId = bookingId;

                flightSegment.BookingSegmentId =segment.booking_segment_id;
                flightSegment.FlightConnectionId = Guid.Empty;

                flightSegment.DepartureDate = segment.departure_date;
                flightSegment.DepartureTime = segment.departure_time;

                flightSegment.ArrivalTime = segment.arrival_time;
               // flightSegment.DepartureTime = segment.departure_time;


                flightSegment.AirlineRcd = segment.airline_rcd;
                flightSegment.FlightNumber = segment.flight_number;
                flightSegment.OriginRcd = segment.origin_rcd;
                flightSegment.DestinationRcd = segment.destination_rcd;
                flightSegment.OdOriginRcd = segment.origin_rcd;
                flightSegment.OdDestinationRcd = segment.destination_rcd;
                flightSegment.BookingClassRcd = segment.booking_class_rcd;
                flightSegment.BoardingClassRcd = segment.boarding_class_rcd;

                flightSegment.FlightId = segment.flight_id;

                //b2s
                flightSegment.SegmentStatusRcd = "PO";

                //Use for passenger count in the flight.
                flightSegment.NumberOfUnits = passengerCount;

                flightSegment.CreateBy = userId;
                flightSegment.CreateDateTime = DateTime.Now;
                flightSegment.UpdateBy = userId;
                flightSegment.UpdateDateTime = DateTime.Now;


            }

            return flightSegment;
        }

        public static IList<Avantik.Web.Service.Message.Booking.Passenger> FillObjectB2sRequest(this  IList<Passenger> objMessage, Guid bookingId, Guid userId)
        {
            IList<Avantik.Web.Service.Message.Booking.Passenger> passengertList = null;
            if (objMessage != null)
            {
                passengertList = new List<Avantik.Web.Service.Message.Booking.Passenger>();
                for (int i = 0; i < objMessage.Count; i++)
                {
                    passengertList.Add(objMessage[i].FillObjectB2sRequest(bookingId, userId));
                }
            }
            return passengertList;
        }

        public static Avantik.Web.Service.Message.Booking.Passenger FillObjectB2sRequest(this  Passenger p, Guid bookingId, Guid userId)
        {
            Avantik.Web.Service.Message.Booking.Passenger passenger = null;

            if (p != null)
            {
                passenger = new Avantik.Web.Service.Message.Booking.Passenger();
                passenger.BookingId = bookingId;

                passenger.PassengerId = p.passenger_id;
                passenger.DateOfBirth = p.date_of_birth;

                passenger.GuardianPassengerId = p.guardian_passenger_id;
                passenger.PassengerTypeRcd = p.passenger_type_rcd;
                passenger.Lastname = p.lastname;
                passenger.Firstname = p.firstname;
                passenger.Middlename = p.middlename;
                passenger.TitleRcd = p.title_rcd;
                passenger.GenderTypeRcd = p.gender_type_rcd;
                passenger.NationalityRcd = p.nationality_rcd;
                passenger.CreateBy = userId;
                passenger.CreateDateTime = DateTime.Now;
                passenger.UpdateBy = userId;
                passenger.UpdateDateTime = DateTime.Now;

            }

            return passenger;
        }

        public static IList<Avantik.Web.Service.Message.Booking.Fee> FillObjectB2sRequest(this  IList<Fee> objMessage, Guid bookingId, Guid userId, string agencyCode)
        {
            IList<Avantik.Web.Service.Message.Booking.Fee> feeList = null;
            if (objMessage != null)
            {
                feeList = new List<Avantik.Web.Service.Message.Booking.Fee>();
                for (int i = 0; i < objMessage.Count; i++)
                {
                    feeList.Add(objMessage[i].FillObjectB2sRequest(bookingId, userId, agencyCode));
                }
            }
            return feeList;
        }
        
        public static Avantik.Web.Service.Message.Booking.Fee FillObjectB2sRequest(this  Fee f, Guid bookingId, Guid userId, string agencyCode)
        {
            Avantik.Web.Service.Message.Booking.Fee fee = null;

            if (f != null)
            {
                fee = new Avantik.Web.Service.Message.Booking.Fee();

                fee.BookingId = bookingId;

                fee.PassengerId = f.passenger_id;
                fee.BookingSegmentId = f.booking_segment_id;

                // auto retrive and add by Avantik 
                // if not set map route and fee  can not retrive it
                // Remove assign FeeId
                //fee.FeeId = f.fee_id;

                //if (f.fee_id != Guid.Empty)
                //    fee.BookingFeeId = f.fee_id;
                //else
                fee.BookingFeeId = Guid.NewGuid();

                fee.FeeRcd = f.fee_rcd;
                fee.VendorRcd = f.vendor_rcd;
                fee.OdOriginRcd = f.origin_rcd;
                fee.OdDestinationRcd = f.destination_rcd;
                fee.CurrencyRcd = f.currency_rcd;
                fee.ChargeCurrencyRcd = f.currency_rcd;
               // fee.UnitRcd = f.unit_rcd;
               // fee.MpdNumber = f.mpd_number;
                fee.ChangeComment = f.comment;
               // fee.ExternalReference = f.external_reference;

                fee.FeeAmount = f.fee_amount;
                fee.FeeAmountIncl = f.fee_amount_incl;
                fee.VatPercentage = f.vat_percentage;
                fee.ChargeAmount = f.charge_amount;
                fee.ChargeAmountIncl = f.charge_amount_incl;
               // fee.WeightLbs = f.weight_lbs;
               // fee.WeightKgs = f.weight_kgs;
                fee.NumberOfUnits = f.number_of_units;

                fee.CreateBy = userId;
                fee.CreateDateTime = DateTime.Now;
                fee.UpdateBy = userId;
                fee.UpdateDateTime = DateTime.Now;

                fee.AgencyCode = agencyCode;

            }

            return fee;
        }

        public static IList<Avantik.Web.Service.Message.Booking.Remark> FillObjectB2sRequest(this  IList<Remark> objMessage, Guid bookingId, Guid userId)
        {
            IList<Avantik.Web.Service.Message.Booking.Remark> remarkList = null;
            if (objMessage != null)
            {
                remarkList = new List<Avantik.Web.Service.Message.Booking.Remark>();
                for (int i = 0; i < objMessage.Count; i++)
                {
                    remarkList.Add(objMessage[i].FillObjectB2sRequest(bookingId, userId));
                }
            }
            return remarkList;
        }
       
        public static Avantik.Web.Service.Message.Booking.Remark FillObjectB2sRequest(this  Remark re, Guid bookingId, Guid userId)
        {
            Avantik.Web.Service.Message.Booking.Remark remark = null;

            if (re != null)
            {
                remark = new Avantik.Web.Service.Message.Booking.Remark();

                remark.BookingId = bookingId;

               // remark.TimelimitDateTime = re.timelimit_date_time;

                remark.RemarkTypeRcd = re.remark_type_rcd;
                remark.RemarkText = re.remark_text;
                remark.Nickname = re.nickname;

                remark.CreateBy = userId;
                remark.CreateDateTime = DateTime.Now;
                remark.UpdateBy = userId;
                remark.UpdateDateTime = DateTime.Now;

            }

            return remark;
        }

        public static IList<Avantik.Web.Service.Message.Booking.PassengerService> FillObjectB2sRequest(this  IList<Service> objMessage, Guid userId)
        {
            IList<Avantik.Web.Service.Message.Booking.PassengerService> serviceList = null;
            if (objMessage != null)
            {
                serviceList = new List<Avantik.Web.Service.Message.Booking.PassengerService>();
                for (int i = 0; i < objMessage.Count; i++)
                {
                    serviceList.Add(objMessage[i].FillObjectB2sRequest(userId));
                }
            }
            return serviceList;
        }
       
        public static Avantik.Web.Service.Message.Booking.PassengerService FillObjectB2sRequest(this  Service ser, Guid userId)
        {
            Avantik.Web.Service.Message.Booking.PassengerService service = null;

            if (ser != null)
            {
                service = new Avantik.Web.Service.Message.Booking.PassengerService();

                // gen new guid
               // service.PassengerSegmentServiceId = Guid.NewGuid();
                service.PassengerId = ser.passenger_id;
                service.BookingSegmentId = ser.booking_segment_id;

                service.NumberOfUnits = ser.number_of_units;

                service.SpecialServiceRcd = ser.special_service_rcd;

                if (!string.IsNullOrEmpty(ser.service_text))
                    service.ServiceText = ser.service_text;
                //The value should be SS or NN
                //service.SpecialServiceStatusRcd = "RQ";
               // service.UnitRcd = ser.unit_rcd;

                service.CreateBy = userId;
                service.CreateDateTime = DateTime.Now;
                service.UpdateBy = userId;
                service.UpdateDateTime = DateTime.Now;

            }

            return service;
        }

        public static IList<Avantik.Web.Service.Message.Booking.Payment> FillObjectB2sRequest(this  IList<Payment> objMessage, Guid bookingId, string agencyCode, Guid userId)
        {
            IList<Avantik.Web.Service.Message.Booking.Payment> paymentList = null;
            if (objMessage != null)
            {
                paymentList = new List<Avantik.Web.Service.Message.Booking.Payment>();
                for (int i = 0; i < objMessage.Count; i++)
                {
                    paymentList.Add(objMessage[i].FillObjectB2sRequest(bookingId, agencyCode, userId));
                }
            }
            return paymentList;
        }
        
        public static Avantik.Web.Service.Message.Booking.Payment FillObjectB2sRequest(this  Payment pay, Guid bookingId, string agencyCode, Guid userId)
        {
            Avantik.Web.Service.Message.Booking.Payment payment = null;

            if (pay != null)
            {
                payment = new Avantik.Web.Service.Message.Booking.Payment();

                payment.BookingId = bookingId;
                //If not passin use server time.
                payment.PaymentDateTime = pay.payment_date_time;

                //Electronic fund transfer information.

                //Credit card information.

                payment.PaymentAmount = pay.payment_amount;
                payment.ReceivePaymentAmount = pay.receive_payment_amount;

                //Value will be SALE or REFUND
                payment.PaymentTypeRcd = pay.payment_type_rcd;
                payment.FormOfPaymentRcd = pay.form_of_payment_rcd;
                payment.FormOfPaymentSubtypeRcd = pay.form_of_payment_subtype_rcd;
                payment.AgencyCode = agencyCode;
                payment.DebitAgencyCode = pay.debit_agency_code;
                payment.CurrencyRcd = pay.currency_rcd;
                payment.ReceiveCurrencyRcd = pay.receive_currency_rcd;
                payment.ApprovalCode = pay.approval_code;
                payment.TransactionReference = pay.transaction_reference;
                payment.PaymentNumber = pay.payment_number;
                payment.PaymentReference = pay.payment_reference;
                payment.PaymentRemark = pay.payment_remark;

                // credit card
                if (pay.credit_card != null)
                {
                    payment.DocumentNumber = pay.credit_card.credit_card_number;
                    payment.NameOnCard = pay.credit_card.name_on_card;
                    payment.CvvCode = pay.credit_card.cvv_code;
                    payment.IssueNumber = pay.credit_card.issue_number;

                    payment.ExpiryMonth = pay.credit_card.expiry_month;
                    payment.ExpiryYear = pay.credit_card.expiry_year;
                    payment.IssueMonth = pay.credit_card.issue_month;
                    payment.IssueYear = pay.credit_card.issue_year;
                }

                payment.CreateBy = userId;
                payment.CreateDateTime = DateTime.Now;
                payment.UpdateBy = userId;
                payment.UpdateDateTime = DateTime.Now;

                payment.PaymentBy = userId;

            }

            return payment;
        }

        // Mapping
        public static IList<Avantik.Web.Service.Message.Booking.Mapping> FillObjectB2sRequest(this  IList<Mapping> objMessage, Guid booking_id, Guid userId, string agencyCode)
        {
            IList<Avantik.Web.Service.Message.Booking.Mapping> mappingtList = null;
            if (objMessage != null)
            {
                mappingtList = new List<Avantik.Web.Service.Message.Booking.Mapping>();
                for (int i = 0; i < objMessage.Count; i++)
                {
                    mappingtList.Add(objMessage[i].FillObjectB2sRequest(booking_id, userId, agencyCode));
                }
            }
            return mappingtList;
        }
       
        public static Avantik.Web.Service.Message.Booking.Mapping FillObjectB2sRequest(this  Mapping m, Guid booking_id, Guid userId, string agencyCode)
        {
            Avantik.Web.Service.Message.Booking.Mapping mapping = null;

            if (m != null)
            {
                mapping = new Avantik.Web.Service.Message.Booking.Mapping();

                mapping.AgencyCode = agencyCode;
                mapping.BookingId = booking_id;
                mapping.BookingSegmentId = m.booking_segment_id;
                mapping.PassengerId = m.passenger_id;

               // mapping.NotValidBeforeDate = m.not_valid_before_date;
              //  mapping.NotValidAfterDate = m.not_valid_after_date;

             //   mapping.RefundWithChargeHours = m.refund_with_charge_hours;
             //   mapping.RefundNotPossibleHours = m.refund_not_possible_hours;

            //    mapping.PieceAllowance = m.piece_allowance;

                //mapping.FareCode = m.fare_code;

                if (string.IsNullOrEmpty(m.seat_number) == false && m.seat_number.Length > 1)
                {
                    mapping.SeatColumn = m.seat_number.Substring(m.seat_number.Length - 1, 1);
                    mapping.SeatRow = Convert.ToInt16(m.seat_number.Substring(0, m.seat_number.Length - 1));
                }

                mapping.SeatNumber = m.seat_number;
                mapping.CurrencyRcd = m.currency_rcd;
              //  mapping.EndorsementText = m.endorsement_text;
              //  mapping.RestrictionText = m.restriction_text;

                //mapping.FareAmount = m.fare_amount;
                //mapping.FareAmountIncl = m.fare_amount_incl;
                //mapping.FareVat = m.fare_vat;
              //  mapping.NetTotal = m.net_total;
                //Optinal
              //  mapping.CommissionAmount = m.commission_amount;
                //Optinal
              //  mapping.CommissionAmountIncl = m.commission_amount_incl;
                //Optinal
             //   mapping.CommissionPercentage = m.commission_percentage;
                //Optinal
             //   mapping.PublicFareAmount = m.public_fare_amount;
                //Optinal
            //    mapping.PublicFareAmountIncl = m.public_fare_amount_incl;
            //    mapping.RefundCharge = m.refund_charge;
             //   mapping.BaggageWeight = m.baggage_weight;
            //    mapping.RedemptionPoints = m.redemption_points;

               // mapping.RefundableFlag = m.refundable_flag;
              //  mapping.TransferableFareFlag = m.transferable_fare_flag;
              //  mapping.ThroughFareFlag = m.through_fare_flag;
              //  mapping.ItFareFlag = m.it_fare_flag;
               // mapping.DutyTravelFlag = m.duty_travel_flag;
             //   mapping.StandbyFlag = m.standby_flag;
                mapping.ExcludePricingFlag = 1;

                mapping.CreateBy = userId;
                mapping.CreateDateTime = DateTime.Now;
                mapping.UpdateBy = userId;
                mapping.UpdateDateTime = DateTime.Now;

                //b2s
                mapping.ETicketFlag = 1;
                mapping.ExcludePricingFlag = 1;
                mapping.PassengerStatusRcd = "PO";
            }

            return mapping;
        }

        public static IList<Avantik.Web.Service.Message.Booking.Mapping> FillObjectB2sRequest(this  IList<Passenger> passtList, IList<Avantik.Web.Service.Message.Booking.Mapping> m)
        {

            if (passtList != null && m != null)
            {
                for (int i = 0; i < passtList.Count; i++)
                {
                    for (int j = 0; j < m.Count; j++)
                    {
                        if (passtList[i].passenger_id == m[j].PassengerId)
                            passtList[i].FillObjectB2sRequest(m[j]);
                    }
                }
            }
            return m;
        }
        
        public static Avantik.Web.Service.Message.Booking.Mapping FillObjectB2sRequest(this  Passenger passenger, Avantik.Web.Service.Message.Booking.Mapping mapping)
        {
            if (passenger != null && mapping != null)
            {
                mapping.Lastname = passenger.lastname;
                mapping.Firstname = passenger.firstname;
                mapping.PassengerTypeRcd = passenger.passenger_type_rcd;
                mapping.TitleRcd = passenger.title_rcd;
                mapping.GenderTypeRcd = passenger.gender_type_rcd;
                mapping.DateOfBirth = passenger.date_of_birth;
            }

            return mapping;
        }

        public static IList<Avantik.Web.Service.Message.Booking.Mapping> FillObjectB2sRequest(this  IList<FlightSegment> segList, IList<Avantik.Web.Service.Message.Booking.Mapping> m)
        {

            if (segList != null && m != null)
            {
                for (int i = 0; i < segList.Count; i++)
                {
                    for (int j = 0; j < m.Count; j++)
                    {
                        if (segList[i].booking_segment_id == m[j].BookingSegmentId)
                            segList[i].FillObjectB2sRequest(m[j]);
                    }
                }
            }
            return m;
        }
      
        public static Avantik.Web.Service.Message.Booking.Mapping FillObjectB2sRequest(this  FlightSegment segment, Avantik.Web.Service.Message.Booking.Mapping mapping)
        {
            if (segment != null && mapping != null)
            {
                mapping.AirlineRcd = segment.airline_rcd;
                mapping.FlightNumber = segment.flight_number;
                mapping.DepartureDate = segment.departure_date;
                mapping.BoardingClassRcd = segment.boarding_class_rcd;
                mapping.BookingClassRcd = segment.booking_class_rcd;
                mapping.OriginRcd = segment.origin_rcd;
                mapping.DestinationRcd = segment.destination_rcd;
                mapping.FlightId = segment.flight_id;
                mapping.InventoryClassRcd = segment.booking_class_rcd;

                mapping.FlightId = segment.flight_id;
            }

            return mapping;
        }



        public static IList<Avantik.Web.Service.Message.Booking.FlightSegment> FillObjectB2sRequest(this  IList<BookingSegment> objMessage)
        {
            IList<Avantik.Web.Service.Message.Booking.FlightSegment> segmentList = null;
            if (objMessage != null)
            {
                segmentList = new List<Avantik.Web.Service.Message.Booking.FlightSegment>();
                for (int i = 0; i < objMessage.Count; i++)
                {
                    segmentList.Add(objMessage[i].FillObjectB2sRequest());
                }
            }
            return segmentList;
        }

        public static Avantik.Web.Service.Message.Booking.FlightSegment FillObjectB2sRequest(this  BookingSegment segment)
        {
            Avantik.Web.Service.Message.Booking.FlightSegment flightSegment = null;

            if (segment != null)
            {
                flightSegment = new Avantik.Web.Service.Message.Booking.FlightSegment();

                flightSegment.DepartureDate = segment.departure_date;
               // flightSegment.DepartureTime = segment.departure_time;

                //flightSegment.ArrivalTime = segment.arrival_time;

                flightSegment.AirlineRcd = segment.airline_rcd;
                flightSegment.FlightNumber = segment.flight_number;
                flightSegment.OriginRcd = segment.origin_rcd;
                flightSegment.DestinationRcd = segment.destination_rcd;
                flightSegment.OdOriginRcd = segment.origin_rcd;
                flightSegment.OdDestinationRcd = segment.destination_rcd;
                flightSegment.BookingClassRcd = segment.booking_class_rcd;
                //flightSegment.BoardingClassRcd = segment.boarding_class_rcd;

            }

            return flightSegment;
        }



    }
}
