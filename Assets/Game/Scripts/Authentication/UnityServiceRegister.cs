using System;
using System.Collections;
using System.Threading.Tasks;
using Game.Scripts.Loader;
using Game.Scripts.Scene;
using Game.Scripts.Utils;
using Unity.Services.Core;
using UnityEngine;

namespace Game.Scripts.Authentication {
    public class UnityServiceRegister : MonoBehaviour {
        private void Awake() {
            Registry();
        }

        private void Registry() {
            var logOffTask = new TaskProcessor(() => TaskExtension.WhenAll(
                 UnityServices.InitializeAsync,
                 () => Task.Delay(2000)
                ));
            
            logOffTask.OnExecute += () => {
                LoaderListener.Instance.Load("Initialize Service...");
            };
            logOffTask.OnFailedExecute += Registry;
            logOffTask.OnSuccessExecute += () => {
                SceneLoader.Instance.Load(SceneType.Signin);
            };
            
            //Test

            logOffTask.ExecuteTask();
        }
    }
    

    public class TaskRunner {
        public Task task;

        public TaskRunner(ref Task tsk) {
            task = tsk;
        }
    }
}