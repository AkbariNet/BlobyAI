using BlobyAI.Methods;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace BlobyAI.ViewModels
{
    /// <summary>
    /// View‑model for the main page.  It holds the collection of
    /// message views that are displayed in the chat and exposes a
    /// helper to send a new message to the backend.
    /// </summary>
    internal class MainPageVM : INotifyPropertyChanged
    {
        /// <summary>
        /// Backing field for <see cref="MessagesViewer"/>.
        /// </summary>
        private StackLayout _messagesViewer = new StackLayout();

        /// <summary>
        /// The visual container that holds all chat message views.
        /// The UI can bind to this property to display the current
        /// list of messages.  When replaced, the property raises
        /// <see cref="PropertyChanged"/>.
        /// </summary>
        public StackLayout MessagesViewer
        {
            get => _messagesViewer;
            set
            {
                if (_messagesViewer != value)
                {
                    _messagesViewer = value;
                    OnPropertyChanged(nameof(MessagesViewer));
                }
            }
        }

        #region ------------- INotifyPropertyChanged Implementation -------------
        // -------------------------------------------------------

        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event for a given
        /// property name.  The caller should only invoke this when
        /// the property value actually changes.
        /// </summary>
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        /// <summary>
        /// Sends a message asynchronously via <see cref="MessageManager"/>.
        /// The method forwards the caller’s <c>MainPage</c> reference
        /// (so that the manager can update the UI) and the message text.
        /// </summary>
        /// <param name="mainPage">Reference to the owning page.</param>
        /// <param name="contextOfText">Text to send.</param>
        /// <returns>A task that completes with true if the send succeeded.</returns>
        public Task<bool> SendMessage(MainPage mainPage, string contextOfText)
        {
            return MessageManager.SendMessageAsync(contextOfText, mainPage);
        }
    }
}
