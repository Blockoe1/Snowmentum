/*****************************************************************************
// File Name : EditableSOEditor.cs
// Author : Brandon Koederitz
// Creation Date : 10/25/2025
// Last Modified : 10/25/2025
//
// Brief Description : Draws the nested editor within the base object's editor.
*****************************************************************************/
using System;
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
            UnityEngine.Object obj = property.objectReferenceValue;
            EditorGUI.PropertyField(position, property, label);

            // Dont draw the nested editor of the serializedObject and our property object are the same type or
            // inherit from each other, since that will cause an infinite loop.
            if (obj != null && !IsClassOrSubclass(obj.GetType(), property.serializedObject.targetObject.GetType()))
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

                // Destroy the created editor once we're done with it to prevent errors.
                UnityEngine.Object.DestroyImmediate(objEditor);
            }
        }

        /// <summary>
        /// Checks if two types are the same or if they are subclasses of each other.
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <returns></returns>
        private static bool IsClassOrSubclass(Type t1, Type t2)
        {
            return t1 == t2 || t1.IsSubclassOf(t2) || t2.IsSubclassOf(t1);
        }
    }
}
