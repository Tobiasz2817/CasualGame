using UnityEngine;

namespace Game.Scripts.Utils {
    public class SizeFitter : MonoBehaviour
    {
        [Header("Horizontal Settings")]
        [SerializeField] private HorizontalFitterType horizontalFitterType;
        [SerializeField] private float horizontalFreeSpace = 50;
        
        [Header("Vertical Settings")]
        [SerializeField] private VerticalFitterType verticalFitterType;
        [SerializeField] private float verticalFreeSpace = 50;

        
        private void Awake() 
        {
            if ((RectTransform)transform == null) return;
            if (!IsValidChildCount()) return;
            
            
            InitializeFitters();
        }

        private void InitializeFitters() {
            if(horizontalFitterType == HorizontalFitterType.Process) InitializeHorizontalFitter(); 
            if(verticalFitterType == VerticalFitterType.Process) InitializeVerticalFitter();
            
            var parentRect = ((RectTransform)transform);
            //parentRect.anchoredPosition = new Vector2(0, 0);
        }
        
        private void InitializeHorizontalFitter() {
            var parentRect = (RectTransform)transform;
            var resultMinMax = GetFirstLastRects(FitterType.Horizontal);
            var firstRect = resultMinMax.Item1;        
            var lastRect = resultMinMax.Item2;        

            var fAbsPosX = Mathf.Abs(firstRect.anchoredPosition.x);
            var lAbsPosX = Mathf.Abs(lastRect.anchoredPosition.x);

            var leftWidthSize = firstRect.sizeDelta.x / 2;
            var rightWidthSize = lastRect.sizeDelta.x / 2;
            
            var width = (fAbsPosX + lAbsPosX) + leftWidthSize + rightWidthSize + (horizontalFreeSpace * 2);
            parentRect.sizeDelta = new Vector2(width , parentRect.sizeDelta.y);

            ReChangeChildrenPosition(fAbsPosX,lAbsPosX);
        }
        
        private void InitializeVerticalFitter() {
            var parentRect = (RectTransform)transform;
            var resultMinMax = GetFirstLastRects(FitterType.Vertical);
            var firstRect = resultMinMax.Item1;        
            var lastRect = resultMinMax.Item2;        

            var fAbsPosY = Mathf.Abs(firstRect.anchoredPosition.y);
            var lAbsPosY = Mathf.Abs(lastRect.anchoredPosition.y);

            var leftWidthSize = firstRect.sizeDelta.y / 2;
            var rightWidthSize = lastRect.sizeDelta.y / 2;
            
            var width = (fAbsPosY + lAbsPosY) + leftWidthSize + rightWidthSize + (verticalFreeSpace * 2);
            parentRect.sizeDelta = new Vector2(parentRect.sizeDelta.x , width);

            ReChangeChildrenPosition(fAbsPosY,lAbsPosY);
        }

        private void ReChangeChildrenPosition(float firstPosX, float lastPosX) {
            if (!IsValidChildCount()) return;
            var dis = lastPosX - firstPosX;
            
            for (int i = 0; i < transform.childCount; i++) {
                var child = (RectTransform)transform.GetChild(i);

                var anchoredPosition = child.anchoredPosition;
                anchoredPosition = new Vector2(anchoredPosition.x - (dis / 2), anchoredPosition.y);
                child.anchoredPosition = anchoredPosition;
            }
        }
        
        public (RectTransform, RectTransform) GetFirstLastRects(FitterType fitterType) {
            if (!IsValidChildCount()) return (null, null);
            
            RectTransform firstRect = (RectTransform)transform.GetChild(0);
            RectTransform lastRect = (RectTransform)transform.GetChild(0);
            
            for (int i = 0; i < transform.childCount; i++) {
                var child = (RectTransform)transform.GetChild(i);

                float pos;
                
                switch (fitterType) {
                    case FitterType.Horizontal: {
                        pos = child.anchoredPosition.x;
                
                        if (pos < firstRect.anchoredPosition.x) firstRect = child;
                        if (pos > lastRect.anchoredPosition.x) lastRect = child;
                        break;
                    }
                    case FitterType.Vertical: {
                        pos = child.anchoredPosition.y;
                
                        if (pos < firstRect.anchoredPosition.y) firstRect = child;
                        if (pos > lastRect.anchoredPosition.y) lastRect = child;
                        break;
                    }
                }
            }

            return (firstRect, lastRect);
        }

        private bool IsValidChildCount() => transform.childCount > 0;

        public (float, float) GetMinMaxValues() {
            float minValue = float.MaxValue;
            float maxValue = float.MinValue;
            
            for (int i = 0; i < transform.childCount; i++) {
                var child = (RectTransform)transform.GetChild(i);

                var posX = child.anchoredPosition.x;
                
                if (posX < minValue) minValue = posX;
                if (posX > maxValue) maxValue = posX;
            }

            return (minValue, maxValue);
        }
    }
    
    public enum FitterType
    {
        Horizontal,
        Vertical,
    }

    public enum HorizontalFitterType
    {
        None,
        Process
    }
    
    public enum VerticalFitterType
    {
        None,
        Process
    }
}