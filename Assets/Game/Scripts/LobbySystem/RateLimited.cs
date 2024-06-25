using System.Threading.Tasks;
using UnityEngine;

namespace Game.Scripts.LobbySystem {
    public class RateLimited {
        public float Time { private set; get; }
        public int Calls { private set; get; }

        public int _currentCalls;
        
        public RateLimited(float time, int calls) {
            this.Time = time;
            this.Calls = calls;
        }

        public void Call() {
            if (_currentCalls == 0) {
                ResetCalls();
            }
            
            _currentCalls++;
        }

        async void ResetCalls() {
            await Task.Delay((int)Time * 1000);
            _currentCalls = 0;
        }

        public bool CanInvoke() => _currentCalls < Calls;
    }
}