using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics.UI
{
	public class DisplayableDisplayController : DisplayController<DisplayableScriptableObject>
	{
        protected virtual void Awake()
        {
            DisplayItems();
        }
    }
}