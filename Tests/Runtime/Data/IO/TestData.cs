using System;
using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using UnityEngine;
using UnityEngine.UI;

namespace HexTecGames.Basics.Tests
{
    [Serializable]
    public class TestData
    {
        public string Name;
        public int Value;

        public override bool Equals(object obj)
        {
            if (obj is not TestData other)
                return false;

            return Name == other.Name && Value == other.Value;
        }

        public override int GetHashCode()
        {
            return (Name, Value).GetHashCode();
        }
    }

}