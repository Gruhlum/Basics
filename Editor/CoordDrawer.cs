using UnityEditor;
using UnityEngine;

namespace HexTecGames.Basics.Editor
{
    [CustomPropertyDrawer(typeof(Coord))]
    public class CoordDrawer : PropertyDrawer
    {
        //public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        //{
        //    EditorGUI.BeginProperty(position, label, property);

        //    int total = 3;
        //    int index = 0;
        //    //EditorGUI.LabelField(EditorUtility.GetNextSpacedRect(position, ref index, total), "X");
        //    EditorGUI.PropertyField(EditorUtility.GetNextSpacedRect(position, ref index, total), property.FindPropertyRelative("x"), GUIContent.none);
        //    //EditorGUI.LabelField(EditorUtility.GetNextSpacedRect(position, ref index, total), "Y");
        //    EditorGUI.PropertyField(EditorUtility.GetNextSpacedRect(position, ref index, total), property.FindPropertyRelative("y"), GUIContent.none);
        //    EditorGUI.PropertyField(EditorUtility.GetNextSpacedRect(position, ref index, total), property.FindPropertyRelative("y"), GUIContent.none);
        //    EditorGUI.EndProperty();
        //}



        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            label = EditorGUI.BeginProperty(position, label, property);
            Rect contentRect = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            GUIContent[] labels = new[] { new GUIContent("X"), new GUIContent("Y") };
            SerializedProperty[] properties = new[] { property.FindPropertyRelative("x"), property.FindPropertyRelative("y") };
            EditorUtility.DrawMultiplePropertyFields(contentRect, labels, properties);

            EditorGUI.EndProperty();
        }



    }
}