using System.Collections;
using System.Collections.Generic;
using HexTecGames.EaseFunctions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HexTecGames.Basics
{
    public class SelectableAnimator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Transform target = default;
        [SerializeField] private EaseFunction easeFunction = new EaseFunction(EasingType.EaseInOut, FunctionType.Quad);
        [SerializeField] private float speed = 10f;

        [LinkedVector][SerializeField] private Vector3 startSize = Vector3.one;
        [LinkedVector][SerializeField] private Vector3 endSize = new Vector3(1.05f, 1.05f, 1.05f);

        private float progress;
        private Coroutine scaleRoutine;


        private void Reset()
        {
            target = transform;
        }

        private void OnDisable()
        {
            progress = 0;
            target.localScale = startSize;
        }

        private IEnumerator AnimateScale(bool hover)
        {
            float time = progress;
            float direction = hover ? 1f : -1f;

            while ((hover && time < 1f) || (!hover && time > 0f))
            {
                time += Time.deltaTime * speed * direction;
                time = Mathf.Clamp01(time);
                target.localScale = Vector3.Lerp(startSize, endSize, easeFunction.GetValue(time));
                progress = time;
                yield return null;
            }

            scaleRoutine = null;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (scaleRoutine != null) StopCoroutine(scaleRoutine);
            scaleRoutine = StartCoroutine(AnimateScale(true));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (scaleRoutine != null) StopCoroutine(scaleRoutine);
            scaleRoutine = StartCoroutine(AnimateScale(false));
        }
    }
}