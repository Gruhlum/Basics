using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics
{
    public class HideConditionally : MonoBehaviour
    {
        [SerializeField] private GameObject affectedGO = default;

        [SerializeField] private bool hideInEditor = default;
        [SerializeField] private bool hideInBuild = default;

        [SerializeField] private bool hasActivationKey = default;
        [SerializeField][DrawIf("hasActivationKey", true)] private KeyCode activationKey = default;

        private void Awake()
        {
            if (affectedGO == null)
            {
                Debug.LogWarning("No GameObject!");
                return;
            }
            if (hideInEditor && Application.isEditor)
            {
                affectedGO.SetActive(false);
            }
            else if (hideInBuild && !Application.isEditor)
            {
                affectedGO.SetActive(false);
            }
            if (!hasActivationKey)
            {
                gameObject.SetActive(false);
            }
        }
        private void Reset()
        {
            affectedGO = gameObject;
        }
        private void OnValidate()
        {
            if (hasActivationKey && affectedGO == gameObject)
            {
                Debug.LogWarning("Warning: can't activate itself with key, move to child instead");
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(activationKey))
            {
                affectedGO.SetActive(!affectedGO.activeSelf);
            }
        }
    }
}