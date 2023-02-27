using UnityEngine;
using UnityEngine.UI;

namespace Dedalord.LiveAr
{
    /// <summary>
    /// Attach to a MonoBehaviour with a button for the selected message id to be sent when the button is pressed.
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class SimpleMessageButton : MonoBehaviour
    {
        /// <summary>
        /// Message to send.
        /// </summary>
        public SimpleMessageId MessageId;

        /// <summary>
        /// Unity Awake.
        /// </summary>
        public void Awake()
        {
            GetComponent<Button>().onClick.AddListener(Send);
        }
        
        /// <summary>
        /// Send the message to listeners.
        /// </summary>
        private void Send()
        {
            SimpleMessageRouter.SendMessage(MessageId);
        }
    }
}