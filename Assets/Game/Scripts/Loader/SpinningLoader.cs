using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Scripts.Loader {
    public class SpinningLoader : MonoBehaviour
    {
        public GameObject spin;
        public TextMeshProUGUI displayText;
        public CanvasGroup canvasGroup;

        [SerializeField]
        private AnimationCurve openCurve;
        [SerializeField]
        private AnimationCurve closeCurve;

        [SerializeField] 
        private float rotateSpeed = 100;

        [field: SerializeField] private bool DisableOnNewScene { set; get; }

        private float openDuration, closeDuration;
        private WaitForEndOfFrame EndOfFrame;

        private Coroutine _loadingCoroutine;

        public string Message { set; get; }

        private void Awake() {
            DontDestroyOnLoad(this);
            EndOfFrame = new WaitForEndOfFrame();   
            openDuration = openCurve.length;
            closeDuration = closeCurve.length;
            
            SceneManager.sceneLoaded += CallDisableInterface;
        }
        
        private void OnDestroy() {
            SceneManager.sceneLoaded -= CallDisableInterface;
        }
        
        public void UpdateMessage(string message) {
            if (message == Message) return;
            
            Message = message;
            displayText.text = Message;
        }

        private void CallDisableInterface(UnityEngine.SceneManagement.Scene x, LoadSceneMode y) {
            if (!DisableOnNewScene) return;
            
            DisableInterface();
        }
        
        public void EnableInterface() {
            canvasGroup.blocksRaycasts = true;
            ShowLoadingUI();
        }
    
        public void DisableInterface() {
            canvasGroup.blocksRaycasts = false;
            CloseLoadingUI();
        }
    
        private void ShowLoadingUI() {
            if(_loadingCoroutine != null)
                StopCoroutine(_loadingCoroutine);
            
            _loadingCoroutine = StartCoroutine(ShowLoadingUICoroutine());
        }
        
        private void CloseLoadingUI() {
            if(_loadingCoroutine != null)
                StopCoroutine(_loadingCoroutine);
            
            _loadingCoroutine = StartCoroutine(CloseLoadingUICoroutine());
        }

        IEnumerator ShowLoadingUICoroutine() {
            float timer = canvasGroup.alpha;
            int a = 0;
            int b = 1;
            
            canvasGroup.alpha = openCurve.Evaluate(a);

            while (timer < openDuration)
            {
                timer += Time.deltaTime;
                float alpha = Mathf.Lerp(a, b, openCurve.Evaluate(timer));

                canvasGroup.alpha = alpha;
                yield return EndOfFrame;
            }
            
            canvasGroup.alpha = openCurve.Evaluate(b);
        }
        
        IEnumerator CloseLoadingUICoroutine() {
            float timer = 1 - canvasGroup.alpha;
            int a = 0;
            int b = 1;
            
            canvasGroup.alpha = closeCurve.Evaluate(a);

            while (timer < closeDuration)
            {
                timer += Time.deltaTime;
                float alpha = Mathf.Lerp(a, b, closeCurve.Evaluate(timer));

                canvasGroup.alpha = alpha;
                yield return EndOfFrame;
            }
            
            canvasGroup.alpha = closeCurve.Evaluate(b);

            Destroy(gameObject);
        }

        private void Update() {
            if(canvasGroup.alpha > 0)
                Spinning();
        }

        private IEnumerator SpinningSprite() {
            while (canvasGroup.alpha > 0) {
                Spinning();
                yield return null;
            }
        }

        private void Spinning() {
            spin.transform.Rotate(0,0,1 * Time.deltaTime * rotateSpeed);
        }
    }
}
