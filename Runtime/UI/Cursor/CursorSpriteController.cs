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

        private void Update()
        {
            if (!MouseController.IsPointerOverUI)
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

        private void OnEnable()
        {
            MouseController.OnPointerOverUIChanged += MouseController_OnPointerOverUIChanged;
        }
        private void OnDisable()
        {
            MouseController.OnPointerOverUIChanged -= MouseController_OnPointerOverUIChanged;
        }
        private void MouseController_OnPointerOverUIChanged(bool overUI)
        {
            if (overUI)
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