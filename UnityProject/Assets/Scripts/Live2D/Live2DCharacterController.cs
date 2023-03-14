using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dedalord.LiveAr
{
    public class Live2DCharacterController : MonoBehaviour
    {
        public event Action<Expression> OnChangeExpression;
        public event Action<Vector2> OnChangeEyesLookAt;
        public event Action<Vector2> OnChangeHeadLookAt;
        public event Action<bool> OnChangeTalkingState;
        
        public Expression[] AvailableExpressions;
        public Expression CurrentExpresion => AvailableExpressions[_currentExpressionIndex];
        
        public int CurrentExpressionIndex
        {
            get
            {
                return _currentExpressionIndex;
            }
            set
            {
                _currentExpressionIndex = value;
                _currentExpressionIndex %= AvailableExpressions.Length;
                OnChangeExpression?.Invoke(CurrentExpresion);
            }
        }

        private Expression _currentExpression;
        private int _currentExpressionIndex;

        public void LookAtEyes(Vector2 direction)
        {
            OnChangeEyesLookAt?.Invoke(direction);
        }
        
        public void LookAtHead(Vector2 direction)
        {
            OnChangeHeadLookAt?.Invoke(direction);
        }
        
        public void StartTalking()
        {
            OnChangeTalkingState?.Invoke(true);
        }
        
        public void StopTalking()
        {
            OnChangeTalkingState?.Invoke(false);
        }
    }
}
