using BlobyAI.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlobyAI.Methods
{
    /// <summary>
    /// PingFromServer – small helper that checks whether the
    /// backend server is reachable.  It performs a simple GET
    /// request to the base address and returns the HTTP status
    /// code so the caller can decide how to react.
    /// </summary>
    internal class PingFromServer
    {
        #region -------------------- Public API --------------------
        //----------------------------------------
        /// <summary>
        /// Sends a lightweight ping request to the server that filled by <see cref="ConnectionModel"/>.
        /// </summary>
        /// <returns>
        /// <see cref="HttpStatusCode"/> representing the server response.
        /// </returns>
        public static async Task<HttpStatusCode> StartAsync()
        {
            // Configure a HttpClient that accepts any certificate
            // (useful for local or self‑signed dev environments)
            using var handler = new HttpClientHandler
            {
                UseProxy = false,
                ServerCertificateCustomValidationCallback = (s, c, ch, e) => true,
                AllowAutoRedirect = true
            };

            // Build the client with a 10‑second timeout and the
            // base address taken from the global connection settings
            using var client = new HttpClient(handler)
            {
                Timeout = TimeSpan.FromSeconds(10),
                BaseAddress = new Uri($"http://{ConnectionModel.IPAddress}:{ConnectionModel.Port}")
            };

            // Perform the GET request and return the status code
            HttpResponseMessage pingResponse = await client.GetAsync(client.BaseAddress);
            return pingResponse.StatusCode;
        }

        #endregion

        #region -------------------- From Custom API --------------------
        //----------------------------------------

        //----------------------------------------
        /// <summary>
        /// Sends a lightweight ping request to custom server.
        /// </summary>
        /// <returns>
        /// <see cref="HttpStatusCode"/> representing the server response.
        /// </returns>
        public static async Task<HttpStatusCode> StartAsync(Uri uri)
        {
            // Configure a HttpClient that accepts any certificate
            // (useful for local or self‑signed dev environments)
            using var handler = new HttpClientHandler
            {
                UseProxy = false,
                ServerCertificateCustomValidationCallback = (s, c, ch, e) => true,
                AllowAutoRedirect = true
            };

            // Build the client with a 10‑second timeout and the
            // base address taken from the global connection settings
            using var client = new HttpClient(handler)
            {
                Timeout = TimeSpan.FromSeconds(10),
                BaseAddress = uri
            };

            // Perform the GET request and return the status code
            HttpResponseMessage pingResponse = await client.GetAsync(client.BaseAddress);
            return pingResponse.StatusCode;
        }

        #endregion
    }
}
