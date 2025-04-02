using System.Collections;
using System.Collections.Generic;
using HexTecGames.EaseFunctions;
using UnityEngine;
using UnityEngine.Serialization;

namespace HexTecGames.Basics
{
    /// <summary>
    /// Sets this objects position equal to the target's position + offset.
    /// </summary>
    public class FollowTarget : MonoBehaviour
    {
        public Transform Target
        {
            get
            {
                return target;
            }
            set
            {
                target = value;
            }
        }

        public Vector3 Offset
        {
            get
            {
                return this.offset;
            }
            private set
            {
                this.offset = value;
            }
        }

        [SerializeField, FormerlySerializedAs("Target")] private Transform target = default;
        [SerializeField, FormerlySerializedAs("Offset")] private Vector3 offset = default;


        private void Reset()
        {
            Offset = transform.position;
        }

        private void Update()
        {
            if (target != null)
            {
                transform.position = target.transform.position + Offset;
            }
        }

        public void SetOffset(Vector3 offset)
        {
            this.Offset = offset;
        }
        public void SetOffset(Vector3 offset, float duration, EaseFunction easeFunction)
        {
            StartCoroutine(AnimateOffsetChange(this.Offset, offset, duration, easeFunction));
        }

        public void AddOffset(Vector3 offsetToAdd)
        {
            this.Offset += offsetToAdd;
        }
        public void AddOffset(Vector3 offsetToAdd, float duration, EaseFunction easeFunction)
        {
            StartCoroutine(AnimateOffsetChange(this.Offset, this.Offset + offsetToAdd, duration, easeFunction));
        }
        private IEnumerator AnimateOffsetChange(Vector3 startOffset, Vector3 endOffset, float duration, EaseFunction easeFunction)
        {
            float timer = 0;
            while (timer < duration)
            {
                float progress = easeFunction.GetValue(timer / duration);
                this.Offset = Vector3.Lerp(startOffset, endOffset, progress);
                yield return null;
                timer += Time.deltaTime;

            }
        }
    }
}