using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;

namespace MyStudentsApp
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                // Cambiar el color de fondo de la barra de estado (status bar)  
                Window.SetStatusBarColor(Android.Graphics.Color.White);

                // Opcional: iconos oscuros para fondo claro  
                if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
                {
                    Window.DecorView.SystemUiFlags = SystemUiFlags.LightStatusBar;
                }
            }
        }
    }



}
