using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;

namespace Acento
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Verifica que la propiedad Window no sea nula antes de usarla
            if (Window != null)
            {
                // Establece el color de la barra de estado utilizando una API compatible
                if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop && Build.VERSION.SdkInt < BuildVersionCodes.Tiramisu)
                {
                    Window.SetStatusBarColor(Android.Graphics.Color.ParseColor("#ffffff")); // Cambia a tu color deseado
                }
                else if (Build.VERSION.SdkInt >= BuildVersionCodes.Tiramisu)
                {
                    Window.SetDecorFitsSystemWindows(false);
                    Window.InsetsController?.SetSystemBarsAppearance(0, 0); // Configuración alternativa para versiones más recientes

                    // Nueva forma de establecer el color de la barra de estado en Android 35.0 y versiones posteriores
                    Window.InsetsController?.SetSystemBarsAppearance(
                        (int)WindowInsetsControllerAppearance.LightStatusBars,
                        (int)WindowInsetsControllerAppearance.LightStatusBars
                    );
                }
            }
        }
    }


}
