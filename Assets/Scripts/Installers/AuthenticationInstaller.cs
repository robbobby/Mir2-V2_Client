using System.ComponentModel;
using Login.Authentication;
using Login.Authentication.HttpClient;
using Login.HttpClientAccess;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

public class AuthenticationInstaller : MonoInstaller {
    public override void InstallBindings() {
        Container.Bind<ILoginAuthentication>().To<UnityWebRequestService>().AsSingle().NonLazy();
    }
}
