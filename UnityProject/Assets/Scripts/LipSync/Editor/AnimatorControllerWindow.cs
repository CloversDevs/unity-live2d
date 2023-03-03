using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace Dedalord.LiveAr
{
    
    public class AnimatorControllerWindow : EditorWindow
    {
        private string controllerFilePath = "";
        private float transitionDuration = 0.1f;
    
        [MenuItem("Window/Animator Controller Editor")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow<AnimatorControllerWindow>("Animator Controller Editor");
        }
    
        private void OnGUI()
        {
            GUILayout.Label("Select Animator Controller", EditorStyles.boldLabel);

            // Allow user to select an Animator Controller file
            GUILayout.BeginHorizontal();
            GUILayout.Label("Controller File: ", GUILayout.Width(100));
            controllerFilePath = GUILayout.TextField(controllerFilePath, GUILayout.ExpandWidth(true));

            // Allow user to drag and drop a file onto the window
            if (Event.current.type == EventType.DragUpdated || Event.current.type == EventType.DragPerform)
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                if (Event.current.type == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();
                    controllerFilePath = DragAndDrop.paths[0];
                }
            }

            GUILayout.EndHorizontal();

            // Allow user to set the transition duration
            GUILayout.BeginHorizontal();
            GUILayout.Label("Transition Duration: ", GUILayout.Width(100));
            transitionDuration = EditorGUILayout.FloatField(transitionDuration, GUILayout.ExpandWidth(true));
            GUILayout.EndHorizontal();

            // Apply changes button
            if (GUILayout.Button("Apply Changes"))
            {
                ApplyChanges(transitionDuration);
            }
        }
    
        private void ApplyChanges(float newDuration)
        {
            if (string.IsNullOrEmpty(controllerFilePath))
            {
                Debug.LogError("No Animator Controller file selected!");
                return;
            }

            // Read the contents of the controller file into a string
            var controllerData = File.ReadAllText(controllerFilePath);

            // Replace all lines containing "m_TransitionDuration: [oldDuration]" with a new line containing "m_TransitionDuration: [newDuration]"
            var pattern = @"m_TransitionDuration:\s*\d+\.?\d*";
            var replacement = "m_TransitionDuration: " + newDuration;
            controllerData = Regex.Replace(controllerData, pattern, replacement);

            // Write the modified data back to the controller file
            File.WriteAllText(controllerFilePath, controllerData);

            // Notify Unity that the file has been modified
            AssetDatabase.ImportAsset(controllerFilePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            Debug.Log("Animator Controller updated!");
        }
    }

}
