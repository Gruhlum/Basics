using System.Collections.Generic;
using HexTecGames.Basics.UI;
using UnityEngine;

namespace HexTecGames.Basics.Credits
{
    public class CreditsItemDisplayController : DisplayController<CreditsItemDisplay, CreditsItem>
    {
        [SerializeField] private List<CreditsItem> itemsToDisplay = default;

        private bool initialized;


        private void Start()
        {
            SetItems(itemsToDisplay);
            ShuffleCredits();
            initialized = true;
        }

        private void OnEnable()
        {
            if (initialized)
            {
                ShuffleCredits();
            }
        }
        private void ShuffleCredits()
        {
            List<Transform> children = new List<Transform>();
            Transform displayParent = displaySpawner.Parent;

            for (int i = 0; i < displayParent.childCount; i++)
            {
                children.Add(displayParent.GetChild(i));
            }

            children.Shuffle();

            for (int i = 0; i < displayParent.childCount; i++)
            {
                children[i].SetSiblingIndex(i);
            }
        }
    }
}