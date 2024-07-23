using HexTecGames.Basics.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HexTecGames.Basics.Credits
{
	public class CreditsItemDisplayController : DisplayController<CreditsItem>
	{
        private void OnEnable()
        {
            ShuffleCredits();
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