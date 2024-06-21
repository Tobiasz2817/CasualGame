using Game.Scripts.Utils;
using UnityEngine;

namespace Game.Scripts.Windows {
    [DefaultExecutionOrder(-999)]
    public class LoaderListener : Singleton<LoaderListener> {
        [SerializeField] private SpinningLoader _loaderPrefab;
        private SpinningLoader _loader;
    
        public void Load(string message) {
            if (_loader != null) {
                _loader.Message = message;
                _loader.RefreshInterface();
                _loader.EnableInterface();

                return;
            }
            
            _loader = Instantiate(_loaderPrefab);
            _loader.Message = message;
            _loader.EnableInterface();
        }
        public void Load() => Load("");

        public void UpdateMessage(string message) {
            if (_loader == null) return;
            _loader.Message = message;
        }
        
        public void Break() {
            if (_loader == null) return;
            _loader.DisableInterface();
        }
    }
}