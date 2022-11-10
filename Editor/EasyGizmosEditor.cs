﻿using UnityEditor;
using UnityEngine;


namespace EasyGizmos
{
    [InitializeOnLoad, CustomEditor(typeof(EasyGizmosView)), CanEditMultipleObjects]
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

            EditorGUILayout.Space(2);
            DrawGeneralProperties();

            EditorGUILayout.Space(5);
            DrawShapeProperties();

            EditorGUILayout.Space(5);
            DrawTextProperties();

            EditorGUILayout.Space(5);
            DrawIconProperties();

            EditorGUILayout.Space(5);
            DrawLineProperties();

            EditorGUILayout.Space(5);
            DrawFrustumProperties();
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

            bool showShapeValue = showShape.boolValue;
            EasyGizmosView.GizmosShapes shapeValue = context.shape;
            float shapeSizeValue = shapeSize.floatValue;
            bool syncWithColliderValue = syncWithCollider.boolValue;
            bool overrideShapeColorValue = overrideShapeColor.boolValue;
            Color shapeColorValue = shapeColor.colorValue;
            Vector3 shapeOffsetValue = shapeOffset.vector3Value;

            EditorGUI.BeginChangeCheck();
            showShapeValue = EditorGUILayout.Toggle("Show", showShape.boolValue);

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

        private void DrawTextProperties()
        {
            EditorGUILayout.LabelField("Text");
            var showText = serializedObject.FindProperty("showText");
            var overrideText = serializedObject.FindProperty("overrideText");
            var text = serializedObject.FindProperty("text");
            var textFontSize = serializedObject.FindProperty("textFontSize");
            var overrideTextColor = serializedObject.FindProperty("overrideTextColor");
            var textColor = serializedObject.FindProperty("textColor");
            var textOffset = serializedObject.FindProperty("textOffset");

            bool showTextValue = showText.boolValue;
            bool overrideTextValue = overrideText.boolValue;
            string textValue = text.stringValue;
            int textFontSizeValue = textFontSize.intValue;
            bool overrideTextColorValue = overrideTextColor.boolValue;
            Color textColorValue = textColor.colorValue;
            Vector3 textOffsetValue = textOffset.vector3Value;

            EditorGUI.BeginChangeCheck();
            showTextValue = EditorGUILayout.Toggle("Show", showText.boolValue);

            if (showTextValue)
            {
                overrideTextValue = EditorGUILayout.Toggle("Override Text", overrideText.boolValue);

                if (overrideTextValue)
                {
                    textValue = EditorGUILayout.TextField("Text", text.stringValue);
                }

                textFontSizeValue = EditorGUILayout.IntField("Font Size", textFontSize.intValue);
                overrideTextColorValue = EditorGUILayout.Toggle("Override Color", overrideTextColor.boolValue);
                if (overrideTextColorValue)
                {
                    textColorValue = EditorGUILayout.ColorField("Color", textColor.colorValue);
                }

                textOffsetValue = EditorGUILayout.Vector3Field("Offset", textOffset.vector3Value);
            }

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, target.name + " params");
                showText.boolValue = showTextValue;
                overrideText.boolValue = overrideTextValue;
                text.stringValue = textValue;
                textFontSize.intValue = textFontSizeValue;
                textOffset.vector3Value = textOffsetValue;
                overrideTextColor.boolValue = overrideTextColorValue;
                textColor.colorValue = textColorValue;
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(target);
            }
        }

        private void DrawIconProperties()
        {
            EditorGUILayout.LabelField("Icon");
            var showIcon = serializedObject.FindProperty("showIcon");
            var iconOffset = serializedObject.FindProperty("iconOffset");

            bool showIconValue = showIcon.boolValue;
            Sprite icon = context.icon;
            Vector3 iconOffsetValue = iconOffset.vector3Value;

            EditorGUI.BeginChangeCheck();
            showIconValue = EditorGUILayout.Toggle("Show", showIcon.boolValue);
            if (showIconValue)
            {
                iconOffsetValue = EditorGUILayout.Vector3Field("Offset", iconOffset.vector3Value);
                context.icon = (Sprite) EditorGUILayout.ObjectField("Icon", context.icon, typeof(Sprite), true);
            }

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, target.name + "icon params");
                showIcon.boolValue = showIconValue;
                iconOffset.vector3Value = iconOffsetValue;
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(target);
            }
        }

        private void DrawLineProperties()
        {
            EditorGUILayout.LabelField("Line");
            var showLine = serializedObject.FindProperty("showLine");
            var overrideLineColor = serializedObject.FindProperty("overrideLineColor");
            var lineColor = serializedObject.FindProperty("lineColor");
            var lineOffset = serializedObject.FindProperty("lineOffset");

            bool showLineValue = showLine.boolValue;
            bool overrideLineColorValue = overrideLineColor.boolValue;
            Color lineColorValue = lineColor.colorValue;
            Vector3 lineOffsetValue = lineOffset.vector3Value;

            EditorGUI.BeginChangeCheck();
            showLineValue = EditorGUILayout.Toggle("Show", showLine.boolValue);

            if (showLineValue)
            {
                context.lineStartPoint = (Transform) EditorGUILayout.ObjectField("Start Point",
                    context.lineStartPoint, typeof(Transform), true);
                context.lineEndPoint = (Transform) EditorGUILayout.ObjectField("End Point",
                    context.lineEndPoint, typeof(Transform), true);

                overrideLineColorValue = EditorGUILayout.Toggle("Override Color", overrideLineColor.boolValue);
                if (overrideLineColorValue)
                {
                    lineColorValue = EditorGUILayout.ColorField("Color", lineColor.colorValue);
                }

                lineOffsetValue = EditorGUILayout.Vector3Field("Offset", lineOffset.vector3Value);
            }

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, target.name + " params");
                showLine.boolValue = showLineValue;
                lineOffset.vector3Value = lineOffsetValue;
                overrideLineColor.boolValue = overrideLineColorValue;
                lineColor.colorValue = lineColorValue;
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(target);
            }
        }

        private void DrawFrustumProperties()
        {
            EditorGUILayout.LabelField("Frustum");
            var showFrustum = serializedObject.FindProperty("showFrustum");
            var frustumFov = serializedObject.FindProperty("frustumFov");
            var frustumAspect = serializedObject.FindProperty("frustumAspect");
            var frustumNearPlane = serializedObject.FindProperty("frustumNearPlane");
            var frustumFarPlane = serializedObject.FindProperty("frustumFarPlane");
            var overrideFrustumColor = serializedObject.FindProperty("overrideFrustumColor");
            var frustumColor = serializedObject.FindProperty("frustumColor");

            bool showFrustumValue = showFrustum.boolValue;
            bool overrideFrustumColorValue = overrideFrustumColor.boolValue;
            Color frustumColorValue = frustumColor.colorValue;
            float frustumFovValue = frustumFov.floatValue;
            float frustumAspectValue = frustumAspect.floatValue;
            float frustumNearPlaneValue = frustumNearPlane.floatValue;
            float frustumFarPlaneValue = frustumFarPlane.floatValue;

            EditorGUI.BeginChangeCheck();
            showFrustumValue = EditorGUILayout.Toggle("Show", showFrustum.boolValue);

            if (showFrustumValue)
            {
                frustumFovValue = EditorGUILayout.FloatField("Field of View", frustumFov.floatValue);
                frustumAspectValue = EditorGUILayout.FloatField("Aspect", frustumAspect.floatValue);
                frustumNearPlaneValue = EditorGUILayout.FloatField("Near", frustumNearPlane.floatValue);
                frustumFarPlaneValue = EditorGUILayout.FloatField("Far", frustumFarPlane.floatValue);

                overrideFrustumColorValue = EditorGUILayout.Toggle("Override Color", overrideFrustumColor.boolValue);
                if (overrideFrustumColorValue)
                {
                    frustumColorValue = EditorGUILayout.ColorField("Color", frustumColor.colorValue);
                }
            }

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, target.name + " params");
                showFrustum.boolValue = showFrustumValue;
                overrideFrustumColor.boolValue = overrideFrustumColorValue;
                frustumColor.colorValue = frustumColorValue;
                frustumFov.floatValue = frustumFovValue;
                frustumAspect.floatValue = frustumAspectValue;
                frustumNearPlane.floatValue = frustumNearPlaneValue;
                frustumFarPlane.floatValue = frustumFarPlaneValue;
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