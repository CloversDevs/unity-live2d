using System.Collections.Generic;
using System.Text.RegularExpressions;
using Live2D.Cubism.Core;
using Live2D.Cubism.Framework;
using UnityEngine;

namespace Dedalord.LiveAr
{
    public static class Map
    {
        public const string ANGLE_X = "ParamAngleX";
        public const string ANGLE_Y = "ParamAngleY";
        public const string ANGLE_Z = "ParamAngleZ";
        public const string EYE_LOPEN = "ParamEyeLOpen";
        public const string EYE_LSMILE = "ParamEyeLSmile";
        public const string EYE_ROPEN = "ParamEyeROpen";
        public const string EYE_RSMILE = "ParamEyeRSmile";
        public const string EYE_BALLX = "ParamEyeBallX";
        public const string EYE_BALLY = "ParamEyeBallY";
        public const string BROW_LY = "ParamBrowLY";
        public const string BROW_RY = "ParamBrowRY";
        public const string BROW_LX = "ParamBrowLX";
        public const string BROW_RX = "ParamBrowRX";
        public const string BROW_LANGLE = "ParamBrowLAngle";
        public const string BROW_RANGLE = "ParamBrowRAngle";
        public const string BROW_LFORM = "ParamBrowLForm";
        public const string BROW_RFORM = "ParamBrowRForm";
        public const string MOUTH_FORM = "ParamMouthForm";
        public const string MOUTH_OPEN_UPY = "ParamMouthOpenUpY";
        public const string MOUTH_OPEN_DOWNY = "ParamMouthOpenDownY";
        public const string JAW_OPEN = "ParamJawOpen";
        public const string CHEEK_ = "ParamCheek";
        public const string BODY_ANGLEX = "ParamBodyAngleX";
        public const string BODY_ANGLEY = "ParamBodyAngleY";
        public const string BODY_ANGLEZ = "ParamBodyAngleZ";
        public const string BREATH_ = "ParamBreath";
        public const string HAIR_FRONT = "ParamHairFront";
        public const string HAIR_SIDE = "ParamHairSide";
        public const string HAIR_BACK = "ParamHairBack";
    }
    
    public class Live2DCharacterBridge : MonoBehaviour
    {
        /// <summary>
        /// Reference to cubism model on same GameObject to modify.
        /// </summary>
        [SerializeField, HideInInspector]
        private CubismModel _model;
        
        /// <summary>
        /// Reference to cubism parameter store on same GameObject to use to apply changes.
        /// </summary>
        private CubismParameterStore _parameterStore;
        
        /// <summary>
        /// All cubism parameters mapped to their identifier.
        /// </summary>
        private readonly Dictionary<string, CubismParameter> _parameters = new();
        
        /// <summary>
        /// All changes to parameters that will be applied at the end of LateUpdate.
        /// </summary>
        private readonly Dictionary<CubismParameter, float> _pendingModifications = new();
        
        /// <summary>
        /// All changes to parameter blending that will be applied at the end of LateUpdate.
        /// </summary>
        private readonly Dictionary<CubismParameter, (float, float)> _pendingBlendings = new();

        /// <summary>
        /// Get an array with all the parameter ids. 
        /// </summary>
        /// <returns></returns>
        public string[] GetAllParameters()
        {
            var result = new string[_parameters.Count];
            var i = 0;
            foreach (var p in _parameters)
            {
                result[i] = p.Key;
                i++;
            }

            return result;
        }
        
        /// <summary>
        /// Set the parameter value at the end of the next LateUpdate.
        /// Using SetNormalized instead is recommended.
        /// </summary>
        public void Set(string id, float value)
        {
            var p = _parameters[id];
            _pendingModifications[p] = value;
        }
    
        /// <summary>
        /// Set the parameter value at the end of the next LateUpdate.
        /// Values will go from min to max with -1 to 1 regardless of actual range.
        /// </summary>
        public void SetNormalized(string id, float value)
        {
            var p = _parameters[id];
            _pendingModifications[p] = Mathf.Lerp(p.MinimumValue, p.MaximumValue, value);
        }
        
        public void BlendNormalized(string id, float value, float rate)
        {
            var p = _parameters[id];
            _pendingBlendings[p] = (Mathf.Lerp(p.MinimumValue, p.MaximumValue, value), rate);
        }
        
        public void BlendNormalized2(string id, float value, float rate)
        {
            var p = _parameters[id];
            _pendingBlendings[p] = (Mathf.Lerp(p.MinimumValue, p.MaximumValue, value * 0.5f + 0.5f), rate);
        }

        private void OnValidate()
        {
            _model ??= GetComponentInChildren<CubismModel>();

            if (_model == null)
            {
                Debug.LogError("Live2DCharacterBridge MUST have a CubismModel on the same MonoBehaviour or in its children to control!");
            }
        }

        private string GenerateMap(string name)
        {
            var str = $"public static class {name}\n    {{\n";
                
            for (var i = 0; i < _model.Parameters.Length; ++i)
            {
                var output = Regex.Replace(_model.Parameters[i].Id, @"^Param([A-Z][a-z]+)|([A-Z][a-z]+)([A-Z][a-z]+)", "$1$2_$3").ToUpper();
                str += $"    public const string {output} = \"{_model.Parameters[i].Id}\";\n";
            }
            return $"{str}\n}}\n";
        }

        /// <summary>
        /// Unity Awake.
        /// </summary>
        private void Awake()
        {
            // Get a reference to the CubismParameterStore component on the model
            _parameterStore ??= _model.GetComponent<CubismParameterStore>();

            if (_parameters.Count == 0)
            {
                for (var i = 0; i < _model.Parameters.Length; ++i)
                {
                    _parameters[_model.Parameters[i].Id] = _model.Parameters[i];
                }
                Debug.LogError(GenerateMap("TestMap"));
            }
        }
        
        /// <summary>
        /// Unity LateUpdate.
        /// Apply changes and blends.
        /// </summary>
        private void LateUpdate()
        {
            if (_pendingModifications.Count == 0 && _pendingBlendings.Count == 0)
            {
                return;
            }

            foreach (var modification in _pendingModifications)
            {
                modification.Key.Value = modification.Value;
            }
            _pendingModifications.Clear();
            
            foreach (var blend in _pendingBlendings)
            {
                blend.Key.Value = Mathf.Lerp(blend.Key.Value, blend.Value.Item1, blend.Value.Item2) ;
            }
            _pendingBlendings.Clear();
            
            // Save the current values of the model's parameters and parts
            _parameterStore.SaveParameters();
        }
    }
}