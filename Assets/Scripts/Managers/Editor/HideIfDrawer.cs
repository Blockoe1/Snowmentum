/*****************************************************************************
// File Name : SoundEditor.cs
// Author : Brandon Koederitz
// Creation Date : 10/15/2025
// Last Modified : 10/15/2025
//
// Brief Description : Custom PropertyDrawer for AudioManager sounds that lets you toggle a randomized or non-random
// sound.
*****************************************************************************/
using UnityEditor;
using UnityEngine;

namespace Snowmentum
{
    [CustomPropertyDrawer(typeof(HideIf))]
    public class HideIfDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Convert the attribute on this property to a HideIf attribute.
            HideIf atb = attribute as HideIf;

            // Get the bool property that we're using to determine the show status of this property.
            SerializedProperty boolProperty = property.serializedObject.FindProperty(atb.BoolFieldName);

            // If the bool is true, then draw the corresponding property field.
            if (boolProperty != null && !boolProperty.boolValue)
            {
                EditorGUI.PropertyField(position, property, label, true);
            }
        }

        /// <summary>
        /// If this property is hidden, the height should be 0.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="label"></param>
        /// <returns></returns>
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // Convert the attribute on this property to a HideIf attribute.
            HideIf atb = attribute as HideIf;

            // Get the bool property that we're using to determine the show status of this property.
            SerializedProperty boolProperty = property.serializedObject.FindProperty(atb.BoolFieldName);

            // If the bool is true, then this property is hidden and should have a height of 0.
            if (boolProperty != null && boolProperty.boolValue)
            {
                return 0;
            }

            return base.GetPropertyHeight(property, label);
        }
    }
}
