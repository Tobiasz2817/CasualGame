using Game.Scripts.Authentication;
using Game.Scripts.Scene;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Scripts.Windows {
    public class ServiceLoading : MonoBehaviour {
        private void Awake() {
            LoaderListener.Instance.Load();
            SceneManager.sceneLoaded += (x, y) => { LoaderListener.Instance.Break(); };
            Authenticate.Instance.OnInitializeService += () => { SceneLoader.Instance.Load(SceneType.Signin); };
            Authenticate.Instance.SigningIn += () => { LoaderListener.Instance.Load(Authenticate.Instance.Operation.OperationMessage); };
            Authenticate.Instance.SignedIn += (x) => {
                if(x == AuthenticateStatus.Success) SceneLoader.Instance.Load();
                else if(x == AuthenticateStatus.Failed) LoaderListener.Instance.Break();
            };
            Authenticate.Instance.SignedOut += (x) => { SceneLoader.Instance.Load(); };
        }
    }
}