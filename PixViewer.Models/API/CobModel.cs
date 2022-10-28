using System.Text.Json.Serialization;

namespace PixViewer.Models.API {
  public class CobModel: BaseModel {

    [JsonPropertyName("requester_id")]
    public int RequesterId { get; set; }

    [JsonPropertyName("copy_paste")]
    public string CopyPaste { get; set; }

    [JsonPropertyName("qrcode")]
    public string QrCode { get; set; }

    [JsonPropertyName("expire_time")]
    public int ExpireTime { get; set; }

    [JsonPropertyName("cob_location")]
    public string CobLocation { get; set; }

    [JsonPropertyName("tx_id")]
    public string TxId { get; set; }

    [JsonPropertyName("debtor_name")]
    public string DebtorName { get; set; }

    [JsonPropertyName("debtor_cpf")]
    public string DebtorCpf { get; set; }

    [JsonPropertyName("pix_key")]
    public string PixKey { get; set; }

    [JsonPropertyName("payer_description")]
    public string PayerDescription { get; set; }

    [JsonPropertyName("additional_infos")]
    public string AdditionalInfos { get; set; }

    [JsonPropertyName("monetary_value")]
    public string Value { get; set; }

    [JsonPropertyName("cob_status")]
    public string Status { get; set; }

    [JsonPropertyName("historic_date")]
    public DateTime HistoricDate { get; set; }
  }
}
