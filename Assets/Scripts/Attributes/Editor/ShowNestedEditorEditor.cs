/*****************************************************************************
// File Name : EditableSOEditor.cs
// Author : Brandon Koederitz
// Creation Date : 10/25/2025
// Last Modified : 10/25/2025
//
// Brief Description : Draws the nested editor within the base object's editor.
*****************************************************************************/
using UnityEditor;
using UnityEngine;

namespace Snowmentum
{
    [CustomPropertyDrawer(typeof(ShowNestedEditor))]
    public class ShowNestedEditorEditor : PropertyDrawer
    {
        private bool showProperties;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            //base.OnGUI(position, property, label);
            ShowNestedEditor esoAtb = (ShowNestedEditor)attribute;
            Object obj = property.objectReferenceValue;
            EditorGUI.PropertyField(position, property, label);

            if (obj != null)
            {
                // Create the editor for the given object.  Returns null if it cant.
                Editor objEditor = Editor.CreateEditor(obj);

                // Create the foldout for showing the nested propertie's editor.
                showProperties = EditorGUI.Foldout(position, showProperties, "");
                if (objEditor != null && showProperties)
                {
                    EditorGUI.indentLevel++;
                    // Draw the object's editor.
                    objEditor.OnInspectorGUI();
                    EditorGUI.indentLevel--;
                }
            }
        }
    }
}
