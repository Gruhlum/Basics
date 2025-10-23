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
        [SerializeField] private EaseFunction easeFunction = new EaseFunction(EasingType.EaseInOut, FunctionType.Quad);
        [SerializeField] private float speed = 10f;

        [LinkedVector][SerializeField] private Vector3 startSize = Vector3.one;
        [LinkedVector][SerializeField] private Vector3 endSize = new Vector3(1.05f, 1.05f, 1.05f);
        [LinkedVector][SerializeField] private int endSi2ze;

        private float progress;
        private Coroutine scaleRoutine;

        private void OnDisable()
        {
            progress = 0;
        }

        private IEnumerator AnimateScale(bool hover)
        {
            float t = progress;
            float direction = hover ? 1f : -1f;

            while ((hover && t < 1f) || (!hover && t > 0f))
            {
                t += Time.deltaTime * speed * direction;
                t = Mathf.Clamp01(t);
                transform.localScale = Vector3.Lerp(startSize, endSize, easeFunction.GetValue(t));
                progress = t;
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