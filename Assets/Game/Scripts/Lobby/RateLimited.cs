using System.Collections;
using UnityEngine;

namespace Game.Scripts.Lobby {
    public class RateLimited {
        private MonoBehaviour _invoker;
        private float _time;
        private int _calls;

        private int _currentCalls;
        private Coroutine _resetCoroutine;
        
        public RateLimited(MonoBehaviour invoker,float time, int calls) {
            this._invoker = invoker;
            this._time = time;
            this._calls = calls;
        }

        public void Call() {
            if (_currentCalls == 0) {
                _invoker.StartCoroutine(ResetCalls());
            }
            
            _currentCalls++;
        }

        IEnumerator ResetCalls() {
            yield return new WaitForSeconds(_time);
            _currentCalls = 0;
        }

        public bool CanInvoke() => _currentCalls < _calls;
    }
}