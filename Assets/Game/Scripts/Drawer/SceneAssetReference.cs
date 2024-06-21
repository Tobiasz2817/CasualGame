namespace Game.Scripts.Drawer {
    [System.Serializable]
    public class SceneAssetReference
    {
        public string assetPath;

        public SceneAssetReference(string assetPath)
        {
            this.assetPath = assetPath;
        }
    }
}