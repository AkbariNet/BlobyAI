using BlobyAI.Methods;

namespace BlobyAI.Views;

/// <summary>
/// LLMListView – a lightweight wrapper around a <see cref="Grid"/> that
/// displays a list of available Large Language Models (LLMs).  
/// The actual UI is defined in LLMListView.xaml; this code‑behind
/// simply injects the generated LLM entry views into the
/// <c>LayoutOfLLMElement</c> <see cref="StackLayout"/> defined in XAML.
/// </summary>
public partial class LLMListView : Grid
{
    public LLMListView()
    {
        InitializeComponent();

        // ImportLLMS.ReturnLLMLayouts() returns a Layout (e.g. StackLayout)
        // containing all <LLMElement> buttons.  We add that layout to
        // the XAML‑defined container so the list is rendered at runtime.
        LayoutOfLLMElement.Children.Add(ImportLLMS.ReturnLLMLayouts());
    }
}
