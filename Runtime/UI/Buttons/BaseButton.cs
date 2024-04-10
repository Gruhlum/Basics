using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HexTecGames
{
	public abstract class BaseButton : MonoBehaviour
	{
        [SerializeField] private Button btn = default;
        [SerializeField] private bool addClickListenerOnAwake = true;

        protected virtual void Reset()
        {
            btn = GetComponent<Button>();
        }
        protected virtual void Awake()
        {
            if (btn != null && addClickListenerOnAwake)
            {
                btn.onClick.AddListener(OnClicked);
            }
        }
        public void OnClicked()
        {
            ClickEffect();
        }
        protected abstract void ClickEffect();
    }
}