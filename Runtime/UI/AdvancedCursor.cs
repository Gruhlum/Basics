using UnityEngine;

namespace HexTecGames.Basics
{
    public class AdvancedCursor : Cursor
    {
        public static bool ForceCursorHide
        {
            get
            {
                return forceCursorHide;
            }
            set
            {
                forceCursorHide = value;
                if (forceCursorHide)
                {
                    Cursor.visible = false;
                }
            }
        }
        private static bool forceCursorHide;

        public static void SetCursorVisiblity(bool active)
        {
            if (ForceCursorHide)
            {
                Cursor.visible = false;
            }
            else Cursor.visible = active;
        }
    }
}