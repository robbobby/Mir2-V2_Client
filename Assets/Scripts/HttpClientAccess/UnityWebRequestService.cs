using System.Collections;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using SharedModels_Mir2_V2.BaseModels;
using UnityEngine;
using UnityEngine.Networking;
namespace HttpClientAccess {
    public class UnityWebRequestService {
        private static string baseUrl = "http://localhost:5000";

        public void MakeRequest(Account _account) {
            UnityWebRequest getRequest = CreateRequest(RegisterAccountString(), UnityWebRequestType.Post, _account);
            getRequest.SendWebRequest();
            Account deserializedGetData = JsonUtility.FromJson<Account>(getRequest.downloadHandler.text);
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
    }
}
