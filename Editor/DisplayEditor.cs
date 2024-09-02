//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;

//namespace HexTecGames.Basics.UI
//{
//    [CustomEditor(typeof(Display<>))]
//    public class DisplayEditor : UnityEditor.Editor
//    {
//    	public override void OnInspectorGUI()
//    	{            
//            var display = target as Display<>;
//            if (display.Item != null)
//            {
//                if (GUILayout.Button("Display Item"))
//                {
//                    display.SetItem(display.Item);
//                }
//            }
//            DrawDefaultInspector();
//    	}
//    }
//}