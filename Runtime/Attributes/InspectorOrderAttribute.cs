using System;
using UnityEngine;


namespace HexTecGames.Basics
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class InspectorOrderAttribute : PropertyAttribute
    {
        public int Order { get; private set; }
        public string Header { get; private set; }

        public InspectorOrderAttribute(int order, string header = null)
        {
            Order = order;
            Header = header;
        }
    }
}