using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.PlayerUI {
    public class InterfacePlayerUnit : MonoBehaviour {
        public Image avatarImage;
        public Sprite avatarSprite;
        
        public void AddPlayer() {
            avatarImage.sprite = avatarSprite;
        }

        public void Destroy() {
            avatarImage.sprite = null;
        }
    }
}