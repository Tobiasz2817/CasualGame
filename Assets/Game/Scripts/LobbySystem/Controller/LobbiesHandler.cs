using Game.Scripts.LobbySystem.Service;
using Unity.Services.Lobbies.Models;
using Unity.Services.Authentication;
using System.Collections.Generic;
using Game.Scripts.JobSystem;
using Unity.Services.Core;
using System.Collections;
using Unity.Services.Lobbies;
using UnityEngine;

namespace Game.Scripts.LobbySystem.Controller {
    public class LobbiesHandler : MonoBehaviour {
        
        [SerializeField] private GameObject _contentLobbies;
        [SerializeField] private LobbyUnitHandler _lobbyUnitPrefab;

        private float _refreshTime;
        private float _validateRefreshTime = 0.1f;


        private List<LobbyUnitHandler> _currentLobbies;
        private int _startCount = 20;
        
        private void Awake() {
            _currentLobbies = new List<LobbyUnitHandler>();
            ResizeLobbies(_startCount);
            QueryLobbiesService.OnSuccess += UpdateInterface;
        }
        
        private void OnDestroy() {
            QueryLobbiesService.OnSuccess -= UpdateInterface;
        }
        
        private void Start() {
            _refreshTime = QueryLobbiesService.Limited.Time / QueryLobbiesService.Limited.Calls;
        }
        
        private void OnEnable() {
            StartCoroutine(LobbiesRefresher());
        }

        private void OnDisable() {
            StopCoroutine(LobbiesRefresher());
        }

        private void ResizeLobbies(int count) {
            for (int i = 0; i < count; i++) {
                var unit = Instantiate(_lobbyUnitPrefab, _contentLobbies.transform);
                unit.gameObject.SetActive(false);
                _currentLobbies.Add(unit);
            }
        }
        
        private void UpdateInterface(QueryResponse query) {
            if(query.Results.Count > _currentLobbies.Count)
                ResizeLobbies((query.Results.Count - _currentLobbies.Count));
            
            for (int i = 0; i < query.Results.Count; i++) {
                _currentLobbies[i].UpdateLobbyUnit(query.Results[i]);
                _currentLobbies[i].gameObject.SetActive(true);
            }

            var last = query.Results.Count;
            for (int i = last; i < _currentLobbies.Count; i++) {
                _currentLobbies[i].UpdateLobbyUnit(null);
                _currentLobbies[i].gameObject.SetActive(false);
            }
        }

        IEnumerator LobbiesRefresher() {
            yield return new WaitUntil(() => UnityServices.State == ServicesInitializationState.Initialized);
            yield return new WaitForSeconds(1f);
            while (AuthenticationService.Instance.IsSignedIn) {
                var options = new QueryLobbiesOptions {
                    Count = 25,
                    // Filter for open lobbies only
                    Filters = new List<QueryFilter>()
                    {
                        new QueryFilter(
                            field: QueryFilter.FieldOptions.AvailableSlots,
                            op: QueryFilter.OpOptions.GT,
                            value: "0")
                    },
                    // Order by newest lobbies first
                    Order = new List<QueryOrder>()
                    {
                        new QueryOrder(
                            asc: false,
                            field: QueryOrder.FieldOptions.Created)
                    }
                };

                JobScheduler.Instance.QueueTask(() => QueryLobbiesService.QueryLobby(options)).
                    WithCondition(QueryLobbiesService.Limited.CanInvoke).
                    PushNoneQueue(LobbyJobId.QueryLobby);    

                //Debug.Log("Query");
                yield return new WaitForSeconds(_refreshTime + _validateRefreshTime);
            }
        }
    }
}