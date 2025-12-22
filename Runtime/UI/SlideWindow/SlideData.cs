using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics.UI
{
    [CreateAssetMenu(menuName = "HexTecGames/Basics/UI/SlideData")]
    public class SlideData : ScriptableObject
    {
        public Sprite screenshot = default;
        [TextArea] public string text = default;
    }
}