using Game.Scripts.Scene;
using UnityEngine;

namespace Game.Scripts.Windows {
    public class BackWindow : MonoBehaviour {

        public void BackToBootstrap(SceneType type) => SceneLoader.Instance.Load(type);

    }
}
