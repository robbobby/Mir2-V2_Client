using UnityEngine;
namespace Login.Authentication {
    public abstract class AuthenticationAbstract : MonoBehaviour, ILoginAuthentication {
        protected LoginController loginController;

        protected AuthenticationAbstract(LoginController _loginController) {
            loginController = _loginController;
        }
        
        public LoginController LoginController { get; set; }
        

        public abstract void AttemptLogin(string _id, string _password);
        
        public abstract void SignUp(string _email, string _id, string _password);
    }
}