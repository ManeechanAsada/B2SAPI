using System;
using System.Collections.Generic;

namespace Avantik.Web.Service.BookingRead
{
    public class BookingRead
    {
        public nl_booking_header Header { get; set; }
        public List<nl_booking_segment> Segments { get; set; }
        public List<nl_passenger> Passengers { get; set; }
        public List<nl_booking_remark> Remarks { get; set; }
        public List<nl_booking_payment> Payments { get; set; }
        public List<nl_passenger_segment_mapping> Mappings { get; set; }
        public List<nl_passenger_segment_service> Services { get; set; }
        public List<nl_passenger_segment_tax> Taxs { get; set; }
        public List<nl_booking_fee> Fees { get; set; }
        public List<SummarizeQuote> Quotes { get; set; }
    }
    public enum RequestType
    {
        NONE,
        ADD,
        UPD,
        DEL
    }
    public partial class nl_booking_header
    {
        public Guid booking_id { get; set; }
        public Guid client_profile_id { get; set; }
        public Nullable<long> booking_number { get; set; }
        public string record_locator { get; set; } = "";
        public string booking_source_rcd { get; set; } = "";
        public DateTime create_date_time { get; set; }
        public Guid create_by { get; set; }
        public int number_of_adults { get; set; }
        public int number_of_children { get; set; }
        public int number_of_infants { get; set; }
        public DateTime update_date_time { get; set; }
        public Guid update_by { get; set; }
        public string language_rcd { get; set; } = "";
        public string currency_rcd { get; set; } = "";
        public string agency_code { get; set; } = "";
        public string contact_name { get; set; } = "";
        public string contact_email { get; set; } = "";
        public string contact_email_cc { get; set; } = "";
        public string mobile_email { get; set; } = "";
        public string phone_mobile { get; set; } = "";
        public string phone_mobile_cc { get; set; } = "";
        public string phone_home { get; set; } = "";
        public string phone_business { get; set; } = "";
        public string received_from { get; set; } = "";
        public string phone_fax { get; set; } = "";
        public string phone_search { get; set; } = "";
        public string comment { get; set; } = "";
        public string notify_by_rcd { get; set; } = "";
        public byte notify_by_email_flag { get; set; }
        public byte notify_by_sms_flag { get; set; }
        public string group_name { get; set; } = "";
        public byte group_booking_flag { get; set; }
        public string tty_sender { get; set; } = "";
        public string tty_receiver { get; set; } = "";
        public DateTime tty_date { get; set; }
        public string tty_sequence { get; set; } = "";
        public string tty_action { get; set; } = "";
        public string sender_system { get; set; } = "";
        public string sender_city { get; set; } = "";
        public string sender_locator { get; set; } = "";
        public string sender_locator_search { get; set; } = "";
        public string originator_locator { get; set; } = "";
        public string originator_record_locator { get; set; } = "";
        public int group_count { get; set; }
        public string agency_city { get; set; } = "";
        public string agency_identification { get; set; } = "";
        public string agency_airport { get; set; } = "";
        public string agency_airline { get; set; } = "";
        public string agency_type { get; set; } = "";
        public string agency_country { get; set; } = "";
        public string agency_currency { get; set; } = "";
        public byte package_booking_flag { get; set; }
        public string company_name { get; set; } = "";
        public string lastname { get; set; } = "";
        public string firstname { get; set; } = "";
        public string middlename { get; set; } = "";
        public string address_line1 { get; set; } = "";
        public string address_line2 { get; set; } = "";
        public string street { get; set; } = "";
        public string state { get; set; } = "";
        public string district { get; set; } = "";
        public string province { get; set; } = "";
        public string city { get; set; } = "";
        public string zip_code { get; set; } = "";
        public string po_box { get; set; } = "";
        public string title_rcd { get; set; } = "";
        public string country_rcd { get; set; } = "";
        public string invoice_receiver { get; set; } = "";
        public string tax_id { get; set; } = "";
        public string purchase_order { get; set; } = "";
        public string project_number { get; set; } = "";
        public string cost_center { get; set; } = "";
        public DateTime auto_cancel_date_time { get; set; }
        public string external_payment_reference { get; set; } = "";
        public string ip_address { get; set; } = "";
        public byte approval_flag { get; set; }
        public byte newsletter_flag { get; set; }
        public byte business_flag { get; set; }
        public string vendor_rcd { get; set; } = "";
        public string tour_operator_locator { get; set; } = "";
        public byte no_vat_flag { get; set; }
        public byte non_commercial_booking_flag { get; set; }
        public string agency_name { get; set; } = "";
        public string iata_number { get; set; } = "";
        public string phone { get; set; } = "";
        public string email { get; set; } = "";
        public string agcy_city { get; set; } = "";
        public byte own_agency_flag { get; set; }
        public byte web_agency_flag { get; set; }
        public Nullable<long> client_number { get; set; }
        public string client_lastname { get; set; } = "";
        public string client_firstname { get; set; } = "";
        public string client_city { get; set; } = "";
        public string create_name { get; set; } = "";
        public string update_name { get; set; } = "";
        public string lock_name { get; set; } = "";
        public DateTime lock_date_time { get; set; }
        public DateTime utc_date_time { get; set; }
        public DateTime last_flight_date { get; set; }
        public string segment_airline_rcd { get; set; }
        public DateTime split_date_time { get; set; }
        public int interline_booking_flag { get; set; }
        public RequestType row_type { get; set; }

        public nl_booking_header Clone()
        {
            return (nl_booking_header)this.MemberwiseClone();
        }
    }

    public partial class nl_booking_segment
    {
        public Guid booking_segment_id { get; set; }
        public Guid flight_id { get; set; }
        public string airline_rcd { get; set; }
        public string flight_number { get; set; }
        public string booking_class_rcd { get; set; }
        public string boarding_class_rcd { get; set; }
        public Guid create_by { get; set; }
        public DateTime create_date_time { get; set; }
        public Guid update_by { get; set; }
        public DateTime update_date_time { get; set; }
        public DateTime departure_date { get; set; }
        public int departure_time { get; set; }
        public int arrival_time { get; set; }
        public int journey_time { get; set; }
        public string segment_status_rcd { get; set; }
        public Guid booking_id { get; set; }
        public int number_of_units { get; set; }
        public string origin_rcd { get; set; }
        public string destination_rcd { get; set; }
        public string segment_change_status_rcd { get; set; }
        public string marketing_airline_rcd { get; set; }
        public string marketing_flight_number { get; set; }
        public string marketing_booking_class_rcd { get; set; }
        public byte info_segment_flag { get; set; }
        public byte passive_segment_flag { get; set; }
        public byte check_in_segment_flag { get; set; }
        public string od_origin_rcd { get; set; }
        public string od_destination_rcd { get; set; }
        public Guid flight_connection_id { get; set; }
        public byte temp_seatmap_flag { get; set; }
        public byte temp_map_segment_flag { get; set; }
        public byte overbook_flag { get; set; }
        public DateTime flight_flown_date_time { get; set; }
        public int day_change_value { get; set; }
        public string nesting_string { get; set; }
        public byte allow_web_checkin_flag { get; set; }
        public string flight_status_rcd { get; set; }
        public string flight_information_1 { get; set; }
        public string aircraft_type_rcd { get; set; }
        public DateTime utc_departure_date_time { get; set; }
        public DateTime pnl_sent_date_time { get; set; }
        public string flight_check_in_status_rcd { get; set; }
        public string irregularity_rcd { get; set; }
        public short seatmap_flag { get; set; }
        public string origin_name { get; set; }
        public string destination_name { get; set; }
        public string segment_status_name { get; set; }
        public string boarding_class { get; set; }
        public int planned_arrival_time { get; set; }
        public int flight_std { get; set; }
        public int flight_etd { get; set; }
        public int flight_atd { get; set; }
        public int flight_sta { get; set; }
        public int flight_eta { get; set; }
        public int flight_ata { get; set; }
        public byte e_ticket_flag { get; set; }
        public byte require_open_status_flag { get; set; }
        public DateTime arrival_date { get; set; }
        public int number_of_stops { get; set; }
        public int close_web_sales_flag { get; set; }
        public int open_sequence { get; set; }
        public short codeshare_option { get; set; }
        public int origin_sequence { get; set; }
        public int destination_sequence { get; set; }
        public string pnr_airline_rcd { get; set; }
        public string pnr_flight_number { get; set; }
        public int arrival_day_change { get; set; }
        public DateTime utc_arrival_date_time { get; set; }
        public int cancel_not_exists_flag { get; set; }
        public byte iet_added_segment { get; set; }
        public byte iet_cancellation_eligibility { get; set; }
        public RequestType row_type { get; set; }
        public string segment_status_rcd_original_value { get; set; }

        public nl_booking_segment Clone()
        {
            return (nl_booking_segment)this.MemberwiseClone();
        }
    }

    public partial class nl_passenger
    {
        public Guid passenger_id { get; set; }
        public Guid booking_id { get; set; }
        public Guid passenger_profile_id { get; set; }
        public Guid guardian_passenger_id { get; set; }
        public string passenger_type_rcd { get; set; }
        public string lastname { get; set; }
        public string firstname { get; set; }
        public string middlename { get; set; }
        public string redress_number { get; set; }
        public DateTime date_of_birth { get; set; }
        public string title_rcd { get; set; }
        public Guid create_by { get; set; }
        public DateTime create_date_time { get; set; }
        public Guid update_by { get; set; }
        public DateTime update_date_time { get; set; }
        public string gender_type_rcd { get; set; }
        public string nationality_rcd { get; set; }
        public string residence_country_rcd { get; set; }
        public string passport_issue_country_rcd { get; set; }
        public string passport_number { get; set; }
        public DateTime passport_issue_date { get; set; }
        public DateTime passport_expiry_date { get; set; }
        public string passport_issue_place { get; set; }
        public string passport_birth_place { get; set; }
        public string document_type_rcd { get; set; }
        public byte wheelchair_flag { get; set; }
        public string pnr_lastname { get; set; }
        public string pnr_firstname { get; set; }
        public int pnr_lastname_sequence { get; set; }
        public int pnr_firstname_sequence { get; set; }
        public long client_number { get; set; }
        public string member_number { get; set; }
        public string member_airline_rcd { get; set; }
        public string member_level_rcd { get; set; }
        public byte vip_flag { get; set; }
        public decimal passenger_weight { get; set; }
        public DateTime name_change_date_time { get; set; }
        public byte name_change_complete_flag { get; set; }
        public string country_code_long { get; set; }
        public string known_traveler_number { get; set; }
        public string pnr_pax_name { get; set; }
        public string pnr_complete_name { get; set; }
        public string pnr_name { get; set; }
        public long profile_client_number { get; set; }
        public string profile_member_number { get; set; }
        public string profile_member_level { get; set; }
        public RequestType row_type { get; set; }
        public string lastname_original_value { get; set; }

        public nl_passenger Clone()
        {
            return (nl_passenger)this.MemberwiseClone();
        }
    }

    public partial class nl_booking_remark
    {
        public Guid booking_remark_id { get; set; }
        public Guid client_profile_id { get; set; }
        public string remark_type_rcd { get; set; }
        public DateTime timelimit_date_time { get; set; }
        public byte complete_flag { get; set; }
        public string remark_text { get; set; }
        public string nickname { get; set; }
        public string agency_code { get; set; }
        public Guid create_by { get; set; }
        public DateTime create_date_time { get; set; }
        public Guid update_by { get; set; }
        public DateTime update_date_time { get; set; }
        public Guid booking_id { get; set; }
        public Guid split_booking_id { get; set; }
        public Guid booking_segment_id { get; set; }
        public Guid passenger_segment_service_id { get; set; }
        public string added_by { get; set; }
        public byte process_message_flag { get; set; }
        public byte protected_flag { get; set; }
        public byte warning_flag { get; set; }
        public byte system_flag { get; set; }
        public DateTime utc_timelimit_date_time { get; set; }
        public string display_name { get; set; }
        public string airline_rcd { get; set; }
        public string split_record_locator { get; set; }
        public DateTime split_create_date_time { get; set; }
        public int pax_cnt { get; set; }
        public RequestType row_type { get; set; }

        public nl_booking_remark Clone()
        {
            return (nl_booking_remark)this.MemberwiseClone();
        }
    }

    public partial class nl_booking_payment
    {
        public Guid booking_payment_id { get; set; }
        public Guid booking_id { get; set; }
        public string form_of_payment_rcd { get; set; }
        public string currency_rcd { get; set; }
        public string agency_code { get; set; }
        public string debit_agency_code { get; set; }
        public string payment_type_rcd { get; set; }
        public decimal payment_amount { get; set; }
        public decimal acct_payment_amount { get; set; }
        public Guid payment_by { get; set; }
        public DateTime payment_date_time { get; set; }
        public DateTime payment_due_date_time { get; set; }
        public string document_number { get; set; }
        public string document_number_encrypted { get; set; }
        public string approval_code { get; set; }
        public int expiry_month { get; set; }
        public int expiry_year { get; set; }
        public string name_on_card { get; set; }
        public byte due_date_flag { get; set; }
        public decimal document_amount { get; set; }
        public string form_of_payment_subtype_rcd { get; set; }
        public DateTime issue_date { get; set; }
        public string payment_remark { get; set; }
        public string booking_reference { get; set; }
        public string cvv_code { get; set; }
        public string issue_by { get; set; }
        public Guid void_by { get; set; }
        public DateTime void_date_time { get; set; }
        public Guid create_by { get; set; }
        public DateTime create_date_time { get; set; }
        public Guid update_by { get; set; }
        public DateTime update_date_time { get; set; }
        public Guid origin_booking_payment_id { get; set; }
        public Guid voucher_payment_id { get; set; }
        public Guid passenger_id { get; set; }
        public string payment_number { get; set; }
        public Guid booking_segment_id { get; set; }
        public string payment_reference { get; set; }
        public string transaction_reference { get; set; }
        public byte processed_authorization_flag { get; set; }
        public byte processed_settlement_flag { get; set; }
        public byte processed_credit_flag { get; set; }
        public DateTime processed_authorization_date_time { get; set; }
        public DateTime processed_settlement_date_time { get; set; }
        public DateTime processed_credit_date_time { get; set; }
        public byte manual_creditcard_transaction_flag { get; set; }
        public Guid client_profile_id { get; set; }
        public string street { get; set; }
        public string address_line1 { get; set; }
        public string address_line2 { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public string zip_code { get; set; }
        public string country_rcd { get; set; }
        public decimal fee_amount { get; set; }
        public decimal receive_payment_amount { get; set; }
        public string receive_currency_rcd { get; set; }
        public string pos_indicator { get; set; }
        public int issue_month { get; set; }
        public int issue_year { get; set; }
        public string issue_number { get; set; }
        public string ip_address { get; set; }
        public Guid merchant_id { get; set; }
        public string bank_code { get; set; }
        public string bank_name { get; set; }
        public string bank_iban { get; set; }
        public string agency_name { get; set; }
        public Guid voucher_id { get; set; }
        public string create_name { get; set; }
        public string update_name { get; set; }
        public string payment_name { get; set; }
        public string void_name { get; set; }
        public string record_locator { get; set; }
        public decimal allocated_amount { get; set; }
        public decimal refunded_amount { get; set; }
        public decimal exchange_rate { get; set; }
        public RequestType row_type { get; set; }

        public nl_booking_payment Clone()
        {
            return (nl_booking_payment)this.MemberwiseClone();
        }
    }

    public partial class nl_passenger_segment_mapping
    {
        public Guid booking_id { get; set; }
        public int departure_time { get; set; }
        public Guid passenger_id { get; set; }
        public Guid booking_segment_id { get; set; }
        public string origin_rcd { get; set; }
        public string destination_rcd { get; set; }
        public string lastname { get; set; }
        public string firstname { get; set; }
        public string gender_type_rcd { get; set; }
        public string title_rcd { get; set; }
        public string title { get; set; }
        public string passenger_type_rcd { get; set; }
        public DateTime date_of_birth { get; set; }
        public Guid flight_id { get; set; }
        public string airline_rcd { get; set; }
        public string flight_number { get; set; }
        public DateTime departure_date { get; set; }
        public string boarding_class_rcd { get; set; }
        public string booking_class_rcd { get; set; }
        public string inventory_class_rcd { get; set; }
        public string boarded_class_rcd { get; set; }
        public string record_locator { get; set; }
        public string seat_number { get; set; }
        public int seat_row { get; set; }
        public string seat_column { get; set; }
        public string seat_fee_rcd { get; set; }
        public string currency_rcd { get; set; }
        public decimal net_total { get; set; }
        public decimal tax_amount { get; set; }
        public decimal fare_amount { get; set; }
        public decimal fare_quote_amount { get; set; }
        public decimal yq_amount { get; set; }
        public decimal base_ticketing_fee_amount { get; set; }
        public decimal ticketing_fee_amount { get; set; }
        public decimal reservation_fee_amount { get; set; }
        public decimal commission_amount { get; set; }
        public decimal commission_percentage { get; set; }
        public decimal refund_charge { get; set; }
        public decimal refundable_amount { get; set; }
        public decimal fare_vat { get; set; }
        public decimal tax_vat { get; set; }
        public decimal yq_vat { get; set; }
        public decimal ticketing_fee_vat { get; set; }
        public decimal reservation_fee_vat { get; set; }
        public decimal fare_amount_incl { get; set; }
        public decimal tax_amount_incl { get; set; }
        public decimal yq_amount_incl { get; set; }
        public decimal commission_amount_incl { get; set; }
        public decimal ticketing_fee_amount_incl { get; set; }
        public decimal reservation_fee_amount_incl { get; set; }
        public decimal acct_net_total { get; set; }
        public decimal acct_tax_amount { get; set; }
        public decimal acct_fare_amount { get; set; }
        public decimal acct_yq_amount { get; set; }
        public decimal acct_ticketing_fee_amount { get; set; }
        public decimal acct_reservation_fee_amount { get; set; }
        public decimal acct_commission_amount { get; set; }
        public decimal acct_fare_amount_incl { get; set; }
        public decimal acct_tax_amount_incl { get; set; }
        public decimal acct_yq_amount_incl { get; set; }
        public decimal acct_commission_amount_incl { get; set; }
        public decimal acct_ticketing_fee_amount_incl { get; set; }
        public decimal acct_reservation_fee_amount_incl { get; set; }
        public decimal acct_refundable_amount { get; set; }
        public decimal agency_fare_amount { get; set; }
        public decimal agency_commission_amount { get; set; }
        public decimal agency_fare_amount_incl { get; set; }
        public decimal agency_commission_amount_incl { get; set; }
        public decimal agency_net_total { get; set; }
        public decimal agency_tax_amount_incl { get; set; }
        public decimal agency_tax_amount { get; set; }
        public string agency_currency_rcd { get; set; }
        public Guid flight_connection_id { get; set; }
        public string od_origin_rcd { get; set; }
        public string od_destination_rcd { get; set; }
        public decimal prorate_acct_fare_amount { get; set; }
        public decimal prorate_acct_yq_amount { get; set; }
        public decimal prorate_sales_fare_amount { get; set; }
        public decimal prorate_sales_yq_amount { get; set; }
        public Guid fare_id { get; set; }
        public string fare_code { get; set; }
        public DateTime fare_date_time { get; set; }
        public int accounting_refund_batch { get; set; }
        public byte refundable_flag { get; set; }
        public byte transferable_fare_flag { get; set; }
        public byte unavailable_flag { get; set; }
        public short refund_with_charge_hours { get; set; }
        public short refund_not_possible_hours { get; set; }
        public byte standby_flag { get; set; }
        public byte exclude_pricing_flag { get; set; }
        public string passenger_check_in_status_rcd { get; set; }
        public string priority_code { get; set; }
        public string agency_code { get; set; }
        public string passenger_status_rcd { get; set; }
        public string ticket_number { get; set; }
        public DateTime ticketing_date_time { get; set; }
        public Guid ticketing_by { get; set; }
        public decimal public_fare_amount { get; set; }
        public decimal public_fare_amount_incl { get; set; }
        public string document_number { get; set; }
        public string form_of_payment { get; set; }
        public string tour_code { get; set; }
        public byte fim_flag { get; set; }
        public byte duty_travel_flag { get; set; }
        public byte it_fare_flag { get; set; }
        public DateTime not_valid_before_date { get; set; }
        public DateTime not_valid_after_date { get; set; }
        public string boarding_pass_number { get; set; }
        public DateTime check_in_date_time { get; set; }
        public string check_in_comment { get; set; }
        public int check_in_sequence { get; set; }
        public int group_sequence { get; set; }
        public Guid unload_by { get; set; }
        public DateTime unload_date_time { get; set; }
        public string unload_comment { get; set; }
        public int number_of_pieces { get; set; }
        public decimal baggage_weight { get; set; }
        public short piece_allowance { get; set; }
        public decimal excess_baggage_weight { get; set; }
        public decimal check_in_baggage_weight { get; set; }
        public decimal group_baggage_weight { get; set; }
        public decimal changed_baggage_weight { get; set; }
        public string check_in_user_code { get; set; }
        public Guid check_in_by { get; set; }
        public Guid cancel_by { get; set; }
        public DateTime cancel_date_time { get; set; }
        public Guid create_by { get; set; }
        public DateTime create_date_time { get; set; }
        public Guid update_by { get; set; }
        public DateTime update_date_time { get; set; }
        public string refund_agency_code { get; set; }
        public Guid refund_by { get; set; }
        public DateTime refund_date_time { get; set; }
        public Guid void_by { get; set; }
        public DateTime void_date_time { get; set; }
        public DateTime exchanged_date_time { get; set; }
        public Guid exchanged_segment_id { get; set; }
        public string onward_airline_rcd { get; set; }
        public string onward_flight_number { get; set; }
        public DateTime onward_departure_date { get; set; }
        public string onward_destination_rcd { get; set; }
        public string onward_booking_class_rcd { get; set; }
        public string restriction_text { get; set; }
        public string endorsement_text { get; set; }
        public string fare_line { get; set; }
        public string check_in_code { get; set; }
        public byte present_flag { get; set; }
        public byte through_fare_flag { get; set; }
        public byte bsp_checked_flag { get; set; }
        public byte adm_raised_flag { get; set; }
        public string adm_document { get; set; }
        public int bsp_period { get; set; }
        public int bsp_month { get; set; }
        public int bsp_year { get; set; }
        public string bsp_comment { get; set; }
        public byte advanced_seating_flag { get; set; }
        public Guid ticket_id { get; set; }
        public int coupon_number { get; set; }
        public string fare_type_rcd { get; set; }
        public decimal redemption_points { get; set; }
        public DateTime pnl_sent_date_time { get; set; }
        public byte e_ticket_flag { get; set; }
        public string e_ticket_status { get; set; }
        public string baggage_weight_unit { get; set; }
        public string check_in_boarding_class_rcd { get; set; }
        public string check_in_booking_class_rcd { get; set; }
        public string check_in_category_rcd { get; set; }
        public Guid contract_id { get; set; }
        public long unique_passenger_number { get; set; }
        public string known_traveler_number { get; set; }
        public DateTime coupon_exchange_date_time { get; set; }
        public DateTime coupon_void_date_time { get; set; }
        public int origin_sequence { get; set; }
        public int destination_sequence { get; set; }
        public int fare_column { get; set; }
        public string fare_description { get; set; }
        public string fare_note { get; set; }
        public int day_of_year { get; set; }
        public decimal payment_amount { get; set; }
        public string create_name { get; set; }
        public string update_name { get; set; }
        public byte approval_flag { get; set; }
        public string group_name { get; set; }
        public string contact_name { get; set; }
        public string contact_email { get; set; }
        public string phone_mobile { get; set; }
        public string phone_home { get; set; }
        public string phone_business { get; set; }
        public string received_from { get; set; }
        public string phone_fax { get; set; }
        public string vendor_rcd { get; set; }
        public string tour_operator_locator { get; set; }
        public int group_count { get; set; }
        public string record_locator_display { get; set; }
        public Guid guardian_passenger_id { get; set; }
        public int planned_arrival_time { get; set; }
        public int planned_departure_time { get; set; }
        public DateTime arrival_date { get; set; }
        public int arrival_time { get; set; }
        public int boarding_time { get; set; }
        public int journey_time { get; set; }
        public int staff_flag { get; set; }
        public string conjunction_ticket_number { get; set; }
        public string marketing_airline_rcd { get; set; }
        public string marketing_flight_number { get; set; }
        public string base_currency { get; set; }
        public decimal base_fare_amount { get; set; }
        public string original_issue_information { get; set; }
        public string middlename { get; set; }
        public int hand_number_of_pieces { get; set; }
        public decimal hand_baggage_weight { get; set; }
        public string issuing_airline_rcd { get; set; }
        public byte n_to_o_751 { get; set; }
        public byte allow_seat_dupe_flag { get; set; }
        public string settlement_code { get; set; }
        public DateTime settlement_date_time { get; set; }
        public string sender_system { get; set; }
        public string keysearch { get; set; }
        public int gosho_norec_flag { get; set; }
        public int synchronize_flag { get; set; }
        public int display_fare_amount_flag { get; set; }
        public int display_no_adc_flag { get; set; }
        public string display_base_currency { get; set; }
        public string display_totl_currency { get; set; }
        public string display_base_amount { get; set; }
        public string display_totl_amount { get; set; }
        public string firstname_search { get; set; }
        public byte suppress_142_flag { get; set; }
        public byte info_segment_flag { get; set; }
        public string fare_basis { get; set; }
        public string ticket_designator { get; set; }
        public string marketing_flight_display { get; set; }
        public string sender_locator_display { get; set; }
        public string iata_number { get; set; }
        public string ori_ticket_number { get; set; }
        public int etl_update_ticket_flag { get; set; }
        public string ticket_number_mapping { get; set; }
        public RequestType row_type { get; set; }
        public Guid? exchanged_segment_id_original_value { get; set; }
        public string passenger_status_rcd_original_value { get; set; }

        public nl_passenger_segment_mapping Clone()
        {
            return (nl_passenger_segment_mapping)this.MemberwiseClone();
        }
    }

    public partial class nl_passenger_segment_service
    {
        public Guid booking_id { get; set; }
        public Guid passenger_segment_service_id { get; set; }
        public Guid passenger_id { get; set; }
        public Guid booking_segment_id { get; set; }
        public Guid advise_passenger_segment_service_id { get; set; }
        public string special_service_rcd { get; set; }
        public string special_service_airline_rcd { get; set; }
        public string service_text { get; set; }
        public string unit_rcd { get; set; }
        public short number_of_units { get; set; }
        public string special_service_status_rcd { get; set; }
        public string special_service_change_status_rcd { get; set; }
        public string member_airline_rcd { get; set; }
        public string member_number { get; set; }
        public Guid create_by { get; set; }
        public DateTime create_date_time { get; set; }
        public Guid update_by { get; set; }
        public DateTime update_date_time { get; set; }
        public byte temp_map_service_flag { get; set; }
        public Guid guardian_passenger_id { get; set; }
        public string lastname { get; set; }
        public string firstname { get; set; }
        public string title_rcd { get; set; }
        public string passenger_type_rcd { get; set; }
        public DateTime date_of_birth { get; set; }
        public string passenger_name { get; set; }
        public string passenger_guardian { get; set; }
        public string airline_rcd { get; set; }
        public string flight_number { get; set; }
        public string marketing_airline_rcd { get; set; }
        public string marketing_flight_number { get; set; }
        public string origin_rcd { get; set; }
        public string destination_rcd { get; set; }
        public DateTime departure_date { get; set; }
        public int departure_time { get; set; }
        public short codeshare_option { get; set; }
        public Guid flight_id { get; set; }
        public string boarding_class_rcd { get; set; }
        public RequestType row_type { get; set; }
        public string special_service_status_rcd_original_value { get; set; }
        public string special_service_change_status_rcd_original_value { get; set; }
        public Guid? advise_passenger_segment_service_id_original_value { get; set; }
        public string special_service_rcd_original_value { get; set; }
        public string service_text_original_value { get; set; }
        public string unit_rcd_original_value { get; set; }
        public int? number_of_units_original_value { get; set; }

        public nl_passenger_segment_service Clone()
        {
            return (nl_passenger_segment_service)this.MemberwiseClone();
        }
    }

    public partial class nl_passenger_segment_tax
    {
        public Guid booking_id { get; set; }
        public Guid passenger_segment_tax_id { get; set; }
        public decimal tax_amount { get; set; }
        public decimal tax_amount_incl { get; set; }
        public decimal acct_amount { get; set; }
        public decimal acct_amount_incl { get; set; }
        public decimal agency_amount { get; set; }
        public decimal agency_amount_incl { get; set; }
        public decimal sales_amount { get; set; }
        public decimal sales_amount_incl { get; set; }
        public decimal vat_percentage { get; set; }
        public string tax_rcd { get; set; }
        public int accounting_add_batch { get; set; }
        public int accounting_void_batch { get; set; }
        public DateTime void_date_time { get; set; }
        public Guid void_by { get; set; }
        public Guid create_by { get; set; }
        public DateTime create_date_time { get; set; }
        public Guid update_by { get; set; }
        public DateTime update_date_time { get; set; }
        public Guid passenger_id { get; set; }
        public Guid tax_id { get; set; }
        public Guid booking_segment_id { get; set; }
        public string tax_currency_rcd { get; set; }
        public string sales_currency_rcd { get; set; }
        public string display_name { get; set; }
        public string summarize_up { get; set; }
        public string coverage_type { get; set; }
        public string create_name { get; set; }
        public string update_name { get; set; }
        public string void_name { get; set; }
        public string tax_action { get; set; }
        public string tax_currency_rcd_display { get; set; }
        public string sales_currency_rcd_display { get; set; }
        public RequestType row_type { get; set; }
        public DateTime? void_date_time_original_value { get; set; }

        public nl_passenger_segment_tax Clone()
        {
            return (nl_passenger_segment_tax)this.MemberwiseClone();
        }
    }

    public partial class nl_booking_fee
    {
        public Guid booking_fee_id { get; set; }
        public decimal fee_amount { get; set; }
        public Guid booking_id { get; set; }
        public string agency_code { get; set; }
        public string currency_rcd { get; set; }
        public decimal acct_fee_amount { get; set; }
        public string change_comment { get; set; }
        public int accounting_add_batch { get; set; }
        public int accounting_void_batch { get; set; }
        public Guid passenger_id { get; set; }
        public Guid booking_segment_id { get; set; }
        public Guid passenger_segment_service_id { get; set; }
        public Guid fee_id { get; set; }
        public decimal vat_percentage { get; set; }
        public decimal fee_amount_incl { get; set; }
        public decimal acct_fee_amount_incl { get; set; }
        public decimal charge_amount { get; set; }
        public decimal charge_amount_incl { get; set; }
        public string charge_currency_rcd { get; set; }
        public Guid void_by { get; set; }
        public DateTime void_date_time { get; set; }
        public Guid account_fee_by { get; set; }
        public DateTime account_fee_date_time { get; set; }
        public Guid waive_by { get; set; }
        public DateTime waive_date_time { get; set; }
        public Guid create_by { get; set; }
        public DateTime create_date_time { get; set; }
        public Guid update_by { get; set; }
        public DateTime update_date_time { get; set; }
        public string od_origin_rcd { get; set; }
        public string od_destination_rcd { get; set; }
        public string fee_category_rcd { get; set; }
        public decimal agency_fee_amount { get; set; }
        public decimal agency_fee_amount_incl { get; set; }
        public string document_number { get; set; }
        public DateTime document_date_time { get; set; }
        public string comment { get; set; }
        public string external_reference { get; set; }
        public string vendor_rcd { get; set; }
        public decimal weight_lbs { get; set; }
        public decimal weight_kgs { get; set; }
        public decimal number_of_units { get; set; }
        public int units { get; set; }
        public string mpd_number { get; set; }
        public string lastname { get; set; }
        public string firstname { get; set; }
        public string title_rcd { get; set; }
        public string passenger_type_rcd { get; set; }
        public string airline_rcd { get; set; }
        public string flight_number { get; set; }
        public string origin_rcd { get; set; }
        public string destination_rcd { get; set; }
        public DateTime departure_date { get; set; }
        public string boarding_class_rcd { get; set; }
        public string booking_class_rcd { get; set; }
        public string passenger_status_rcd { get; set; }
        public string fee_rcd { get; set; }
        public string display_name { get; set; }
        public string fee_calculation_rcd { get; set; }
        public decimal payment_amount { get; set; }
        public string user_create { get; set; }
        public string user_update { get; set; }
        public string user_void { get; set; }
        public string user_account { get; set; }
        public string user_waive { get; set; }
        public RequestType row_type { get; set; }

        public nl_booking_fee Clone()
        {
            return (nl_booking_fee)this.MemberwiseClone();
        }
    }

    public class SummarizeQuote
    {
        public Guid booking_segment_id { get; set; }
        public string origin_rcd { get; set; }
        public string destination_rcd { get; set; }
        public DateTime departure_date { get; set; }
        public string airline_rcd { get; set; }
        public string flight_number { get; set; }
        public string passenger_type_rcd { get; set; }
        public int passenger_count { get; set; }
        public string currency_rcd { get; set; }
        public string charge_type { get; set; }
        public string charge_name { get; set; }
        public decimal charge_amount { get; set; }
        public decimal charge_amount_incl { get; set; }
        public decimal total_amount { get; set; }
        public decimal tax_amount { get; set; }
        public decimal redemption_points { get; set; }
        public int sort_sequence { get; set; }
    }

}
