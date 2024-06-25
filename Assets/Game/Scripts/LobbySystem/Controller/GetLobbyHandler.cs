using Game.Scripts.LobbySystem.Service;
using Unity.Services.Lobbies.Models;
using Game.Scripts.JobSystem;
using System.Collections;
using UnityEngine;

namespace Game.Scripts.LobbySystem.Controller {
    public class GetLobbyHandler : MonoBehaviour {

        private Coroutine _readingCoroutine;
        private float _getLobbyTime;
        
        private void Awake() {
            _getLobbyTime = GetLobbyService.Limited.Time / GetLobbyService.Limited.Calls;
        }

        private void OnEnable() {
            CreateRoomService.OnSuccess += StartReadingLobby;
            JoinRoomService.OnSuccess += StartReadingLobby;
            DeleteRoomService.OnSuccess += StopReadingLobby;
        }
        
        private void OnDisable() {
            CreateRoomService.OnSuccess -= StartReadingLobby;
            JoinRoomService.OnSuccess -= StartReadingLobby;
            DeleteRoomService.OnSuccess -= StopReadingLobby;
        }
        
        private void StartReadingLobby(Lobby obj) {
            if(_readingCoroutine != null)
                StopCoroutine(_readingCoroutine);

            _readingCoroutine = StartCoroutine(ReadingLobby());
        }
        
        private void StopReadingLobby() {
            if (_readingCoroutine == null) return;
            
            StopCoroutine(_readingCoroutine);
        }

        IEnumerator ReadingLobby() {
            while (LobbyManager.Instance.IsLobbyExist()) {
                var lobby = LobbyManager.Instance.Data.Lobby;
                JobScheduler.Instance.QueueTask(() => GetLobbyService.GetLobby(lobby.Id)).
                    WithCondition(() => GetLobbyService.Limited.CanInvoke()).
                    PushNoneQueue(LobbyJobId.GetLobby);
                
                yield return new WaitUntil(() => GetLobbyService.Limited.CanInvoke());
                //Debug.Log("Refresh Lobby ");
            }
        }

        void ReadLobby(Lobby lobby) {
            if (lobby == null) return;
            
            JobScheduler.Instance.QueueTask(() => GetLobbyService.GetLobby(lobby.Id)).
                WithCondition(() => GetLobbyService.Limited.CanInvoke()).
                PushNoneQueue(LobbyJobId.GetLobby);
        }
    }
}