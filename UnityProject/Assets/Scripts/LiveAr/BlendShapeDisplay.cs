using System.Collections.Generic;
using TMPro;
using UnityEngine;
#if UNITY_IOS && !UNITY_EDITOR
using UnityEngine.XR.ARKit;
#endif

namespace Dedalord.LiveAr
{
    /// <summary>
    /// Use the BlendShapeInfoSource to modify a CubismCharacter parameters.
    /// </summary>
    public class BlendShapeDisplay : MonoBehaviour
    {
        public TMP_Text Display;
        public Live2DCharacterBridge Live2DCharacterBridge;
#if UNITY_IOS && !UNITY_EDITOR
        private void Start()
        {
            BlendShapeInfoSource.Instance.OnChange += BlendShapes_OnChange;
        }

        private void BlendShapes_OnChange(Dictionary<ARKitBlendShapeLocation, float> blendShapes)
        {
            var jawOpen = Mathf.InverseLerp(
                10,
                100,
                Mathf.Clamp(
                    blendShapes[ARKitBlendShapeLocation.JawOpen],
                    10,
                    100
                )
            );
            CharacterBridge.SetNormalized(Map.JAW_OPEN, jawOpen);

            var mouthOpen = 1 - Mathf.InverseLerp(
                10,
                100,
                Mathf.Clamp(
                    blendShapes[ARKitBlendShapeLocation.MouthClose], 
                    10, 
                    100
                    )   
                );
            CharacterBridge.SetNormalized(Map.MOUTH_OPEN_DOWNY, jawOpen);
            CharacterBridge.SetNormalized(Map.MOUTH_OPEN_UPY, jawOpen);
            
            if (Display == null)
            {
                return;
            }
            Display.text = $"{blendShapes[ARKitBlendShapeLocation.JawOpen]:000}\n{blendShapes[ARKitBlendShapeLocation.MouthClose]:000}\n{blendShapes[ARKitBlendShapeLocation.MouthPucker]:000}\n{blendShapes[ARKitBlendShapeLocation.MouthLowerDownLeft]:000}";
        }
#endif
    }
}