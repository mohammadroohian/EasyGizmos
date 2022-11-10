using UnityEngine;

namespace EasyGizmos
{
    public class EasyGizmosView : MonoBehaviour
    {
#if UNITY_EDITOR
        public enum GizmosShapes
        {
            None = 0,
            Sphere = 1,
            Cube = 2,
            WireSphere = 3,
            WireCube = 4,
            Mesh = 5,
            WireMesh = 6
        }

        public Color generalColor = Color.white;
        public bool drawOnItself = true;
        public bool drawOnChildren = false;

        public bool showShape = true;
        public GizmosShapes shape = GizmosShapes.Sphere;
        public Mesh mesh;
        public float shapeSize = 0.3f;
        public bool syncShapeWithCollider = false;
        public bool overrideShapeColor = false;
        public Color shapeColor = Color.white;
        public Vector3 shapeOffset = Vector3.zero;

        public bool showText = false;
        public bool overrideText = false;
        [TextArea] public string text = string.Empty;
        public int textFontSize = 20;
        public bool overrideTextColor = false;
        public Color textColor = Color.white;
        public Vector3 textOffset = new Vector3(0, 1, 0);

        public bool showIcon = false;
        public Sprite icon = null;
        public Vector3 iconOffset = Vector3.zero;

        public bool showLine = false;
        public Transform lineStartPoint = null;
        public Transform lineEndPoint = null;
        public Vector3 lineOffset = Vector3.zero;
        public Color lineColor = Color.white;
        public bool overrideLineColor = false;

        public bool showFrustum = false;
        public float frustumFov = 60f;
        public float frustumAspect = 1;
        public float frustumNearPlane = 0.3f;
        public float frustumFarPlane = 2f;
        public bool overrideFrustumColor = false;
        public Color frustumColor = Color.white;

        private void OnDrawGizmos()
        {
            if (!IsActive) return;

            if (drawOnItself)
                Draw(transform);

            if (drawOnChildren)
                DrawChildren();

            if (showLine)
                ShowLine();
        }

        private void Draw(Transform target)
        {
            if (showShape)
                ShowShape(target);

            if (showText)
                ShowText(target);

            if (showIcon)
                ShowIcon(target);

            if (showFrustum)
                ShowFrustum(target);
        }

        private void DrawChildren()
        {
            for (int i = 0; i < transform.childCount; i++)
                Draw(transform.GetChild(i));
        }

        private GUIStyle GetTextStyle()
        {
            var style = new GUIStyle {fontSize = textFontSize};
            style.normal.textColor = overrideTextColor ? textColor : generalColor;
            style.richText = true;
            return style;
        }

        private void ShowShape(Transform baseTransform)
        {
            var sc = GetComponent<SphereCollider>();
            var bc = GetComponent<SphereCollider>();

            if (syncShapeWithCollider)
            {
                shapeSize = sc != null ? sc.radius : shapeSize;
                shapeSize = bc != null ? bc.radius : shapeSize;

                if (sc != null) shapeOffset = sc.center;
                if (bc != null) shapeOffset = bc.center;
            }

            Gizmos.color = overrideShapeColor ? shapeColor : generalColor;
            switch (shape)
            {
                case GizmosShapes.Sphere:
                    Gizmos.DrawSphere(baseTransform.position + shapeOffset, shapeSize / 2);
                    break;
                case GizmosShapes.WireSphere:
                    Gizmos.DrawWireSphere(baseTransform.position + shapeOffset, shapeSize / 2);
                    break;
                case GizmosShapes.Cube:
                    Gizmos.DrawCube(baseTransform.position + shapeOffset,
                        new Vector3(shapeSize, shapeSize, shapeSize));
                    break;
                case GizmosShapes.WireCube:
                    Gizmos.DrawWireCube(baseTransform.position + shapeOffset,
                        new Vector3(shapeSize, shapeSize, shapeSize));
                    break;
                case GizmosShapes.Mesh:
                    Gizmos.DrawMesh(mesh, baseTransform.position + shapeOffset, Quaternion.identity,
                        new Vector3(shapeSize, shapeSize, shapeSize));
                    break;
                case GizmosShapes.WireMesh:
                    Gizmos.DrawWireMesh(mesh, baseTransform.position + shapeOffset, Quaternion.identity,
                        new Vector3(shapeSize, shapeSize, shapeSize));
                    break;
            }
        }

        private void ShowText(Transform baseTransform)
        {
            UnityEditor.Handles.Label(baseTransform.position + textOffset, overrideText ? text : name, GetTextStyle());
        }

        private void ShowIcon(Transform baseTransform)
        {
            if (icon == null) return;
            Gizmos.DrawIcon(baseTransform.position + iconOffset,
                UnityEditor.AssetDatabase.GetAssetPath(icon.GetInstanceID()));
        }

        private void ShowLine()
        {
            if (lineStartPoint == null) return;
            if (lineEndPoint == null) return;
            Gizmos.color = overrideLineColor ? lineColor : generalColor;
            Gizmos.DrawLine(lineStartPoint.position + lineOffset, lineEndPoint.position);
        }

        private void ShowFrustum(Transform baseTransform)
        {
            Gizmos.color = overrideFrustumColor ? frustumColor : generalColor;
            Gizmos.matrix = Matrix4x4.TRS(baseTransform.position, baseTransform.rotation, Vector3.one);
            Gizmos.DrawFrustum(Vector3.zero, frustumFov, frustumFarPlane, frustumNearPlane, frustumAspect);
        }

        //////////////////////////////////////////////////////
        /// STATIC MEMBERS
        //////////////////////////////////////////////////////

        public static bool IsActive { get; set; } = true;
#endif
    }
}