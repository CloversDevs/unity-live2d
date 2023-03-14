using UnityEngine;

namespace Dedalord.LiveAr
{
    
    [RequireComponent(typeof(Live2DCharacterBridge))]
    public abstract class Live2DControllerElement : MonoBehaviour
    {
        public Live2DCharacterBridge Bridge => _bridge;
        
        [SerializeField, HideInInspector]
        private Live2DCharacterBridge _bridge;
        
        private void OnValidate()
        {
            _bridge ??= GetComponent<Live2DCharacterBridge>();
        }
    }
}