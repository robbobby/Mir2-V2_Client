using System;
using System.Text;
using System.Threading.Tasks;
using HttpClientAccess;
using Login.Authentication;
using Newtonsoft.Json;
using SharedModels_Mir2_V2.AccountDto;
using SharedModels_Mir2_V2.AccountDto.LoginDto;
using SharedModels_Mir2_V2.Enums;
using UnityEngine;
using UnityEngine.Networking;
namespace Login.HttpClientAccess {
    public class UnityWebRequestService : ILoginAuthentication {
        private static readonly string BaseUrl = "http://localhost:5000";

        private async Task<UnityWebRequest> MakeRequest(AccountRegisterDtoC2S account, UnityWebRequestType requestType) {
            UnityWebRequest getRequest = CreateRequest(RegisterAccountString(), requestType, account);
            UnityWebRequestAsyncOperation webRequestOperation = getRequest.SendWebRequest();
            while (!webRequestOperation.isDone) await Task.Yield();

            return webRequestOperation.webRequest;
        }

        private T MapJsonBodyToObject<T>(string jsonBody) {
            T result = JsonConvert.DeserializeObject<T>(jsonBody);
            return result;
        }

        private UnityWebRequest CreateRequest(string _path, UnityWebRequestType requestType = UnityWebRequestType.Get, object data = null) {
            UnityWebRequest request = new UnityWebRequest(_path, requestType.ToString());

            if (data != null) {
                var jsonString = JsonConvert.SerializeObject(data, Formatting.None);
                var bodyRaw = Encoding.UTF8.GetBytes(jsonString);
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            }

            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            return request;
        }

        private void AttachHeader(UnityWebRequest request, string key, string value) {
            request.SetRequestHeader(key, value);
        }


        private string GetRequestString(int id) {
            return $"{BaseUrl}/Account/GetAccount?{id}";
        }

        private string RegisterAccountString() {
            return $"{BaseUrl}/Account/RegisterNewAccount";
        }

        public void AttemptLogin(string id, string password) {
            AccountLoginDtoC2S account = new AccountLoginDtoC2S(id, password);
        }

        public async Task<AccountRegisterResult> AttemptRegisterRequest(string email, string id, string password) {
            AccountRegisterDtoC2S account = new AccountRegisterDtoC2S("", "", id, password, email);
            UnityWebRequest webRequest = await MakeRequest(account, UnityWebRequestType.Post);

            AccountRegisterResult accountRegisterResult;
            switch (webRequest.result) {
                case UnityWebRequest.Result.Success:
                    return MapJsonBodyToObject<AccountRegisterResult>(webRequest.downloadHandler.text);
                case UnityWebRequest.Result.InProgress:
                    return AccountRegisterResult.InProgress;
                case UnityWebRequest.Result.ConnectionError:
                    return AccountRegisterResult.ConnectionError;
                case UnityWebRequest.Result.ProtocolError:
                    return AccountRegisterResult.ProtocolError;
                case UnityWebRequest.Result.DataProcessingError:
                    return AccountRegisterResult.DataProcessingError;
                default:
                    return AccountRegisterResult.UnknownError;
            }
            Debug.Log(accountRegisterResult);
        }
    }
}
