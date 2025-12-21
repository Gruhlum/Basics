using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace HexTecGames.Basics.UI
{
    public class InputRing : FillRing
    {
        [SerializeField] private KeyCode keyCode = KeyCode.Escape;

        private bool blockInput;

        private void Update()
        {
            if (blockInput && Input.GetKeyDown(keyCode))
            {
                blockInput = false;
            }
            if (!blockInput && Input.GetKey(keyCode))
            {
                IncreaseTime(Time.deltaTime);
            }
            else if (Input.GetKeyUp(keyCode))
            {
                ResetTime();
            }
        }
        protected override void Complete()
        {
            blockInput = true;
            base.Complete();
        }
    }
}