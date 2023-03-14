using UnityEngine;

namespace Dedalord.LiveAr
{
    /// <summary>
    /// Test using the Character Bridge to override parameters.
    /// </summary>
    [RequireComponent(typeof(Live2DCharacterBridge))]
    public class CubismCharacterOverrideTest : MonoBehaviour
    {
        private float v;
        public float changeRate;
        private Live2DCharacterBridge _live2DCharacter;
        /// <summary>
        /// Unity LateUpdate.
        /// </summary>
        private void Update()
        {
            v += Input.GetKey(KeyCode.D) ? changeRate * Time.deltaTime : (Input.GetKey(KeyCode.A) ? -changeRate * Time.deltaTime : 0f);
            v = Mathf.Clamp(v, -1, 1);
            
            // Get a reference to the CubismModel you want to modify
            _live2DCharacter ??= GetComponent<Live2DCharacterBridge>();
            _live2DCharacter.SetNormalized(Map.MOUTH_OPEN_DOWNY, v);
            _live2DCharacter.SetNormalized(Map.JAW_OPEN, v);
        }
    }
}