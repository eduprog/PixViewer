using PixViewer.BLL;
using PixViewer.DAL;
using PixViewer.Models.API;
using PixViewer.Utils;

bool PIXTESTE = false;
bool CREATE = true;
bool TOKEN = false;

if(CREATE) {
  // does all the management and filling of tables with necessary values. Create an adm user and a standardized subadm
  DataDefaultValuesTestDAL.Fill(); 
}

if(PIXTESTE) {
 
}

if(TOKEN) {
  var newUser = new UserModel {
    Login = $"subadm",
    Password = "1234",
    Active = true,
    CreateDate = DateTime.UtcNow,
    FullName = Helper.GetRandomString(15),
    UserProfile = UserProfile.Client,
    MaxWebhooksAvailables = new Random().Next(0, 4)
  };

  Console.Write(UserBLL.Create(newUser, UserBLL.Get("admin")) + "\n");


}

Console.ReadKey();
