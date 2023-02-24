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
        /// Scene to go to when testing live2d basic functionality.
        /// </summary>
        public string Live2DTestScene;
        
        /// <summary>
        /// Unity Start.
        /// </summary>
        private void Start()
        {
            SceneManager.LoadScene(Live2DTestScene);
        }
    }
}

