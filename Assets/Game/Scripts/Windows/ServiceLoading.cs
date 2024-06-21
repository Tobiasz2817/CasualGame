using Game.Scripts.Authentication;
using Game.Scripts.Scene;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace Game.Scripts.Windows {
    public class ServiceLoading : MonoBehaviour {
        private void Awake() {
            LoaderListener.Instance.Load();
            SceneManager.sceneLoaded += (x, y) => { LoaderListener.Instance.Break(); };
            Authenticate.Instance.OnInitializeService += () => { SceneLoader.Instance.Load(SceneType.Signin); };
            Authenticate.Instance.SignedIn += (x) => {
                if(x == AuthenticateStatus.Success) SceneLoader.Instance.Load();
                else if(x == AuthenticateStatus.Failed) LoaderListener.Instance.Break();
            };
            Authenticate.Instance.SignedOut += (x) => { SceneLoader.Instance.Load(); };
        }
    }
}