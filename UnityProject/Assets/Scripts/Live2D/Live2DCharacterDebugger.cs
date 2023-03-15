using UnityEngine;

namespace Dedalord.LiveAr
{
    public class Live2DCharacterDebugger : Live2DControllerElement
    {
        private bool _isTalking;
        
        // Update is called once per frame
        private void Update()
        {
            bool touchStart = false;
            bool touch = false;
            Vector2 touchPosition = Vector2.zero;

#if !UNITY_EDITOR
            touch = Input.touchCount != 0;
            if (touch)
            {
                touchPosition = Input.GetTouch(0).position;
                touchStart = Input.GetTouch(0).phase == TouchPhase.Began;
            }
#else
            touch = Input.GetMouseButton(0);
            if (touch)
            {
                touchPosition = Input.mousePosition;
                touchStart = Input.GetMouseButtonDown(0);
            }
#endif
            
            if (touch)
            {
                Vector3 mouseScreenPosition = touchPosition;
                mouseScreenPosition.z = transform.position.z;
                var mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
                Controller.LookAtEyes(mouseWorldPosition);
                Controller.LookAtHead(mouseWorldPosition);
            }
            
            if (!touchStart)
            {
                return;
            }

            Controller.CurrentExpressionIndex++;
            _isTalking = !_isTalking;

            if (_isTalking)
            {
                Controller.StartTalking();
                return;
            }
            Controller.StopTalking();
        }
    }
}