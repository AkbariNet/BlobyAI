using BlobyAI.Models;
using BlobyAI.ViewModels;

namespace BlobyAI.Views;

/// <summary>
/// ConnectionPanel – modal UI for configuring the backend server address.
/// The panel is a <see cref="Border"/> that contains text fields for IP
/// address and port and two buttons (Submit / Cancel).  It exposes
/// a simple event (Done) that callers can subscribe to in order to
/// react when the panel is closed.
/// </summary>
public partial class ConnectionPanel : Border
{
    /// <summary>
    /// ViewModel instance that holds the current IP/Port values.
    /// </summary>
    private  ConnectionPanelVM VM = new ConnectionPanelVM();

    public ConnectionPanel()
    {
        InitializeComponent();

        // The Border's BindingContext is set to the ViewModel so that
        // bindings in XAML can resolve to VM properties.
        this.BindingContext = VM;
    }

    #region Public Properties (Proxy to ViewModel)

    /// <summary>
    /// Two‑way proxy to the ViewModel's <c>IPConnection</c>.
    /// </summary>
    public string IPConnection
    {
        get => VM.IPConnection;
        set => VM.IPConnection = value;
    }

    /// <summary>
    /// Two‑way proxy to the ViewModel's <c>PortConnection</c>.
    /// </summary>
    public string PortConnection
    {
        get => VM.PortConnection;
        set => VM.PortConnection = value;
    }

    #endregion

    #region Event Handling

    /// <summary>
    /// Raised when the user finishes interacting with the panel
    /// (either Submit or Cancel).  Callers typically hide the
    /// panel in response to this event.
    /// </summary>
    public event EventHandler Done;

    /// <summary>
    /// Handles the Submit button click.  Stores the user input into
    /// the static <see cref="ConnectionModel"/> and raises <c>Done</c>.
    /// </summary>
    private void SubmitConnectionPanel_Clicked(object sender, EventArgs e)
    {
        // Persist the values to the global connection model
        ConnectionModel.IPAddress = IPAdressValue.Text;
        ConnectionModel.Port = PortValue.Text;

        // Temporarily disable the inputs to force a UI refresh,
        // then re‑enable them.
        IPAdressValue.IsEnabled = false;
        PortValue.IsEnabled = false;
        IPAdressValue.IsEnabled = true;
        PortValue.IsEnabled = true;

        Done?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Handles the Cancel button click.  Restores the text boxes to the
    /// values currently stored in <see cref="ConnectionModel"/> and
    /// raises <c>Done</c>.
    /// </summary>
    private void CancelConnectionPanel_Clicked(object sender, EventArgs e)
    {
        // Reset the text boxes to the last saved values
        IPAdressValue.Text = ConnectionModel.IPAddress;
        PortValue.Text = ConnectionModel.Port;

        // Force a UI refresh by disabling/re‑enabling the fields
        IPAdressValue.IsEnabled = false;
        PortValue.IsEnabled = false;
        IPAdressValue.IsEnabled = true;
        PortValue.IsEnabled = true;

        Done?.Invoke(this, EventArgs.Empty);
    }

    #endregion

    // NOTE: The following delegate is no longer used but left for
    // backward‑compatibility.  It can be removed once no callers rely on it.
    // public Action IsSubmitClicked { get; set; } = delegate { };
}
