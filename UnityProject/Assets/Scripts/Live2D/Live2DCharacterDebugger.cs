using UnityEngine;

namespace Dedalord.LiveAr
{
    [RequireComponent(typeof(Live2DCharacterController))]
    public class Live2DCharacterDebugger : MonoBehaviour
    {
        [SerializeField, HideInInspector]
        private Live2DCharacterController _targetController;
        private bool _talking;
        private void OnValidate()
        {
            _targetController = GetComponent<Live2DCharacterController>();
        }
        
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
                _targetController.LookAtEyes(mouseWorldPosition);
                _targetController.LookAtHead(mouseWorldPosition);
            }
            
            if (!touchStart)
            {
                return;
            }

            _targetController.CurrentExpressionIndex++;
            _talking = !_talking;

            if (_talking)
            {
                _targetController.StartTalking();
                return;
            }
            _targetController.StopTalking();
        }
    }
}