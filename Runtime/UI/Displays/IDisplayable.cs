using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics
{
	public interface IDisplayable
	{
        public abstract IntValue FindIntValue(ValType type);
    }
}