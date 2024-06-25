using Unity.Networking.Transport;
using UnityEngine.UI;
using Unity.Entities;
using Unity.NetCode;
using UnityEngine;
using System;
using TMPro;
using Unity.Services.Relay.Models;
using UnityEngine.SceneManagement;

namespace Game.Scripts.Netcode {
    public class Bootstrap : MonoBehaviour {
        [SerializeField] private TMP_Dropdown _connectionDropdownType;
        [SerializeField] private Button _connectButton;
        
        private ushort Port => 7979;
        private string Address => "127.0.0.1";

        private ConnectType _connectType = ConnectType.Host;

        private void Awake() {
            CreateDropdownTypes();
        }

        private void OnEnable() {
            _connectionDropdownType.onValueChanged.AddListener(SetConnectionType);
            _connectButton.onClick.AddListener(Connect);
        }
        
        private void OnDisable() {
            _connectionDropdownType.onValueChanged.RemoveListener(SetConnectionType);
            _connectButton.onClick.RemoveListener(Connect);
        }

        private void CreateDropdownTypes() {
            _connectionDropdownType.options.Clear();
            
            foreach (var connectTypeString in Enum.GetNames(typeof(ConnectType))) {
                var optionData = new TMP_Dropdown.OptionData(connectTypeString);
                _connectionDropdownType.options.Add(optionData);
            }
        }
        
        private void SetConnectionType(int value) {
            _connectType = (ConnectType)value;
        }
        
        private void Connect() {
            SceneManager.LoadScene("Game/Scenes/Game");
            DestroyLocalWorld();

            switch (_connectType) {
                case ConnectType.Host: {
                    StartServer();
                    StartClient();
                }
                    break;
                case ConnectType.Server: 
                    StartServer();
                    break;
                case ConnectType.Client: 
                    StartClient();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static void DestroyLocalWorld() {
            foreach (var world in World.All) {
                if (world.Flags == WorldFlags.Game) {
                    world.Dispose();
                    
                    break;
                }
            }
        }

        private void StartServer() {
            Debug.Log("Starting server connection...");
            var serverWorld = ClientServerBootstrap.CreateServerWorld("Real Server World");

            var endpoint = NetworkEndpoint.AnyIpv4.WithPort(Port);
            {
                using var driverQuery = serverWorld.EntityManager.CreateEntityQuery(ComponentType.ReadWrite<NetworkStreamDriver>());
                driverQuery.GetSingletonRW<NetworkStreamDriver>().ValueRW.Listen(endpoint);
            }
        }

        private void StartClient() {
            Debug.Log("Starting client connection...");
            var clientWorld = ClientServerBootstrap.CreateClientWorld("Real Client World");
            
            var endpoint = NetworkEndpoint.Parse(Address, Port);
            {
                using var driverQuery = clientWorld.EntityManager.CreateEntityQuery(ComponentType.ReadWrite<NetworkStreamDriver>());
                driverQuery.GetSingletonRW<NetworkStreamDriver>().ValueRW.Connect(clientWorld.EntityManager, endpoint);
            }
            
            World.DefaultGameObjectInjectionWorld = clientWorld;
        }

        
        public enum ConnectType {
            Host = 0,
            Server = 1,
            Client = 2
        }
    }
}
