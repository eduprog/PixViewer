using PixViewer.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixViewer.Models.API.Input {
  public class UserUpdateModel: BaseModel {

    [Required]
    public bool Active { get; set; }

    [Required]
    [StringLength(150)]
    public string FullName { get; set; }

    [Required]
    public DateTime LastAccess { get; set; }

    [Required]
    [StringLength(30)]
    public string Password { get; set; }

    [Required]
    public UserProfile UserProfile { get; set; }

    [Required]
    public int MaxWebhooksAvailables { get; set; }
  }
}
