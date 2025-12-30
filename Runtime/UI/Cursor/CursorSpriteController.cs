using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace HexTecGames.Basics.UI
{
    public class CursorSpriteController : AdvancedBehaviour
    {
        [SerializeField] private CursorSpriteData defaultCursor = default;
        [SerializeField] private CursorSpriteData hoverCursor = default;
        [SerializeField] private CursorSpriteData clickCursor = default;
        [Space]
        [SerializeField] private CursorMode cursorMode = default;

        private CursorSpriteData currentCursor;


        protected override void Reset()
        {
            base.Reset();
            if (FindObjectOfType<MouseController>(true) == null)
            {
                Debug.Log("Added MouseController");
                GameObject mouseController = new GameObject("MouseController", typeof(MouseController));
                mouseController.transform.SetParent(null);
            }
        }

        private void Awake()
        {
            ApplyCursor(defaultCursor);
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
                ApplyCursor(clickCursor);
            }
            if (Input.GetMouseButtonUp(0))
            {
                ApplyCursor(hoverCursor);
            }
        }
        private void MouseController_OnPointerOverSelectableChanged(bool overSelectable)
        {
            ApplyCursor(overSelectable ? hoverCursor : defaultCursor);
        }

        private void ApplyCursor(CursorSpriteData data)
        {
            if (data == null || data.Texture == null)
            {
                return;
            }

            if (currentCursor == data)
            {
                return;
            }
            currentCursor = data;
            Cursor.SetCursor(data.Texture, data.Offset, cursorMode);
        }
    }
}