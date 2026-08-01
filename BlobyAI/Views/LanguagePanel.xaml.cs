using BlobyAI.Properties;

namespace BlobyAI.Views;

/// <summary>
/// LanguagePanel – modal UI for select languages.
/// The panel is a <see cref="Border"/> that contains text fields for languages.
/// It exposes
/// a simple event (Done) that callers can subscribe to in order to
/// react when the panel is closed.
/// </summary>
public partial class LanguagePanel : Border
{
    public LanguagePanel()
    {
        InitializeComponent();


    }


    #region Event Handling

    /// <summary>
    /// Raised when the user finishes interacting with the panel
    /// (either Submit or Cancel).  Callers typically hide the
    /// panel in response to this event.
    /// </summary>
    public event EventHandler Done;


    private void English_Clicked(object sender, EventArgs e)
    {

        MainLanguage.TheLanguage = MainLanguage.Language.EN;
        Done?.Invoke(this, EventArgs.Empty);
    }

    private void Persian_Clicked(object sender, EventArgs e)
    {

        MainLanguage.TheLanguage = MainLanguage.Language.FA;
        Done?.Invoke(this, EventArgs.Empty);
    }
    #endregion

}
