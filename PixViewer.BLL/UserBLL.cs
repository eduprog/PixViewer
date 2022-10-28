using PixViewer.DAL;
using PixViewer.Models.API;
using PixViewer.Models.API.Input;
using PixViewer.Utils;

namespace PixViewer.BLL {
  public static class UserBLL {
    private static readonly string _noPermission = "The requesting user does not have permission. # 403";
    private static readonly string _userExists = "User already registered # 200";
    private static readonly string _actionBlock = "Localized risk action. Attempt to change identifier. # 400";

    private static UserModel GetImutableFields(UserModel user) {
      var currentInfos = UserDAL.Get(user.Id);
      if(currentInfos == null)
        throw new Exception(_actionBlock);

      user.CreateDate = currentInfos.CreateDate;
      user.Login = currentInfos.Login;

      return user;
    }

    #region GET

    public static UserModel Get(string uniqueLogin) => UserDAL.Get(uniqueLogin);
    public static UserModel Get(int userId) => UserDAL.Get(userId);
    public static List<UserModel> GetAll(UserModel requester) {
      #region RULE 1: PERMISSION REQUIRED TO PERFORM THE ACTION.
      var permissions = requester.Permissions;
      if(!permissions.Contains(UserActions.Read))
        return null;

      #endregion

      return UserDAL.GetAll();
    }
    public static UserModel Get(int userId, UserModel requester) {
      var result = UserDAL.Get(userId);
      if(!result.IsFilled())
        return null;

      #region RULE 1: PERMISSION REQUIRED TO PERFORM THE ACTION.
      var permissions = requester.Permissions;
      if(!permissions.Contains(UserActions.Read))
        return null;
        
      if(result.UserProfile > requester.UserProfile)
        return null;

      #endregion

      return result;

    }
    public static UserModel Get(string uniqueLogin, UserModel requester) {
      var result = UserDAL.Get(uniqueLogin);
      if(!result.IsFilled())
        return null;

      #region RULE 1: PERMISSION REQUIRED TO PERFORM THE ACTION.
      var permissions = requester.Permissions;
      if(!permissions.Contains(UserActions.Read))
        return null;

      if(result.UserProfile > requester.UserProfile)
        return null;
      #endregion

      return result;
    }
    public static UserModel Get(LoginModel login) {
      try {
        var cryptoPass = Helper.CryptValue($"{login.Login}_{login.Password}");
        return UserDAL.Get(new LoginModel { Login = login.Login, Password = cryptoPass });
      } catch { return null; }

    }

    #endregion

    public static string Create(UserModel newUser, UserModel requester) {

      #region RULE 1: PERMISSION REQUIRED TO PERFORM THE ACTION.
      var permissions = requester.Permissions;
      if(!permissions.Contains(UserActions.Write))
        return _noPermission;

      if(newUser.UserProfile > requester.UserProfile)
        return _noPermission;

      #endregion

      #region RULE 2: USER MUST BE UNIQUE

      if(Get(newUser.Login, requester).IsFilled())
        return _userExists;

      newUser.Password = Helper.CryptValue($"{newUser.Login}_{newUser.Password}");

      #endregion

      return UserDAL.Create(newUser);
    }

    public static string Update(UserModel newInfos, UserModel requester) {

      #region RULE 1: KEEP THE VALIDITY OF IMMUTABLE FIELDS
      /* Ensure that immutable fields stay with their initial values */

      newInfos = GetImutableFields(newInfos);

      #endregion

      #region RULE 2: PERMISSION REQUIRED TO PERFORM THE ACTION.
      var permissions = requester.Permissions;
      if(!permissions.Contains(UserActions.Modify))
        return _noPermission;

      if(newInfos.UserProfile > requester.UserProfile)
        return _noPermission;
      #endregion

      #region RULE 3: PASSWORD MUST BE ENCRYPTED
      var currentInfo = Get(newInfos.Id, requester);
      if(newInfos.Password != currentInfo.Password)
        newInfos.Password = Helper.CryptValue($"{newInfos.Login}_{newInfos.Password}");

      #endregion

      return UserDAL.Update(newInfos);
    }

    public static string Delete(UserModel userForDel, UserModel requester) {

      #region RULE 1: KEEP THE VALIDITY OF IMMUTABLE FIELDS
      /* Ensure that immutable fields stay with their initial values */

      userForDel = GetImutableFields(userForDel);

      #endregion

      #region RULE 2: PERMISSION REQUIRED TO PERFORM THE ACTION.
      var permissions = requester.Permissions;
      if(!permissions.Contains(UserActions.Delete))
        return _noPermission;

      if(userForDel.UserProfile >= requester.UserProfile)
        return _noPermission;
      #endregion

      #region RULE 3: THE REQUESTOR MUST BE DIFFERENT FROM THE DELETION TARGET.

      if(userForDel.Login == requester.Login)
        return _noPermission;

      #endregion

      return UserDAL.Delete(userForDel);
    }
  }

}
