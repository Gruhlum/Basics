using System.Collections;
using System.Collections.Generic;
using HexTecGames.Basics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

namespace HexTecGames.Basics.Editor
{
    [CustomPropertyDrawer(typeof(LinkedVectorAttribute))]
    public class LinkedVectorDrawer : PropertyDrawer
    {
        private bool linkAxes;


        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            if (property.propertyType != SerializedPropertyType.Vector3 
                && property.propertyType != SerializedPropertyType.Vector2
                && property.propertyType != SerializedPropertyType.Vector3Int)
            {
                EditorGUI.PropertyField(position, property, label, true);
                return;
            }

            var attr = (LinkedVectorAttribute)attribute;

            string key = "LinkedVector_" + property.propertyPath;
            linkAxes = EditorPrefs.GetBool(key, attr.LinkAxesByDefault);

            float labelWidth = EditorGUIUtility.labelWidth;
            float iconSize = 18f;
            float spacing = 4f;

            Rect labelRect = new Rect(position.x, position.y, labelWidth - iconSize - spacing, position.height);
            Rect iconRect = new Rect(labelRect.xMax + spacing, position.y + 1, iconSize, iconSize);
            Rect fieldRect = new Rect(iconRect.xMax + spacing, position.y, position.width - labelRect.width - iconSize - spacing * 2, position.height);

            EditorGUI.LabelField(labelRect, label);

            GUIContent icon = EditorGUIUtility.IconContent(linkAxes ? "d_Linked" : "d_Unlinked");
            icon.tooltip = "Enable constrained proportions";

            if (GUI.Button(iconRect, icon, EditorStyles.iconButton))
            {
                linkAxes = !linkAxes;
                EditorPrefs.SetBool(key, linkAxes);
            }

            EditorGUI.BeginChangeCheck();
            ShowPropertyField(position, property, fieldRect);

            EditorGUI.EndProperty();
        }

        private void ShowPropertyField(Rect position, SerializedProperty property, Rect fieldRect)
        {
            if (property.propertyType == SerializedPropertyType.Vector3)
            {
                Vector3 newValue = EditorGUI.Vector3Field(fieldRect, GUIContent.none, property.vector3Value);
                if (EditorGUI.EndChangeCheck())
                {
                    if (linkAxes)
                    {
                        float uniform = newValue.x;
                        newValue = new Vector3(uniform, uniform, uniform);
                    }
                    property.vector3Value = newValue;
                }
            }
            else if (property.propertyType == SerializedPropertyType.Vector2)
            {
                Vector2 newValue = EditorGUI.Vector2Field(fieldRect, GUIContent.none, property.vector2Value);
                if (EditorGUI.EndChangeCheck())
                {
                    if (linkAxes)
                    {
                        float uniform = newValue.x;
                        newValue = new Vector2(uniform, uniform);
                    }
                    property.vector2Value = newValue;
                }
            }
            else if (property.propertyType == SerializedPropertyType.Vector3Int)
            {
                Vector3Int newValue = EditorGUI.Vector3IntField(fieldRect, GUIContent.none, property.vector3IntValue);
                if (EditorGUI.EndChangeCheck())
                {
                    if (linkAxes)
                    {
                        int uniform = newValue.x;
                        newValue = new Vector3Int(uniform, uniform, uniform);
                    }
                    property.vector3IntValue = newValue;
                }
            }
            //else
            //{
            //    EditorGUI.HelpBox(helpBoxRect, $"[LinkedVector3] only supports Vector2, Vector3, or Vector3Int.\n'{property.name}' is of type {property.propertyType}.", MessageType.Warning);
            //}
        }
    }
}