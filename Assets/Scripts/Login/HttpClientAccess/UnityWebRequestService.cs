using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HttpClientAccess;
using Login.Authentication;
using ModestTree;
using Newtonsoft.Json;
using SharedModels_Mir2_V2.AccountDto;
using SharedModels_Mir2_V2.BaseModels;
using SharedModels_Mir2_V2.Enums;
using UnityEngine;
using UnityEngine.Networking;

namespace Login.HttpClientAccess {
    public class UnityWebRequestService : ILoginAuthentication {
        private static string baseUrl = "http://localhost:5000";

        private async Task MakeRequest<T>(AccountRegisterDtoC2S _account, UnityWebRequestType _requestType) {
            UnityWebRequest getRequest = CreateRequest(RegisterAccountString(), UnityWebRequestType.Post, _account);
            UnityWebRequestAsyncOperation x = getRequest.SendWebRequest();
            Debug.Log("Hello");
            while (!x.isDone) {
                await Task.Yield();
            }
            Debug.Log(getRequest.result);
            
            if (getRequest.result == UnityWebRequest.Result.Success) {
                AccountRegisterResult y = JsonConvert.DeserializeObject<AccountRegisterResult>(getRequest.downloadHandler.text);
                Debug.Log(y);
            }
        }
        
        private UnityWebRequest CreateRequest(string _path, UnityWebRequestType _requestType = UnityWebRequestType.Get, object _data = null) {
            UnityWebRequest request = new UnityWebRequest(_path, _requestType.ToString());
            
            if(_data != null) {
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


        private string GetRequestString(int _id) => $"{baseUrl}/Account/GetAccount?{_id}";

        private string RegisterAccountString() => $"{baseUrl}/Account/RegisterNewAccount";

        public void AttemptLogin(string _id, string _password) {
            throw new System.NotImplementedException();
        }
        
        public AccountRegisterResult AttemptRegisterRequest(string _email, string _id, string _password) {
            AccountRegisterDtoC2S account = new AccountRegisterDtoC2S("", "", _id, _password, _email);
            var x =  MakeRequest<AccountRegisterResult>(account, UnityWebRequestType.Post);
            return AccountRegisterResult.Success;
        }
    }
}
