using System;
using System.Collections;
using Game.Scripts.Utils;
using UnityEngine;

namespace Game.Scripts.Loader {
    [DefaultExecutionOrder(-999)]
    public class LoaderListener : Singleton<LoaderListener> {
        [SerializeField] private SpinningLoader _loaderPrefab;
        private SpinningLoader _loader;
    
        public void Load(string message) {
            if (_loader != null) {
                _loader.UpdateMessage(message);
                _loader.EnableInterface();
                
                return;
            }
            
            _loader = Instantiate(_loaderPrefab);
            _loader.UpdateMessage(message);
            _loader.EnableInterface();
        }
        public void Load() => Load("");

        public void UpdateMessage(string message) {
            if (_loader == null) return;

            _loader.UpdateMessage(message);
        }
        
        public void UpdateMessage(string message, float timeToBreak) {
            if (_loader == null) return;
            
            StopAllCoroutines();
            StartCoroutine(Wait(() => UpdateMessage(message), timeToBreak));
        }
        
        public void Break() {
            if (_loader == null) return;
            _loader.DisableInterface();
        }
        
        public void Break(float timeToBreak) {
            if (_loader == null) return;
            
            StopAllCoroutines();
            StartCoroutine(Wait(Break, timeToBreak));
        }


        public IEnumerator Wait(Action action, float time) {
            yield return new WaitForSeconds(time);
            action?.Invoke();
        }
    }
}