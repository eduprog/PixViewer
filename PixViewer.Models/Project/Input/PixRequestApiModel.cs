using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace PixViewer.Models.Project.Input {
  public class PixRequestApiModel {
    [Required]
    public DateTime StartDate { get; set; }
    [Required]
    public DateTime EndDate { get; set; }
    public string TxId { get; set; }
    public string Cpf { get; set; }
    public string Cnpj { get; set; }
    public int CurrentPage { get; set; }
    public int MaxItensPerPage { get; set; }
  }
}
