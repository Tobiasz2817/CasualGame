using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

[DefaultExecutionOrder(-999)]
public class Authenticate : Singleton<Authenticate> {
    private AuthenticateStatus _status;
    public AuthenticateStatus Status => _status;
    
    public bool IsAuthorized => UnityServices.State == ServicesInitializationState.Initialized && AuthenticationService.Instance.IsAuthorized;

    public Action OnAuthenticating;
    public Action OnInitializeService;
    public Action<AuthenticateStatus> OnAuthenticate;

    private int _initExceptionTime = 5000;
    
    public bool autoInit = true;

    public override void Awake() {
        base.Awake();
        if (!autoInit) return;
        InitializeService();
    }

    public async void AuthenticateClient(AuthenticateAction operation) {
        if (IsServiceUnInitialized())
            InitializeService();
        
        if (IsAuthorized) {
            Debug.LogWarning("Client is authenticated");
            return;
        }
        
        OnAuthenticating?.Invoke();
        _status = AuthenticateStatus.Initializing;

        try {
            await TaskExtension.While(() => !IsServiceInitialized(), _initExceptionTime);
            await operation.Action.Invoke();
            await Task.Delay(1000);

            _status = AuthenticateStatus.Success;
        }
        catch (Exception ex) when (ex is AuthenticationException or RequestFailedException) {
            Debug.LogException(ex);

            _status = AuthenticateStatus.Failed;
        }
        
        OnAuthenticate?.Invoke(_status);
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
        await UnityServices.InitializeAsync();
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



public interface IAuthOperation {
    Func<Task> Operation();
}

public enum AuthenticateStatus {
    Failed,
    Initializing,
    Success
}

public struct AuthenticateAction {
    public Func<Task> Action;
}
