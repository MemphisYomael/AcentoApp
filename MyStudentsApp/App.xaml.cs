using MyStudentsApp.DbContext;

namespace MyStudentsApp
{
    public partial class App : Application
    {
        public App()
        {

            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var appMainShell = new AppMainShell();
            //appMainShell.BackgroundColor = Colors.White;
            //appMainShell.FlyoutBackgroundColor = Colors.White;
            return new Window(appMainShell);
        }
    }
}
