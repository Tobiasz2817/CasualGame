using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Scripts.Utils {
    public static class TaskExtension {
        public static async Task While(Func<bool> condition, int frequency = 25, int timeout = -1)
        {
            var waitTask = Task.Run(async () => {
                while (condition()) await Task.Delay(frequency);
            });

            if(waitTask != await Task.WhenAny(waitTask, Task.Delay(timeout)))
                throw new TimeoutException();
        }
        
        public static async Task WhenAll(params Task[] tasks) {
            try {
                foreach (var task in tasks) {
                    await task;
                }
            }
            catch (Exception e) {
                Debug.Log(e);
            }
        }
        
        public static async Task WhenAll(params Func<Task>[] tasks) {
            try {
                foreach (var task in tasks) {
                    await task.Invoke();
                }
            }
            catch (Exception e) {
                Debug.Log(e);
            }
        }
    }
}
