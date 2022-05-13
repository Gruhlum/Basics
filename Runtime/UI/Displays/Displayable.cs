using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics
{
	public abstract class Displayable : MonoBehaviour
	{
        public List<IntValue> IntValues
        {
            get
            {
                return intValues;
            }
        }
        [SerializeField] private List<IntValue> intValues;
    }
}