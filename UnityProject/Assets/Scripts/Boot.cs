using UnityEngine;
using UnityEngine.SceneManagement;

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
        /// Unity Awake.
        /// Subscribe to application control messages.
        /// </summary>
        public void Awake()
        {
            SimpleMessageRouter.Clear();
            SimpleMessageRouter.AddListener(SimpleMessageId.GO_ROOT, OnGoBackToRoot);
            SimpleMessageRouter.AddListener(SimpleMessageId.GO_LIVE_2D, OnLive2D);
            SimpleMessageRouter.AddListener(SimpleMessageId.GO_AR_SAMPLES, OnARSamples);
        }
        
        /// <summary>
        /// Invoke to discard everything and go to the Live2D test scene
        /// </summary>
        private void OnLive2D()
        {
            SceneManager.LoadScene(Live2DTestScene);
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

