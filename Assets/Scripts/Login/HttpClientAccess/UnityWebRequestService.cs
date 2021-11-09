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
        internal static readonly string BaseUrl = "http://localhost:5000";

        private async Task<UnityWebRequest> MakeRequest<T>(T account, IUnityWebRequest requestType) {
            UnityWebRequest getRequest = CreateRequest(requestType.EndPointAddress, requestType.WebRequestType, account);
            UnityWebRequestAsyncOperation webRequestOperation = getRequest.SendWebRequest();
            while (!webRequestOperation.isDone) await Task.Yield();

            return webRequestOperation.webRequest;
        }

        private T MapJsonBodyToObject<T>(string jsonBody) {
            T result = JsonConvert.DeserializeObject<T>(jsonBody);
            return result;
        }

        private UnityWebRequest CreateRequest(string path, UnityWebRequestType requestType = UnityWebRequestType.Get, object data = null) {
            UnityWebRequest request = new UnityWebRequest(path, requestType.ToString());

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

        public async Task<AccountLoginDtoS2C> AttemptLogin(string id, string password) {
            AccountLoginDtoC2S accountDto = new AccountLoginDtoC2S(id, password);
            UnityWebRequest webRequest = await MakeRequest(accountDto, new RegisterAccount());

            AccountLoginDtoS2C accountReply;

            if (webRequest.result == UnityWebRequest.Result.Success)
                return MapJsonBodyToObject<AccountLoginDtoS2C>(webRequest.downloadHandler.text);
            throw new Exception();
        }

        public async Task<AccountRegisterResult> AttemptRegisterRequest(string email, string id, string password) {
            AccountRegisterDtoC2S account = new AccountRegisterDtoC2S("", "", id, password, email);
            UnityWebRequest webRequest = await MakeRequest(account, new RegisterAccount());

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
    internal interface IUnityWebRequest {
        public UnityWebRequestType WebRequestType { get; set; }
        public string EndPointAddress { get; set; }
    }

    internal class RegisterAccount : IUnityWebRequest {

        public UnityWebRequestType WebRequestType { get; set; }
        public string EndPointAddress { get; set; }
        public RegisterAccount() {
            WebRequestType = UnityWebRequestType.Post;
            EndPointAddress = $"{UnityWebRequestService.BaseUrl}/Account/RegisterNewAccount";
        }
    }
}
