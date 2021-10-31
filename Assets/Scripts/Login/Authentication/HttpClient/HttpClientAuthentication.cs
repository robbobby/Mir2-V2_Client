namespace Login.Authentication.HttpClient {
    public class HttpClientAuthentication : AuthenticationAbstract {

        public HttpClientAuthentication(LoginController _loginController) : base(_loginController) {
            
        }
        public override void AttemptLogin(string _id, string _password) {
            throw new System.NotImplementedException();
        }
        
        public override void SignUp(string _email, string _id, string _password) {
            throw new System.NotImplementedException();
        }
        
    }
}
