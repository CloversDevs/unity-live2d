using Live2D.Cubism.Framework;
using UnityEngine;

namespace Dedalord.LiveAr
{
    public class Live2DBlinkController : Live2DControllerElement
    {
        [Range(0f, 2f)]
        public float BlinkDuration = 2f;
        
        [Range(1f, 60f)]
        public float TimeBetweenBlinksMin = 2f;
        
        [Range(1f, 60f)]
        public float TimeBetweenBlinksMax = 10f;
        
        [Range(0f, 1f)]
        public float LerpRate = 0.1f;
        
        [Range(0f, 1f)]
        public float OpenEyesValue = 0.8f;
        
        private bool _isBlinking;
        private float _countdownToNextBlink = 2f;
        private float _blinkingTimer;
        private CubismEyeBlinkController _blinkController;
        
        private void Awake()
        {
            _blinkController = GetComponentInChildren<CubismEyeBlinkController>();
            
            var controller = GetComponent<Live2DCharacterController>();
            if (controller == null)
            {
                return;
            }

            controller.OnChangeExpression += expression => OpenEyesValue = expression.OpenEyesValue;
        }

        private void Update()
        {
            BlinkAnimation();
        }
        
        private void BlinkAnimation()
        {
            // If in blinking animation lerp to close eyes.
            if (_isBlinking)
            {
                _blinkingTimer += Time.deltaTime;
                _isBlinking = _blinkingTimer < BlinkDuration;
                _blinkController.EyeOpening = Mathf.Lerp(_blinkController.EyeOpening, 0f, LerpRate);
                return;
            }
            
            // Check if it is time to blink.
            _countdownToNextBlink -= Time.deltaTime;
            _isBlinking = _countdownToNextBlink <= 0;
            if (_isBlinking)
            {
                _countdownToNextBlink = Random.Range(TimeBetweenBlinksMin, TimeBetweenBlinksMax);
                _blinkingTimer = 0f;
                Debug.Log("Blink!");
                return;
            }
            
            // Lerp to open eyes.
            _blinkController.EyeOpening = Mathf.Lerp(_blinkController.EyeOpening, OpenEyesValue, LerpRate);
        }
    }
}