using System;

namespace Game.Scripts.Scene {
    [Serializable]
    public enum SceneType {
        Bootstrap = 1,
        Signin = 2,
        UnityAuthID = 10,
        MainMenu = 11,
        Game = 12
    }
}