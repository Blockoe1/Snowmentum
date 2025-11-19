/*****************************************************************************
// File Name : ObstacleControllerEditor.cs
// Author :
// Creation Date : 
// Last Modified : 
//
// Brief Description : 
*****************************************************************************/
using Snowmentum.Score;
using Snowmentum.Size;
using UnityEditor;
using UnityEngine;

namespace Snowmentum
{
    [CustomEditor(typeof(ObstacleController))]
    public class ObstacleControllerEditor : Editor
    {
        // SerializedProperty variables.
        private SerializedProperty obstacleData;
        private SerializedProperty autoUpdateObstacleData;

        // Components
        private SerializedProperty rend;
        private SerializedProperty obstacleCollider;
        private SerializedProperty score;
        private SerializedProperty scaler;
        private SerializedProperty relay;
        private SerializedProperty colorizer;
        private SerializedProperty particles;
        private SerializedProperty rb;

        bool showComponents;

        /// <summary>
        /// Initialize SerializedProperties here.
        /// </summary>
        protected virtual void OnEnable()
        {
            obstacleData = serializedObject.FindProperty(nameof(obstacleData));
            autoUpdateObstacleData = serializedObject.FindProperty(nameof(autoUpdateObstacleData));

            // Components
            rend = serializedObject.FindProperty(nameof(rend));
            obstacleCollider = serializedObject.FindProperty(nameof(obstacleCollider));
            score = serializedObject.FindProperty(nameof(score));
            scaler = serializedObject.FindProperty(nameof(scaler));
            relay = serializedObject.FindProperty(nameof(relay));
            colorizer = serializedObject.FindProperty(nameof(colorizer));
            particles = serializedObject.FindProperty(nameof(particles));
            rb = serializedObject.FindProperty(nameof(rb));
        }

        /// <summary>
        /// Draw the editor.
        /// </summary>
        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();

            serializedObject.Update();
            ObstacleController controller = (ObstacleController)target;

            // Get any changes made to other components.
            if (controller.AutoUpdateObstacleData)
            {
                //Debug.Log("Obstacle GO automatically Updated");
                // Run SetObstacle so other values are updated.
                controller.Editor_WriteObstacleData();

            }

            // Show the default script reference.
            GUI.enabled = false;
            EditorGUILayout.ObjectField("Script", controller, typeof(ObstacleController), true);
            GUI.enabled = true;

            EditorGUILayout.PropertyField(obstacleData);
            EditorGUILayout.PropertyField(autoUpdateObstacleData);

            //if (GUILayout.Button("Write Obstacle Data"))
            //{
            //    controller.WriteObstacleData();
            //}
            //if (GUILayout.Button("Read Obstacle Data"))
            //{
            //    controller.ReadObstacleData();
            //}

            // Draw components
            showComponents = EditorGUILayout.Foldout(showComponents, "Components");
            if (showComponents)
            {
                EditorGUI.indentLevel++;
                EditorGUI.BeginDisabledGroup(true);

                EditorGUILayout.PropertyField(rend);
                EditorGUILayout.PropertyField(obstacleCollider);
                EditorGUILayout.PropertyField(score);
                EditorGUILayout.PropertyField(scaler);
                EditorGUILayout.PropertyField(relay);
                EditorGUILayout.PropertyField(colorizer);
                EditorGUILayout.PropertyField(particles);
                EditorGUILayout.PropertyField(rb);

                EditorGUI.EndDisabledGroup();
                EditorGUI.indentLevel--;
            }

            serializedObject.ApplyModifiedProperties();

            // Update the component data based on the obstacle data.
            if (controller.AutoUpdateObstacleData)
            {
                //Debug.Log("Obstacle GO automatically Updated");
                // Run SetObstacle so other values are updated.
                controller.Editor_ReadObstacleData();
            }
        }
    }
}
