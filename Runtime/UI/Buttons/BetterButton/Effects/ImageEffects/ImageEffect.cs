using UnityEngine;
using UnityEngine.UI;

namespace HexTecGames.Basics.UI.Buttons
{
    public abstract class ImageEffect : ButtonEffect
    {
        [SerializeField] protected Image img = default;

        public override void OnValidate(GameObject go)
        {
            base.OnValidate(go);
            if (img == null)
            {
                img = go.GetComponent<Image>();
            }
            if (img == null)
            {
                img = go.GetComponentInChildren<Image>();
            }
        }
    }
}