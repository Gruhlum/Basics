using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace HexTecGames.Basics.Credits
{
	[CreateAssetMenu(menuName = "HexTecGames/Basics/CreditsItem")]
	public class CreditsItem : ScriptableObject
	{
		public string role;
		public string itchURL;
	}
}