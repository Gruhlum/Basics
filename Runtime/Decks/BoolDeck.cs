using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics.Decks
{
    [System.Serializable]
    public class BoolDeck : Deck<bool>
    {
        public BoolDeck(int trueCards, int falseCards) : base(new DeckItem<bool>(true, trueCards), new DeckItem<bool>(false, falseCards))
        { }
    }
}