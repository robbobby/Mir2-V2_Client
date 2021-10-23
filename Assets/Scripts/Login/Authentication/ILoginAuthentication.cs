namespace Login.Authentication {
    public interface ILoginAuthentication {

        
        // This interface must also implement the MonoAuthentication
        public LoginController LoginController { get; set; }
        
        void AttemptLogin(string _idLoginText, string _passwordLoginText);

        public void SignUp(string _email, string _id, string _password);
    }
}
