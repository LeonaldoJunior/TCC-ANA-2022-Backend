using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TCC_Ana.DataModels
{

    public class ApplicationIds
    {
        [JsonPropertyName("application_id")]
        public string ApplicationId { get; set; }
    }

    public class EndDeviceIds
    {
        [JsonPropertyName("device_id")]
        public string DeviceId { get; set; }

        [JsonPropertyName("application_ids")]
        public ApplicationIds ApplicationIds { get; set; }

        [JsonPropertyName("dev_eui")]
        public string DevEui { get; set; }

        [JsonPropertyName("dev_addr")]
        public string DevAddr { get; set; }
    }

    public class DecodedPayload
    {
        [JsonPropertyName("analog_in_1")]
        public double AnalogIn1 { get; set; }

        [JsonPropertyName("analog_in_2")]
        public double AnalogIn2 { get; set; }
    }

    public class GatewayIds
    {
        [JsonPropertyName("gateway_id")]
        public string GatewayId { get; set; }

        [JsonPropertyName("eui")]
        public string Eui { get; set; }
    }

    public class RxMetadata
    {
        [JsonPropertyName("gateway_ids")]
        public GatewayIds GatewayIds { get; set; }

        [JsonPropertyName("timestamp")]
        public long Timestamp { get; set; }

        [JsonPropertyName("rssi")]
        public int Rssi { get; set; }

        [JsonPropertyName("channel_rssi")]
        public int ChannelRssi { get; set; }

        [JsonPropertyName("snr")]
        public double Snr { get; set; }

        [JsonPropertyName("uplink_token")]
        public string UplinkToken { get; set; }
    }

    public class Lora
    {
        [JsonPropertyName("bandwidth")]
        public int Bandwidth { get; set; }

        [JsonPropertyName("spreading_factor")]
        public int SpreadingFactor { get; set; }
    }

    public class DataRate
    {

        [JsonPropertyName("lora")]
        public Lora Lora { get; set; }
    }

    public class Settings
    {

        [JsonPropertyName("data_rate")]
        public DataRate DataRate { get; set; }

        [JsonPropertyName("lodata_rate_indexra")]
        public int DataRateIndex { get; set; }

        [JsonPropertyName("coding_rate")]
        public string CodingRate { get; set; }

        [JsonPropertyName("frequency")]
        public string Frequency { get; set; }

        [JsonPropertyName("timestamp")]
        public long Timestamp { get; set; }

    }

    public class NetworkIds
    {
        [JsonPropertyName("net_id")]
        public string NetId { get; set; }

        [JsonPropertyName("tenant_id")]
        public string TenantId { get; set; }

        [JsonPropertyName("cluster_id")]
        public string ClusterId { get; set; }
    }

    public class UplinkMessage
    {
        [JsonPropertyName("f_port")]
        public int FPort { get; set; }

        [JsonPropertyName("frm_payload")]
        public string FrmPayload { get; set; }

        [JsonPropertyName("decoded_payload")]
        public DecodedPayload DecodedPayload { get; set; }

        [JsonPropertyName("rx_metadata")]
        public List<RxMetadata> RxMetadata { get; set; }

        [JsonPropertyName("settings")]
        public Settings Settings { get; set; }

        [JsonPropertyName("received_at")]
        public string ReceivedAt { get; set; }

        [JsonPropertyName("consumed_airtime")]
        public string ConsumedAirtime { get; set; }

        [JsonPropertyName("network_ids")]
        public NetworkIds NetworkIds { get; set; }
    }

    public class EndDeviceEvent
    {
        [JsonPropertyName("end_device_ids")]
        public EndDeviceIds EndDeviceIds { get; set; }

        [JsonPropertyName("correlation_ids")]
        public List<string> CorrelationIds { get; set; }

        [JsonPropertyName("received_at")]
        public string ReceivedAt { get; set; }

        [JsonPropertyName("uplink_message")]
        public UplinkMessage UplinkMessage { get; set; }
    }




}
