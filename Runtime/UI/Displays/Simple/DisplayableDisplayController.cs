using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics.UI
{
	public class DisplayableDisplayController : DisplayController<DisplayableObject>
	{
        protected virtual void Awake()
        {
            DisplayItems();
        }
    }
}