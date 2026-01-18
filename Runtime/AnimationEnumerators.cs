using System;
using System.Collections;
using HexTecGames.EaseFunctions;
using UnityEngine;
using UnityEngine.UI;

namespace HexTecGames.Basics
{
    public static class AnimationEnumerators
    {
        public static IEnumerator LerpRoutine<T>(T start, T end, Action<T> applyValue, Func<T, T, float, T> lerpFunc,
            float duration = 1f, EaseFunction easing = null)
        {
            float timer = 0f;
            while (timer < duration)
            {
                timer += Time.deltaTime;
                float t = timer / duration;
                float progress = easing != null ? easing.GetValue(t) : t;
                applyValue(lerpFunc(start, end, progress));
                yield return null;
            }
            applyValue(end);
        }

        #region CanvasGroup
        public static IEnumerator Fade(this CanvasGroup target, float start, float end, float duration = 1f, EaseFunction easing = null)
        {
            return LerpRoutine(start, end, value => target.alpha = value, Mathf.Lerp, duration, easing);
        }
        public static IEnumerator FadeIn(this CanvasGroup target, float duration = 1f, EaseFunction easing = null)
        {
            yield return Fade(target, 0f, 1f, duration, easing);
        }
        public static IEnumerator FadeOut(this CanvasGroup target, float duration = 1f, EaseFunction easing = null)
        {
            yield return Fade(target, target.alpha, 0f, duration, easing);
        }
        #endregion
        #region SpriteRenderer
        public static IEnumerator Fade(this SpriteRenderer target, float startAlpha, float endAlpha, float duration = 1f, EaseFunction easing = null)
        {
            yield return LerpColor(target, target.color.GetColorWithAlpha(startAlpha), target.color.GetColorWithAlpha(endAlpha), duration,  easing);
        }
        public static IEnumerator FadeIn(this SpriteRenderer target, float duration = 1f, EaseFunction easing = null)
        {
            yield return Fade(target, 0f, 1f, duration,  easing);
        }
        public static IEnumerator FadeOut(this SpriteRenderer target, float duration = 1f, EaseFunction easing = null)
        {
            yield return Fade(target, target.color.a, 0f, duration, easing);
        }
        #endregion

        public static IEnumerator LerpColor(this SpriteRenderer target, Color start, Color end, float duration = 1f, EaseFunction easing = null)
        {
            return LerpRoutine(start, end, value => target.color = value, Color.Lerp, duration, easing);
        }

        public static IEnumerator FlashColor(this SpriteRenderer target, Color startColor, Color endColor, float duration = 1f, float repeats = 1f)
        {
            for (int i = 0; i < repeats; i++)
            {
                target.color = startColor;
                yield return new WaitForSeconds(duration);
                target.color = endColor;
                yield return new WaitForSeconds(duration);
            }
        }

        #region Image

        public static IEnumerator Fade(this Image target, float startAlpha, float endAlpha, float duration = 1f, EaseFunction easing = null)
        {
            yield return LerpColor(target, target.color.GetColorWithAlpha(startAlpha), target.color.GetColorWithAlpha(endAlpha), duration, easing);
        }
        public static IEnumerator FadeIn(this Image target, float duration = 1f, EaseFunction easing = null)
        {
            yield return Fade(target, 0f, 1f, duration, easing);
        }

        public static IEnumerator FadeOut(this Image target, float duration = 1f, EaseFunction easing = null)
        {
            yield return Fade(target, target.color.a, 0f, duration, easing);
        }

        public static IEnumerator LerpColor(this Image target, Color start, Color end, float duration = 1f, EaseFunction easing = null)
        {
            return LerpRoutine(start, end, value => target.color = value, Color.Lerp, duration, easing);
        }
        #endregion

        public static IEnumerator ToggleActivationAfterDelay(GameObject target, bool active, float delay)
        {
            yield return new WaitForSeconds(delay);
            target.SetActive(active);
        }
    }
}