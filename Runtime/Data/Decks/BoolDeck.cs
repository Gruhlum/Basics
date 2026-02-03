using UnityEngine;

namespace HexTecGames.Basics.Decks
{
    [System.Serializable]
    public class BoolDeck : Deck<bool>
    {

        public BoolDeck(float percent) : this(Mathf.RoundToInt(percent * 100))
        { }

        public BoolDeck(int trueCards) : this(trueCards, 100 - trueCards)
        { }

        public BoolDeck(int trueCards, int falseCards) : base(new DeckItem<bool>(true, trueCards), new DeckItem<bool>(false, falseCards))
        { }

        public void ChangeOdds(int trueCards)
        {
            base.ChangeOdds(new DeckItem<bool>(true, trueCards), new DeckItem<bool>(false, 100 - trueCards));
        }
        public void ChangeOdds(int trueCards, int falseCards)
        {
            base.ChangeOdds(new DeckItem<bool>(true, trueCards), new DeckItem<bool>(false, falseCards));
        }
    }
}