using System;
using System.Collections.Generic;
using System.Linq;
using Live2D.Cubism.Rendering.Masking;
using UnityEngine;

namespace Dedalord.LiveAr
{
    [Serializable]
    public class MaskWithUsers
    {
        [SerializeField]
        private CubismMaskTexture _maskTexture;
        public int Users => _users;
        private int _users;
        
        public CubismMaskTexture Use()
        {
            _users++;
            return _maskTexture;
        }

        public void Return()
        {
            _users--;
            if (_users < 0)
            {
                _users = 0;
                Debug.LogError("Mask has been removed more times than it was used!");
            }
        }
    }
    public class Live2DMaskProvider : MonoBehaviour
    {
        public List<MaskWithUsers> AvailableMaskTextures;
        
        public int Get(out CubismMaskTexture texture)
        {
            var sortedList = AvailableMaskTextures.OrderBy(x => x.Users).ToList();
            var leastUsed = sortedList.First();
            texture = leastUsed.Use();
            
            return leastUsed.GetHashCode();
        }

        public void Return(int id)
        {
            var text = AvailableMaskTextures.Find(x => x.GetHashCode() == id);
            text.Return();
        }
    }
}