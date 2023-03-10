using System;
using UnityEngine;

namespace Dedalord.LiveAr
{
    /// <summary>
    /// MonoBehaviour for turning sentence strings into speech streams
    /// with event hooks 
    /// </summary>
    public class SpeechStreamController : MonoBehaviour
    {
        /// <summary>
        /// Playback speed.
        /// </summary>
        public float Speed = 20f;
        
        /// <summary>
        /// Invoked when a speech starts playing.
        /// </summary>
        public event Action OnStartTalking;
        
        /// <summary>
        /// Invoked when a speech progresses, moving along the text.
        /// string: text of the speech so far.
        /// </summary>
        public event Action<string> OnAddText;
        
        /// <summary>
        /// Invoked when a speech stops playing or is completed.
        /// </summary>
        public event Action OnStopTalking;
        
        /// <summary>
        /// Invoked when a new mouth viseme is reached in the playing speech.
        /// </summary>
        public event Action<Viseme> OnReachViseme;
        
        /// <summary>
        /// Invoked when the speech controller is ready to start.
        /// </summary>
        private event Action OnReady;

        /// <summary>
        /// Have all the required assets loaded.
        /// </summary>
        private bool _isReady;

        /// <summary>
        /// Text to Visemes converter.
        /// </summary>
        private TextToVisemes _vicemes;
        
        /// <summary>
        /// Current speech being read.
        /// </summary>
        private SpeechStream _currentSpeech;
        
        
        public TextAsset Dictionary;
        
        /// <summary>
        /// Unity Awake.
        /// </summary>
        private async void Awake()
        {
            _vicemes = new TextToVisemes();
            _vicemes.Load(Dictionary.text);
            _isReady = true;
            OnReady?.Invoke();
            OnReady = null;
        }

        /// <summary>
        /// Use this to make sure the action is only called when the speech controller is ready.
        /// If it is already ready, it is invoked immediately.
        /// </summary>
        public void WhenReady(Action actionWhenReady)
        {
            if (_isReady)
            {
                actionWhenReady?.Invoke();
                return;
            }

            OnReady += actionWhenReady;
        }

        /// <summary>
        /// Play a speech from the given sentence.
        /// Stop any playing speech.
        /// </summary>
        public void Play(string sentence)
        {
            _currentSpeech?.Stop();
            
            CreateStream(sentence);
            _currentSpeech?.Play();
        }
        
        /// <summary>
        /// Create a speech stream from the sentence and hook it up to trigger the controller events.
        /// </summary>
        private void CreateStream(string sentence)
        {
            _currentSpeech = new SpeechStream(sentence, Speed, _vicemes);
            _currentSpeech.OnReachViseme += v => OnReachViseme?.Invoke(v);
            _currentSpeech.OnStartTalking += () => OnStartTalking?.Invoke();
            _currentSpeech.OnStopTalking += () => OnStopTalking?.Invoke();
            _currentSpeech.OnAddText += s => OnAddText?.Invoke(s);
        }
        
        /// <summary>
        /// Unity Update().
        /// </summary>
        private void Update()
        {
            if (!_isReady || _currentSpeech == null)
            {
                return;
            }
            _currentSpeech.Update();
        }
    }
}