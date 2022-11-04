using UnityEditor;
using UnityEngine;


namespace EasyGizmos
{
    [InitializeOnLoad, CustomEditor(typeof(EasyGizmosView))]
    public class EasyGizmosEditor : Editor
    {
        private EasyGizmosView context;

        private void OnEnable()
        {
            context = (EasyGizmosView) target;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // General Properties

            EditorGUILayout.LabelField("General");
            context.generalColor = EditorGUILayout.ColorField("Color", context.generalColor);
            context.drawOnItself = EditorGUILayout.Toggle("Draw On Itself", context.drawOnItself);
            context.drawOnChildren = EditorGUILayout.Toggle("Draw On Children", context.drawOnChildren);

            // Shape Properties
            // Text Properties
            // Icon Properties
            // Line Properties
            // Frustum Properties
        }


        //////////////////////////////////////////////////////
        /// STATIC MEMBERS
        //////////////////////////////////////////////////////
        static EasyGizmosEditor()
        {
            SceneView.duringSceneGui -= OnDuringSceneGui;
            SceneView.duringSceneGui += OnDuringSceneGui;
        }

        private static void OnDuringSceneGui(SceneView obj)
        {
            Handles.BeginGUI();
            {
                var sceneWidth = obj.position.width;
                var rect = new Rect((sceneWidth - 150) / 2, 5, 150, 70);
                GUILayout.BeginArea(rect);
                {
                    var showHide = EasyGizmosView.IsActive ? "Hide" : "Show";
                    var buttonText = $"{showHide} Easy Gizmos";
                    if (GUILayout.Button(buttonText))
                        EasyGizmosView.IsActive = !EasyGizmosView.IsActive;
                }
                GUILayout.EndArea();
            }
            Handles.EndGUI();


            Event e = Event.current;
            switch (e.type)
            {
                case EventType.KeyDown:
                {
                    if (Event.current.keyCode == KeyCode.G)
                    {
                        EasyGizmosView.IsActive = !EasyGizmosView.IsActive;
                        e.Use();
                    }

                    break;
                }
            }
        }
    }
}