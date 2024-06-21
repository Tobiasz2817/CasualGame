using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;

namespace Game.Scripts.Authentication {
    public class AnonymouslyAuthenticator : IAuthOperation {
        public async Task ExecuteAsync() {
            try {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
            catch (Exception) {
                // ignored
            }
        }
    }
}