using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HexTecGames.Basics.Editor
{
    public abstract class SingleLineDrawer : PropertyDrawer
    {
        protected abstract string PropertyName
        {
            get;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.PropertyField(position, property.FindPropertyRelative(PropertyName), label);
            EditorGUI.EndProperty();
        }
    }
}