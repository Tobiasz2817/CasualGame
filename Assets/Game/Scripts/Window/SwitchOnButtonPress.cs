using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Window {
    public class SwitchOnButtonPress : MonoBehaviour {
        [SerializeField] private WindowPanel _panel;
        [SerializeField] private Button _button;
        private void Awake() {
            _button.onClick.AddListener(SwitchPanel);
        }
        
        private void OnDestroy() {
            _button.onClick.AddListener(SwitchPanel);
        }
        
        private void SwitchPanel() {
            WindowManager.Instance.EnableWindow(_panel);
        }
    }
}