using UnityEngine;

public class BackWindow : MonoBehaviour {

    public void BackToBootstrap(SceneType type) => SceneLoader.Instance.Load(type);

}
