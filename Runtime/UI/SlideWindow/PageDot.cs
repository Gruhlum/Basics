using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace HexTecGames.Basics.UI
{
    public class PageDot : AdvancedBehaviour
    {
        [SerializeField] private Image img = default;
        [Space]
        [SerializeField] private Color activeColor = Color.green;
        [SerializeField] private Color inactiveColor = Color.black;


        public void SetActive(bool active)
        {
            img.color = active ? activeColor : inactiveColor;
        }
    }
}