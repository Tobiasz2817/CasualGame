using System;
using System.Threading.Tasks;

namespace Game.Scripts.Connection {
    public class AuthOperationSequence : IAuthOperation {
        private Func<Task>[] _authTasks;
        
        public AuthOperationSequence(params Func<Task>[] tasks) {
            _authTasks = tasks;
        }

        public Func<Task> Operation() {
            return null;
        }
    }
}