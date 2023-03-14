using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dedalord.LiveAr
{
    public class Live2DMouthController : Live2DControllerElement
    {
        [Range(0f,1f)]
        public float MouthShape = 0.5f;
        
        [Range(0.01f,1f)]
        public float LerpRate = 0.2f;

        private void Awake()
        {
            var controller = GetComponent<Live2DCharacterController>();
            if (controller == null)
            {
                return;
            }

            void ListenToController(Expression expression)
            {
                MouthShape = expression.MouthShape;
            }
            
            controller.OnChangeExpression += ListenToController;
        }
        
        private void Update()
        {
            MouthShapeAnimation();
        }
        
        private void MouthShapeAnimation()
        {
            Bridge.BlendNormalized(GeraldoDebugMap.MOUTH_OPEN_UPY, MouthShape, LerpRate);
        }
    }
}
