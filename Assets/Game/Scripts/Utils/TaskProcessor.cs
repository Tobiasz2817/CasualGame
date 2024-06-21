using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Scripts.Utils {
    public class TaskProcessor {
        public Action OnExecute;
        public Action OnSuccessExecute;
        public Action OnFailedExecute;

        protected Func<Task> _task;
        protected TaskStatus _status;

        public TaskProcessor(Func<Task> task) {
            this._task = task;
        }

        public async void ExecuteTask() {
            try {
                await Execute();
                _status = TaskStatus.Success;
            }
            catch (Exception e) {
                _status = TaskStatus.Failed;
                Debug.Log(e);
            }
        }
        
        protected virtual async Task Execute() {
            OnExecute?.Invoke();
            try {
                await _task.Invoke();
                OnSuccessExecute?.Invoke();
            }
            catch (Exception e) {
                Debug.Log(e);
                OnFailedExecute?.Invoke();
            }
        }
    }
}