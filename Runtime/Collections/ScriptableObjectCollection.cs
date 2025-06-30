using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace HexTecGames.Basics
{
    public abstract class ScriptableObjectCollection<T> : ScriptableObjectCollectionBase<T> where T : class
    {
        protected override List<T> Items
        {
            get
            {
                return items;
            }
            set
            {
                items = value;
            }
        }
        [SerializeField, SerializeReference] private List<T> items = new List<T>();




        //public T FindItem(string name)
        //{
        //    foreach (var item in items)
        //    {
        //        if (item.name == name)
        //        {
        //            return item;
        //        }
        //    }
        //    return null;
        //}

    }
}