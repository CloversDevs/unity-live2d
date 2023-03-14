using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dedalord.LiveAr
{
    public class PerformanceBoot : MonoBehaviour
    {
        public Transform Container;
        public float ZOffset = 20f;
        public TMP_Text InstanceCountDisplay;
        public Transform Prefab;
        private readonly List<Transform> _instances = new();
        public int InstancesToAdd = 0;

        private void Awake()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 120;
        }
        
        public void AddInstance()
        {
            InstancesToAdd++;
        }
        
        public void RemoveInstance()
        {
            InstancesToAdd--;
        }

        public void Update()
        {
            if (InstancesToAdd == 0)
            {
                return;
            }
            
            while (InstancesToAdd > 0)
            {
                var instance = Instantiate(Prefab, Container);
                _instances.Add(instance);
                instance.transform.localPosition = new Vector3(0f,0f, _instances.Count * ZOffset);
                InstancesToAdd--;
            }
            
            while (InstancesToAdd < 0)
            {
                if (_instances.Count == 0)
                {
                    break;
                }
                Destroy(_instances[0].gameObject);
                _instances.RemoveAt(0);
                InstancesToAdd++;
            }
            
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(Container as RectTransform);
            InstanceCountDisplay.text = $"{_instances.Count + 1:00}";
        }
    }
}
