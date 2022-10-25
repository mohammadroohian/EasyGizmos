using UnityEngine;
using NaughtyAttributes;

namespace EasyGizmos
{
    public class EasyGizmosView : MonoBehaviour
    {
#if UNITY_EDITOR
        public enum GizmosShapes { None = 0, Sphere = 1, Cube = 2, WireSphere = 3, WireCube = 4 }
        [BoxGroup("General")][Label("Color")] public Color generalColor = Color.white;
        [BoxGroup("General")] public bool drawOnItself = true;
        [BoxGroup("General")] public bool drawOnChilds = false;

        [BoxGroup("Shape")][Label("Show")] public bool showSape = true;
        [BoxGroup("Shape")][ShowIf("showSape")] public GizmosShapes shape = GizmosShapes.Sphere;
        [BoxGroup("Shape")][ShowIf("showSape")] public float shapeRadius = 0.3f;
        [BoxGroup("Shape")][ShowIf("showSape")][Label("Sync With Collider")] public bool syncShapeWithCollider = false;
        [BoxGroup("Shape")][ShowIf("showSape")][Label("Override Color")] public bool overrideShapeColor = false;
        [BoxGroup("Shape")][ShowIf(EConditionOperator.And, "showSape", "overrideShapeColor")][Label("Color")] public Color shapeColor = Color.white;
        [BoxGroup("Shape")][ShowIf("showSape")][Label("Offset")] public Vector3 shapeOffset = Vector3.zero;

        [BoxGroup("Text")][Label("Show")] public bool showText = false;
        [BoxGroup("Text")][ShowIf("showText")] public bool overrideText = false;
        [BoxGroup("Text")][ShowIf(EConditionOperator.And, "showText", "overrideText")][TextArea] public string text = string.Empty;
        [BoxGroup("Text")][ShowIf("showText")][Label("Size")] public int textFontSize = 20;
        [BoxGroup("Text")][ShowIf("showText")][Label("Override Color")] public bool overrideTextColor = false;
        [BoxGroup("Text")][ShowIf(EConditionOperator.And, "showText", "overrideTextColor")][Label("Color")] public Color textColor = Color.white;
        [BoxGroup("Text")][ShowIf("showText")][Label("Offset")] public Vector3 textOffset = new Vector3(0, 1, 0);

        [BoxGroup("Icon")][Label("Show")] public bool showIcon = false;
        [BoxGroup("Icon")][ShowIf("showIcon")] public Sprite icon = null;
        [BoxGroup("Icon")][ShowIf("showIcon")][Label("Offset")] public Vector3 iconOffset = Vector3.zero;

        [BoxGroup("Line")][Label("Show")] public bool showLine = false;
        [BoxGroup("Line")][ShowIf("showLine")][Label("Start Point")] public Transform lineStartPoint = null;
        [BoxGroup("Line")][ShowIf("showLine")][Label("End Point")] public Transform lineEndPoint = null;
        [BoxGroup("Line")][ShowIf("showLine")][Label("Offset")] public Vector3 lineOffset = Vector3.zero;
        [BoxGroup("Line")][ShowIf(EConditionOperator.And, "showLine", "overrideLineColor")][Label("Color")] public Color lineColor = Color.white;
        [BoxGroup("Line")][ShowIf("showLine")][Label("Override Color")] public bool overrideLineColor = false;

        [BoxGroup("Frustum")][Label("Show")] public bool showFrustum = false;
        [BoxGroup("Frustum")][ShowIf("showFrustum")][Label("Fov")] public float frustumFov = 60f;
        [BoxGroup("Frustum")][ShowIf("showFrustum")][Label("Aspect")] public float frustumAspect = 1;
        [BoxGroup("Frustum")][ShowIf("showFrustum")][Label("Min Range")] public float frustumMinRange = 0.3f;
        [BoxGroup("Frustum")][ShowIf("showFrustum")][Label("Max Range")] public float frustumMaxRange = 2f;
        [BoxGroup("Frustum")][ShowIf("showFrustum")][Label("Override Color")] public bool overrideFrustumColor = false;
        [BoxGroup("Frustum")][ShowIf(EConditionOperator.And, "showFrustum", "overrideFrustumColor")][Label("Color")] public Color frustumColor = Color.white;


        private void OnDrawGizmos()
        {
            if (!IsActive) return;

            DrawSelf();
            DrawChilds();
        }

        private void DrawSelf()
        {
            if (!drawOnItself) return;
            ShowShape(transform);
            ShowText(transform);
            ShowIcon(transform);
            ShowLine();
            ShowFrustum(transform);
        }

        private void DrawChilds()
        {
            if (!drawOnChilds) return;
            for (int i = 0; i < transform.childCount; i++)
            {
                ShowShape(transform.GetChild(i));
                ShowText(transform.GetChild(i));
                ShowIcon(transform.GetChild(i));
                ShowFrustum(transform.GetChild(i));
            }
        }

        private GUIStyle GetTextStyle()
        {
            var style = new GUIStyle { fontSize = textFontSize };
            style.normal.textColor = overrideTextColor ? textColor : generalColor;
            style.richText = true;
            return style;
        }

        private void ShowShape(Transform baseTransform)
        {
            if (!showSape) return;

            var sc = GetComponent<SphereCollider>();
            var bc = GetComponent<SphereCollider>();

            if (syncShapeWithCollider)
            {
                shapeRadius = sc != null ? sc.radius : shapeRadius;
                shapeRadius = bc != null ? bc.radius : shapeRadius;

                if (sc != null) shapeOffset = sc.center;
                if (bc != null) shapeOffset = bc.center;
            }

            Gizmos.color = overrideShapeColor ? shapeColor : generalColor;
            switch (shape)
            {
                case GizmosShapes.Sphere:
                    Gizmos.DrawSphere(baseTransform.position + shapeOffset, shapeRadius);
                    break;
                case GizmosShapes.Cube:
                    Gizmos.DrawCube(baseTransform.position + shapeOffset, new Vector3(shapeRadius * 2, shapeRadius * 2, shapeRadius * 2));
                    break;
                case GizmosShapes.WireSphere:
                    Gizmos.DrawWireSphere(baseTransform.position + shapeOffset, shapeRadius);
                    break;
                case GizmosShapes.WireCube:
                    Gizmos.DrawWireCube(baseTransform.position + shapeOffset, new Vector3(shapeRadius * 2, shapeRadius * 2, shapeRadius * 2));
                    break;
            }
        }

        private void ShowText(Transform baseTransform)
        {
            if (!showText) return;
            UnityEditor.Handles.Label(baseTransform.position + textOffset, overrideText ? text : name, GetTextStyle());
        }

        private void ShowIcon(Transform baseTransform)
        {
            if (!showIcon) return;
            if (icon == null) return;
            Gizmos.DrawIcon(baseTransform.position + iconOffset, UnityEditor.AssetDatabase.GetAssetPath(icon.GetInstanceID()));
        }

        private void ShowLine()
        {
            if (!showLine) return;
            if (lineStartPoint == null) return;
            if (lineEndPoint == null) return;
            Gizmos.color = overrideLineColor ? lineColor : generalColor;
            Gizmos.DrawLine(lineStartPoint.position + lineOffset, lineEndPoint.position);
        }

        private void ShowFrustum(Transform baseTransform)
        {
            if (!showFrustum) return;
            Gizmos.color = overrideFrustumColor ? frustumColor : generalColor;
            Gizmos.matrix = Matrix4x4.TRS(baseTransform.position, baseTransform.rotation, Vector3.one);
            Gizmos.DrawFrustum(Vector3.zero, frustumFov, frustumMaxRange, frustumMinRange, frustumAspect);
        }

        //////////////////////////////////////////////////////
        /// STATIC MEMBERS
        //////////////////////////////////////////////////////

        public static bool IsActive { get; set; } = true;
#endif
    }
}