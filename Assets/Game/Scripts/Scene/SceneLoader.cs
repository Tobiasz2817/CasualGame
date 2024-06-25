using Game.Scripts.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Scripts.Scene {
    public class SceneLoader : Singleton<SceneLoader> {
        public SceneData SceneData => sceneData;
        public SceneType Next { set; get; }

        [SerializeField] private SceneData sceneData;
        public void Load() => SceneManager.LoadScene(sceneData.GetScenePath(Next));
        public void Load(SceneType sceneType) => SceneManager.LoadScene(sceneData.GetScenePath(sceneType));
        public void Load(int index) => SceneManager.LoadScene(sceneData.GetScenePath(index));
    }
}
