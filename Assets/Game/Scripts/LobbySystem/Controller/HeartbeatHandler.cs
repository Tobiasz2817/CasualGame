using System.Collections;
using Game.Scripts.JobSystem;
using Game.Scripts.LobbySystem.Service;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace Game.Scripts.LobbySystem.Controller {
    public class HeartbeatHandler : MonoBehaviour {

        private Coroutine _heartbeatCoroutine;
        
        // RateLimit
        private bool _canBeat = true;
        private float _refreshTime;

        private void Awake() {
            _refreshTime = HeartbeatService.Limited.Time / HeartbeatService.Limited.Calls;
        }

        private void OnEnable() {
            DeleteRoomService.OnSuccess += StopInvokeHeartbeat;
            RemovePlayerService.OnSuccess += StopInvokeHeartbeat;
            CreateRoomService.OnSuccess += StartInvokeHeartbeat;
        }

        private void OnDisable() {
            DeleteRoomService.OnSuccess -= StopInvokeHeartbeat;
            RemovePlayerService.OnSuccess -= StopInvokeHeartbeat;
            CreateRoomService.OnSuccess -= StartInvokeHeartbeat;
        }
        
        private void StartInvokeHeartbeat(Lobby lobby) {
            if (!LobbyManager.Instance.IsLobbyHost(lobby.HostId)) return;

            _heartbeatCoroutine = StartCoroutine(HeartbeatRoom());
        }

        private void StopInvokeHeartbeat() {
            if (_heartbeatCoroutine == null) return;
            
            StopCoroutine(_heartbeatCoroutine);
        }

        private IEnumerator HeartbeatRoom() {
            while (LobbyManager.Instance.IsLobbyExist()) {
                var lobby = LobbyManager.Instance.Data.Lobby;
                if (LobbyManager.Instance.IsLobbyHost()) {
                    //Debug.Log("Heartbeat");
                    yield return new WaitUntil(() => _canBeat);
                    JobScheduler.Instance.QueueTask(() => HeartbeatService.Heartbeat(lobby.Id)).
                        WithCondition(() => HeartbeatService.Limited.CanInvoke()).
                        WithCallback(
                            onFailed: () => StartCoroutine(UnlockHeartbeat()),
                            onSuccess: () => StartCoroutine(UnlockHeartbeat())).
                        PushNoneQueue(LobbyJobId.HeartBeat);
                    
                    _canBeat = false;
                }

                yield return null;
            }
        }

        private IEnumerator UnlockHeartbeat() {
            yield return new WaitForSeconds(_refreshTime + 0.2f);
            _canBeat = true;
        }
    }
}