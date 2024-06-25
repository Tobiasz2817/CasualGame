using System;
using System.Collections.Generic;
using Game.Scripts.Drawer;
using UnityEditor;
using UnityEngine;

namespace Game.Scripts.Scene {
    [Serializable]
    [CreateAssetMenu(menuName = "Scene Data")]
    public class SceneData : ScriptableObject {

        [SerializeField] private List<SceneType> sceneTypes;
        [SerializeField] private List<SceneAssetReference> scenes;

        private Dictionary<SceneType, string> _sceneDictionary = new();


        public string GetScenePath(SceneType sceneType) => _sceneDictionary[sceneType];
        public string GetScenePath(int index) => _sceneDictionary[(SceneType)index];
    
        private void OnValidate() {
            _sceneDictionary.Clear();
            ClearDuplications();
        
        
            for (var i = 0; i < sceneTypes.Count; i++) {
                if (i >= scenes.Count || scenes[i] == null ||  string.IsNullOrEmpty(scenes[i].assetPath)) continue;
               
                _sceneDictionary.Add(sceneTypes[i], scenes[i].assetPath);
            }
        
        
            // Registry new scenes
            EditorBuildSettings.scenes = Array.Empty<EditorBuildSettingsScene>();
            if (_sceneDictionary.Count == 0) return;
            var newScenes = new EditorBuildSettingsScene[_sceneDictionary.Count];
            for (int i = 0; i < _sceneDictionary.Count; i++) {
                newScenes[i] = new EditorBuildSettingsScene(scenes[i].assetPath, true);
            }
        
            EditorBuildSettings.scenes = newScenes;
        }

        private void ClearDuplications() {
            for (int i = sceneTypes.Count - 1; i >= 0; i--) {
                var currentType = sceneTypes[i];
                for (int j = sceneTypes.Count - 1; j >= 0; j--) {
                    if(i == j || currentType != sceneTypes[j]) continue;
                    if (sceneTypes[j] == 0) {
                        sceneTypes.RemoveAt(j);
                    
                        continue;
                    }
                
                    sceneTypes[i] = new SceneType();
                }
            }
        
            for (int i = scenes.Count - 1; i >= 0; i--) {
                var currentType = scenes[i].assetPath;
                for (int j = scenes.Count - 1; j >= 0; j--) {
                    if(i == j || scenes[j] == null || !string.Equals(currentType, scenes[j].assetPath)) continue;
                    if (string.IsNullOrEmpty(scenes[i].assetPath)) {
                        scenes.RemoveAt(j);
                    
                        continue;
                    }
                
                    if(i != scenes.Count - 1)
                        scenes.RemoveAt(i);
                    else 
                        scenes[i] = null;
                }
            }
        }
    }
}