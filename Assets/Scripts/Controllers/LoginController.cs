using Login.Authentication;
using Newtonsoft.Json;
using SharedModels_Mir2_V2.AccountDto;
using TMPro;
using UnityEngine;
using Zenject;
using Button = UnityEngine.UI.Button;
namespace Login {
    public class LoginController : MonoBehaviour {
        [SerializeField] private GameObject loginPage;
        [SerializeField] private GameObject registerPage;
        [SerializeField] private TMP_InputField idLogin;
        [SerializeField] private TMP_InputField passwordLogin;
        [SerializeField] private TMP_InputField idRegister;
        [SerializeField] private TMP_InputField passwordRegister;
        [SerializeField] private TMP_InputField password2Register;
        [SerializeField] private TMP_InputField emailRegister;
        [SerializeField] private Button loginButton;
        [SerializeField] private Button goToRegisterButton;
        [SerializeField] private Button goBackToLoginButton;
        [SerializeField] private Button registerAccountButton;
        [SerializeField] private TMP_Text registerErrorText;
        [SerializeField] private TMP_Text loginResult;
        
        [Inject]
        private ILoginAuthentication authentication;

        public void SetRegisterErrorText(string _text) {
            registerErrorText.SetText(_text);
        }

        public void SetLoginResultText(string _text) {
            loginResult.SetText(_text);
        }

        private void Start() {
        }

        public void RegisterSuccess() {
            ShowLoginPage();
        }

        public void LoginButtonClicked() {
            if (LoginCheck())
                authentication.AttemptLogin(idLogin.text, passwordLogin.text);
        }
        private bool LoginCheck() {
            if (idLogin.text.Contains(" ")) {
                loginResult.SetText("Username cannot contain spaces");
                return false;
            }
            if (idLogin.text.Length < 5) {
                loginResult.SetText("Username must be at least 5 characters long");
                return false;
            }
            if (passwordLogin.text.Length < 6) {
                loginResult.SetText("Password must contain at least 6 characters");
                return false;
            }
            
            return true;
        }


        public void RegisterAccountButtonClicked() {
            if (CheckValidRegisterDetails()) return;
            authentication.SignUp(emailRegister.text, idRegister.text, passwordRegister.text);
            var x = new AccountRegisterDtoC2S("", "", "", "username", "password");
            Debug.Log(JsonConvert.SerializeObject(x));
        }
        private bool CheckValidRegisterDetails() {

            if (passwordRegister.text.Length < 5) {
                registerErrorText.text = "Your password must be at least 5 characters long";
                return true;
            }
            if (passwordRegister.text != password2Register.text) {
                registerErrorText.text = "The passwords don't match";
                return true;
            }
            return false;
        }

        public void GoBackToLoginButtonClicked() {
            ShowLoginPage();
        }
        public void GoToRegisterButtonClicked() {
            ShowRegisterPage();
        }
        private void ShowLoginPage() {
            loginPage.SetActive(true);
            registerPage.SetActive(false);
        }
        private void ShowRegisterPage() {
            loginPage.SetActive(false);
            registerPage.SetActive(true);
        }
        private void ClearRegisterPageInputFields() {
            idRegister.text = "";
            emailRegister.text = "";
            password2Register.text = "";
            passwordRegister.text = "";
        }
        private void ClearLoginPageInputFields() {
            idLogin.text = "";
            passwordLogin.text = "";
        }
        
        
    }
}
