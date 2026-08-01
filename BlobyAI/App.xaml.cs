using System.Globalization;

namespace BlobyAI
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            SetCulture("en-US");
            //MainPage = new Shell();
            OnLanguageChanged += App_OnLanguageChanged;
        }

        private async void App_OnLanguageChanged(object? sender, EventArgs e)
        {
            await Task.Delay(0);

        }

        public static event EventHandler OnLanguageChanged;




        public static void SetCulture(string cultureCode)
        {
            var ci = new CultureInfo(cultureCode);
            CultureInfo.CurrentCulture = ci;
            CultureInfo.CurrentUICulture = ci;
        }
        public static void ChangeLanguageTo(string languageCode)
        {
            SetCulture(languageCode);
            OnLanguageChanged.Invoke(null, new EventArgs());



        }
        public static Window Main = new Window(new AppShell());
        protected override Window CreateWindow(IActivationState? activationState)
        {
            return Main;
        }
    }
}