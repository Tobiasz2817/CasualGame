using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Scene {
    [DefaultExecutionOrder(-1)]
    [RequireComponent(typeof(Button))]
    public class NextScene : MonoBehaviour {
    
        private Button _button;
        public SceneType nextScene;

        private void Awake() {
            _button = GetComponent<Button>();
        }
    
        private void OnEnable() {
            _button.onClick.AddListener(SetNextScene);
        }

        private void OnDisable() {
            _button.onClick.AddListener(SetNextScene);
        }

        public void SetNextScene() => SceneLoader.Instance.Next = nextScene;
    }
}
