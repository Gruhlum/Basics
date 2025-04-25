using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics.UI.Buttons
{
    [System.Serializable]
    public class GameObjectEffect : ButtonEffect
    {
        [SerializeField] private GameObject gameObject = default;

        public override void Apply()
        {
            gameObject.SetActive(true);
        }

        public override void Remove()
        {
            gameObject.SetActive(false);
        }
    }
}