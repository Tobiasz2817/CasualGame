using System;
using System.Threading.Tasks;

namespace Game.Scripts.Authentication {
    public struct AuthenticateAction {
        public Func<Task> Action;
    }
}