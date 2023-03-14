using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
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

        public GeraldoDebugController GeraldoController;
        
        /// <summary>
        /// Unity Start.
        /// </summary>
        private void Start()
        {
            SpeechController.OnStartTalking += () => Debug.LogError($"START");
            SpeechController.OnStopTalking += () => Debug.LogError($"END");
            //SpeechController.OnAddText += s => Debug.LogError($"TEXT: {s}");
            
            SpeechController.OnReachViseme += v => Display?.SetMouthViseme(v);
            SpeechController.OnStopTalking += () => Display?.SetMouthViseme(Viseme.Silence);
            SpeechController.OnAddText += s => TextDisplay.text = s;

            SpeechController.OnStartTalking += GeraldoController.StartTalking;
            SpeechController.OnStopTalking += GeraldoController.StopTalking;
            SpeechController.WhenReady(()=>
            {
                Display?.SetEmotion(TestEmotion);
                SpeechController.Play(TestString);
            });
        }

        public void Update()
        {
            if (!Input.GetKeyDown(KeyCode.D))
            {
                return;
            }

            PlayTestInput();
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
        public void SetMouthViseme(Viseme viseme);
    }

    public abstract class CharacterDisplayMono : MonoBehaviour, ICharacterDisplay
    {
        public abstract void SetEmotion(Emotion emotion);
        public abstract void SetMouthViseme(Viseme viseme);
    }
}
