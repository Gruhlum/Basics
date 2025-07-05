using UnityEngine;
using UnityEngine.UI;

namespace HexTecGames.Basics.UI.Effects
{
    public class ScrollingImage : MonoBehaviour
    {
        [SerializeField] private RawImage rawImage;
        [SerializeField] private Vector2 direction;

        private void Update()
        {
            rawImage.uvRect = new Rect(rawImage.uvRect.position + (direction * Time.deltaTime), rawImage.uvRect.size);
        }
    }
}