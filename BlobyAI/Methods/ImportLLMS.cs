using BlobyAI.Models;
using BlobyAI.Views;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BlobyAI.Methods
{
    /// <summary>
    /// ImportLLMS – helper that fetches the list of available LLMs
    /// from the backend API and builds a <see cref="StackLayout"/> of
    /// <see cref="LLMElement"/> controls that can be dropped into the UI.
    /// </summary>
    internal static class ImportLLMS
    {
        #region -------------------------------- Public Methods -------------------------------- 
        // -------------------------------- 

        /// <summary>
        /// Creates a <see cref="StackLayout"/> containing all
        /// <see cref="LLMElement"/> instances that represent the
        /// LLMs returned by the backend.
        /// </summary>
        public static StackLayout ReturnLLMLayouts()
        {
            var layout = new StackLayout
            {
                Spacing = 20
            };

            // List to hold the individual LLMElement objects
            var lLMElement = new List<LLMElement>();

            // Configure a HttpClient that accepts any certificate
            // (useful when the backend is running on a local dev box)
            var handler = new HttpClientHandler
            {
                UseProxy = false,
                ServerCertificateCustomValidationCallback = (sender, certificate, chain, error) => true,
                AllowAutoRedirect = true
            };
            var client = new HttpClient(handler)
            {
                Timeout = TimeSpan.FromSeconds(10)
            };

            // Build the base address from the global connection settings
            try
            {
                client.BaseAddress = new Uri(
                    $"http://{ConnectionModel.IPAddress}:{ConnectionModel.Port}/api/");
            }
            catch (Exception e)
            {
                // If the address is malformed we show an alert
                Application.Current.MainPage.DisplayAlert(
                    "API Address Error",
                    e.Data.ToString(),
                    "ok3");
                throw;
            }

            // Fetch the API data synchronously (blocking) – this method
            // is called from the constructor so we can block safely.
            ReciveAPIsAsync(client, layout, lLMElement);

            return layout;
        }

        /// <summary>
        /// Calls the `/tags` endpoint, deserialises the JSON response,
        /// creates an <see cref="LLMElement"/> for each model and adds it
        /// to the supplied <paramref name="placeholder"/> layout.
        /// </summary>
        public static async Task<bool> ReciveAPIsAsync(
            HttpClient client,
            StackLayout placeholder,
            List<LLMElement> lLMElement)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("tags");

                if (response.IsSuccessStatusCode)
                {
                    string jsonString = await response.Content.ReadAsStringAsync();
                    ModelsResponse dataRecived = JsonSerializer.Deserialize<ModelsResponse>(jsonString);

                    if (dataRecived?.models != null)
                    {
                        foreach (Model model in dataRecived.models)
                        {
                            var lLMElement1 = new LLMElement
                            {
                                LLMName = model.name,
                                RealLLMName = model.model
                            };

                            // Highlight the currently selected model
                            if (lLMElement1.RealLLMName == ConnectionModel.Model)
                            {
                                lLMElement1.Background = lLMElement1.BorderColor;
                                lLMElement1.TextColor = Colors.White;
                                lLMElement1.ImageSource = new FontImageSource
                                {
                                    Color = Colors.White,
                                    Glyph = FontAwsomeIconLoader.Bahai,
                                    FontFamily = "7Awesome",
                                    Size = 14
                                };
                            }

                            placeholder.Children.Add(lLMElement1);
                        }
                    }

                    return true;
                }

                Console.WriteLine(
                    $"API request failed with status code: {response.StatusCode}");
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error making API request: {e.Message}");
                return false;
            }
        }

        #endregion
    }

    #region -------------------------------- Data contracts for deserialising the `/tags` response --------------------------------

    public class ModelDetails
    {
        [JsonPropertyName("parent_model")]
        public string parent_model { get; set; }

        [JsonPropertyName("format")]
        public string format { get; set; }

        [JsonPropertyName("family")]
        public string family { get; set; }

        [JsonPropertyName("families")]
        public List<string> families { get; set; }

        [JsonPropertyName("parameter_size")]
        public string parameter_size { get; set; }

        [JsonPropertyName("quantization_level")]
        public string quantization_level { get; set; }
    }

    public class Model
    {
        [JsonPropertyName("name")]
        public string name { get; set; }

        [JsonPropertyName("model")]
        public string model { get; set; }

        [JsonPropertyName("modified_at")]
        public DateTime modified_at { get; set; }

        [JsonPropertyName("size")]
        public long size { get; set; }

        [JsonPropertyName("digest")]
        public string digest { get; set; }

        [JsonPropertyName("details")]
        public ModelDetails details { get; set; }
    }

    public class ModelsResponse
    {
        [JsonPropertyName("models")]
        public List<Model> models { get; set; }
    }

    #endregion
}
