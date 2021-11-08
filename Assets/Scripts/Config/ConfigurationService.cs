using Controllers;
using Login;
using Login.Authentication;
namespace Config {
    public static class ConfigurationService {
        private const AuthenticationProvider AuthenticationProvider = global::Config.AuthenticationProvider.DefaultHttpClient;

        public static ILoginAuthentication GetAuthenticationProvider(LoginController _loginController) {
            return AccountServiceConfig.GetAuthenciationProvider(AuthenticationProvider, _loginController);
        }
    }
}