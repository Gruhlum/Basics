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

        [SerializeField] private UnityEvent OnLeftClick = default;
        [SerializeField] private UnityEvent OnRightClick = default;
        [SerializeField] private UnityEvent OnMouseEnter = default;


#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
        }
        protected override void OnValidate()
        {
            base.OnValidate();

            if (hoverEffects != null)
            {
                foreach (ButtonEffect effect in hoverEffects)
                {
                    if (effect != null)
                    {
                        effect.OnValidate(gameObject);
                    }
                }
            }
            if (mouseDownEffects != null)
            {
                foreach (ButtonEffect effect in mouseDownEffects)
                {
                    if (effect != null)
                    {
                        effect.OnValidate(gameObject);
                    }
                }
            }
        }
#endif
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
            foreach (ButtonEffect effect in hoverEffects)
            {
                effect.Remove();
            }
            foreach (ButtonEffect effect in mouseDownEffects)
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
            foreach (ButtonEffect effect in mouseDownEffects)
            {
                effect.Remove();
            }
            if (isHovering)
            {
                foreach (ButtonEffect effect in hoverEffects)
                {
                    effect.Apply();
                }
            }

            if (eventData.button == PointerEventData.InputButton.Left)
            {
                OnLeftClick?.Invoke();
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                OnRightClick?.Invoke();
            }
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            isPointerDown = true;
            if (isHovering)
            {
                foreach (ButtonEffect effect in hoverEffects)
                {
                    effect.Remove();
                }
            }
            foreach (ButtonEffect effect in mouseDownEffects)
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
            foreach (ButtonEffect effect in hoverEffects)
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
            foreach (ButtonEffect effect in hoverEffects)
            {
                effect.Remove();
            }
        }
    }
}