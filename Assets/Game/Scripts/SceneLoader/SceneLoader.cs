using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader> {
    public SceneData sceneData;

    public void Load(SceneType sceneType) => SceneManager.LoadScene(sceneData.GetScenePath(sceneType));
    public void Load(int index) => SceneManager.LoadScene(sceneData.GetScenePath(index));
}
