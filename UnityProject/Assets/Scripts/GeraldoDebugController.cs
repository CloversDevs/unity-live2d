using System.Collections;
using System.Collections.Generic;
using Live2D.Cubism.Framework;
using UnityEngine;

namespace Dedalord.LiveAr
{
    public static class GeraldoDebugMap
    {
        public const string MOUTH_OPEN_UPY = "ParamMouthOpenUpY";
        public const string MOUTH_OPEN_DOWNY = "ParamMouthOpenDownY";
        public const string JAW_OPEN = "ParamJawOpen";
        public const string PARAM = "Param";
        public const string EYE_ROPEN = "ParamEyeROpen";
        public const string EYE_BALLX = "ParamEyeBallX";
        public const string EYE_BALLY = "ParamEyeBallY";
        public const string BROW_RX = "ParamBrowRX";
        public const string BROW_RY = "ParamBrowRY";
        public const string BROW_RANGLE = "ParamBrowRAngle";
        public const string BROW_RFORM = "ParamBrowRForm";
        public const string ANGLE_X = "ParamAngleX";
        public const string ANGLE_Y = "ParamAngleY";
        public const string ANGLE_Z = "ParamAngleZ";
        public const string EYE_LOPEN = "ParamEyeLOpen";
        public const string EYE_LSMILE = "ParamEyeLSmile";
        public const string EYE_RSMILE = "ParamEyeRSmile";
        public const string BROW_LY = "ParamBrowLY";
        public const string BROW_LX = "ParamBrowLX";
        public const string BROW_LANGLE = "ParamBrowLAngle";
        public const string BROW_LFORM = "ParamBrowLForm";
        public const string MOUTH_FORM = "ParamMouthForm";
        public const string MOUTH_OPENY = "ParamMouthOpenY";
        public const string CHEEK_ = "ParamCheek";
        public const string BODY_ANGLEX = "ParamBodyAngleX";
        public const string BODY_ANGLEY = "ParamBodyAngleY";
        public const string BODY_ANGLEZ = "ParamBodyAngleZ";
        public const string BREATH_ = "ParamBreath";
        public const string HAIR_FRONT = "ParamHairFront";
        public const string HAIR_SIDE = "ParamHairSide";
        public const string HAIR_BACK = "ParamHairBack";
    }

    [System.Serializable]
    public class EyeBrowPosition
    {
        [Range(0f, 1f)]
        public float PositionX = 0.5f;
        [Range(0f, 1f)]
        public float PositionY = 0.5f;
        [Range(0f, 1f)]
        public float Angle = 0.5f;
        [Range(0f, 1f)]
        public float Form = 0.5f;
    }

    [RequireComponent(typeof(CubismCharacterBridge)),
     RequireComponent(typeof(CubismEyeBlinkController))]
    public class GeraldoDebugController : MonoBehaviour
    {
        public Expression[] Expressions;
        public int ExpressionSelectionIndex;
        private Expression CurrentExpresion => Expressions[ExpressionSelectionIndex];
        
        private Vector2 HeadDirection = Vector2.one * .5f;
        private Vector2 EyesDirection = Vector2.one * .5f;
        public Vector2 HeadRangeMin;
        public Vector2 HeadRangeMax;
        [Range(0f, 1f)]
        public float EyeBrowsAnimationLerpRate = 0.1f;
        [Range(0f, 1f)]
        public float MouthAnimationLerpRate = 0.1f;
        public float OpenEyesValue => CurrentExpresion.OpenEyesValue;
        public float MouthShape => CurrentExpresion.MouthShape;
        public EyeBrowPosition EyeBrows => CurrentExpresion.Eyebrows;
        public Transform FaceZeroPoint;
        public Vector2 LookAtCorrection;
        
        [Range(0f, 2f)]
        public float BlinkDuration = 2f;
        [Range(1f, 60f)]
        public float TimeBetweenBlinksMin = 2f;
        [Range(1f, 60f)]
        public float TimeBetweenBlinksMax = 10f;
        [Range(0f, 1f)]
        public float BlinkAnimationLerpRate = 0.1f;
        
        private bool _isBlinking;
        private float _countdownToNextBlink = 2f;
        private float _blinkingTimer = 0f;
        
        private CubismCharacterBridge _bridge;
        private CubismEyeBlinkController _blinkController;
        
        private void Awake()
        {
            _bridge = GetComponent<CubismCharacterBridge>();
            _blinkController = GetComponent<CubismEyeBlinkController>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                ExpressionSelectionIndex++;
                ExpressionSelectionIndex %= Expressions.Length;
            }
            
            LookAtAnimation();
            EyebrowAnimation();
            BlinkAnimation();
            MouthAnimation();
        }

        private bool _isTalking;
        public void StartTalking()
        {
            _isTalking = true;
            _talkAnimTimer = TalkAnimDuration;
        }

        public void StopTalking()
        {
            _isTalking = false;
        }

        public float TalkAnimDuration = 0.2f;
        public float MouthTalkingClose = 0f;
        public float MouthTalkingOpen = 0.8f;
        private float _talkAnimTimer;
        private int _mouthAnimFrame;
        private float[] _mouthTalkingPositions = new float[]{0.1f, 0.8f};
        private void MouthAnimation()
        {
            _bridge.BlendNormalized(GeraldoDebugMap.MOUTH_OPEN_UPY, MouthShape, MouthAnimationLerpRate);
            
            _talkAnimTimer -= Time.deltaTime;
            
            if (!_isTalking)
            {
                _bridge.BlendNormalized(GeraldoDebugMap.MOUTH_OPEN_DOWNY, 0f, MouthAnimationLerpRate);
                return;
            }
            else
            {
                _bridge.BlendNormalized(GeraldoDebugMap.MOUTH_OPEN_DOWNY, _mouthAnimFrame == 0 ? MouthTalkingClose : MouthTalkingOpen, MouthAnimationLerpRate);
            }
            if (_talkAnimTimer > 0)
            {
                return;
            }
            _talkAnimTimer = TalkAnimDuration;
            
            _mouthAnimFrame++;
            _mouthAnimFrame %= _mouthTalkingPositions.Length;
        }

        public enum MouseMode
        {
            NONE = 0,
            HEAD_FOLLOW = 1,
            EYE_FOLLOW = 2,
            ALL_FOLLOW = 3,
        }
        
        private int _mouseMode;
        private int _mousModeCount = 4;
        private void LookAtAnimation()
        {
            var x = ((Input.mousePosition.x) / Screen.width - 0.5f) * 2f + LookAtCorrection.x;
            var y = ((Input.mousePosition.y) / Screen.height - 0.5f) * 2f + LookAtCorrection.y;

            if (Input.GetMouseButtonDown(0))
            {
                _mouseMode = 0;
                Debug.LogError((MouseMode) _mouseMode);
            }
            
            if (Input.GetMouseButtonDown(1))
            {
                _mouseMode++;
                _mouseMode %= _mousModeCount;
                Debug.LogError((MouseMode) _mouseMode);
            }

            if (_mouseMode == 1 || _mouseMode == 3)
            {
                HeadDirection = new Vector2(x, y);
            }
            if (_mouseMode == 2 || _mouseMode == 3)
            {
                EyesDirection = new Vector2(x, y);
            }
            
            HeadDirection.x = Mathf.Clamp(HeadDirection.x, -1f,1f);
            HeadDirection.y = Mathf.Clamp(HeadDirection.y, -1f,1f);

            HeadDirection.x = Mathf.Clamp(HeadDirection.x, HeadRangeMin.x, HeadRangeMax.x);
            HeadDirection.y = Mathf.Clamp(HeadDirection.y, HeadRangeMin.y, HeadRangeMax.y);
            
            EyesDirection.x = Mathf.Clamp(EyesDirection.x, -1f,1f);
            EyesDirection.y = Mathf.Clamp(EyesDirection.y, -1f,1f);
            
            _bridge.BlendNormalized(GeraldoDebugMap.EYE_BALLX, EyesDirection.x, EyeBrowsAnimationLerpRate);
            _bridge.BlendNormalized(GeraldoDebugMap.EYE_BALLY, EyesDirection.y, EyeBrowsAnimationLerpRate);
            _bridge.BlendNormalized(GeraldoDebugMap.ANGLE_X, HeadDirection.x, EyeBrowsAnimationLerpRate);
            _bridge.BlendNormalized(GeraldoDebugMap.ANGLE_Y, HeadDirection.y, EyeBrowsAnimationLerpRate);
        }
        
        private void EyebrowAnimation()
        {
            _bridge.BlendNormalized(GeraldoDebugMap.BROW_RX, EyeBrows.PositionX, EyeBrowsAnimationLerpRate);
            _bridge.BlendNormalized(GeraldoDebugMap.BROW_RY, EyeBrows.PositionY, EyeBrowsAnimationLerpRate);
            _bridge.BlendNormalized(GeraldoDebugMap.BROW_RFORM, EyeBrows.Form, EyeBrowsAnimationLerpRate);
            _bridge.BlendNormalized(GeraldoDebugMap.BROW_RANGLE, EyeBrows.Angle, EyeBrowsAnimationLerpRate);
        }
        private void BlinkAnimation()
        {
            if (_isBlinking)
            {
                _blinkingTimer += Time.deltaTime;
                _isBlinking = _blinkingTimer < BlinkDuration;
                _blinkController.EyeOpening = Mathf.Lerp(_blinkController.EyeOpening, 0f, BlinkAnimationLerpRate);
                return;
            }
            
            _countdownToNextBlink -= Time.deltaTime;
            _isBlinking = _countdownToNextBlink <= 0;
            if (_isBlinking)
            {
                _countdownToNextBlink = Random.Range(TimeBetweenBlinksMin, TimeBetweenBlinksMax);
                _blinkingTimer = 0f;
                Debug.LogError("Blink!");
                return;
            }
            _blinkController.EyeOpening = Mathf.Lerp(_blinkController.EyeOpening, OpenEyesValue, BlinkAnimationLerpRate);
        }
    }
}
