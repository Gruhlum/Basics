using UnityEngine;

namespace HexTecGames.Basics
{
    public class MouseRing : FillRing
    {
        private void Update()
        {
            SetPosition(Camera.main.GetMousePosition());
        }
    }
}