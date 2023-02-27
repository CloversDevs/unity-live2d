using TMPro;
using UnityEngine;

namespace Dedalord.LiveAr
{
    /// <summary>
    /// Debug class that calculates the AR element position relative to the AR camera.
    /// </summary>
    public class TriAxesTracker : MonoBehaviour
    {
        /// <summary>
        /// Debug text display.
        /// </summary>
        public TMP_Text Text;
        
        /// <summary>
        /// Reference to AR Camera
        /// </summary>
        private Transform _arCamera;
        
        /// <summary>
        /// Tag to identify the AR Camera with
        /// </summary>
        private readonly string AR_TAG = "AR_Camera";
        
        /// <summary>
        /// Unity OnEnable.
        /// </summary>
        private void OnEnable()
        {
            var cams = FindObjectsOfType<Camera>();
            _arCamera = null;
            foreach (var cam in cams)
            {
                if (cam.tag != AR_TAG)
                {
                    continue;
                }

                _arCamera = cam.transform;
            }

            if (_arCamera != null)
            {
                return;
            }
            Debug.LogError($"No AR Camera found, please make sure that the AR camera is tagged as '{AR_TAG}'");
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Unity Update.
        /// </summary>
        private void Update()
        {
            var pos = _arCamera.InverseTransformDirection(transform.position - _arCamera.position);
            Text.text = $"{pos.x:00.000}\n{pos.y:00.000}\n{pos.z:00.000}";
        }
    }
}
