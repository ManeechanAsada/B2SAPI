using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Avantik.Web.Service.Message.b2s.map;

namespace Avantik.Web.Service.Message.b2s.Validation
{
    public class Validation
    {
        private BookingHeaderResponse HeaderValidation(BookingHeader bookingHeader,
                                                IList<Passenger> passenger)
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
        private IList<FlightSegmentResponse> SegmentValidation(IList<FlightSegment> flightSegment,
                                                                IList<Passenger> passenger)
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
        private IList<PassengerResponse> PassengerValidation(IList<Passenger> passenger)
        {
            IList<PassengerResponse> objPassengerResponse = null;
            PassengerResponse passengerResponse = null;
            List<string> listStrName = new List<string>();
            string strName = string.Empty;

            int iAdult = 0;
            int iInf = 0;

            bool bError = false;
            for (int i = 0; i < passenger.Count; i++)
            {
                //check passenger duplicate
                strName = passenger[i].title_rcd + passenger[i].firstname + passenger[i].lastname;
                listStrName.Add(strName);
                if (listStrName.Distinct().Count() != listStrName.Count())
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
                    else if (passenger[i].passenger_type_rcd == "INF")
                    { }
                    else
                    {
                        bError = true;
                        passengerResponse = passenger[i].MapPassengerResponse();
                        passengerResponse.error_code = "143";
                        passengerResponse.error_message = "Invalid or Ineligible Passenger Type Code.";
                    }

                    //check number of adult = inf
                    if (passenger[i].passenger_type_rcd == "ADULT")
                    {
                        iAdult += 1;
                    }
                    else if (passenger[i].passenger_type_rcd == "INF")
                    {
                        iInf += 1;
                    }
                    if (iInf > iAdult)
                    {
                        bError = true;
                        passengerResponse = passenger[i].MapPassengerResponse();
                        passengerResponse.error_code = "324";
                        passengerResponse.error_message = "Number of infants exceed maximum allowed per adult passenger. Infant No- " + (i - 1);
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

        private IList<MappingResponse> MappingValidation(IList<Mapping> mapping)
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
                // End Validate

                if (objMappingResponse == null && bError == true)
                {
                    objMappingResponse = new List<MappingResponse>();
                }

                if (mappingResponse != null)
                {
                    objMappingResponse.Add(mappingResponse);
                }

                mappingResponse = null;
            }

            return objMappingResponse;
        }

        private IList<FeeResponse> FeeValidation(IList<Fee> fee)
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

        private IList<RemarkResponse> RemarkValidation(IList<Remark> remark)
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

        private IList<ServiceResponse> ServiceValidation(IList<Service> service)
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
                    else if (service[i].service_text.Length > 300)
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

        private IList<PaymentResponse> PaymentValidation(IList<Payment> payment)
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

    }
}
