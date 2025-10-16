/*****************************************************************************
// File Name : RandomizedBracketSpriteEditor.cs
// Author : Brandon Koederitz
// Creation Date : 10/16/2025
// Last Modified : 10/16/2025
//
// Brief Description : Custom editor for hiding the bracketSprites field.
*****************************************************************************/
using UnityEditor;
using UnityEngine;

namespace Snowmentum.Editors
{
    [CustomEditor(typeof(RandomizedBracketSprite))]
    public class RandomizedBracketSpriteEditor : Editor
    {
        private SerializedProperty updateOnEnable;
        private SerializedProperty OnNewSprite;
        private SerializedProperty rend;
        private SerializedProperty spriteGroups;


        /// <summary>
        /// Get SerializedProperty References
        /// </summary>
        private void OnEnable()
        {
            updateOnEnable = serializedObject.FindProperty(nameof(updateOnEnable));
            OnNewSprite = serializedObject.FindProperty(nameof(OnNewSprite));
            rend = serializedObject.FindProperty(nameof(rend));
            spriteGroups = serializedObject.FindProperty(nameof(spriteGroups));
        }

        /// <summary>
        /// Draw all elements normally, except for bracketSprites;
        /// </summary>
        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
            serializedObject.Update();

            RandomizedBracketSprite rbs = target as RandomizedBracketSprite;

            GUI.enabled = false;
            EditorGUILayout.ObjectField("Script:", MonoScript.FromMonoBehaviour(rbs), typeof(RandomizedBracketSprite), false);
            GUI.enabled = true;

            EditorGUILayout.PropertyField(updateOnEnable);
            EditorGUILayout.PropertyField(spriteGroups);
            EditorGUILayout.PropertyField(OnNewSprite);

            EditorGUILayout.PropertyField(rend);

            serializedObject.ApplyModifiedProperties();
        }
    }
}
