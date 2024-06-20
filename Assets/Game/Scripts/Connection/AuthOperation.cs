using System;
using System.Threading.Tasks;

namespace Game.Scripts.Connection {
    public class AuthOperation : IAuthOperation {
        private Func<Task> _task;

        public AuthOperation(Func<Task> task) {
            this._task = task;
        }


        public async Task AsyncOperation(params Func<Task>[] tasks) {
            foreach (var task in tasks) 
                await task.Invoke();
        }

        public Func<Task> Operation() {
            return null;
        }
    }
}