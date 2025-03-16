using Avantik.Web.Service.Model.Contract;
using System;
using System.Collections.Generic;

using Avantik.Web.Service.COMHelper;
using System.Runtime.InteropServices;

using Avantik.Web.Service.Entity;
using Avantik.Web.Service.Entity.Booking;

using Avantik.Web.Service.Helpers;
using System.Data.SqlClient;
using System.Data;

namespace Avantik.Web.Service.Model.COM
{
    public class FeeService : RunComplus, IFeeService
    {
        string _server = string.Empty;
        public FeeService(string server, string user, string pass, string domain)
            :base(user,pass,domain)
        {
            _server = server;
        }

     

        public ServiceFee SpecialServiceFee(string agencyCode,string currencyCode,string service,string origin,string destination,string flight)
        {
            ServiceFee specialServices = null;
            string strSQLConnectionString = ConfigHelper.ToString("SQLConnectionString");

            using (SqlConnection conn = new SqlConnection(strSQLConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("get_booking_fee", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@agency", agencyCode);
                    cmd.Parameters.AddWithValue("@currency", currencyCode);
                    cmd.Parameters.AddWithValue("@service ", "");
                    cmd.Parameters.AddWithValue("@fee", service);
                    cmd.Parameters.AddWithValue("@form_of_payment", "");
                    cmd.Parameters.AddWithValue("@form_of_payment_subtype", "");
                    cmd.Parameters.AddWithValue("@bookingdate", "");
                    cmd.Parameters.AddWithValue("@origin", origin);
                    cmd.Parameters.AddWithValue("@destination", destination);
                    cmd.Parameters.AddWithValue("@flight", flight);
                    cmd.Parameters.AddWithValue("@language", "EN");

                    SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();

                    try
                    {
                        conn.Open();

                        dataAdapter.Fill(dataTable);


                        foreach (DataRow row in dataTable.Rows)
                        {
                            specialServices = new ServiceFee
                            {
                              //  OriginRcd = row["origin_rcd"] != DBNull.Value ? row["origin_rcd"].ToString() : string.Empty,
                              //  DestinationRcd = row["destination_rcd"] != DBNull.Value ? row["destination_rcd"].ToString() : string.Empty,

                              //  OdOriginRcd = row["origin_rcd"] != DBNull.Value ? row["origin_rcd"].ToString() : string.Empty,
                              //  OdDestinationRcd = row["destination_rcd"] != DBNull.Value ? row["destination_rcd"].ToString() : string.Empty,
                                //  BookingClassRcd = row["booking_class_rcd"] != DBNull.Value ? row["booking_class_rcd"].ToString() : string.Empty, 
                                //  FareCode = row["fare_code"] != DBNull.Value ? row["fare_code"].ToString() : string.Empty,                                                                                                                                                                                                                                     
                                // FlightNumber = row["flight_number"] != DBNull.Value ? row["flight_number"].ToString() : string.Empty,
                                FeeAmount = row["fee_amount"] != DBNull.Value ? Convert.ToDecimal(row["fee_amount"]) : 0,
                                FeeAmountIncl = row["fee_amount_incl"] != DBNull.Value ? Convert.ToDecimal(row["fee_amount_incl"]) : 0,
                                TotalFeeAmount = row["fee_amount"] != DBNull.Value ? Convert.ToDecimal(row["fee_amount"]) : 0,
                                TotalFeeAmountIncl = row["fee_amount_incl"] != DBNull.Value ? Convert.ToDecimal(row["fee_amount_incl"]) : 0,
                                CurrencyRcd = row["currency_rcd"] != DBNull.Value ? row["currency_rcd"].ToString() : string.Empty
                              //  FeeRcd = row["fee_rcd"] != DBNull.Value ? row["fee_rcd"].ToString() : string.Empty
                            };

                        }
                    }
                    catch (System.Exception ex)
                    {

                    }
                }
            }

            return specialServices;
        }
        public List<ServiceFee> GetSegmentFee(
                                        string agencyCode,
                                       string currencyCode,
                                       string languageCode,
                                       int numberOfPassenger,
                                       int numberOfInfant,
                                       IList<PassengerService> services,
                                       IList<SegmentService> segmentService,
                                       bool SpecialService, 
                                       bool bNovat)
        {
            List<ServiceFee> fees = new List<ServiceFee>();

            try
            {
                if (segmentService != null && segmentService.Count > 0 && services != null && services.Count > 0)
                {
                    bool b = false;
                    //Declaration
                    if (string.IsNullOrEmpty(_server) == false)
                    {
                    }
                    else
                    {

                    }

                    //Fill Segment information
                    for (int f = 0; f < services.Count; f++)
                    {
                        //Create new segmentFee instance.
                       
                        for (int i = 0; i < segmentService.Count; i++)
                        {
                                                  
                        if (SpecialService == true)
                        {
                            //   b = objFees.SpecialServiceFee(agencyCode, currencyCode, services[f].SpecialServiceRcd, ref rsSegment, languageCode, bNovat);
                            ServiceFee sf = SpecialServiceFee(agencyCode, currencyCode, services[f].SpecialServiceRcd, segmentService[i].OriginRcd,
                                segmentService[i].DestinationRcd, segmentService[i].FlightNumber);
                            if (sf != null)
                            {
                                sf.FeeRcd = services[f].SpecialServiceRcd;
                                sf.SpecialServiceRcd = services[f].SpecialServiceRcd;
                                sf.DisplayName = services[f].DisplayName;
                                sf.ServiceOnRequestFlag = Convert.ToBoolean(services[f].ServiceOnRequestFlag);
                                sf.CutOffTime = Convert.ToBoolean(services[f].CutOffTime);
                                sf.AirlineRcd = segmentService[i].AirlineRcd;
                                sf.FlightNumber = segmentService[i].FlightNumber;
                                sf.BookingClassRcd = segmentService[i].BookingClassRcd;
                                sf.DepartureDate = segmentService[i].DepartureDate;
                                sf.OriginRcd = segmentService[i].OriginRcd;
                                sf.DestinationRcd = segmentService[i].DestinationRcd;
                                sf.OdOriginRcd = segmentService[i].OdOriginRcd;
                                sf.OdDestinationRcd = segmentService[i].OdDestinationRcd;

                                fees.Add(sf);
                            }
                        }
                    }
                       
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
              
            }

            return fees;
        }

    

    }
}
