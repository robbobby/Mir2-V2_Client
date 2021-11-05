using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SharedModels_Mir2_V2.AccountDto;
using SharedModels_Mir2_V2.BaseModels;
using UnityEditor.PackageManager;
using UnityEngine;

namespace HttpClientAccess {
    public class HttpClientService {

        private static string baseUrl = "http://localhost:5000";
        private Account account;


        public async void ShowAccount() {
            using (var httpClient = new HttpClient()) {
                var response = await httpClient.GetAsync(GetRequestString(3));
                if (response.IsSuccessStatusCode) {
                    string json = await response.Content.ReadAsStringAsync();
                    account = JsonConvert.DeserializeObject<Account>(json);
                    Debug.LogError(json);
                } else {
                    Debug.LogError(response.ReasonPhrase);
                }
            }

            if (account != null) {
                PrintAccountDetails();
            } else {
                Debug.Log("Account is null");
            }
        }

        private void PrintAccountDetails() {
            Debug.Log(account.Id);
        }

        private string GetRequestString(int _id) => $"{baseUrl}/Account/GetAccount?{_id}";

        private string RegisterAccountString() => $"{baseUrl}/Account/RegisterNewAccount";

        public async Task AttemptRegisterAccount(string _email, string _id, string _password) {
            using var httpClient = new HttpClient();
            AccountRegisterDtoC2S account = new AccountRegisterDtoC2S("", "", _id, _password, _email);
            var accountPayload = new StringContent(JsonConvert.SerializeObject(account), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(RegisterAccountString(), accountPayload);
            HttpStatusCode statusCode = response.StatusCode;
        }
    }
}
