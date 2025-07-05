using UnityEditor;
using UnityEngine;

namespace HexTecGames.Basics.Editor
{
    public static class EditorUtility
    {
        private const float SubLabelSpacing = 4;

        public static void DrawMultiplePropertyFields(Rect pos, GUIContent[] subLabels, SerializedProperty[] props)
        {
            // backup gui settings
            int indent = EditorGUI.indentLevel;
            float labelWidth = EditorGUIUtility.labelWidth;

            // draw properties
            int propsCount = props.Length;
            float width = (pos.width - ((propsCount - 1) * SubLabelSpacing)) / propsCount;
            Rect contentPos = new Rect(pos.x, pos.y, width, pos.height);
            EditorGUI.indentLevel = 0;
            for (int i = 0; i < propsCount; i++)
            {
                EditorGUIUtility.labelWidth = EditorStyles.label.CalcSize(subLabels[i]).x + 2;
                EditorGUI.PropertyField(contentPos, props[i], subLabels[i]);
                contentPos.x += width + SubLabelSpacing;
            }

            // restore gui settings
            EditorGUIUtility.labelWidth = labelWidth;
            EditorGUI.indentLevel = indent;
        }
    }
}