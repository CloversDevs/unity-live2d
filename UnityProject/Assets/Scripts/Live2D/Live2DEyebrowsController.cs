using UnityEngine;

namespace Dedalord.LiveAr
{
    public class Live2DEyebrowsController : Live2DControllerElement
    {
        public EyeBrowPosition EyeBrows;
        public float LerpRate = 0.2f;

        private void Awake()
        {
            Controller.OnChangeExpression += expression => EyeBrows = expression.Eyebrows;
        }
        
        private void Update()
        {
            EyebrowAnimation();
        }
        
        private void EyebrowAnimation()
        {
            Bridge.Blend(GeraldoDebugMap.BROW_RX, EyeBrows.PositionX, LerpRate);
            Bridge.Blend(GeraldoDebugMap.BROW_RY, EyeBrows.PositionY, LerpRate);
            Bridge.Blend(GeraldoDebugMap.BROW_RFORM, EyeBrows.Form, LerpRate);
            Bridge.Blend(GeraldoDebugMap.BROW_RANGLE, EyeBrows.Angle, LerpRate);
        }
    }
}