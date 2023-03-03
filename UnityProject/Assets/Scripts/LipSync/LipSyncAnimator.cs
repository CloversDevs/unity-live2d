using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Dedalord.LiveAr
{
    /// <summary>
    /// Controller for a character face display that displays emotion (TODO)
    /// and animates their mouth when talking.
    /// </summary>
    public class LipSyncAnimator : CharacterDisplayMono
    {
        /// <summary>
        /// Unity Animator to control.
        /// </summary>
        public Animator AnimatorController;
        
        /// <summary>
        /// Current emotion displayed.
        /// </summary>
        private Emotion _emotion;
        
        /// <summary>
        /// Current viseme displayed.
        /// </summary>
        private Viseme _viseme = Viseme.Silence;
        
        /// <summary>
        /// Current animation frame displayed.
        /// </summary>
        private int _animFrame = 0;

        /// <summary>
        /// Time between switching animation frames.
        /// </summary>
        public float AnimDuration = 0.75f;
        
        /// <summary>
        /// Time the current animation frame has been displayed for.
        /// </summary>
        private float _animTimer = 1f;
        
        /// <summary>
        /// Is character currently talking.
        /// </summary>
        private bool _isTalking;
        
        /// <summary>
        /// Unity Awake.
        /// </summary>
        private void Awake()
        {
            _emotion = Emotion.IDLE;
            _animTimer = AnimDuration;
            SetViseme(Viseme.Silence);
            SetAnimationFrame(0);
        }
        
        /// <summary>
        /// Set the face emotion.
        /// </summary>
        public override void SetEmotion(Emotion emotion)
        {
            _emotion = emotion;
        }

        /// <summary>
        /// Set the mouth posture.
        /// </summary>
        public override void SetMouthViseme(Viseme viseme)
        {
            _isTalking = viseme != Viseme.Silence;
            SetViseme(viseme);
        }

        private void SetViseme(Viseme viseme)
        {
            _viseme = viseme;
            var visemes = Enum.GetValues(typeof(Viseme));
            foreach (int value in visemes)
            {
                var id = $"{value}_{(Viseme)value}";
                AnimatorController.SetBool(id, (int)_viseme == value);
            }
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
            
            if (!_isTalking)
            {
                SetViseme(Viseme.Silence);
                return;
            }

        }

        /// <summary>
        /// Set the animation controller parameters to display the given animation state.
        /// </summary>
        private void SetAnimationFrame(int index)
        {
            _animFrame = index;
            switch (index)
            {
                case 0:
                {
                    AnimatorController.SetBool("Viseme_A", false);
                    AnimatorController.SetBool("Viseme_B", true);
                    AnimatorController.SetBool("Viseme_S", false);
                    return;
                }
                case 1:
                {
                    AnimatorController.SetBool("Viseme_A", true);
                    AnimatorController.SetBool("Viseme_B", false);
                    AnimatorController.SetBool("Viseme_S", false);
                    return;
                }
                case 2:
                {
                    AnimatorController.SetBool("Viseme_A", false);
                    AnimatorController.SetBool("Viseme_B", false);
                    AnimatorController.SetBool("Viseme_S", true);
                    return;
                }
            }
        }
    }
}