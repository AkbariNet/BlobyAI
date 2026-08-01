using BlobyAI.Models;
using BlobyAI.Views;
using System.ComponentModel;

namespace BlobyAI.ViewModels;

/// <summary>
/// View‑model for the <see cref="ConnectionPanel"/>.  It exposes the current
/// server IP address and port to the UI via data binding.
/// The values are read directly from the static
/// <see cref="ConnectionModel"/> – the setters are intentionally
/// empty because the panel writes back to the model itself
/// (in the button click handlers).
/// </summary>
internal class ConnectionPanelVM : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Raises the <see cref="PropertyChanged"/> event.
    /// </summary>
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Two‑way proxy for the IP address.  The getter reads from
    /// <see cref="ConnectionModel.IPAddress"/>; the setter is
    /// intentionally left blank – updates are performed
    /// directly in the panel’s click handler.
    /// </summary>
    public string IPConnection
    {
        get => ConnectionModel.IPAddress;
        set { /* the view model is read‑only; updates are handled elsewhere */ }
    }

    /// <summary>
    /// Two‑way proxy for the port.  The getter reads from
    /// <see cref="ConnectionModel.Port"/>; the setter is
    /// intentionally left blank for the same reason as above.
    /// </summary>
    public string PortConnection
    {
        get => ConnectionModel.Port;
        set { /* the view model is read‑only; updates are handled elsewhere */ }
    }
}
