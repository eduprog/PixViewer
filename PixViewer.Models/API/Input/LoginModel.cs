using System.ComponentModel.DataAnnotations;

namespace PixViewer.Models.API.Input {

  public class LoginModel {
    [Required]
    [StringLength(15)]
    public string Login { get; set; }

    [Required]
    [StringLength(30)]
    public string Password { get; set; }
  }
}
