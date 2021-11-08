using System.Collections.Generic;
using System.Threading.Tasks;
using SharedModels_Mir2_V2.Enums;
namespace Login.Authentication {
    public interface ILoginAuthentication {

        
        // This interface must also implement the MonoAuthentication
        
        void AttemptLogin(string _idLoginText, string _passwordLoginText);

        public Task<AccountRegisterResult> AttemptRegisterRequest(string _email, string _id, string _password);
    }
}
