using UnityEngine;

namespace Dedalord.LiveAr
{
    public class Live2DHeadController : Live2DControllerElement
    {
        public Vector2 HeadDirection;
        public Vector2 HeadMinRange = -Vector2.one;
        public Vector2 HeadMaxRange = Vector2.one;
        
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
            Controller.OnChangeHeadLookAt += ListenToController;
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
                HeadDirection = new(
                    LookTarget.localPosition.x * LookAtMultiplier, 
                    LookTarget.localPosition.y * LookAtMultiplier);
            }
            
            HeadLookAnimation();
        }
        
        private void HeadLookAnimation()
        {   
            HeadDirection.x = Mathf.Clamp(HeadDirection.x, -1f,1f);
            HeadDirection.y = Mathf.Clamp(HeadDirection.y, -1f,1f);

            HeadDirection.x = Mathf.Clamp(HeadDirection.x, HeadMinRange.x, HeadMaxRange.x);
            HeadDirection.y = Mathf.Clamp(HeadDirection.y, HeadMinRange.y, HeadMaxRange.y);
            
            Bridge.Blend(GeraldoDebugMap.ANGLE_X, HeadDirection.x, LerpRate, RangeMode.MINUS_ONE_TO_ONE);
            Bridge.Blend(GeraldoDebugMap.ANGLE_Y, HeadDirection.y, LerpRate, RangeMode.MINUS_ONE_TO_ONE);
        }
    }
}