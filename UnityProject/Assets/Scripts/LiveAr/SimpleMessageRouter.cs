using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dedalord.LiveAr
{
    /// <summary>
    /// Quick class to send and listen to messages throughout the app.
    /// </summary>
    public static class SimpleMessageRouter
    {
        /// <summary>
        /// Map of message ids to event listeners.
        /// </summary>
        private static readonly Dictionary<SimpleMessageId, Action> _listeners = new();

        /// <summary>
        /// Remove all listeners.
        /// </summary>
        public static void Clear()
        {
            _listeners.Clear();
        }
        
        /// <summary>
        /// Add function to be invoked when the message id is sent.
        /// </summary>
        /// <returns>Unsubscription function.</returns>
        public static Action AddListener(SimpleMessageId messageId, Action action)
        {
            _listeners[messageId] = action;
            return () => _listeners.Remove(messageId);
        }
        
        /// <summary>
        /// Invoke all listeners of the given message id.
        /// </summary>
        public static void SendMessage(SimpleMessageId msgId)
        {
            try
            {
                _listeners[msgId]?.Invoke();
            }
            catch (KeyNotFoundException)
            {
                //
            }
        }
    }
}