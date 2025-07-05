using UnityEngine;

namespace HexTecGames.Basics.Credits
{
    [CreateAssetMenu(menuName = "HexTecGames/Basics/CreditsItem")]
    public class CreditsItem : ScriptableObject
    {
        public string role;
        public string itchURL;
    }
}