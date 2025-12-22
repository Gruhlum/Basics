using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics.UI
{
    [CreateAssetMenu(menuName = "HexTecGames/Sandbox/TutorialPage")]
    public class TutorialPage : ScriptableObject
    {
        public Sprite screenshot = default;
        [TextArea] public string text = default;
    }
}