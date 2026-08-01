
using System.ComponentModel;

namespace BlobyAI.ChatElement;

public partial class Recived : Border
{
    public Recived()
    {
        InitializeComponent();
        this.BindingContext = MVM;
    }

    MessagesVM MVM = new MessagesVM();
    public string ContextOfText
    {

        get
        {
            return MVM.ContextOfText;

        }
        set
        {
            MVM.ContextOfText = value;
            OnPropertyChanged(ContextOfText);
        }

    }
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}