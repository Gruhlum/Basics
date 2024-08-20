using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HexTecGames
{
    [RequireComponent(typeof(Button))]
    public abstract class BaseButton : MonoBehaviour
	{
        [SerializeField] private Button btn = default;

        protected virtual void Reset()
        {
            btn = GetComponent<Button>();
#if UNITY_EDITOR
            UnityEditor.Events.UnityEventTools.AddPersistentListener(btn.onClick, OnClicked);
#endif
        }

        public void OnClicked()
        {
            ClickEffect();
        }
        protected abstract void ClickEffect();
    }
}