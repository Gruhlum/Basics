using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HexTecGames.Basics
{
    public interface ISetup<T>
    {
        public void Setup(T data);
    }
}