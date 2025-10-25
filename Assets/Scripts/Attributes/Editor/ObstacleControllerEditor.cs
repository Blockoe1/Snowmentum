///*****************************************************************************
//// File Name : ObstacleControllerEditor.cs
//// Author :
//// Creation Date : 
//// Last Modified : 
////
//// Brief Description : 
//*****************************************************************************/
//using Snowmentum.Score;
//using Snowmentum.Size;
//using UnityEditor;
//using UnityEngine;

//namespace Snowmentum
//{
//    [CustomEditor(typeof(ObstacleController))]
//    public class ObstacleControllerEditor : Editor
//    {
//        // SerializedProperty variables.
//        private SerializedProperty obstacleData;
//        private SerializedProperty autoUpdateObstacleData;

//        // Components
//        private SerializedProperty rend;
//        private SerializedProperty obstacleCollider;
//        private SerializedProperty score;
//        private SerializedProperty scaler;
//        private SerializedProperty relay;
//        private SerializedProperty outliner;
//        private SerializedProperty particles;

//        bool showComponents;

//        /// <summary>
//        /// Initialize SerializedProperties here.
//        /// </summary>
//        protected virtual void OnEnable()
//        {
//            obstacleData = serializedObject.FindProperty(nameof(obstacleData));
//            autoUpdateObstacleData = serializedObject.FindProperty(nameof(autoUpdateObstacleData));

//            // Components
//            rend = serializedObject.FindProperty(nameof(rend));
//            obstacleCollider = serializedObject.FindProperty(nameof(obstacleCollider));
//            score = serializedObject.FindProperty(nameof(score));
//            scaler = serializedObject.FindProperty(nameof(scaler));
//            relay = serializedObject.FindProperty(nameof(relay));
//            outliner = serializedObject.FindProperty(nameof(outliner));
//            particles = serializedObject.FindProperty(nameof(particles));
//        }

//        /// <summary>
//        /// Draw the editor.
//        /// </summary>
//        public override void OnInspectorGUI()
//        {
//            //base.OnInspectorGUI();

//            serializedObject.Update();
//            ObstacleController controller = (ObstacleController)target;

//            EditorGUILayout.PropertyField(obstacleData);
//            EditorGUILayout.PropertyField(autoUpdateObstacleData);

//            if (GUILayout.Button("Write Obstacle Data"))
//            {
//                controller.WriteObstacleData();
//            }
//            if (GUILayout.Button("Read Obstacle Data"))
//            {
//                controller.ReadObstacleData();
//            }

//            // Draw components
//            showComponents = EditorGUILayout.Foldout(showComponents, "Components");
//            if (showComponents)
//            {
//                EditorGUI.indentLevel++;
//                EditorGUI.BeginDisabledGroup(true);

//                EditorGUILayout.PropertyField(rend);
//                EditorGUILayout.PropertyField(obstacleCollider);
//                EditorGUILayout.PropertyField(score);
//                EditorGUILayout.PropertyField(scaler);
//                EditorGUILayout.PropertyField(relay);
//                EditorGUILayout.PropertyField(outliner);
//                EditorGUILayout.PropertyField(particles);

//                EditorGUI.EndDisabledGroup();
//                EditorGUI.indentLevel--;
//            }

//            serializedObject.ApplyModifiedProperties();
//        }
//    }
//}
