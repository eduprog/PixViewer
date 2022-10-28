using PixViewer.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixViewer.Models.API.Input {
  public class UserCreateModel {

    [Required]
    [StringLength(150)]
    public string FullName { get; set; }

    [Required]
    [StringLength(15)]
    public string Login { get; set; }

    [Required]
    [StringLength(30)]
    public string Password { get; set; }

    [Required]
    public UserProfile UserProfile { get; set; }

    [Required]
    public int MaxWebhooksAvailables { get; set; }
  }
}
