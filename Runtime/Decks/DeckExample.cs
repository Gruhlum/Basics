using HexTecGames.Basics.Decks;
using UnityEngine;

namespace HexTecGames
{
    public class DeckExample : MonoBehaviour
    {
        [Range(0f, 1f)] public float critChance = 0.5f;

        private float lastCritChance = -1;

        private BoolDeck critDeck;


        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Attack();
            }
        }

        private void UpdateCritDeck()
        {
            int critCards = Mathf.RoundToInt(Mathf.Lerp(0, 100, critChance));
            int falseCards = 100 - critCards;
            critDeck = new BoolDeck(critCards, falseCards);
            Debug.Log("Generating Deck");
        }

        private void Attack()
        {
            if (lastCritChance != critChance)
            {
                lastCritChance = critChance;
                UpdateCritDeck();
            }
            if (critDeck.GetNext())
            {
                Debug.Log("Crit!");
            }
            else Debug.Log("no crit");
        }
    }
}