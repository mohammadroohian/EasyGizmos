using UnityEditor;
using UnityEngine;


namespace EasyGizmos
{
    [InitializeOnLoad]
    public class EasyGizmosEditor : Editor
    {
        static EasyGizmosEditor()
        {
            SceneView.duringSceneGui -= OnDuringSceneGui;
            SceneView.duringSceneGui += OnDuringSceneGui;
        }

        private static void OnDuringSceneGui(SceneView obj)
        {
            Handles.BeginGUI();
            {
                var sceneWidth = ((EditorWindow)obj).position.width;
                var rect = new Rect((sceneWidth - 150) / 2, 0, 150, 70);
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
                        if (Event.current.keyCode == (KeyCode.G))
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