using BlobyAI.ViewModels;
using BlobyAI.Views;

namespace BlobyAI
{
    /// <summary>
    /// MainPage – UI entry point of the application.
    /// It hosts the chat UI, LLM selector, and connection panel.
    /// </summary>
    public partial class MainPage : ContentPage
    {
        

        #region --------------------------------  Fields & Static Members --------------------------------
        // -------------------------------------------------------

        // ViewModel instance for data binding
        MainPageVM MPVM = new MainPageVM();

        // Static LLM list view – used by both UI and ViewModel
        public static LLMListView LLMListViewStatic = new LLMListView();

        // Event that notifies the page when the list of LLMs changes
        public static Action LLMListChanged;

        // Flag used to remember whether the LLM panel is currently shown
        bool IsClickedSelectLLMButton;

        #endregion

        #region -------------------------------- Contstructor --------------------------------
        //----------------------------------------------------------------
        public MainPage()
        {
            InitializeComponent();



            // Wire up event that refreshes the LLM list
            LLMListChanged += MainPage_LLMListChanged;

            // Add the initial LLM list view to the placeholder
            LLMListViewPlaceholder.Children.Add(LLMListViewStatic);

            // Set the page’s BindingContext to the ViewModel
            this.BindingContext = MPVM;
        }
        #endregion

        #region -------------------------------- Event Handlers --------------------------------
        //----------------------------------------------------------------
        /// <summary>
        /// Invoked when the list of available LLMs changes.
        /// Re‑creates the placeholder and re‑opens the selector panel.
        /// </summary>
        private void MainPage_LLMListChanged()
        {
            LLMListViewPlaceholder.Children.Clear();
            LLMListViewPlaceholder.Children.Add(LLMListViewStatic);
            

            // Simulate a click to show the updated list
            SelectLLMButton_Clicked(null, null);
        }

        /// <summary>
        /// Handles the Send button click.
        /// Sends the current text and scrolls to the bottom.
        /// </summary>
        private void SendMessage_Clicked(object sender, EventArgs e)
        {
            // Fire the ViewModel command (non‑blocking)
            if (MPVM.SendMessage(this, TextValue.Text).IsCompleted)
            {
                GoToLastLineOfMainScrollViewer();
            }

            // Reset the input field
            TextValue.Text = string.Empty;
            TextValue.IsEnabled = false;
            TextValue.IsEnabled = true;
            TextValue.Unfocus();
        }

        /// <summary>
        /// Handles the LLM selector button.
        /// Toggles the LLM panel visibility with animation.
        /// </summary>
        private void SelectLLMButton_Clicked(object sender, EventArgs e)
        {
            if (IsClickedSelectLLMButton)
            {
                // Hide the panel
                HideWithFade(SelectLLMView);
                IsClickedSelectLLMButton = false;
            }
            else
            {
                // Refresh the static list view and show the panel
                MainPage.LLMListViewStatic = new LLMListView();
                LLMListViewPlaceholder.Children.Clear();
                LLMListViewPlaceholder.Children.Add(LLMListViewStatic);
                ShowWithFade(SelectLLMView);
                IsClickedSelectLLMButton = true;
            }
        }

        /// <summary>
        /// Called when the user submits the connection panel.
        /// Currently just displays a debug alert.
        /// </summary>
        private async void ConnectionPanel_IsSubmitClicked()
        {
            await Application.Current.MainPage.DisplayAlert(
                "e.Message.ToString()",
                "e.Data.ToString()",
                "ok31"
            );
        }

        /// <summary>
        /// Shows the connection panel over the main window.
        /// </summary>
        private void LinkConnectionPanelButton_Clicked(object sender, EventArgs e)
            => ShowWithFade(ConnectionPanel, MainWindow);

        /// <summary>
        /// Shows the languages panel over the main window.
        /// </summary>
        private void SelectLanguagePanelButton_Clicked(object sender, EventArgs e)
            => ShowWithFade(LanguagePanel, MainWindow);
        /// <summary>
        /// Hides the connection panel when the user finishes the process.
        /// </summary>
        private void ConnectionPanel_Done(object sender, EventArgs e)
            => HideWithFade(ConnectionPanel, MainWindow);

        /// <summary>
        /// Hides the language panel when the user finishes the process.
        /// </summary>
        private void LanguagePanel_Done(object sender, EventArgs e)
            => HideWithFade(LanguagePanel, MainWindow);
        #endregion

        #region -------------------------------- helpers --------------------------------
        //----------------------------------------------------------------
        /// <summary>
        /// Property wrapper that updates the chat message list.
        /// When a new message view is set, it is added to the layout,
        /// the scroll view is refreshed, and we jump to the bottom.
        /// </summary>
        public StackLayout MessagesViewer
        {
            get => MPVM.MessagesViewer;
            set
            {
                MPVM.MessagesViewer.Children.Add(value);
                MainScrollOfMessages.ClearLogicalChildren();
                MainScrollOfMessages.Content = MPVM.MessagesViewer;
                GoToLastLineOfMainScrollViewer();
            }
        }

        /// <summary>
        /// Scrolls the main chat ScrollView to the bottom.
        /// </summary>
        public async void GoToLastLineOfMainScrollViewer()
        {
            await MainScrollOfMessages.ScrollToAsync(
                MainScrollOfMessages.Content.Height,
                MainScrollOfMessages.Content.Height,
                true);
        }
        #endregion

        #region -------------------------------- Fade Animations --------------------------------
        //----------------------------------------------------------------

        /// <summary>
        /// Shows a view with a fade‑in (and upward) animation.
        /// </summary>
        private async Task ShowWithFade(View view)
        {
            view.TranslationY = 0; // Ensure view starts at its normal Y
            view.Opacity = 0;
            view.IsVisible = true;

            await view.FadeTo(1, 400, Easing.CubicInOut);
        }

        /// <summary>
        /// Shows a view with fade‑in while fading out its parent.
        /// The parent is temporarily disabled.
        /// </summary>
        private async Task ShowWithFade(View view, View Parent)
        {
            Parent.IsEnabled = false;

            view.TranslationY = 0;
            view.Opacity = 0;
            view.IsVisible = true;

            await Task.WhenAll(
                view.FadeTo(1, 400, Easing.CubicInOut),
                Parent.FadeTo(0.3, 400, Easing.CubicInOut));
        }

        /// <summary>
        /// Hides a view with a fade‑out animation.
        /// </summary>
        private async Task HideWithFade(View view)
        {
            await view.FadeTo(0, 400, Easing.CubicInOut);
            view.IsVisible = false;
        }

        /// <summary>
        /// Hides a view while restoring the parent’s opacity and enabled state.
        /// </summary>
        private async Task HideWithFade(View view, View Parent)
        {
            Parent.IsEnabled = true;

            await Task.WhenAll(
                view.FadeTo(0, 400, Easing.CubicInOut),
                Parent.FadeTo(1, 400, Easing.CubicInOut));

            view.IsVisible = false;
        }
        #endregion
    }
}
