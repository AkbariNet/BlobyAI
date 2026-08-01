using BlobyAI.Models;
using System.ComponentModel;

namespace BlobyAI.Views;

/// <summary>
/// <see cref="LLMElement"/> is a reusable <see cref="Button"/> that
/// represents a single Large Language Model (LLM) entry in the UI.
/// The button displays the model name and an icon (defined in XAML).
/// When clicked, it stores the selected model name in the global
/// <see cref="ConnectionModel"/> and notifies the main page to
/// refresh its list.
/// </summary>
public partial class LLMElement : Button
{
    #region -------------------------------- Constructor & LLMModel --------------------------------
    //----------------------------------------------------------------

    /// <summary>
    /// Holds the data for this element.  The Button’s BindingContext
    /// is set to this instance so that XAML bindings (e.g. LLMName)
    /// resolve correctly.
    /// </summary>
    private LLMElementModel LLMModel = new LLMElementModel();

    public LLMElement()
    {
        InitializeComponent();
        this.BindingContext = LLMModel;
    }
    #endregion

    #region -------------------------------- Public Properties --------------------------------
    //----------------------------------------------------------------

    /// <summary>
    /// Exposes the underlying <see cref="LLMElementModel"/> instance.
    /// This can be used by callers that need to set the entire model.
    /// </summary>
    public LLMElementModel Model
    {
        get => LLMModel;
        set => LLMModel = value;
    }

    /// <summary>
    /// Two‑way proxy for the LLM’s display name (used by XAML).
    /// </summary>
    public string LLMName
    {
        get => LLMModel.LLMName;
        set => LLMModel.LLMName = value;
    }

    /// <summary>
    /// Two‑way proxy for the actual (real) LLM name used in the
    /// backend connection logic.
    /// </summary>
    public string RealLLMName
    {
        get => LLMModel.RealLLMName;
        set => LLMModel.RealLLMName = value;
    }

    #endregion

    #region -------------------------------- Event and methods --------------------------------
    //----------------------------------------------------------------
    /// <summary>
    /// Handles the button click event.  The chosen LLM name is persisted
    /// in <see cref="ConnectionModel"/> and the main page is notified
    /// to refresh its list.
    /// </summary>
    private void LLMElement_Clicked(object sender, EventArgs e)
    {
        ConnectionModel.Model = RealLLMName;

        // Re‑create the static LLM list view (the view will be refreshed)
        // MainPage.LLMListViewStatic = new LLMListView(); // commented out – kept for reference

        // Trigger the event that causes the UI to refresh
        MainPage.LLMListChanged?.Invoke();
    }
    #endregion
}
#region -------------------------------- Model and Event Properties  --------------------------------
//----------------------------------------------------------------

/// <summary>
/// View‑model backing <see cref="LLMElement"/>.  Implements
/// <see cref="INotifyPropertyChanged"/> so that UI updates
/// automatically when properties change.
/// </summary>
public class LLMElementModel : INotifyPropertyChanged
{
    private string _lLMName = string.Empty;
    private string _realLLMName = string.Empty;

    /// <summary>
    /// Display name shown in the UI.
    /// </summary>
    public string LLMName
    {
        get => _lLMName;
        set
        {
            if (_lLMName != value)
            {
                _lLMName = value;
                OnPropertyChanged(nameof(LLMName));
            }
        }
    }

    /// <summary>
    /// The actual model identifier used by the backend.
    /// </summary>
    public string RealLLMName
    {
        get => _realLLMName;
        set
        {
            if (_realLLMName != value)
            {
                _realLLMName = value;
                OnPropertyChanged(nameof(RealLLMName));
            }
        }
    }

    /// <inheritdoc/>
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Raises the <see cref="PropertyChanged"/> event.
    /// </summary>
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    #endregion
}
