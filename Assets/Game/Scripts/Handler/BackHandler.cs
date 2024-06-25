using Game.Scripts.Scene;
using UnityEngine;

namespace Game.Scripts.Handler {
    public class BackHandler : MonoBehaviour {
        public void BackToBootstrap(SceneType type) => SceneLoader.Instance.Load(type);
    }
}
