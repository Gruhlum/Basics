using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HexTecGames.Basics.UI
{
    public class AdvancedNumericUpDown : NumericUpDown
    {
        
        [SerializeField] private Selectable hardLeftButton = default;
        [SerializeField] private Selectable hardRightButton = default;
       

        public void MoveHardLeft()
        {
            if (IsMinValue(CurrentNumber))
            {
                return;
            }
            SetCurrentNumber(MinNumber);
        }
        public void MoveHardRight()
        {
            if (IsMaxValue(CurrentNumber))
            {
                return;
            }
            SetCurrentNumber(MaxNumber);
        }

        protected override void SetButtonInteractable(int value)
        {
            base.SetButtonInteractable(value);
            if (hardLeftButton != null)
            {
                hardLeftButton.interactable = !IsMinValue(value);
            }   
            if (hardRightButton != null)
            {
                hardRightButton.interactable = !IsMaxValue(value);
            }    
        }
    }
}