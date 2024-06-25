using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Scripts {
    
    [UpdateInGroup(typeof(SimulationSystemGroup), OrderFirst = true)]
    public partial class InputSystem : SystemBase {
        
        private PlayerInput _playerInput;
        private Entity _self;

        protected override void OnCreate() {
            RequireForUpdate<MoveInput>();
            RequireForUpdate<InputIdentify>();

            //TODO:: EntityManager.AddComponent<Testes>(SystemHandle);
            
            _playerInput = new PlayerInput();
        }

        protected override void OnStartRunning() {
            _playerInput.Enable();
            _playerInput.Player.Movement.performed += MoveAction;
            _playerInput.Player.Movement.canceled += MoveAction;
            
            _self = SystemAPI.GetSingletonEntity<InputIdentify>();
        }

        protected override void OnStopRunning() {
            _playerInput.Player.Movement.performed -= MoveAction;
            _playerInput.Player.Movement.canceled -= MoveAction;
            _playerInput.Disable();
        }

        protected override void OnUpdate() {
            var value = _playerInput.Player.Movement.ReadValue<Vector3>();
            SystemAPI.SetComponent(_self,new MoveInput {
                Direction = value
            });
        }
        
        private void MoveAction(InputAction.CallbackContext obj) {
            SystemAPI.SetComponentEnabled<MoveInput>(_self, obj.performed);
        }
    }
}