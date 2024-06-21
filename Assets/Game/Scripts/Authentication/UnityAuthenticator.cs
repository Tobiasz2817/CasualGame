using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Authentication.PlayerAccounts;

namespace Game.Scripts.Authentication {
    public class UnityAuthenticator : IAuthOperation {
        private int _delay;
        public UnityAuthenticator(int delayBetweenSignIn) {
            this._delay = delayBetweenSignIn;
        }
        
        public async Task ExecuteAsync() {
            try {
                await PlayerAccountService.Instance.StartSignInAsync();
                await Task.Delay(_delay);
                await AuthenticationService.Instance.SignInWithUnityAsync(PlayerAccountService.Instance.AccessToken);
            }
            catch (Exception) {
                // ignored
            }
        }
    }
}