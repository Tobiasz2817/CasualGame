using System;
using System.Threading.Tasks;
using Game.Scripts.Utils;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace Game.Scripts.Authentication {
    [DefaultExecutionOrder(-999)]
    public class Authenticate : Singleton<Authenticate> {
    
        private AuthenticateStatus _status;
        public AuthenticateStatus Status => _status;
    
        private AuthOperation _operation;
        public AuthOperation Operation => _operation;
    
        public bool IsAuthorized => UnityServices.State == ServicesInitializationState.Initialized && AuthenticationService.Instance.IsAuthorized;

        public bool autoInit = true;
    
        public event Action<AuthenticateStatus> SignedIn;
        public event Action<AuthenticateStatus> SignedOut;
        public event Action OnInitializeService;

        private int _initExceptionTime = 5000;
    


        public override void Awake() {
            base.Awake();
            if (!autoInit) return;
            InitializeService();
        }

        public async void SignInClient(AuthOperation operation) {
            if (IsServiceUnInitialized())
                InitializeService();
            
            _status = AuthenticateStatus.Initializing;
            _operation = operation;
            
            if (IsAuthorized) {
                Debug.LogWarning("Client is signed in");

                _operation.OperationMessage = "Is already signed in";
                SignedIn?.Invoke(AuthenticateStatus.Failed);
                return;
            }
            
            try {
                await TaskExtension.While(() => !IsServiceInitialized(), _initExceptionTime);
                await operation.Action.ExecuteAsync();
                await Task.Delay(1000);

                _status = AuthenticateStatus.Success;
            }
            catch (Exception ex) when (ex is AuthenticationException or RequestFailedException) {
                Debug.LogException(ex);

                _status = AuthenticateStatus.Failed;
                _operation.OperationMessage = _status.ToString();
            }
        
            SignedIn?.Invoke(_status);
        }
        
        //TODO::
        public async void SignOutClient(AuthOperation operation) {
            _status = AuthenticateStatus.Initializing;
            _operation = operation;
            
            if (!IsAuthorized) {
                Debug.LogWarning("Client is not signed");
                return;
            }
            
            try {
                await operation.Action.ExecuteAsync();
                await Task.Delay(1000);
            }
            catch (Exception ex) when (ex is AuthenticationException or RequestFailedException) {
                Debug.LogException(ex);
                _operation.OperationMessage = _status.ToString();
            }
            
            SignedOut?.Invoke(_status);
        }
    
        public bool IsServiceInitialized() {
            return UnityServices.State == ServicesInitializationState.Initialized;
        }
    
        public bool IsServiceInitializing() {
            return UnityServices.State == ServicesInitializationState.Initializing;
        }
    
        public bool IsServiceUnInitialized() {
            return UnityServices.State == ServicesInitializationState.Uninitialized;
        }

        public async void InitializeService() {
            try {
                await UnityServices.InitializeAsync();
            }
            catch (Exception e) {
                Debug.Log(e);
            }
        
            OnInitializeService?.Invoke();
        }
    
        public void SwitchProfileForClone() {
#if UNITY_EDITOR && PARREL_SYNC 
        if (ParrelSync.ClonesManager.IsClone()) {
            string customArgument = ParrelSync.ClonesManager.GetArgument();
            AuthenticationService.Instance.SwitchProfile($"Clone_{customArgument}_Profile");
        }
#endif
        }
    }
}