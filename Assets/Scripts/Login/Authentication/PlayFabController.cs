using PlayFab;
using PlayFab.ClientModels;
using PlayFabError = PlayFab.PlayFabError;

namespace Login.Authentication {
    public class PlayFabController : AuthenticationAbstract {

        public PlayFabController(LoginController _loginController) : base(_loginController) { }

        public override void AttemptLogin(string _id, string _password) {
            LoginWithPlayFabRequest request = new LoginWithPlayFabRequest() {
                Username = _id,
                Password = EncryptPassword(_password)
            };
            
            PlayFabClientAPI.LoginWithPlayFab(request, LoginSuccess, LoginFailure);
        }

        public override void SignUp(string _email, string _id, string _password) {
            RegisterPlayFabUserRequest registerRequest = new RegisterPlayFabUserRequest() {
                Email = _email,
                Password = EncryptPassword(_password),
                Username = _id
            };

            PlayFabClientAPI.RegisterPlayFabUser(registerRequest, RegisterSuccess, RegisterFailure);
        }
        private void RegisterSuccess(RegisterPlayFabUserResult _result) {
            loginController.SetRegisterErrorText("");
        }
        
        private void RegisterFailure(PlayFabError _result) {
            loginController.SetRegisterErrorText(_result.GenerateErrorReport());
        }

        public void LoginSuccess(LoginResult _result) {
            loginController.SetLoginResultText(_result.SessionTicket);
        }
        
        private void LoginFailure(PlayFabError _result) {
            loginController.SetLoginResultText(_result.GenerateErrorReport());
        }
        
        private string EncryptPassword(string _password) {
            var data = System.Text.Encoding.ASCII.GetBytes(_password);
            data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
            var hash = System.Text.Encoding.ASCII.GetString(data);
            return hash;
        }
    }
}