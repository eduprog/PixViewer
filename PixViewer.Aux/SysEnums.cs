namespace PixViewer.Utils {
  public enum TokenType {
    Bearer,
    Basic
  }

  public enum UserProfile {
    Client = 1,
    Basic = 2,
    Moderator = 3,
    Adminstrator = 4
  }

  public enum UserActions {
    None = 1,
    Read = 2,
    Write = 3,
    Modify = 4,
    Delete = 5
  }

}
