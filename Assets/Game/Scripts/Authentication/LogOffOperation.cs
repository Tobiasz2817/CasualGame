using Unity.Services.Authentication;
using System.Threading.Tasks;
using System;

namespace Game.Scripts.Authentication {
    public class LogOffOperation : IAuthOperation {
        private int _time;
        
        public LogOffOperation(int timeAfterLogOff = 2000) {
            _time = timeAfterLogOff;
        }
        
        public async Task ExecuteAsync() {
            AuthenticationService.Instance.SignOut();
            
            try {
                await Task.Delay(_time);
            }
            catch (Exception) {
                //ignore
            }
        }
    }
}