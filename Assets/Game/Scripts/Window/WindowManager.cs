using System.Collections.Generic;
using Game.Scripts.Utils;

namespace Game.Scripts.Window {
    public class WindowManager : Singleton<WindowManager> {
        public WindowType startWindow;

        private WindowPanel _currentWindow;
        public WindowPanel CurrentWindow => _currentWindow;

        private List<WindowPanel> _panels = new List<WindowPanel>();

        public override void Awake() {
            base.Awake();
            var panels = GetComponentsInChildren<WindowPanel>();
            _panels.AddRange(panels);

            EnableWindowWithDisableOthers(GetPanel(startWindow));
        }

        public void EnableWindow(WindowType windowType) => EnableWindow(GetPanel(windowType));
        public void EnableWindow(WindowPanel panel) {
            if (panel == null) return;
            
            if(_currentWindow != null)
                _currentWindow.DisableWindow();
            
            panel.EnableWindow();

            _currentWindow = panel;
        }
        
        public void EnableWindowWithDisableOthers(WindowPanel windowPanel) {
            if (windowPanel == null) return;

            windowPanel.EnableWindow();

            foreach (var panel in _panels) {
                if(panel.Type == windowPanel.Type) continue;
                
                panel.DisableWindow();
            }
            
            _currentWindow = windowPanel;
        }

        public void DisableWindow(WindowType windowType) => DisableWindow(GetPanel(windowType));
        public void DisableWindow(WindowPanel panel) {
            if (panel == null) return;
            
            panel.DisableWindow();
            _currentWindow = null;
        }

        public void DisableWindow() => DisableWindow(_currentWindow);
        public WindowPanel GetPanel(WindowType windowType) {
            foreach (var panel in _panels) {
                if (panel.Type == windowType) {
                    return panel;
                }
            }

            return null;
        }
    }
}