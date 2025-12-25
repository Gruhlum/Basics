using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace HexTecGames.Basics.UI
{
    public class CursorDe : AdvancedBehaviour
    {
        [SerializeField] private CursorSpriteData defaultCursor = default;
        [SerializeField] private CursorSpriteData hoverCursor = default;
        [SerializeField] private CursorSpriteData clickCursor = default;
        [Space]
        [SerializeField] private MouseController mouseController = default;
        [Space]
        [SerializeField] private CursorMode cursorMode = default;


        //protected override void Reset()
        //{
        //    base.Reset();
        //    if (FindObjectOfType<MouseController>() == null)
        //    {
        //        Debug.LogWarning("No MouseController found in the scene!");
        //    }
        //}

        private void Awake()
        {
            SetSprite(defaultCursor);
        }
        private void OnEnable()
        {
            MouseController.OnPointerOverSelectableChanged += MouseController_OnPointerOverSelectableChanged;
        }

        private void OnDisable()
        {
            MouseController.OnPointerOverSelectableChanged -= MouseController_OnPointerOverSelectableChanged;
        }

        private void Update()
        {
            if (!MouseController.PointerOverSelectable)
            {
                return;
            }
            if (Input.GetMouseButtonDown(0))
            {
                SetSprite(clickCursor);
            }
            if (Input.GetMouseButtonUp(0))
            {
                SetSprite(hoverCursor);
            }
        }
        private void MouseController_OnPointerOverSelectableChanged(bool overSelectable)
        {
            if (overSelectable)
            {
                SetSprite(hoverCursor);
            }
            else SetSprite(defaultCursor);
        }

        public void SetSprite(Texture2D texture)
        {
            Cursor.SetCursor(texture, Vector2.zero, cursorMode);
        }
        public void SetSprite(CursorSpriteData data)
        {
            Cursor.SetCursor(data.Texture, data.Offset, cursorMode);
        }
    }
}