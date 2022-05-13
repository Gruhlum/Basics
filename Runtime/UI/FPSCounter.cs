using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace HexTecGames.Basics
{
    public class FPSCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textGUI = default;

        public static bool Show = true;

        private float framesSec;

        private float secTimer;

        private void Awake()
        {
            if (Show == false)
            {
                gameObject.SetActive(false);
            }
        }


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
                framesSec = 0;
                textGUI.text = framesSec.ToString();
            }
        }
    }
}