
using System;
using UnityEngine;

namespace HexTecGames.Basics
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class GhostObject : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer sr = default;

        public event Action OnMoved;
        public event Action OnDeactivated;
        public event Action OnActivated;

        private void Reset()
        {
            sr = GetComponent<SpriteRenderer>();
        }

        public void Activate(Vector3 pos, Sprite sprite, Color col)
        {
            SetPosition(pos);
            SetSprite(sprite);
            sr.color = col;
            Activate();
        }
        public void Activate(Vector3 pos, Sprite sprite)
        {
            Activate(pos, sprite, Color.white);
        }
        public void Activate()
        {
            gameObject.SetActive(true);
            OnActivated?.Invoke();
        }
        public void SetSprite(Sprite sprite)
        {
            sr.sprite = sprite;
        }
        public void Deactivate()
        {
            gameObject.SetActive(false);
            OnDeactivated?.Invoke();
        }
        public void Rotate(int index)
        {
            transform.eulerAngles = new Vector3(0, 0, -90 * index);
            OnMoved?.Invoke();
        }
        public void SetPosition(Vector3 pos)
        {
            transform.position = pos;
            OnMoved?.Invoke();
        }
    }
}