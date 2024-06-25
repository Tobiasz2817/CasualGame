using Game.Scripts.JobSystem;
using Game.Scripts.Loader;
using UnityEngine;

namespace Game.Scripts.LobbySystem {
    public class ViewJobMessage : MonoBehaviour {

        private bool _lastState = true;
        private bool _currentState = true;
        private void Update() {
            if (JobScheduler.Instance == null) return;
            
            var current = JobScheduler.Instance.Current;
            if(current != null)
                LoaderListener.Instance.UpdateMessage(current.Name);
            
            _currentState = current == null;
            if (_currentState == _lastState) return;
            _lastState = _currentState;

            if (current == null) 
                LoaderListener.Instance.Break();
            else 
                LoaderListener.Instance.Load(current.Name);
        }
    }
}