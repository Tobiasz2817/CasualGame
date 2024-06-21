using System.Threading.Tasks;
using Game.Scripts.Scene;
using Game.Scripts.Utils;
using Game.Scripts.Windows;
using Unity.Services.Core;
using UnityEngine;

namespace Game.Scripts.Authentication {
    public class UnityServiceRegister : MonoBehaviour {
        private void Awake() {
            Registry();
        }

        private void Registry() {
            var logOffTask = new TaskProcessor(() => Task.WhenAll(
                UnityServices.InitializeAsync(),
                Task.Delay(2000))
            );
            logOffTask.OnExecute += () => {
                LoaderListener.Instance.Load("Initialize Service...");
            };
            logOffTask.OnFailedExecute += Registry;
            logOffTask.OnSuccessExecute += () => {
                SceneLoader.Instance.Load(SceneType.Signin);
            };

            logOffTask.ExecuteTask();
        }
    }
}