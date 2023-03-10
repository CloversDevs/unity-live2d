using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Live2D.Cubism.Core;
using Live2D.Cubism.Framework;
using UnityEngine;
using UnityEngine.XR.ARKit;

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
    
    
    public class CubismCharacterBridge : MonoBehaviour
    {
        private CubismModel model;
        private CubismParameterStore parameterStore;
        private readonly Dictionary<string, CubismParameter> _parameters = new();
        private readonly Dictionary<CubismParameter, float> _pendingModifications = new();
        private readonly Dictionary<CubismParameter, (float, float)> _pendingBlendings = new();

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
        public void Set(string id, float value)
        {
            var p = _parameters[id];
            _pendingModifications[p] = value;
        }
    
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

        private string GenerateMap(string name)
        {
            var str = $"public static class {name}\n    {{\n";
                
            for (var i = 0; i < model.Parameters.Length; ++i)
            {
                var output = Regex.Replace(model.Parameters[i].Id, @"^Param([A-Z][a-z]+)|([A-Z][a-z]+)([A-Z][a-z]+)", "$1$2_$3").ToUpper();
                str += $"    public const string {output} = \"{model.Parameters[i].Id}\";\n";
            }
            return $"{str}\n}}\n";
        }
        
        /// <summary>
        /// Unity LateUpdate.
        /// </summary>
        private void LateUpdate()
        {
            // Get a reference to the CubismModel you want to modify
            model ??= GetComponent<CubismModel>();

            // Get a reference to the CubismParameterStore component on the model
            parameterStore ??= model.GetComponent<CubismParameterStore>();

            if (_parameters.Count == 0)
            {
                for (var i = 0; i < model.Parameters.Length; ++i)
                {
                    _parameters[model.Parameters[i].Id] = model.Parameters[i];
                }
                Debug.LogError(GenerateMap("TestMap"));
            }

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
            parameterStore.SaveParameters();
        }
    }
}