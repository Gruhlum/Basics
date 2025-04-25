using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace HexTecGames.Basics.UI.Buttons
{
    public abstract class TextEffect : ButtonEffect
    {
        [SerializeField] protected TMP_Text textGUI = default;

        public override void OnValidate(GameObject go)
        {
            base.OnValidate(go);
            if (textGUI == null)
            {
                textGUI = go.GetClosestComponent<TMP_Text>();
            }
        }
    }
}