using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avantik.Web.Service.Message.Booking;
using Avantik.Web.Service.BookingRead;

namespace Avantik.Web.Service.Extension
{
    public static class MapReadBooking
    {
        public static BookingHeader ToBookingMessage(this nl_booking_header header)
        {
            return new BookingHeader
            {
                BookingId = header.booking_id,
                BookingSourceRcd = header.booking_source_rcd,
                CurrencyRcd = header.currency_rcd,
                ClientProfileId = header.client_profile_id,
                BookingNumber = header.booking_number.HasValue ? header.booking_number.Value : 0,
                RecordLocator = header.record_locator,
                NumberOfAdults = header.number_of_adults,
                NumberOfChildren = header.number_of_children,
                NumberOfInfants = header.number_of_infants,
                LanguageRcd = header.language_rcd,
                AgencyCode = header.agency_code,
                ContactName = header.contact_name,
                ContactEmail = header.contact_email,
                PhoneMobile = header.phone_mobile,
                PhoneHome = header.phone_home,
                PhoneBusiness = header.phone_business,
                ReceivedFrom = header.received_from,
                PhoneFax = header.phone_fax,
                PhoneSearch = header.phone_search,
                Comment = header.comment,
                NotifyByEmailFlag = header.notify_by_email_flag,
                NotifyBySmsFlag = header.notify_by_sms_flag,
                GroupName = header.group_name,
                GroupBookingFlag = header.group_booking_flag,
                AgencyName = header.agency_name,
                OwnAgencyFlag = header.own_agency_flag,
                WebAgencyFlag = header.web_agency_flag,
                ClientNumber = header.client_number.HasValue ? header.client_number.Value : 0,
                Lastname = header.lastname,
                Firstname = header.firstname,
                City = header.city,
                CreateName = header.create_name,
                Street = header.street,
                LockDateTime = header.lock_date_time,
                Middlename = header.middlename,
                AddressLine1 = header.address_line1,
                AddressLine2 = header.address_line2,
                State = header.state,
                District = header.district,
                Province = header.province,
                ZipCode = header.zip_code,
                PoBox = header.po_box,
                CountryRcd = header.country_rcd,
                TitleRcd = header.title_rcd,
                ExternalPaymentReference = header.external_payment_reference,
                CreateBy = header.create_by,
                UpdateBy = header.update_by,
                CreateDateTime = header.create_date_time,
                UpdateDateTime = header.update_date_time,
                CostCenter = header.cost_center,
                PurchaseOrder = header.purchase_order,
                ProjectNumber = header.project_number,
                LockName = header.lock_name,
                IpAddress = header.ip_address,
                ApprovalFlag = header.approval_flag,
                InvoiceReceiver = header.invoice_receiver,
                TaxId = header.tax_id,
                NewsletterFlag = header.newsletter_flag,
                ContactEmailCc = header.contact_email_cc,
                MobileEmail = header.mobile_email,
                VendorRcd = header.vendor_rcd,
                BookingDateTime = header.create_date_time,
                NoVatFlag = header.no_vat_flag,
                CompanyName = header.company_name,
                BusinessFlag = header.business_flag
            };
        }

        public static IList<FlightSegment> ToBookingMessage(this IList<nl_booking_segment> objRestList)
        {
            List<FlightSegment> objMessageList = null;
            if (objRestList != null)
            {
                objMessageList = new List<FlightSegment>();
                for (int i = 0; i < objRestList.Count; i++)
                {
                    objMessageList.Add(objRestList[i].ToBookingMessage());
                }
            }
            return objMessageList;
        }

        public static FlightSegment ToBookingMessage(this nl_booking_segment segment)
        {
            if (segment == null) return null;

            var flightSegment = new FlightSegment
            {
                OriginRcd = segment.origin_rcd,
                DestinationRcd = segment.destination_rcd,
                OdOriginRcd = segment.od_origin_rcd,
                OdDestinationRcd = segment.od_destination_rcd,
                CreateBy = segment.create_by,
                UpdateBy = segment.update_by,
                CreateDateTime = segment.create_date_time,
                UpdateDateTime = segment.update_date_time,
                DepartureDate = segment.departure_date,
                ArrivalDate = segment.arrival_date,
                FlightId = segment.flight_id,
                ExchangedSegmentId = segment.flight_connection_id, // Assuming a mapping, modify as necessary
                AirlineRcd = segment.airline_rcd,
                FlightNumber = segment.flight_number,
                DepartureTime = segment.departure_time,
                ArrivalTime = segment.arrival_time,
                OriginName = segment.origin_name,
                DestinationName = segment.destination_name,
                FareId = Guid.NewGuid(), // Placeholder, set actual value
                BookingClassRcd = segment.booking_class_rcd,
                BoardingClassRcd = segment.boarding_class_rcd,
                AircraftTypeRcd = segment.aircraft_type_rcd,
                JourneyTime = segment.journey_time,
                BookingSegmentId = segment.booking_segment_id,
                SegmentStatusRcd = segment.segment_status_rcd,
                BookingId = segment.booking_id,
                NumberOfUnits = segment.number_of_units,
                SegmentChangeStatusRcd = segment.segment_change_status_rcd,
                InfoSegmentFlag = segment.info_segment_flag,
                HighPriorityWaitlistFlag = Convert.ToByte(segment.passive_segment_flag), // Assuming a mapping
                SegmentStatusName = segment.segment_status_name,
                SeatmapFlag = Convert.ToByte(segment.seatmap_flag),
                TempSeatmapFlag = Convert.ToByte(segment.temp_seatmap_flag),
                AllowWebCheckinFlag = Convert.ToByte(segment.allow_web_checkin_flag),
                CloseWebSalesFlag = Convert.ToInt16(segment.close_web_sales_flag),
                ExcludeQuoteFlag = 0, // Placeholder, set actual value
                CurrencyRate = 1.0, // Placeholder, set actual value
                OpenSequence = Convert.ToByte(segment.open_sequence),
                NumberOfStops = Convert.ToByte(segment.number_of_stops),
                //  FlightCheckInStatusRcd = segment.flight_check_in_status_rcd,
                // FlightStatusRCD = segment.flight_status_rcd,
                //  FlightFlownDateTime = segment.flight_flown_date_time
            };

            return flightSegment;
        }

        public static IList<Passenger> ToBookingMessage(this IList<nl_passenger> objRestList)
        {
            List<Passenger> objMessageList = null;
            if (objRestList != null)
            {
                objMessageList = new List<Passenger>();
                for (int i = 0; i < objRestList.Count; i++)
                {
                    objMessageList.Add(objRestList[i].ToBookingMessage());
                }
            }
            return objMessageList;
        }

        public static Passenger ToBookingMessage(this nl_passenger obj)
        {
            if (obj == null) return null;

            var passenger = new Passenger
            {
                TitleRcd = obj.title_rcd,
                Lastname = obj.lastname,
                Firstname = obj.firstname,
                Middlename = obj.middlename,
                NationalityRcd = obj.nationality_rcd,
                PassengerWeight = obj.passenger_weight,
                GenderTypeRcd = obj.gender_type_rcd,
                PassengerTypeRcd = obj.passenger_type_rcd,
                ClientProfileId = obj.passenger_profile_id,
                ClientNumber = obj.client_number,
                CreateBy = obj.create_by,
                CreateDateTime = obj.create_date_time,
                UpdateBy = obj.update_by,
                UpdateDateTime = obj.update_date_time,
                DateOfBirth = obj.date_of_birth,
                DocumentTypeRcd = obj.document_type_rcd,
                DocumentNumber = obj.passport_number,
                ResidenceCountryRcd = obj.residence_country_rcd,
                PassportNumber = obj.passport_number,
                PassportIssueDate = obj.passport_issue_date,
                PassportExpiryDate = obj.passport_expiry_date,
                PassportIssuePlace = obj.passport_issue_place,
                PassportBirthPlace = obj.passport_birth_place,
                PassportIssueCountryRcd = obj.passport_issue_country_rcd,
                CountryCodeLong = obj.country_code_long,
                PassengerId = obj.passenger_id,
                BookingId = obj.booking_id,
                PassengerProfileId = obj.passenger_profile_id,
                GuardianPassengerId = obj.guardian_passenger_id,
                RedressNumber = obj.redress_number,
                WheelchairFlag = Convert.ToByte(obj.wheelchair_flag),
                VipFlag = Convert.ToByte(obj.vip_flag),
                MemberNumber = obj.member_number,
                MemberAirlineRcd = obj.member_airline_rcd,
                MemberLevelRcd = obj.member_level_rcd
            };

            return passenger;
        }

        public static IList<Mapping> ToBookingMessage(this IList<nl_passenger_segment_mapping> objRestList)
        {
            List<Mapping> objMessageList = null;
            if (objRestList != null)
            {
                objMessageList = new List<Mapping>();
                for (int i = 0; i < objRestList.Count; i++)
                {
                    objMessageList.Add(objRestList[i].ToBookingMessage());
                }
            }
            return objMessageList;
        }

        public static Mapping ToBookingMessage(this nl_passenger_segment_mapping obj)
        {
            if (obj == null) return null;

            var mapping = new Mapping
            {
                OriginRcd = obj.origin_rcd,
                DestinationRcd = obj.destination_rcd,
                DisplayName = $"{obj.firstname} {obj.lastname}",
                CreateBy = obj.create_by,
                UpdateBy = obj.update_by,
                CreateDateTime = obj.create_date_time,
                UpdateDateTime = obj.update_date_time,
                CurrencyRcd = obj.currency_rcd,
                RoutesTot = 0, // Placeholder, set actual value if available
                RoutesAvl = 0, // Placeholder, set actual value if available
                RoutesB2c = 0, // Placeholder, set actual value if available
                RoutesB2b = 0, // Placeholder, set actual value if available
                RoutesB2s = 0, // Placeholder, set actual value if available
                RoutesApi = 0, // Placeholder, set actual value if available
                RoutesB2t = 0, // Placeholder, set actual value if available
                SegmentChangeFeeFlag = false, // Placeholder, set actual value if available
                TransitFlag = false, // Placeholder, set actual value if available
                DirectFlag = false, // Placeholder, set actual value if available
                AvlFlag = false, // Placeholder, set actual value if available
                B2cFlag = false, // Placeholder, set actual value if available
                B2bFlag = false, // Placeholder, set actual value if available
                B2tFlag = false, // Placeholder, set actual value if available
                DayRange = 0, // Placeholder, set actual value if available
                ShowRedressNumberFlag = false, // Placeholder, set actual value if available
                RequirePassengerTitleFlag = false, // Placeholder, set actual value if available
                RequirePassengerGenderFlag = false, // Placeholder, set actual value if available
                RequireDateOfBirthFlag = false, // Placeholder, set actual value if available
                RequireDocumentDetailsFlag = false, // Placeholder, set actual value if available
                RequirePassengerWeightFlag = false, // Placeholder, set actual value if available
                SpecialServiceFeeFlag = false, // Placeholder, set actual value if available
                ShowInsuranceOnWebFlag = false, // Placeholder, set actual value if available
                FlightId = obj.flight_id,
                ExchangedSegmentId = obj.exchanged_segment_id,
                AirlineRcd = obj.airline_rcd,
                FlightNumber = obj.flight_number,
                RefundableFlag = obj.refundable_flag,
                // GroupFlag = obj.group_flag,
                // NonRevenueFlag = obj.non_revenue_flag,
                EticketFlag = obj.e_ticket_flag,
                //  FareReduction = obj.fare_red
                DepartureDate = obj.departure_date,
                ArrivalDate = obj.arrival_date,
                OdOriginRcd = obj.od_origin_rcd,
                OdDestinationRcd = obj.od_destination_rcd,
                //  WaitlistFlag = obj.waitlist_flag,
                //  OverbookFlag = obj.overbook_flag,
                FlightConnectionId = obj.flight_connection_id,
                //  AdvancedPurchaseFlag = obj.advanced_purchase_flag,
                JourneyTime = obj.journey_time,
                DepartureTime = obj.departure_time,
                ArrivalTime = obj.arrival_time,
                //  TransitFlightId = obj.transit_flight_id,
                //  TransitFareId = obj.transit_fare_id,
                // TransitDepartureDate = obj.transit_departure_date,
                //  TransitArrivalDate = obj.transit_arrival_date,
                FareId = obj.fare_id,
                BookingClassRcd = obj.booking_class_rcd,
                BoardingClassRcd = obj.boarding_class_rcd,
                //   PlannedDepartureTime = obj.planned_departure_time,
                //  PlannedArrivalTime = obj.planned_arrival_time,
                //   DepartureDayofweek = obj.departure_dayofweek,
                //  ArrivalDayofweek = obj.arrival_dayofweek,
                //  DepartureMonth = obj.departure_month,
                BookingSegmentId = obj.booking_segment_id,
                BookingId = obj.booking_id,
                //   NumberOfUnits = obj.number_of_units,
                InfoSegmentFlag = obj.info_segment_flag,
                //   HighPriorityWaitlistFlag = obj.high_priority_waitlist_flag,
                //    SeatmapFlag = obj.seatmap_flag,
                //   TempSeatmapFlag = obj.temp_seatmap_flag,
                //   AllowWebCheckinFlag = obj.allow_web_checkin_flag,
                //   CloseWebSalesFlag = obj.close_web_sales_flag,
                //    ExcludeQuoteFlag = obj.exclude_quote_flag,
                //   CurrencyRate = obj.currency_rate,
                //   OpenSequence = obj.open_sequence,
                //   NumberOfStops = obj.number_of_stops,
                PassengerId = obj.passenger_id,
                Lastname = obj.lastname,
                Firstname = obj.firstname,
                GenderTypeRcd = obj.gender_type_rcd,
                TitleRcd = obj.title_rcd,
                PassengerTypeRcd = obj.passenger_type_rcd,
                DateOfBirth = obj.date_of_birth,
                PassengerStatusRcd = obj.passenger_status_rcd,
                Middlename = obj.middlename,
                //DocumentTypeRcd = obj.document_type_rcd,
                //PassportNumber = obj.passport_number,
                //PassportIssuePlace = obj.passport_issue_place,
                //PassportIssueDate = obj.passport_issue_date,
                //PassportExpiryDate = obj.passport_expiry_date,
                //Sendmail = obj.sendmail,
                //ClientNumber = obj.client_number,
                //PassengerProfileId = obj.passenger_profile_id,
                //ClientProfileId = obj.client_profile_id,
                //EmployeeNumber = obj.employee_number,
                //NationalityRcd = obj.nationality_rcd,
                ContactEmail = obj.contact_email,
                InventoryClassRcd = obj.inventory_class_rcd,
                SeatNumber = obj.seat_number,
                SeatRow = obj.seat_row,
                SeatColumn = obj.seat_column,
                NetTotal = obj.net_total,
                TaxAmount = obj.tax_amount,
                FareAmount = obj.fare_amount,
                YqAmount = obj.yq_amount,
                BaseTicketingFeeAmount = obj.base_ticketing_fee_amount,
                TicketingFeeAmount = obj.ticketing_fee_amount,
                ReservationFeeAmount = obj.reservation_fee_amount,
                CommissionAmount = obj.commission_amount,
                FareVat = obj.fare_vat,
                TaxVat = obj.tax_vat,
                YqVat = obj.yq_vat,
                TicketingFeeVat = obj.ticketing_fee_vat,
                ReservationFeeVat = obj.reservation_fee_vat,
                FareAmountIncl = obj.fare_amount_incl,
                TaxAmountIncl = obj.tax_amount_incl,
                YqAmountIncl = obj.yq_amount_incl,
                PublicFareAmountIncl = obj.public_fare_amount_incl,
                PublicFareAmount = obj.public_fare_amount,
                CommissionAmountIncl = obj.commission_amount_incl,
                TicketingFeeAmountIncl = obj.ticketing_fee_amount_incl,
                ReservationFeeAmountIncl = obj.reservation_fee_amount_incl,
                AcctNetTotal = obj.acct_net_total,
                AcctTaxAmount = obj.acct_tax_amount,
                AcctFareAmount = obj.acct_fare_amount,
                AcctYqAmount = obj.acct_yq_amount,
                AcctTicketingFeeAmount = obj.acct_ticketing_fee_amount,
                AcctReservationFeeAmount = obj.acct_reservation_fee_amount,
                AcctCommissionAmount = obj.acct_commission_amount,
                AcctFareAmountIncl = obj.acct_fare_amount_incl,
                AcctTaxAmountIncl = obj.acct_tax_amount_incl,
                AcctYqAmountIncl = obj.acct_yq_amount_incl,
                AcctCommissionAmountIncl = obj.acct_commission_amount_incl,
                AcctTicketingFeeAmountIncl = obj.acct_ticketing_fee_amount_incl,
                AcctReservationFeeAmountIncl = obj.acct_reservation_fee_amount_incl,
                FareCode = obj.fare_code,
                FareDateTime = obj.fare_date_time,
                ETicketFlag = obj.e_ticket_flag,
                StandbyFlag = obj.standby_flag,
                TransferableFareFlag = obj.transferable_fare_flag,
                AgencyCode = obj.agency_code,
                TicketNumber = obj.ticket_number,
                TicketingDateTime = obj.ticketing_date_time,
                TicketingBy = obj.ticketing_by,
                CheckInSequence = obj.check_in_sequence,
                GroupSequence = obj.group_sequence,
                UnloadBy = obj.unload_by,
                UnloadDateTime = obj.unload_date_time,
                NumberOfPieces = obj.number_of_pieces,
                BaggageWeight = obj.baggage_weight,
                ExcessBaggageWeight = obj.excess_baggage_weight,
                CheckInBaggageWeight = obj.check_in_baggage_weight,
                //     GroupBaggageWeight = obj.group_baggage_weight,
                //    ChangedBaggageWeight = obj.changed_baggage_weight,
                CheckInBy = obj.check_in_by,
                CancelBy = obj.cancel_by,
                //   CancelDateTime = obj.cancel_date_time,
                // RefundAgencyCode = obj.refund_agency_code,
                //    RefundBy = obj.refund_by,
                RefundCharge = obj.refund_charge,
                RefundDateTime = obj.refund_date_time,
                CommissionPercentage=  obj.commission_percentage,
                //    VoidBy = obj.void_by,
                VoidDateTime = obj.void_date_time,
                ExchangedDateTime = obj.exchanged_date_time,
                //    ExchangedSegmentId = obj.exchanged_segment_id,
                //   OnwardAirlineRcd = obj.onward_airline_rcd,
                //    OnwardFlightNumber = obj.onward_flight_number,
                //    OnwardDepartureDate = obj.onward_departure_date,
                //    OnwardDestinationRcd = obj.onward_destination_rcd,
                //    OnwardBookingClassRcd = obj.onward_booking_class_rcd,
                RestrictionText = obj.restriction_text,
                EndorsementText = obj.endorsement_text,
                //   FareLine = obj.fare_line,
                //   CheckInCode = obj.check_in_code,
                //  PresentFlag = obj.present_flag,
                ThroughFareFlag = obj.through_fare_flag,
                //   BspCheckedFlag = obj.bsp_checked_flag,
                //AdmRaisedFlag = obj.adm_raised_flag,
                //AdmDocument = obj.adm_document,
                //BspPeriod = obj.bsp_period,
                //BspMonth = obj.bsp_month,
                //BspYear = obj.bsp_year,
                //BspComment = obj.bsp_comment,
                AdvancedSeatingFlag = obj.advanced_seating_flag,
                // FareColumn = obj.fare_column,
                PieceAllowance = obj.piece_allowance,
                BoardingTime = obj.boarding_time,
                ItFareFlag = obj.it_fare_flag,
                //  ThroughFareFlag = obj.through_fare_flag,
                // PassengerCheckinStatusRcd = obj.passenger_check_in_status_rcd,
            };

            return mapping;
        }

        public static IList<Fee> ToBookingMessage(this IList<nl_booking_fee> objRestList)
        {
            List<Fee> objMessageList = null;
            if (objRestList != null)
            {
                objMessageList = new List<Fee>();
                for (int i = 0; i < objRestList.Count; i++)
                {
                    objMessageList.Add(objRestList[i].ToBookingMessage());
                }
            }
            return objMessageList;
        }

        public static Fee ToBookingMessage(this nl_booking_fee obj)
        {
            if (obj == null) return null;

            var fee = new Fee
            {
                BookingFeeId = obj.booking_fee_id,
                FeeAmount = obj.fee_amount,
                BookingId = obj.booking_id,
                PassengerId = obj.passenger_id,
                CurrencyRcd = obj.currency_rcd,
                AcctFeeAmount = obj.acct_fee_amount,
                FeeId = obj.fee_id,
                VatPercentage = obj.vat_percentage,
                FeeAmountIncl = obj.fee_amount_incl,
                AcctFeeAmountIncl = obj.acct_fee_amount_incl,
                FeeRcd = obj.fee_rcd,
                DisplayName = obj.display_name,
                AccountFeeBy = obj.account_fee_by,
                AccountFeeDateTime = obj.account_fee_date_time,
                VoidDateTime = obj.void_date_time,
                VoidBy = obj.void_by,
                PaymentAmount = obj.payment_amount,
                CreateBy = obj.create_by,
                CreateDateTime = obj.create_date_time,
                UpdateBy = obj.update_by,
                UpdateDateTime = obj.update_date_time,
                BookingSegmentId = obj.booking_segment_id,
                AgencyCode = obj.agency_code,
                PassengerSegmentServiceId = obj.passenger_segment_service_id,
                FeeCategoryRcd = obj.fee_category_rcd,
                OriginRcd = obj.origin_rcd,
                DestinationRcd = obj.destination_rcd,
                OdOriginRcd = obj.od_origin_rcd,
                OdDestinationRcd = obj.od_destination_rcd,
                NumberOfUnits = obj.number_of_units,
                TotalAmount = obj.charge_amount,
                TotalAmountIncl = obj.charge_amount_incl,
                ManualFeeFlag = Convert.ToByte(obj.units), // Assuming units are used as a flag here
                                                           //  OdFlag = Convert.ToByte(obj.od_flag),
                                                           // SkipFareAllowanceFlag = Convert.ToByte(obj.skip_fare_allowance_flag),
                                                           //FeeLevel = obj.fee_level,
                FeeCalculationRcd = obj.fee_calculation_rcd,
                // MinimumFeeAmountFlag = Convert.ToByte(obj.minimum_fee_amount_flag),
                // FeePercentage = obj.fee_percentage,
                ChangeComment = obj.change_comment,
                Comment = obj.comment,
                TotalFeeAmount = obj.agency_fee_amount,
                TotalFeeAmountIncl = obj.agency_fee_amount_incl,
                Units = obj.units.ToString(),
                ChargeCurrencyRcd = obj.charge_currency_rcd,
                ChargeAmount = obj.charge_amount,
                ChargeAmountIncl = obj.charge_amount_incl,
                DocumentNumber = obj.document_number,
                DocumentDateTime = obj.document_date_time,
                ExternalReference = obj.external_reference,
                VendorRcd = obj.vendor_rcd,
                BaggageFeeOptionId = Guid.Empty, // Placeholder, replace with actual ID
                NewRecord = false,
                SelectedFee = false,
                UnitRcd = "",
                WeightLbs = obj.weight_lbs,
                WeightKgs = obj.weight_kgs,
                MpdNumber = obj.mpd_number
            };

            return fee;
        }

        public static IList<PassengerService> ToBookingMessage(this IList<nl_passenger_segment_service> objRestList)
        {
            List<PassengerService> objMessageList = null;
            if (objRestList != null)
            {
                objMessageList = new List<PassengerService>();
                for (int i = 0; i < objRestList.Count; i++)
                {
                    objMessageList.Add(objRestList[i].ToBookingMessage());
                }
            }
            return objMessageList;
        }

        public static PassengerService ToBookingMessage(this nl_passenger_segment_service obj)
        {
            if (obj == null) return null;

            var service = new PassengerService
            {
                PassengerSegmentServiceId = obj.passenger_segment_service_id,
                PassengerId = obj.passenger_id,
                BookingSegmentId = obj.booking_segment_id,
                SpecialServiceStatusRcd = obj.special_service_status_rcd,
                SpecialServiceChangeStatusRcd = obj.special_service_change_status_rcd,
                SpecialServiceRcd = obj.special_service_rcd,
                ServiceText = obj.service_text,
                CreateBy = obj.create_by,
                CreateDateTime = obj.create_date_time,
                UpdateBy = obj.update_by,
                UpdateDateTime = obj.update_date_time,
                FlightId = obj.flight_id.ToString(),
                FeeId = obj.advise_passenger_segment_service_id.ToString(),
                NumberOfUnits = obj.number_of_units,
                OriginRcd = obj.origin_rcd,
                DestinationRcd = obj.destination_rcd,
                NewRecord = Convert.ToBoolean(obj.temp_map_service_flag),
                DisplayName = obj.passenger_name,  // Assuming passenger_name is the display name
                CutOffTime = 0,  // Placeholder, replace with actual cut off time if available
                StatusCode = "", // Placeholder, set actual status code if available
                HelpText = "", // Placeholder, set actual help text if available
                SpecialServiceGroupRcd = "", // Placeholder, set actual group RCD if available
                ServiceOnRequestFlag = Convert.ToByte(obj.temp_map_service_flag),
                TextAllowedFlag = Convert.ToByte(obj.temp_map_service_flag),  // Assuming use of temp_map_service_flag for these flags
                ManifestFlag = Convert.ToByte(obj.temp_map_service_flag),
                TextRequiredFlag = Convert.ToByte(obj.temp_map_service_flag),
                IncludePassengerNameFlag = Convert.ToByte(obj.temp_map_service_flag),
                IncludeFlightSegmentFlag = Convert.ToByte(obj.temp_map_service_flag),
                IncludeActionCodeFlag = Convert.ToByte(obj.temp_map_service_flag),
                IncludeNumberOfServiceFlag = Convert.ToByte(obj.temp_map_service_flag),
                IncludeCateringFlag = Convert.ToByte(obj.temp_map_service_flag),
                IncludePassengerAssistanceFlag = Convert.ToByte(obj.temp_map_service_flag),
                ServiceSupportedFlag = Convert.ToByte(obj.temp_map_service_flag),
                SendInterlineReplyFlag = Convert.ToByte(obj.temp_map_service_flag),
                InventoryControlFlag = Convert.ToByte(obj.temp_map_service_flag),
                UnitRcd = obj.unit_rcd
            };

            return service;
        }


        public static IList<Payment> ToBookingMessage(this IList<nl_booking_payment> objRestList)
        {
            List<Payment> objMessageList = null;
            if (objRestList != null)
            {
                objMessageList = new List<Payment>();
                for (int i = 0; i < objRestList.Count; i++)
                {
                    objMessageList.Add(objRestList[i].ToBookingMessage());
                }
            }
            return objMessageList;
        }

        public static Payment ToBookingMessage(this nl_booking_payment obj)
        {
            if (obj == null) return null;

            var payment = new Payment
            {
                BookingPaymentId = obj.booking_payment_id,
                BookingSegmentId = obj.booking_segment_id,
                BookingId = obj.booking_id,
                VoucherPaymentId = obj.voucher_payment_id,
                FormOfPaymentRcd = obj.form_of_payment_rcd,
                CurrencyRcd = obj.currency_rcd,
                ReceiveCurrencyRcd = obj.receive_currency_rcd,
                AgencyPaymentTypeRcd = obj.payment_type_rcd,
                AgencyCode = obj.agency_code,
                DebitAgencyCode = obj.debit_agency_code,
                PaymentAmount = obj.payment_amount,
                ReceivePaymentAmount = obj.receive_payment_amount,
                AcctPaymentAmount = obj.acct_payment_amount,
                PaymentBy = obj.payment_by,
                PaymentDateTime = obj.payment_date_time,
                PaymentDueDateTime = obj.payment_due_date_time,
                DocumentAmount = obj.document_amount,
                VoidBy = obj.void_by,
                ExpiryMonth = obj.expiry_month,
                ExpiryYear = obj.expiry_year,
                VoidDateTime = obj.void_date_time,
                RecordLocator = obj.booking_reference,
                CvvCode = obj.cvv_code,
                NameOnCard = obj.name_on_card,
                DocumentNumber = obj.document_number,
                FormOfPaymentSubtypeRcd = obj.form_of_payment_subtype_rcd,
                City = obj.city,
                State = obj.state,
                Street = obj.street,
                PoBox = "", // Placeholder, replace if available
                AddressLine1 = obj.address_line1,
                AddressLine2 = obj.address_line2,
                ZipCode = obj.zip_code,
                District = "", // Placeholder, replace if available
                Province = "", // Placeholder, replace if available
                CountryRcd = obj.country_rcd,
                CreateBy = obj.create_by,
                CreateDateTime = obj.create_date_time,
                UpdateBy = obj.update_by,
                UpdateDateTime = obj.update_date_time,
                PosIndicator = obj.pos_indicator,
                IssueMonth = obj.issue_month,
                IssueYear = obj.issue_year,
                IssueNumber = obj.issue_number,
                ClientProfileId = obj.client_profile_id,
                TransactionReference = obj.transaction_reference,
                ApprovalCode = obj.approval_code,
                BankName = obj.bank_name,
                BankCode = obj.bank_code,
                BankIban = obj.bank_iban,
                IpAddress = obj.ip_address,
                PaymentReference = obj.payment_reference,
                AllocatedAmount = obj.allocated_amount,
                PaymentNumber =obj.payment_number,
                PaymentTypeRcd = obj.payment_type_rcd
            };

            return payment;
        }

        public static IList<Remark> ToBookingMessage(this IList<nl_booking_remark> objRestList)
        {
            List<Remark> objMessageList = null;
            if (objRestList != null)
            {
                objMessageList = new List<Remark>();
                for (int i = 0; i < objRestList.Count; i++)
                {
                    objMessageList.Add(objRestList[i].ToBookingMessage());
                }
            }
            return objMessageList;
        }

        public static Remark ToBookingMessage(this nl_booking_remark obj)
        {
            if (obj == null) return null;

            var remark = new Remark
            {
                BookingRemarkId = obj.booking_remark_id,
                RemarkTypeRcd = obj.remark_type_rcd,
                TimelimitDateTime = obj.timelimit_date_time,
                CompleteFlag = obj.complete_flag,
                RemarkText = obj.remark_text,
                BookingId = obj.booking_id,
                AddedBy = obj.added_by,
                ClientProfileId = obj.client_profile_id,
                AgencyCode = obj.agency_code,
                CreateBy = obj.create_by,
                CreateDateTime = obj.create_date_time,
                UpdateBy = obj.update_by,
                UpdateDateTime = obj.update_date_time,
                SystemFlag = obj.system_flag,
                UtcTimelimitDateTime = obj.utc_timelimit_date_time,
                Nickname = obj.nickname,
                ProtectedFlag = obj.protected_flag,
                WarningFlag = obj.warning_flag,
                ProcessMessageFlag = obj.process_message_flag
            };

            return remark;
        }

        public static IList<Tax> ToBookingMessage(this IList<nl_passenger_segment_tax> objRestList)
        {
            List<Tax> objMessageList = null;
            if (objRestList != null)
            {
                objMessageList = new List<Tax>();
                for (int i = 0; i < objRestList.Count; i++)
                {
                    objMessageList.Add(objRestList[i].ToBookingMessage());
                }
            }
            return objMessageList;
        }

        public static Tax ToBookingMessage(this nl_passenger_segment_tax obj)
        {
            if (obj == null) return null;

            var tax = new Tax
            {
                PassengerSegmentTaxId = obj.passenger_segment_tax_id,
                TaxAmount = obj.tax_amount,
                TaxAmountIncl = obj.tax_amount_incl,
                AcctAmount = obj.acct_amount,
                AcctAmountIncl = obj.acct_amount_incl,
                SalesAmount = obj.sales_amount,
                SalesAmountIncl = obj.sales_amount_incl,
                TaxRcd = obj.tax_rcd,
                PassengerId = obj.passenger_id,
                TaxId = obj.tax_id,
                BookingSegmentId = obj.booking_segment_id,
                TaxCurrencyRcd = obj.tax_currency_rcd,
                SalesCurrencyRcd = obj.sales_currency_rcd,
                DisplayName = obj.display_name,
                SummarizeUp = obj.summarize_up,
                CoverageType = obj.coverage_type,
                CreateBy = obj.create_by,
                UpdateBy = obj.update_by,
                CreateDateTime = obj.create_date_time,
                UpdateDateTime = obj.update_date_time,
                VatPercentage = obj.vat_percentage
            };

            return tax;
        }

        public static IList<Quote> ToBookingMessage(this IList<SummarizeQuote> objRestList)
        {
            List<Quote> objMessageList = null;
            if (objRestList != null)
            {
                objMessageList = new List<Quote>();
                for (int i = 0; i < objRestList.Count; i++)
                {
                    objMessageList.Add(objRestList[i].ToBookingMessage());
                }
            }
            return objMessageList;
        }

        public static Quote ToBookingMessage(this SummarizeQuote obj)
        {
            if (obj == null) return null;

            var q = new Quote
            {
                BookingSegmentId = obj.booking_segment_id,
                PassengerTypeRcd = obj.passenger_type_rcd,
                CurrencyRcd = obj.currency_rcd,
                ChargeType = obj.charge_type,
                ChargeName = obj.charge_name,
                ChargeAmount = obj.charge_amount,
                ChargeAmountIncl = obj.charge_amount_incl,
                TotalAmount = obj.total_amount,
                TaxAmount = obj.tax_amount,
                PassengerCount = obj.passenger_count,
                SortSequence = obj.sort_sequence,
                RedemptionPoints = obj.redemption_points
            };

            return q;
        }



    }
}
