using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics
{
    public struct MouseInputData
    {
        public int button;
        public ButtonType type;

        public bool DetectMouseInput()
        {
            if (Input.GetMouseButtonUp(0))
            {
                button = 0;
                type = ButtonType.Up;
                return true;
            }
            if (Input.GetMouseButtonUp(1))
            {
                button = 1;
                type = ButtonType.Up;
                return true;
            }
            if (Input.GetMouseButtonDown(0))
            {
                button = 0;
                type = ButtonType.Down;
                return true;
            }
            if (Input.GetMouseButtonDown(1))
            {
                button = 1;
                type = ButtonType.Down;
                return true;
            }
            type = ButtonType.None;
            return false;
        }
    }
}