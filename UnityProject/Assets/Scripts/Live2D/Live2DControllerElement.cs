using UnityEngine;

namespace Dedalord.LiveAr
{
    
    [RequireComponent(typeof(Live2DCharacterBridge)), RequireComponent(typeof(Live2DCharacterController))]
    public abstract class Live2DControllerElement : MonoBehaviour
    {
        public Live2DCharacterBridge Bridge => _bridge;
        public Live2DCharacterController Controller => _controller;
        
        [SerializeField, HideInInspector]
        private Live2DCharacterBridge _bridge;
        
        [SerializeField, HideInInspector]
        private Live2DCharacterController _controller;
        
        private void OnValidate()
        {
            _bridge ??= GetComponent<Live2DCharacterBridge>();
            _controller ??= GetComponent<Live2DCharacterController>();
        }
    }
}