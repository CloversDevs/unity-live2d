using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;

namespace Dedalord.LiveAr
{
    /// <summary>
    /// Unity application bootstrap.
    /// Configure and initialize the app.
    /// </summary>
    public class Boot : MonoBehaviour
    {
        /// <summary>
        /// Scene to go to when testing basic Live2D functionality.
        /// </summary>
        public string Live2DTestScene;
        
        /// <summary>
        /// Scene to go to when testing Unity AR Foundation functionality.
        /// </summary>
        public string ARSamplesScene;
        
        /// <summary>
        /// Scene to go to when returning to the starting scene
        /// </summary>
        public string RootScene;

        /// <summary>
        /// Scene to go to when testing Unity AR Foundation face position functionality.
        /// </summary>
        public string FacePoseScene;
        
        /// <summary>
        /// Scene to go to when testing Unity AR Foundation face mesh functionality.
        /// </summary>
        public string FaceMeshScene;
        
        /// <summary>
        /// Scene to go to when testing Unity AR Foundation eye tracking functionality.
        /// </summary>
        public string EyePoseScene;

        /// <summary>
        /// Unity Awake.
        /// Subscribe to application control messages.
        /// </summary>
        public void Awake()
        {
            SimpleMessageRouter.Clear();
            SimpleMessageRouter.AddListener(SimpleMessageId.GO_ROOT, OnGoBackToRoot);
            SimpleMessageRouter.AddListener(SimpleMessageId.GO_LIVE_2D, OnLive2D);
            SimpleMessageRouter.AddListener(SimpleMessageId.GO_AR_SAMPLES, OnARSamples);
            
            SimpleMessageRouter.AddListener(SimpleMessageId.GO_FACE_POSE, ()=> LoadARScene(FacePoseScene));
            SimpleMessageRouter.AddListener(SimpleMessageId.GO_FACE_MESH, ()=> LoadARScene(FaceMeshScene));
            SimpleMessageRouter.AddListener(SimpleMessageId.GO_EYE_POSE, ()=> LoadARScene(EyePoseScene));
        }
        
        /// <summary>
        /// Initializes the XR Loader before switching to the given scene.
        /// </summary>
        private void LoadARScene(string sceneName)
        {
            LoaderUtility.Initialize();
            SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }
        
        /// <summary>
        /// Invoke to discard everything and go to the Live2D test scene
        /// </summary>
        private void OnLive2D()
        {
            LoadARScene(Live2DTestScene);
        }
        
        /// <summary>
        /// Invoke to discard everything and go to the AR samples scene
        /// </summary>
        private void OnARSamples()
        {
            SceneManager.LoadScene(ARSamplesScene);
        }
        
        /// <summary>
        /// Invoke to discard everything and go to the starting scene
        /// </summary>
        private void OnGoBackToRoot()
        {
            SceneManager.LoadScene(RootScene);
        }
    }
}

