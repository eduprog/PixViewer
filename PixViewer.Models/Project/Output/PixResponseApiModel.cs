using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PixViewer.Models.Project.Output {

  public class Devolution {

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("rtrId")]
    public string RtrId { get; set; }

    [JsonPropertyName("valor")]
    public string Value { get; set; }

    [JsonPropertyName("horario")]
    public Time Time { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }
  }

  public class Time {

    [JsonPropertyName("solicitacao")]
    public DateTime Solicitation { get; set; }
  }

  public class Pages {

    [JsonPropertyName("paginaAtual")]
    public int CurrentPage { get; set; }

    [JsonPropertyName("itensPorPagina")]
    public int MaxItensPerPage { get; set; }

    [JsonPropertyName("quantidadeDePaginas")]
    public int CountPages { get; set; }

    [JsonPropertyName("quantidadeTotalDeItens")]
    public int TotItens { get; set; }
  }

  public class Params {

    [JsonPropertyName("inicio")]
    public DateTime StartDate { get; set; }

    [JsonPropertyName("fim")]
    public DateTime EndDate { get; set; }

    [JsonPropertyName("paginacao")]
    public Pages Pages { get; set; }
  }

  public class Pix {

    [JsonPropertyName("endToEndId")]
    public string EndToEndId { get; set; }

    [JsonPropertyName("txid")]
    public string TxId { get; set; }

    [JsonPropertyName("valor")]
    public string Value { get; set; }

    [JsonPropertyName("horario")]
    public DateTime Time { get; set; }

    [JsonPropertyName("devolucoes")]
    public List<Devolution> Devolutions { get; set; }
  }

  public class PixResponseApiModel {

    [JsonPropertyName("parametros")]
    public Params Params { get; set; }

    [JsonPropertyName("pix")]
    public List<Pix> Pix { get; set; }
  }


}
