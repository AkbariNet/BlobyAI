using System;
using System.Collections.Generic;
using System.Text;

namespace BlobyAI.Models
{
    /// <summary>
    /// ConnectionModel – a simple static holder for the backend connection
    /// settings.  These values are shared across the entire application
    /// (hence static).  They are updated by the ConnectionPanel and read by
    /// the LLM list and message sending logic.
    /// </summary>
    public static class ConnectionModel
    {
        /// <summary>
        /// Server IP address used for the language‑model backend.
        /// Default value is 192.168.0.102.
        /// </summary>
        public static string IPAddress { get; set; } = "192.168.0.102";

        /// <summary>
        /// Port number that the backend is listening on.
        /// Default is 11434.
        /// </summary>
        public static string Port { get; set; } = "11434";

        /// <summary>
        /// The name of the currently selected LLM model.
        /// Default is gemma3:12b.
        /// </summary>
        public static string Model { get; set; } = "gemma3:12b";
    }
}
