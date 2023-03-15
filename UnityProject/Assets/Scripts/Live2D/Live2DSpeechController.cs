using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dedalord.LiveAr
{
    public class Live2DSpeechController : Live2DControllerElement
    {
        public bool IsTalking;
        
        [Range(0f, 1f)]
        public float TalkAnimDuration = 0.2f;
        
        [Range(0f, 1f)]
        public float MouthTalkingClose = 0f;
        
        [Range(0f, 1f)]
        public float MouthTalkingOpen = 0.8f;
        
        [Range(0.01f,1f)]
        public float LerpRate = 0.2f;
        
        private float _talkAnimTimer;
        
        private int _mouthAnimFrame;
        
        
        private void Awake()
        {
            void ListenToController(bool isTalking)
            {
                IsTalking = isTalking;
            }
            
            Controller.OnChangeTalkingState += ListenToController;
        }

        private void Update()
        {
            MouthMoveAnimation();
        }
        
        private void MouthMoveAnimation()
        {
            _talkAnimTimer -= Time.deltaTime;
            
            if (!IsTalking)
            {
                Bridge.Blend(GeraldoDebugMap.MOUTH_OPEN_DOWNY, 0f, LerpRate);
                return;
            }
            Bridge.Blend(GeraldoDebugMap.MOUTH_OPEN_DOWNY, _mouthAnimFrame == 0 ? MouthTalkingClose : MouthTalkingOpen, LerpRate);
            
            if (_talkAnimTimer > 0)
            {
                return;
            }
            _talkAnimTimer = TalkAnimDuration;
            
            _mouthAnimFrame++;
            _mouthAnimFrame %= 2;
        }
    }
}
