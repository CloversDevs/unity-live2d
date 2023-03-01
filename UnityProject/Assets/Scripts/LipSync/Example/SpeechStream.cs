using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dedalord.LiveAr
{
    /// <summary>
    /// Class that takes a string and a viseme provider and allows playback as a speech following a set
    /// of parameters.
    /// Sends events as the speech progresses to track mouth movements.
    /// </summary>
    public class SpeechStream
    {
        /// <summary>
        /// Invoked when the speech starts playing.
        /// </summary>
        public event Action OnStartTalking;
        
        /// <summary>
        /// Invoked when the speech progresses, moving along the text.
        /// string: text of the speech so far.
        /// </summary>
        public event Action<string> OnAddText;
        
        /// <summary>
        /// Invoked when the speech stops playing or is completed.
        /// </summary>
        public event Action OnStopTalking;
        
        /// <summary>
        /// Invoked when the new mouth viseme is reached during playback.
        /// </summary>
        public event Action<string> OnReachViseme;
        
        /// <summary>
        /// String provided to make the speech.
        /// </summary>
        private readonly string _sentence;
        
        /// <summary>
        /// List of visemes and positions along the text.
        /// </summary>
        private readonly List<PhonemeInText> _visemes;
        
        /// <summary>
        /// Playback time.
        /// </summary>
        private float _time;
        
        /// <summary>
        /// Playback speed.
        /// </summary>
        public float Speed = 10f;
        
        /// <summary>
        /// Character index on the text that is currently being said in the speech.
        /// </summary>
        private int _currentCharacterIndex;
        
        /// <summary>
        /// Is the speech currently playing.
        /// </summary>
        private bool _isPlaying;
        
        /// <summary>
        /// Is the speech about to start.
        /// </summary>
        private bool _hasToStart;
        
        /// <summary>
        /// Viseme index on the text that is currently being said in the speech.
        /// </summary>
        private int _currentVisemeIndex;
        
        
        public SpeechStream(string sentence, TextToVisemes visemeProvider)
        {
            _sentence = sentence;
            _visemes = visemeProvider.GetSentencePhonemes(_sentence);
        }

        /// <summary>
        /// Start playing the speech.
        /// </summary>
        public void Play()
        {
            _hasToStart = true;
            _currentCharacterIndex = 0;
        }
        
        /// <summary>
        /// Stop playing the speech.
        /// </summary>
        public void Stop()
        {
            _isPlaying = false;
            _hasToStart = false;
            _currentCharacterIndex = 0;
        }
        
        /// <summary>
        /// Progress the speech if playing.
        /// </summary>
        public void Update()
        {
            if (!_isPlaying)
            {
                if (!_hasToStart)
                {
                    return;
                }

                _hasToStart = false;
                _isPlaying = true;
                OnStartTalking?.Invoke();
            }
            
            _time += Time.deltaTime * Speed;
            var newIndex = Mathf.FloorToInt(_time);
            var hasReachedEnd = newIndex >= _sentence.Length;
            if (hasReachedEnd)
            {
                newIndex = _sentence.Length;
            }

            if (newIndex == _currentCharacterIndex)
            {
                return;
            }

            _currentCharacterIndex = newIndex;
            for (var i = _currentVisemeIndex; i < _visemes.Count; i++)
            {
                if (_visemes[i].Index < _currentCharacterIndex)
                {
                    OnReachViseme?.Invoke(_visemes[i].Phoneme);
                }
            }
            
            OnAddText?.Invoke(_sentence.Substring(0, newIndex));
            if (hasReachedEnd)
            {
                OnStopTalking?.Invoke();
            }
        }
    }
}