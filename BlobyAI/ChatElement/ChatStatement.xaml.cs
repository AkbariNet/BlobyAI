namespace BlobyAI.ChatElement;

public partial class ChatStatement : Border
{
    public ChatStatement()
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

        }

    }
}
