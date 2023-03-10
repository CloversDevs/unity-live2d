using UnityEngine;

namespace Dedalord.LiveAr
{
    [CreateAssetMenu(fileName = "New Expression", menuName = "Cubism/New Expression")]
    public class Expression : ScriptableObject
    {
        public EyeBrowPosition Eyebrows;
        
        [Range(0f, 1f)]
        public float OpenEyesValue = 0.8f;
        
        [Range(0f, 1f)]
        public float MouthShape = .5f;
    }
}