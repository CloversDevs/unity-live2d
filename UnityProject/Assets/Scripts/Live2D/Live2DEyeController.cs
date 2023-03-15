using UnityEngine;

namespace Dedalord.LiveAr
{
    public class Live2DEyeController : Live2DControllerElement
    {
        public Vector2 EyesDirection;
        
        [Range(0.01f,1f)]
        public float LerpRate = 0.2f;
        
        public bool LookAt;
        public Transform LookTarget;
        public float LookAtMultiplier = 2f;
        
        
        private void Awake()
        {
            void ListenToController(Vector2 target)
            {
                LookTarget.position = target;
            }
            Controller.OnChangeEyesLookAt += ListenToController;
        }
        
        private void Update()
        {   
            if (LookAt)
            {
                var range = 1f / LookAtMultiplier;
                LookTarget.localPosition = new(
                    Mathf.Clamp(LookTarget.localPosition.x, -range, range),
                    Mathf.Clamp(LookTarget.localPosition.y, -range, range),
                    0f);
                
                EyesDirection = new(
                    LookTarget.localPosition.x * LookAtMultiplier, 
                    LookTarget.localPosition.y * LookAtMultiplier);
            }
            
            EyeLookAnimation();
        }
        
        private void EyeLookAnimation()
        {
            EyesDirection.x = Mathf.Clamp(EyesDirection.x, -1f,1f);
            EyesDirection.y = Mathf.Clamp(EyesDirection.y, -1f,1f);
            
            Bridge.BlendNormalized2(GeraldoDebugMap.EYE_BALLX, EyesDirection.x, LerpRate);
            Bridge.BlendNormalized2(GeraldoDebugMap.EYE_BALLY, EyesDirection.y, LerpRate);
        }
    }
}