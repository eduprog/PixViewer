namespace PixViewer.Models.Project.Settings {
  public class RequestModel {
    public string Host { get; set; }
    public HttpRequestMessage HttpRequestMessage { get; set; }
    public AuthorizeModel Authorize { get; set; }
  }
}
