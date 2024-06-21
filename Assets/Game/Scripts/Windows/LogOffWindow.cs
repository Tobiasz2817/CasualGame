using System;
using Game.Scripts.Authentication;
using Game.Scripts.Scene;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Windows {
    public class LogOffWindow : MonoBehaviour {
        public SceneType backWindow;
        public Button button;

        private void OnEnable() {
            button.onClick.AddListener(LogOff);
        }

        private void OnDisable() {
            button.onClick.RemoveListener(LogOff);
        }

        private void LogOff() {
            SceneLoader.Instance.Next = backWindow;
            LoaderListener.Instance.Load("Logging Off..");
            Authenticate.Instance.SignOutClient();   
        }
    }
}