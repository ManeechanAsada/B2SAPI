using Avantik.Web.Service.Model.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avantik.Web.Service.Entity;

using System.Data.SqlClient;
using System.Data;
using Avantik.Web.Service.Helpers;

namespace Avantik.Web.Service.Model.COM
{
    public class SystemService : RunComplus, ISystemModelService
    {
        string _server = string.Empty;
        public SystemService(string server, string user, string pass, string domain)
            :base(user,pass,domain)
        {
            _server = server;
        }



        public IList<SpecialService> GetSpecialService(string language)
        {
            List<SpecialService> specialServices = new List<SpecialService>();
            string strSQLConnectionString = ConfigHelper.ToString("SQLConnectionString");

            using (SqlConnection conn = new SqlConnection(strSQLConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[get_special_service]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@language", "");

                    SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();

                    try
                    {
                        conn.Open();
                       
                        dataAdapter.Fill(dataTable);

                        
                        foreach (DataRow row in dataTable.Rows)
                        {
                            SpecialService service = new SpecialService
                            {
                                SpecialServiceRcd = row["special_service_rcd"] != DBNull.Value ? row["special_service_rcd"].ToString() : string.Empty,
                                DisplayName = row["display_name"] != DBNull.Value ? row["display_name"].ToString() : string.Empty,
                              //  HelpText = row["help_text"] != DBNull.Value ? row["help_text"].ToString() : string.Empty,
                              //  SpecialServiceGroupRcd = row["special_service_group_rcd"] != DBNull.Value ? row["special_service_group_rcd"].ToString() : string.Empty,
                              ////  InventoryControlFlag = row["inventory_control_flag"] != DBNull.Value ? Convert.ToByte(row["inventory_control_flag"]) : (byte)0,
                                ManifestFlag = row["manifest_flag"] != DBNull.Value ? Convert.ToByte(row["manifest_flag"]) : (byte)0,
                                TextAllowedFlag = row["text_allowed_flag"] != DBNull.Value ? Convert.ToByte(row["text_allowed_flag"]) : (byte)0,
                                TextRequiredFlag = row["text_required_flag"] != DBNull.Value ? Convert.ToByte(row["text_required_flag"]) : (byte)0,
                                ServiceOnRequestFlag = row["service_on_request_flag"] != DBNull.Value ? Convert.ToByte(row["service_on_request_flag"]) : (byte)0,


                                IncludePassengerNameFlag = row["include_passenger_name_flag"] != DBNull.Value ? Convert.ToByte(row["include_passenger_name_flag"]) : (byte)0,
                                IncludeFlightSegmentFlag = row["include_flight_segment_flag"] != DBNull.Value ? Convert.ToByte(row["include_flight_segment_flag"]) : (byte)0,
                                IncludeActionCodeFlag = row["include_action_code_flag"] != DBNull.Value ? Convert.ToByte(row["include_action_code_flag"]) : (byte)0,

                                IncludeNumberOfServiceFlag = row["include_number_of_service_flag"] != DBNull.Value ? Convert.ToByte(row["include_number_of_service_flag"]) : (byte)0,
                                IncludeCateringFlag = row["include_catering_flag"] != DBNull.Value ? Convert.ToByte(row["include_catering_flag"]) : (byte)0,
                                IncludePassengerAssistanceFlag = row["include_passenger_assistance_flag"] != DBNull.Value ? Convert.ToByte(row["include_passenger_assistance_flag"]) : (byte)0,


                                ServiceSupportedFlag = row["service_supported_flag"] != DBNull.Value ? Convert.ToByte(row["service_supported_flag"]) : (byte)0,
                                SendInterlineReplyFlag = row["send_interline_reply_flag"] != DBNull.Value ? Convert.ToByte(row["send_interline_reply_flag"]) : (byte)0,
                                CutOffTime = row["cut_off_time"] != DBNull.Value ? Convert.ToInt32(row["cut_off_time"]) : 0,
                                StatusCode = row["status_code"] != DBNull.Value ? row["status_code"].ToString() : string.Empty

                                // CreateBy = row["create_by"] != DBNull.Value ? new Guid(row["create_by"].ToString()) : Guid.Empty,
                                // CreateDateTime = row["create_date_time"] != DBNull.Value ? Convert.ToDateTime(row["create_date_time"]) : DateTime.MinValue,
                                //  UpdateBy = row["update_by"] != DBNull.Value ? new Guid(row["update_by"].ToString()) : Guid.Empty,
                                // UpdateDateTime = row["update_date_time"] != DBNull.Value ? Convert.ToDateTime(row["update_date_time"]) : DateTime.MinValue,
                               // PassengerSegmentServiceId = row["passenger_segment_service_id"] != DBNull.Value ? new Guid(row["passenger_segment_service_id"].ToString()) : Guid.Empty,
                              //  PassengerId = row["passenger_id"] != DBNull.Value ? new Guid(row["passenger_id"].ToString()) : Guid.Empty,
                              //  BookingSegmentId = row["booking_segment_id"] != DBNull.Value ? new Guid(row["booking_segment_id"].ToString()) : Guid.Empty,
                              //  ServiceText = row["service_text"] != DBNull.Value ? row["service_text"].ToString() : string.Empty,
                              //  SpecialServiceStatusRcd = row["special_service_status_rcd"] != DBNull.Value ? row["special_service_status_rcd"].ToString() : string.Empty
                            };

                            specialServices.Add(service);
                        }
                    }
                    catch (System.Exception ex)
                    {
                       
                    }
                }
            }

            return specialServices;
        }


    }
}
