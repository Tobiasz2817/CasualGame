using System.Threading.Tasks;
using Game.Scripts.JobSystem;
using Game.Scripts.Loader;
using Game.Scripts.LobbySystem;
using Game.Scripts.LobbySystem.Service;
using Game.Scripts.Scene;
using Game.Scripts.Utils;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Handler {
    public class LogOffHandler : MonoBehaviour {
        public SceneType backWindow;
        public Button button;

        private void OnEnable() {
            button.onClick.AddListener(LogOff);
        }

        private void OnDisable() {
            button.onClick.RemoveListener(LogOff);
        }
        
        

        private void LogOff() {
            
            
            SceneLoader.Instance.Next = backWindow;
            var logOffTask = new TaskProcessor(() => TaskExtension.WhenAll(
                () => Task.Run(() => AuthenticationService.Instance.SignOut()),
                () => Task.Delay(2000))
            );
            
            logOffTask.OnExecute += () => {
                LoaderListener.Instance.Load("Logging off...");
            };
            logOffTask.OnFailedExecute += () => {
                LoaderListener.Instance.Break();
            };
            logOffTask.OnSuccessExecute += () => {
                JobScheduler.Instance.QueueTask(() => DeleteRoomService.DeleteRoom(LobbyManager.Instance.Data.Lobby?.Id)).
                    WithTask(() => Task.Delay(1000)).
                    WithCondition(() => DeleteRoomService.Limited.CanInvoke()).
                    PushInQueue(LobbyJobId.DeleteRoom);
#if UNITY_EDITOR
                AuthenticationService.Instance.ClearSessionToken();
#endif
                SceneLoader.Instance.Load();
            };

            logOffTask.ExecuteTask();
        }
    }
}