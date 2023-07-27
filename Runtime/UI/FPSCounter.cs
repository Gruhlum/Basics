using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace HexTecGames.Basics
{
    public class FPSCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textGUI = default;

        private float framesSec;

        private float secTimer;

        private void Update()
        {
            framesSec++;
        }

        private void FixedUpdate()
        {
            secTimer += Time.fixedDeltaTime;
            if (secTimer >= 1)
            {
                secTimer = 0;
                textGUI.text = framesSec.ToString();
                framesSec = 0;
            }            
        }
    }
}