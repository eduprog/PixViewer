using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixViewer.Models.API.Input {
  public class WebhookApiCreateModel {

    [Required(AllowEmptyStrings = false, ErrorMessage = "The key cannot be empty or null.")]
    public string Key { get; set; }

    [Required(AllowEmptyStrings = false, ErrorMessage = "The url cannot be empty or null.")]
    public string UrlForNotify { get; set; }
  }
}
