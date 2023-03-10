using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARKit;

namespace Dedalord.LiveAr
{
    /// <summary>
    /// Attaach to an ARFace to expose the face blend shapes through a singleton.
    /// </summary>
    [RequireComponent(typeof(ARFace))]
    public class BlendShapeInfoSource : MonoBehaviour
    {
        public static BlendShapeInfoSource Instance => _instance;
        private static BlendShapeInfoSource _instance;
        public float CoefficientScale = 100.0f;

        public event Action<Dictionary<ARKitBlendShapeLocation, float>> OnChange;
        private readonly Dictionary<ARKitBlendShapeLocation, float> _values = new();
        
#if UNITY_IOS && !UNITY_EDITOR
        ARKitFaceSubsystem m_ARKitFaceSubsystem;

        Dictionary<ARKitBlendShapeLocation, int> m_FaceArkitBlendShapeIndexMap;
#endif
        

        ARFace m_Face;
        
        void OnEnable()
        {
            _instance = this;
            m_Face = GetComponent<ARFace>();
#if UNITY_IOS && !UNITY_EDITOR
            var faceManager = FindObjectOfType<ARFaceManager>();
            if (faceManager != null)
            {
                m_ARKitFaceSubsystem = (ARKitFaceSubsystem)faceManager.subsystem;
            }
#endif
        }

        private void Update()
        {
            UpdateFaceFeatures();
        }

        void UpdateFaceFeatures()
        {
#if UNITY_IOS && !UNITY_EDITOR
            using (var blendShapes = m_ARKitFaceSubsystem.GetBlendShapeCoefficients(m_Face.trackableId, Allocator.Temp))
            {
                _values.Clear();
                foreach (var featureCoefficient in blendShapes)
                {
                    _values[featureCoefficient.blendShapeLocation] = featureCoefficient.coefficient * CoefficientScale;
                }
                OnChange?.Invoke(_values);
            }
#endif
        }
    }
}
