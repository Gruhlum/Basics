using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics.UI
{
	public interface IDisplayable
	{
		public Sprite Icon
		{
			get;
		}
		public string Name
		{
			get;
		}
	}
}