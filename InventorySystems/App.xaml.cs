using Microsoft.Maui.Controls;

namespace InventorySystems
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        // Override CreateWindow to set the root page
        protected override Window CreateWindow(IActivationState activationState)
        {
            // Return a new Window with a NavigationPage wrapping the LoginPage
            return new Window(new NavigationPage(new LoginPage()));
        }
    }
}
