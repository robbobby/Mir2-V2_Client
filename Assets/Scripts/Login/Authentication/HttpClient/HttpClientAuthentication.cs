using System.Collections.Generic;
using HttpClientAccess;
using Login.HttpClientAccess;
using Newtonsoft.Json;
using SharedModels_Mir2_V2.AccountDto;
using SharedModels_Mir2_V2.BaseModels;
using SharedModels_Mir2_V2.Enums;
using UnityEngine;
namespace Login.Authentication.HttpClient {
    public class HttpClientAuthentication : ILoginAuthentication {

        private static string baseUrl = "http://localhost:5000/";

        private UnityWebRequestService httpClientService = new UnityWebRequestService();
        
        public async void AttemptLogin(string _id, string _password) {
            // httpClientService.ShowAccount();
        }
        
        private static string SendGetRequest(int _id) {
            const string getAllAccounts = "Account/GetAccount";
            return $"{baseUrl}{getAllAccounts}?{_id}";
        }

        public AccountRegisterResult AttemptRegisterRequest(string _email, string _id, string _password) {
            // httpClientService.AttemptRegisterAccount(_email, _id, _password);
            Account account = new AccountRegisterDtoC2S("", "", _id, _password, _email);
            // httpClientService.MakeRequest(account);
            return AccountRegisterResult.Success;
        }
    }
}
