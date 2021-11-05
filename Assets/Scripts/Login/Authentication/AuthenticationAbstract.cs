using UnityEngine;
namespace Login.Authentication {
    public abstract class AuthenticationAbstract : ILoginAuthentication {
        protected LoginController loginController;
        
        public LoginController LoginController { get; set; }
        

        public abstract void AttemptLogin(string _id, string _password);
        
        public abstract void SignUp(string _email, string _id, string _password);
    }
}