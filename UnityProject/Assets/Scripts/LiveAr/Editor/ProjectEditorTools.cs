using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace Dedalord.LiveAr
{
    /// <summary>
    /// Class that contains Editor extensions for the project such as adding hotkeys.
    /// </summary>
    public static class ProjectEditorTools
    {
        /// <summary>
        /// Path to the main scene.
        /// </summary>
        private const string MainScenePath = "Assets/Scenes/main.unity";
        
        /// <summary>
        /// Ask to save the current scene if needed and go to the main scene. (Control/Command + L)
        /// </summary>
        [MenuItem("Tools/Go to main scene %l")]
        public static void GoToMainScene()
        {
            if (EditorApplication.isPlaying)
            {
                Debug.LogWarning("Switching scenes is not allowed in Play mode.");
                return;
            }

            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                // User canceled instead of choosing yes or no.
                return;
            }
            EditorSceneManager.OpenScene(MainScenePath);
        }
    }
}