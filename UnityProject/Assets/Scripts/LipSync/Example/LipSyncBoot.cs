using TMPro;
using UnityEngine;

namespace Dedalord.LiveAr
{
    public class LipSyncBoot : MonoBehaviour
    {
        public string TestString = "This here is a test string!";
        public Emotion TestEmotion = Emotion.SAD;
        public float Speed = 0.1f;
        public CharacterDisplayMono Display;
        public TMP_Text TextDisplay;

        private float _time;
        
        private void Update()
        {
            _time += Time.deltaTime * Speed;
            var progress = Mathf.FloorToInt(_time);
            if (progress > TestString.Length)
            {
                progress = TestString.Length;
            }
            
            Display.SetMouthMovement(progress == TestString.Length ? "" : TestString[progress].ToString());
            Display.SetEmotion(TestEmotion);
            TextDisplay.text = TestString.Substring(0, progress);
        }
    }

    public interface ICharacterDisplay
    {
        public void SetEmotion(Emotion emotion);
        public void SetMouthMovement(string movement);
    }

    public abstract class CharacterDisplayMono : MonoBehaviour, ICharacterDisplay
    {
        public abstract void SetEmotion(Emotion emotion);
        public abstract void SetMouthMovement(string movement);
    }
}
