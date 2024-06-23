using UnityEngine;

namespace Game.Scripts.Window {
    public class WindowPanel : MonoBehaviour {
        
        public WindowType Type => _type;
        
        [SerializeField] private WindowType _type;
        
        //TODO:: Fading
        //public CanvasGroup _alpha;
        
        public void EnableWindow() {
            gameObject.SetActive(true);
        }

        public void DisableWindow() {
            gameObject.SetActive(false);
        }
    }
}