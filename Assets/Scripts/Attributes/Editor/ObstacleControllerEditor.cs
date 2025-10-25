///*****************************************************************************
//// File Name : ObstacleControllerEditor.cs
//// Author :
//// Creation Date : 
//// Last Modified : 
////
//// Brief Description : 
//*****************************************************************************/
//using UnityEditor;

//namespace Snowmentum
//{
//    [CustomEditor(typeof(ObstacleController))]
//    public class ObstacleControllerEditor : Editor
//    {
//        // SerializedProperty variables.
//        private SerializedProperty obstacleData;
//        private SerializedProperty autoUpdateObstacleData;

//        bool showDataProperties;

//        /// <summary>
//        /// Initialize SerializedProperties here.
//        /// </summary>
//        protected virtual void OnEnable()
//        {
//            obstacleData = serializedObject.FindProperty(nameof(obstacleData));
//            autoUpdateObstacleData = serializedObject.FindProperty(nameof(autoUpdateObstacleData));
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

//            showDataProperties = EditorGUILayout.Foldout(showDataProperties, "Obstacle Data");

//            if (controller.ObstacleData != null && showDataProperties)
//            {
//                // Draw the editor of the obstacle data inside the data properties foldout.
//                Editor obsDataEditor = Editor.CreateEditor(controller.ObstacleData);
//                obsDataEditor.OnInspectorGUI();
//            }

//            serializedObject.ApplyModifiedProperties();
//        }
//    }
//}
