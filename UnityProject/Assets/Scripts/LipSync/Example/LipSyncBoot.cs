using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Dedalord.LiveAr
{
    /// <summary>
    /// Bootstrap for the LipSync test scene.
    /// </summary>
    public class LipSyncBoot : MonoBehaviour
    {
        /// <summary>
        /// Test string 
        /// </summary>
        public string TestString = "This here is a test string! This is Geralt de Rivia!";
        
        /// <summary>
        /// Test emotion to display.
        /// </summary>
        public Emotion TestEmotion = Emotion.SAD;
        
        /// <summary>
        /// 
        /// </summary>
        public CharacterDisplayMono Display;
        
        /// <summary>
        /// Text display to update as the text progresses.
        /// </summary>
        public TMP_Text TextDisplay;
        
        /// <summary>
        /// Debug speech input field.
        /// </summary>
        public TMP_InputField TextInput;

        /// <summary>
        /// Controller for turning text into speech stream events.
        /// </summary>
        public SpeechStreamController SpeechController;
        
        /// <summary>
        /// Unity Start.
        /// </summary>
        private void Start()
        {
            SpeechController.OnStartTalking += () => Debug.LogError($"START");
            SpeechController.OnStopTalking += () => Debug.LogError($"END");
            //SpeechController.OnAddText += s => Debug.LogError($"TEXT: {s}");
            
            SpeechController.OnReachViseme += s => Display.SetMouthViseme(s);
            SpeechController.OnStopTalking += () => Display.SetMouthViseme("");
            SpeechController.OnAddText += s => TextDisplay.text = s;
            
            SpeechController.WhenReady(()=>
            {
                Display.SetEmotion(TestEmotion);
                SpeechController.Play(TestString);
            });
        }

        /// <summary>
        /// Play speech from the test input panel.
        /// </summary>
        public void PlayTestInput()
        {
            SpeechController.Play(TextInput.text);
        }
    }

    public interface ICharacterDisplay
    {
        public void SetEmotion(Emotion emotion);
        public void SetMouthViseme(string viseme);
    }

    public abstract class CharacterDisplayMono : MonoBehaviour, ICharacterDisplay
    {
        public abstract void SetEmotion(Emotion emotion);
        public abstract void SetMouthViseme(string viseme);
    }
}
