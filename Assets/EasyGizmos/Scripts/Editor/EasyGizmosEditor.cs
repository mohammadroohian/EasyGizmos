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
            EditorGUILayout.Space(5);
            DrawGeneralProperties();

            EditorGUILayout.Space(7);
            DrawShapeProperties();

            // Text Properties
            // Icon Properties
            // Line Properties
            // Frustum Properties
        }

        private void DrawGeneralProperties()
        {
            EditorGUILayout.LabelField("General");
            var generalColor = serializedObject.FindProperty("generalColor");
            var drawOnItself = serializedObject.FindProperty("drawOnItself");
            var drawOnChildren = serializedObject.FindProperty("drawOnChildren");

            EditorGUI.BeginChangeCheck();
            var generalColorValue = EditorGUILayout.ColorField("General Color", generalColor.colorValue);
            var drawOnItselfValue = EditorGUILayout.Toggle("Draw On Itself", drawOnItself.boolValue);
            var drawOnChildrenValue = EditorGUILayout.Toggle("Draw On Children", drawOnChildren.boolValue);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, target.name + " params");
                generalColor.colorValue = generalColorValue;
                drawOnItself.boolValue = drawOnItselfValue;
                drawOnChildren.boolValue = drawOnChildrenValue;
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(target);
            }
        }

        private void DrawShapeProperties()
        {
            EditorGUILayout.LabelField("Shape");
            var showShape = serializedObject.FindProperty("showShape");
            var shapeSize = serializedObject.FindProperty("shapeSize");
            var syncWithCollider = serializedObject.FindProperty("syncShapeWithCollider");
            var overrideShapeColor = serializedObject.FindProperty("overrideShapeColor");
            var shapeColor = serializedObject.FindProperty("shapeColor");
            var shapeOffset = serializedObject.FindProperty("shapeOffset");

            EasyGizmosView.GizmosShapes shapeValue = context.shape;
            float shapeSizeValue = shapeSize.floatValue;
            bool syncWithColliderValue = syncWithCollider.boolValue;
            bool overrideShapeColorValue = overrideShapeColor.boolValue;
            Color shapeColorValue = shapeColor.colorValue;
            Vector3 shapeOffsetValue = shapeOffset.vector3Value;

            EditorGUI.BeginChangeCheck();
            var showShapeValue = EditorGUILayout.Toggle("Show", showShape.boolValue);

            if (showShapeValue)
            {
                shapeValue = (EasyGizmosView.GizmosShapes) EditorGUILayout.EnumPopup("Shape", context.shape);

                if (shapeValue == EasyGizmosView.GizmosShapes.Mesh ||
                    shapeValue == EasyGizmosView.GizmosShapes.WireMesh)
                {
                    context.mesh = (Mesh) EditorGUILayout.ObjectField("Custom Shape", context.mesh, typeof(Mesh), true);
                }

                shapeSizeValue = EditorGUILayout.FloatField("Size", shapeSize.floatValue);
                syncWithColliderValue = EditorGUILayout.Toggle("Sync With Collider", syncWithCollider.boolValue);
                shapeOffsetValue = EditorGUILayout.Vector3Field("Offset", shapeOffset.vector3Value);

                overrideShapeColorValue = EditorGUILayout.Toggle("Override Color", overrideShapeColor.boolValue);
                if (overrideShapeColorValue)
                {
                    shapeColorValue = EditorGUILayout.ColorField("Color", shapeColor.colorValue);
                }
            }

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, target.name + " params");
                showShape.boolValue = showShapeValue;
                context.shape = shapeValue;
                shapeSize.floatValue = shapeSizeValue;
                syncWithCollider.boolValue = syncWithColliderValue;
                shapeOffset.vector3Value = shapeOffsetValue;
                overrideShapeColor.boolValue = overrideShapeColorValue;
                shapeColor.colorValue = shapeColorValue;
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(target);
            }
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