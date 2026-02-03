using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics
{
    [System.Serializable]
    public class ColorStack : LayerStack<Color>
    {
        public enum Mode { Combine, Single, Random }
        public Mode CurrentMode
        {
            get
            {
                return currentMode;
            }
            set
            {
                currentMode = value;
            }
        }
        private Mode currentMode;

        public ColorStack(Mode currentMode)
        {
            this.CurrentMode = currentMode;
        }

        protected override Color GetActiveItem(List<Color> results)
        {
            if (CurrentMode == Mode.Combine)
            {
                return results.Combine();
            }
            else if (CurrentMode == Mode.Single)
            {
                return base.GetActiveItem(results);
            }
            else if (CurrentMode == Mode.Random)
            {
                return results.Random();
            }
            Debug.LogError("Invalid Mode");
            return default;
        }
    }
}