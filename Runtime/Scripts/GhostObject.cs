
using System.Collections;
using UnityEngine;

namespace HexTecGames.Basics
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class GhostObject : MonoBehaviour
	{
        [SerializeField] private SpriteRenderer sr = default;

        private void Reset()
        {
            sr = GetComponent<SpriteRenderer>();
        }

        public void Activate(Vector3 pos, Sprite sprite, Color col)
        {
            Activate(pos, sprite);
            sr.color = col;
        }
        public void Activate(Vector3 pos, Sprite sprite)
        {
            SetPosition(pos);
            SetSprite(sprite);
            sr.color = Color.white;
            gameObject.SetActive(true);
        }
        public void SetSprite(Sprite sprite)
        {
            sr.sprite = sprite;
        }
        public void Deactivate()
        {
            gameObject.SetActive(false);
        }

        public void SetPosition(Vector3 pos)
        {
            transform.position = pos;
        }
    }
}