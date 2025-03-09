using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics.UI
{
    public class SubMenu : MonoBehaviour
    {
        public SubMenu PreviousMenu
        {
            get
            {
                return this.previousMenu;
            }
            private set
            {
                this.previousMenu = value;
            }
        }
        [SerializeField] private SubMenu previousMenu = default;

        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }
    }
}