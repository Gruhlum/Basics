using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics
{
    public interface ISpawnable
    {
        public event Action<ISpawnable> OnDeactivated;
    }
}