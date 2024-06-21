using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Scene {
    [RequireComponent(typeof(Button))]
    public class LoadSceneOnButton : MonoBehaviour {
        [SerializeField] private SceneType _sceneType;

        private Button _button;

        private void Awake() {
            _button = GetComponent<Button>();
        }

        private void OnEnable() {
            _button.onClick.AddListener(CallLoadScene);
        }

        private void OnDisable() {
            _button.onClick.AddListener(CallLoadScene);
        }

        private void CallLoadScene() => SceneLoader.Instance.Load(_sceneType);
    }
}
