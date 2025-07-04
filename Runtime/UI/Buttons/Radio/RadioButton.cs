using System;
using UnityEngine;
using UnityEngine.UI;

namespace HexTecGames.Basics.UI.Buttons
{
    public class RadioButton : MonoBehaviour
    {
        public enum AnimationType { ColorSwap, SpriteSwap }

        [SerializeField] private Button btn = default;
        [SerializeField] private Image img = default;

        public KeyCode Hotkey
        {
            get
            {
                return hotkey;
            }
            private set
            {
                hotkey = value;
            }
        }
        [Space][SerializeField] private KeyCode hotkey = default;

        public bool Active
        {
            get
            {
                return active;
            }
            private set
            {
                if (active == value)
                {
                    return;
                }
                active = value;
            }
        }
        private bool active = default;

        [SerializeField] private AnimationType animationType = default;

        [SerializeField][DrawIf("animationType", AnimationType.ColorSwap)] private Color normalColor = Color.white;
        [SerializeField][DrawIf("animationType", AnimationType.ColorSwap)] private Color activeColor = Color.white;

        [SerializeField][DrawIf("animationType", AnimationType.SpriteSwap)] private Sprite normalSprite = default;
        [SerializeField][DrawIf("animationType", AnimationType.SpriteSwap)] private Sprite activeSprite = default;

        public event Action<RadioButton> OnClicked;


        private void Reset()
        {
            btn = GetComponent<Button>();
            img = GetComponent<Image>();
        }

        private void OnValidate()
        {
            if (img != null)
            {
                img.color = normalColor;
            }
        }

        private void Awake()
        {
            btn.onClick.AddListener(delegate { OnButtonClicked(); });
        }
        private void OnButtonClicked()
        {
            OnClicked?.Invoke(this);
        }
        public void SetActive(bool active)
        {
            Active = active;
            if (animationType == AnimationType.ColorSwap)
            {
                SetColor(active);
            }
            else SetSprite(active);
        }
        private void SetColor(bool active)
        {
            if (active)
            {
                img.color = activeColor;
            }
            else img.color = normalColor;
        }
        private void SetSprite(bool active)
        {
            if (active)
            {
                img.sprite = activeSprite;
            }
            else img.sprite = normalSprite;
        }
        public void SimulateClick()
        {
            btn.onClick?.Invoke();
        }
    }
}