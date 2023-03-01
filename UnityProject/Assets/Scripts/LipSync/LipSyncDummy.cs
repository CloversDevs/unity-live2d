using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Dedalord.LiveAr
{
    /// <summary>
    /// Animator controller for a TMP_Text text face.
    /// </summary>
    public class LipSyncDummy : CharacterDisplayMono
    {
        /// <summary>
        /// Definition for a dummy face, with the resting and talking face poses
        /// </summary>
        private struct DummyFaceExpression
        {
            /// <summary>
            /// String to display when the mouth is closed.
            /// </summary>
            public readonly string RestingPose;
            
            /// <summary>
            /// String to display when the mouth is opened.
            /// </summary>
            public readonly string TalkingPose;

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="restingPose">String to display when the mouth is closed.</param>
            /// <param name="talkingPose">String to display when the mouth is opened.</param>
            public DummyFaceExpression(string restingPose, string talkingPose)
            {
                RestingPose = restingPose;
                TalkingPose = talkingPose;
            }
        }
        
        /// <summary>
        /// Where to display face.
        /// </summary>
        public TMP_Text Display;
        
        /// <summary>
        /// Where to display current viseme.
        /// </summary>
        public TMP_Text Viseme;
        
        /// <summary>
        /// Expression to display.
        /// </summary>
        private DummyFaceExpression _expression;
        
        /// <summary>
        /// Mouth viseme to display.
        /// </summary>
        private string _viseme = "";

        /// <summary>
        /// Current animation frame.
        /// </summary>
        private int _animFrame;

        /// <summary>
        /// How fast to change animation frames.
        /// </summary>
        public float AnimDuration = 0.75f;
        
        /// <summary>
        /// How long the current animation frame has been displayed.
        /// </summary>
        private float _animTimer = 1f;
        
        /// <summary>
        /// Map of emotions to faces.
        /// </summary>
        private readonly Dictionary<Emotion, DummyFaceExpression> Expressions = new()
        {
            { Emotion.IDLE, new(":)", ":O") },
            { Emotion.HAPPY,  new(":D", ":O") },
            { Emotion.SAD,  new(":(", ":O") },
            { Emotion.ANGRY,  new(">:(", ">:O") },
        };

        /// <summary>
        /// Unity Awake.
        /// </summary>
        private void Awake()
        {
            _expression = Expressions[Emotion.IDLE];
            _animTimer = AnimDuration;
        }
        
        /// <summary>
        /// Set emotion to display.
        /// </summary>
        public override void SetEmotion(Emotion emotion)
        {
            _expression = Expressions[emotion];
        }

        /// <summary>
        /// Set mouth viseme to display.
        /// </summary>
        public override void SetMouthViseme(string viseme)
        {
            _viseme = viseme;
            Viseme.text = viseme;
        }

        /// <summary>
        /// Unity LateUpdate.
        /// </summary>
        private void LateUpdate()
        {
            _animTimer -= Time.deltaTime;
            if (_animTimer > 0)
            {
                return;
            }
            _animTimer = AnimDuration;
            
            if (_viseme == "")
            {
                Display.text = _expression.RestingPose;
                _animFrame = 0;
                Viseme.text = "";
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