using System;
using System.Collections;
using System.Threading.Tasks;
using Game.Scripts.JobSystem;
using Game.Scripts.LobbySystem.Service;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;

namespace Game.Scripts.LobbySystem.Controller {
    public class CreateRoomHandler : MonoBehaviour {
        [SerializeField] private TMP_Text _roomId;
        
        private string _roomName = "Room";
        private int _playersCount = 4;

        private Coroutine _createRoomCoroutine;
        
        private void Start() => ReCreateRoom();

        private void OnEnable() {
            KickHandler.OnSuccess += ReCreateRoom;
            GetLobbyService.OnSuccess += UpdateId;
            CreateRoomService.OnSuccess += UpdateId;
        }
        
        private void OnDisable() {
            KickHandler.OnSuccess -= ReCreateRoom;
            GetLobbyService.OnSuccess -= UpdateId;
            CreateRoomService.OnSuccess -= UpdateId;
        }
        
        private void UpdateId(Lobby obj) {
            _roomId.text = obj?.Id;
        }
        
        private void ReCreateRoom() {
            if(_createRoomCoroutine != null)
                StopCoroutine(_createRoomCoroutine);
            
            _createRoomCoroutine = StartCoroutine(ReCreate());
        }

        IEnumerator ReCreate() {
            yield return new WaitForSeconds(2f);
            
            if (LobbyManager.Instance.IsLobbyExist() || JobScheduler.Instance.IsProcessJob(LobbyJobId.CreateRoom)) yield break;
            yield return new WaitUntil(CreateRoomRateLimit);
            JobScheduler.Instance.QueueTask(CreateRoom).WithTask(Wait).
                WithName("Creating room...").
                WithCondition(CreateRoomRateLimit).
                PushInQueue(LobbyJobId.CreateRoom);
            
            Debug.Log("Create Room");
        }

        private Task CreateRoom() => CreateRoomService.CreateRoom(_roomName, _playersCount);
        private Task Wait() => Task.Delay(1000);
        private bool CreateRoomRateLimit() => CreateRoomService.Limited.CanInvoke();
    }
}