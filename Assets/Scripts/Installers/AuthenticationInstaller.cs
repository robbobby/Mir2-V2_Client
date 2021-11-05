using System.ComponentModel;
using Login.Authentication;
using Login.Authentication.HttpClient;
using UnityEngine;
using Zenject;

public class AuthenticationInstaller : MonoInstaller {
    public override void InstallBindings() {
        Container.Bind<ILoginAuthentication>().To<HttpClientAuthentication>().AsSingle().NonLazy();
    }
}
