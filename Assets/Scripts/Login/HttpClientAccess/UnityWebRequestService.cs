using System;
using System.Text;
using System.Threading.Tasks;
using HttpClientAccess;
using Login.Authentication;
using Newtonsoft.Json;
using SharedModels_Mir2_V2.AccountDto;
using SharedModels_Mir2_V2.Enums;
using UnityEngine;
using UnityEngine.Networking;
namespace Login.HttpClientAccess {
    public class UnityWebRequestService : ILoginAuthentication {
        private static readonly string baseUrl = "http://localhost:5000";

        private async Task<UnityWebRequest> MakeRequest(AccountRegisterDtoC2S _account, UnityWebRequestType _requestType) {
            UnityWebRequest getRequest = CreateRequest(RegisterAccountString(), _requestType, _account);
            UnityWebRequestAsyncOperation webRequestOperation = getRequest.SendWebRequest();
            while (!webRequestOperation.isDone) await Task.Yield();

            return webRequestOperation.webRequest;
        }

        private T MapJsonBodyToObject<T>(string jsonBody) {
            T result = JsonConvert.DeserializeObject<T>(jsonBody);
            return result;
        }

        private UnityWebRequest CreateRequest(string _path, UnityWebRequestType _requestType = UnityWebRequestType.Get, object _data = null) {
            UnityWebRequest request = new UnityWebRequest(_path, _requestType.ToString());

            if (_data != null) {
                var jsonString = JsonConvert.SerializeObject(_data, Formatting.None);
                var bodyRaw = Encoding.UTF8.GetBytes(jsonString);
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            }

            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            return request;
        }

        private void AttachHeader(UnityWebRequest _request, string _key, string _value) {
            _request.SetRequestHeader(_key, _value);
        }


        private string GetRequestString(int _id) {
            return $"{baseUrl}/Account/GetAccount?{_id}";
        }

        private string RegisterAccountString() {
            return $"{baseUrl}/Account/RegisterNewAccount";
        }

        public void AttemptLogin(string _id, string _password) {
            throw new NotImplementedException();
        }

        public async Task<AccountRegisterResult> AttemptRegisterRequest(string _email, string _id, string _password) {
            AccountRegisterDtoC2S account = new AccountRegisterDtoC2S("", "", _id, _password, _email);
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
