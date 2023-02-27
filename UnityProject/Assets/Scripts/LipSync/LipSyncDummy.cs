using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Dedalord.LiveAr
{
    public class DummyFaceExpression
    {
        public readonly string RestingPose = ":)";
        public readonly string TalkingPose = ":O";

        public DummyFaceExpression(string restingPose, string talkingPose)
        {
            RestingPose = restingPose;
            TalkingPose = talkingPose;
        }
    }

    public enum Emotion
    {
        IDLE,
        HAPPY,
        SAD,
        ANGRY,
    }
    public class LipSyncDummy : CharacterDisplayMono
    {
        public TMP_Text Display;
        private DummyFaceExpression _expression;
        private string _mouthMovement = "";
        private int _animFrame = 0;

        public float AnimDuration = 0.75f;
        private float _animTimer = 1f;
        
        private readonly Dictionary<Emotion, DummyFaceExpression> Expressions = new()
        {
            { Emotion.IDLE, new(":)", ":O") },
            { Emotion.HAPPY,  new(":D", ":O") },
            { Emotion.SAD,  new(":(", ":O") },
            { Emotion.ANGRY,  new(">:(", ">:O") },
        };

        private void Awake()
        {
            _expression = Expressions[Emotion.IDLE];
            _animTimer = AnimDuration;
        }
        
        public override void SetEmotion(Emotion emotion)
        {
            _expression = Expressions[emotion];
        }

        public override void SetMouthMovement(string movement)
        {
            _mouthMovement = movement;
        }

        private void LateUpdate()
        {
            _animTimer -= Time.deltaTime;
            if (_animTimer > 0)
            {
                return;
            }
            _animTimer = AnimDuration;
            
            if (_mouthMovement == "")
            {
                Display.text = _expression.RestingPose;
                _animFrame = 0;
                return;
            }

            _animFrame = 1 - _animFrame;

            if (_animFrame == 0)
            {
                Display.text = _expression.RestingPose;
                return;
            }
            Display.text = _expression.TalkingPose;
        }
    }
}