using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HexTecGames.Basics.UI.Buttons
{
    public class BetterButton : Selectable, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler
    {
        /* events for click and mouseover
		 * list of special effects:
		 * - change text
		 * - change color
		 * - change sprite
		 * 
		 * 
		 * 
		 * 
		 * 
		 * 
		 * 
		 * 
		 */


        [SerializeField, SubclassSelector, SerializeReference]
        private List<ButtonEffect> hoverEffects = default;

        [SerializeField, SubclassSelector, SerializeReference]
        private List<ButtonEffect> mouseDownEffects = default;

        private bool isHovering;
        private bool isPointerDown;

        [SerializeField] private UnityEvent OnClick = default;
        [SerializeField] private UnityEvent OnMouseEnter = default;


        protected override void Reset()
        {
            base.Reset();
        }
        protected override void OnValidate()
        {
            base.OnValidate();

            if (hoverEffects != null)
            {
                foreach (var effect in hoverEffects)
                {
                    if (effect != null)
                    {
                        effect.OnValidate(gameObject);
                    }
                }
            }
            if (mouseDownEffects != null)
            {
                foreach (var effect in mouseDownEffects)
                {
                    if (effect != null)
                    {
                        effect.OnValidate(gameObject);
                    }
                }
            }
        }
        //protected void Update()
        //{
        //    if (!isHovering && Input.GetMouseButtonUp(0))
        //    {
        //        isPointerDown = false;
        //        foreach (var effect in mouseDownEffects)
        //        {
        //            effect.Remove();
        //        }
        //    }
        //}

        protected override void OnDisable()
        {
            base.OnDisable();
            foreach (var effect in hoverEffects)
            {
                effect.Remove();
            }
            foreach (var effect in mouseDownEffects)
            {
                effect.Remove();
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!interactable)
            {
                return;
            }
            isPointerDown = false;
            foreach (var effect in mouseDownEffects)
            {
                effect.Remove();
            }
            if (isHovering)
            {
                foreach (var effect in hoverEffects)
                {
                    effect.Apply();
                }
            }
            OnClick?.Invoke();
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            isPointerDown = true;
            if (isHovering)
            {
                foreach (var effect in hoverEffects)
                {
                    effect.Remove();
                }
            }
            foreach (var effect in mouseDownEffects)
            {
                effect.Apply();
            }
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            if (!interactable)
            {
                return;
            }
            base.OnPointerEnter(eventData);
            isHovering = true;
            if (isPointerDown)
            {
                return;
            }
            foreach (var effect in hoverEffects)
            {
                effect.Apply();
            }
            OnMouseEnter?.Invoke();
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            if (!interactable)
            {
                return;
            }
            base.OnPointerEnter(eventData);
            isHovering = false;
            foreach (var effect in hoverEffects)
            {
                effect.Remove();
            }
        }
    }
}