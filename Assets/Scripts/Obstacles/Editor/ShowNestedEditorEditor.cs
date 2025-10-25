/*****************************************************************************
// File Name : EditableSOEditor.cs
// Author :
// Creation Date : 
// Last Modified : 
//
// Brief Description : 
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

            // Create the foldout for showing the nested propertie's editor.
            showProperties = EditorGUI.Foldout(position, showProperties, label);
            if (obj != null && showProperties)
            {
                EditorGUI.indentLevel++;
                // Draw the object's editor.
                Editor objEditor = Editor.CreateEditor(obj);
                objEditor.OnInspectorGUI();
                EditorGUI.indentLevel--;
            }

        }
    }
}
