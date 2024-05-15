using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics.UI.Buttons
{
	[System.Serializable]
	public abstract class ButtonEffect
	{
		public abstract void Apply();
		public abstract void Remove();

		public virtual void OnValidate(GameObject go)
		{ }
	}
}