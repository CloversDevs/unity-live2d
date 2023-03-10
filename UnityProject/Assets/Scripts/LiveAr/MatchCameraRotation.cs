using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dedalord.LiveAr
{
    public class MatchCameraRotation : MonoBehaviour
    {
        private void LateUpdate()
        {
            transform.rotation = Camera.main.transform.rotation;
        }
    }
}
