using TMPro;
using UnityEngine;

namespace Dedalord.LiveAr
{
    /// <summary>
    /// Displays framerate metrics on a TMP component.
    /// </summary>
    public class FramerateDisplay : MonoBehaviour
    {
        /// <summary>
        /// The TMP component used to display the framerate metrics.
        /// </summary>
        [SerializeField]
        private TMP_Text _fpsText;

        private readonly float[] _frameTimes = new float[240];
        private int _frameTimeIndex;
        private float _minFps = float.MaxValue;
        private float _maxFps = float.MinValue;
        private float _averageFps;
        private float _deltaTime;
        private float _fps;

        /// <summary>
        /// Unity Update.
        /// Calculate framerate metrics and refresh the display.
        /// </summary>
        private void Update()
        {
            // Calculate the current FPS and update the min/max FPS values
            _deltaTime += (Time.deltaTime - _deltaTime) * 0.1f;
            _fps = 1.0f / _deltaTime;
            
            // Update the frameTimes array with the current FPS value
            _frameTimes[_frameTimeIndex] = _fps;
            _frameTimeIndex = (_frameTimeIndex + 1) % _frameTimes.Length;
            
            // Update the min and max framerate
            _minFps = float.MaxValue;
            _maxFps = float.MinValue;
            foreach (var frame in _frameTimes)
            {
                if (frame < _minFps)
                {
                    _minFps = _fps;
                }

                if (frame > _maxFps)
                {
                    _maxFps = _fps;
                }
            }

            // Calculate the average FPS over the last frames
            float sum = 0.0f;
            for (int i = 0; i < _frameTimes.Length; i++)
            {
                sum += _frameTimes[i];
            }
            _averageFps = sum / _frameTimes.Length;

            // Update the TextMeshProUGUI component with the new FPS metrics
            UpdateDisplay();
        }

        /// <summary>
        /// Update displayed FPS metrics.
        /// </summary>
        private void UpdateDisplay()
        {
            var fpsString = $"FPS: {Mathf.Round(_fps)}";
            var averageFpsString = $"Average FPS: {Mathf.Round(_averageFps)}";
            var minFpsString = $"Min FPS: {Mathf.Round(_minFps)}";
            var maxFpsString = $"Max FPS: {Mathf.Round(_maxFps)}";

            _fpsText.text = $"{fpsString}\n{averageFpsString}\n{minFpsString}\n{maxFpsString}";
        }
    }
}
