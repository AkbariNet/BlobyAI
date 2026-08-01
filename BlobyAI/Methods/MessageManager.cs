// ---------------------------------------------------------------------------
//  BlobyAI/Methods/MessageManager.cs
// ---------------------------------------------------------------------------

using BlobyAI.ChatElement;
using BlobyAI.Models;
using System.Net;
using System.Net.Http.Json;

namespace BlobyAI.Methods
{
    /// <summary>
    ///  Handles the entire chat session: building the request payload,
    ///  sending it, parsing the reply, and persisting the conversation.
    /// </summary>
    internal static class MessageManager
    {
        #region -------------  Data‑transfer objects -------------
        // -------------------------------------------------------
        public record ChatMessage(string Role, string Content);

        public record ChatRequest(
            string Model,
            List<ChatMessage> Messages,
            bool Stream = false);

        public record ChatResponse(
            string Model,
            DateTime CreatedAt,
            ChatMessage Message,
            bool Done,
            string DoneReason,
            long TotalDuration,
            long LoadDuration,
            long PromptEvalCount,
            long PromptEvalDuration,
            long EvalCount,
            long EvalDuration
        );

        //  The JSON that we receive back from the server.
        //  We only deserialize the “message” part – everything else is
        //  ignored by the UI.
        public record InternalChatResponse(
            string Model,
            DateTime CreatedAt,
            MessageContent Message,
            bool Done,
            string DoneReason,
            long TotalDuration,
            long LoadDuration,
            long PromptEvalCount,
            long PromptEvalDuration,
            long EvalCount,
            long EvalDuration
        );

        public record MessageContent(string Role, string Content);
        #endregion

        #region -------------  Conversation history -------------
        // A *static* list that lives for the lifetime of the app.
        //  •  The UI updates automatically because we expose it via
        //  •  BlobyAI.ChatElement objects.
        private static readonly List<ChatElement> _conversation = new();

        public static IReadOnlyList<ChatElement> Conversation => _conversation.AsReadOnly();
        #endregion

        #region -------------  Public entry‑points -------------
        /// <summary>
        ///  Public helper that you call from the UI.
        ///  The text you typed will be shown immediately as a “Sent” bubble,
        ///  then we fetch the answer from Ollama.
        /// </summary>
        public static async Task<bool> SendMessageAsync(string userInput, MainPage parent)
        {
            // 1️⃣  Show the user’s message in the UI.
            var sentBubble = new Sent { ContextOfText = userInput };
            var sentLayout = new StackLayout { Children = { sentBubble } };
            parent.MessagesViewer = sentLayout;

            // 2️⃣  Persist the user message in our in‑memory history.
            _conversation.Add(new ChatElement { Role = "user", Content = userInput });

            // 3️⃣  Ask the server for an answer.
            return await ProcessAsync(userInput, parent);
        }

        /// <summary>
        ///  {DESABLED} Loads an existing chat (if any) when the page appears.
        ///  Useful if you want to keep the conversation across navigation.
        /// </summary>
        public static void LoadExistingConversation(MainPage parent)
        {
            /*  var layout = new StackLayout();
              foreach (var item in _conversation)
                  layout.Children.Add(new ChatBubble(item));
              parent.MessagesViewer = layout;*/
        }
        #endregion

        #region -------------  Core implementation -------------
        private static async Task<bool> ProcessAsync(string userInput, MainPage parent)
        {
            //     Build  the chat statement UI
            ChatStatement chatStatement = new ChatStatement();
            parent.MessagesViewer = new StackLayout { Children = { chatStatement } };

            //     Fill the value of that for "Please Wait"
            chatStatement.ContextOfText = BlobyAI.Resources.Languages.Languages.Status__Connecting___;



            // 1️⃣  Build the request payload that includes the *entire*
            //     conversation so far plus the new user message.
            var chatRequest = new ChatRequest(
                Model: ConnectionModel.Model,
                Messages: _conversation
                    .ConvertAll(e => new ChatMessage(e.Role, e.Content))
                    .Concat(new[] { new ChatMessage("user", userInput) })
                    .ToList(),
                Stream: false);

            // 2️⃣  Create a single HttpClient for the request.
            using var handler = new HttpClientHandler
            {
                UseProxy = false,
                ServerCertificateCustomValidationCallback = (s, c, ch, e) => true,
                AllowAutoRedirect = true
            };

            using var client = new HttpClient(handler)
            {
                Timeout = TimeSpan.FromMinutes(60),
                BaseAddress = new Uri($"http://{ConnectionModel.IPAddress}:{ConnectionModel.Port}/api/")
            };


            chatStatement.ContextOfText = BlobyAI.Resources.Languages.Languages.Status__Checking_connection_status__;
            // 2️⃣.1 Ping From Server.

            try
            {
                HttpStatusCode pingStatusCode = await PingFromServer.StartAsync();
                // if server status is success we can continue
                if (pingStatusCode == System.Net.HttpStatusCode.OK)
                {
                    chatStatement.ContextOfText = BlobyAI.Resources.Languages.Languages.Connection_successful_;


                    // 3️⃣  Send the request.
                    HttpResponseMessage response;
                    try
                    {
                        //     Fill the value of chatStatement for "Trying to send message..."
                        chatStatement.ContextOfText = BlobyAI.Resources.Languages.Languages.Status__Message_being_processed_by_AI___;

                        response = await client.PostAsJsonAsync("chat", chatRequest);
                    }
                    catch (Exception ex)
                    {
                        //     Fill the value of that for "Could not connect to the Ollama server."
                        chatStatement.ContextOfText = BlobyAI.Resources.Languages.Languages.Connection_error__Connection_to_server_Ollama_could_not_be_established_ + $"\n {BlobyAI.Resources.Languages.Languages.Error_message_} " + ex.Message;
                        return false;
                    }

                    // 4️⃣  Handle the response.
                    if (!response.IsSuccessStatusCode)
                    {
                        //     Fill the value of that for "Server responded with {response.StatusCode}.\nPlease check your IP / port."
                        chatStatement.ContextOfText = $"{BlobyAI.Resources.Languages.Languages.Connection_error__Received_server_response_with_code} {response.StatusCode}. {BlobyAI.Resources.Languages.Languages.Please_double_check_the_entered_IP_address_and_port_}";

                        return false;
                    }

                    // 5️⃣  Parse the reply.
                    try
                    {
                        //     Fill the value of chatStatement for "Trying to get respound..."
                        chatStatement.ContextOfText = BlobyAI.Resources.Languages.Languages.Status__Processing_and_receiving_the_message___;

                        var raw = await response.Content.ReadFromJsonAsync<InternalChatResponse>();
                        if (raw == null)
                        {
                            //     Fill the value of that for "Empty reply from the server."
                            chatStatement.ContextOfText = BlobyAI.Resources.Languages.Languages.No_response_received_from_the_server_;


                            return false;
                        }

                        var assistantMessage = raw.Message.Content;
                        // Persist the assistant answer in history.
                        _conversation.Add(new ChatElement { Role = "assistant", Content = assistantMessage });

                        // 6️⃣  Show the answer in the UI.
                        var recivedBubble = new Recived { ContextOfText = assistantMessage };
                        var recivedLayout = new StackLayout { Children = { recivedBubble } };
                        parent.MessagesViewer = recivedLayout;

                        chatStatement.ContextOfText = "";
                        chatStatement.IsVisible = false;
                        return true;
                    }
                    catch (Exception ex)
                    {
                        //     Fill the value of that for "Empty reply from the server."
                        chatStatement.ContextOfText = BlobyAI.Resources.Languages.Languages.Problem_processing_server_response_ + $"\n {BlobyAI.Resources.Languages.Languages.Error_message_}" + ex.Message;

                        return false;
                    }
                }
                else
                {
                    chatStatement.ContextOfText = BlobyAI.Resources.Languages.Languages.Error_connecting_to_the_server__HTTP_status_code_ + pingStatusCode;
                    return false;

                }
            }
            catch (Exception ex)
            {
                chatStatement.ContextOfText = BlobyAI.Resources.Languages.Languages.No_response_received_from_the_server_ + $"{BlobyAI.Resources.Languages.Languages.Error_message_} {ex.Message}";
                return false;

                throw;
            }
        }
        /*
                private static Task ShowErrorAsync(MainPage parent, string msg)
                    => parent.DisplayAlert("Error", msg, "OK");*/
        #endregion
    }

    /// <summary>
    /// One message in a conversation.
    /// </summary>
    public class ChatElement
    {
        /// <summary>“user”, “assistant”, or any other role you may define.</summary>
        public string Role { get; set; } = string.Empty;

        /// <summary>The plain text that was sent or received.</summary>
        public string Content { get; set; } = string.Empty;

        /// <summary>Convenience: is this a user‑sent message?</summary>
        public bool IsUser => string.Equals(Role, "user", StringComparison.OrdinalIgnoreCase);

        /// <summary>Convenience: is this an assistant reply?</summary>
        public bool IsAssistant => string.Equals(Role, "assistant", StringComparison.OrdinalIgnoreCase);
    }
}


