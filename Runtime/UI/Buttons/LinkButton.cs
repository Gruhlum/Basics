using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HexTecGames.Basics.UI.Buttons
{
    public class LinkButton : MonoBehaviour
    {
        [SerializeField][TextArea] private string url = default;
        [SerializeField] private Button btn = default;
        [SerializeField] private bool addClickListenerOnAwake = default;

        private void Reset()
        {
            btn = GetComponent<Button>();          
        }
        private void Awake()
        {
            if (btn != null && addClickListenerOnAwake)
            {
                btn.onClick.AddListener(OnClicked);
            }
        }
        public void OnClicked()
        {
            Application.OpenURL(url);
        }
    }
}