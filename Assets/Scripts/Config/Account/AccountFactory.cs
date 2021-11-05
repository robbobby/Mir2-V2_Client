using System;
using Login;
using Login.Authentication;
using Login.Authentication.HttpClient;
namespace Config {
    public class AccountServiceConfig {
        public static ILoginAuthentication GetAuthenciationProvider(AuthenticationProvider provider, LoginController _loginController) {
            switch (provider) {
                case AuthenticationProvider.PlayFab:
                    // return new PlayFabAuthentication(_loginController);
                case AuthenticationProvider.DefaultHttpClient:
                    // return new HttpClientAuthentication(_loginController);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(provider), provider, null);
            }
            return null;
        }
    }


    public enum AuthenticationProvider {
        PlayFab,
        DefaultHttpClient
    }
}
