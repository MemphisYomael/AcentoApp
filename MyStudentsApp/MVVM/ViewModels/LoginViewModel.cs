using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Maui.Alerts;
using Microsoft.Maui.Storage;
using MyStudentsApp.DbContext;
using MyStudentsApp.Services;
using MyStudentsApp.Services.Notifications;
using MyStudentsApp.Shared.DTOShared;
using PropertyChanged;

namespace MyStudentsApp.MVVM.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class LoginViewModel
    {
        private readonly IGestionUsuarioServiceApp _gestionUsuariosService;
        private readonly AuthorizationService _authService;
        private readonly IOneSignalMauiService _oneSignalMauiService;
        private readonly INotificationDeviceService _notificationDeviceService;

        public LoginRequestDTO GuardarSesionModel { get; set; } = new();

        public ICommand loginCommand { get; set; }

        public LoginViewModel(
            IGestionUsuarioServiceApp gestion,
            AuthorizationService authService,
            IOneSignalMauiService oneSignalMauiService,
            INotificationDeviceService notificationDeviceService)
        {
            _gestionUsuariosService = gestion;
            _authService = authService;
            _oneSignalMauiService = oneSignalMauiService;
            _notificationDeviceService = notificationDeviceService;

            loginCommand = new Command(async () =>
            {
                await Login();
            });

            PreferencesInitialize();
        }

        public void PreferencesInitialize()
        {
            try
            {
                var pass = Preferences.Get("password", "");
                var email = Preferences.Get("email", "");

                if (GuardarSesionModel == null)
                    Debug.WriteLine("GuardarSesionModel es NULL");

                GuardarSesionModel.Password = pass;
                GuardarSesionModel.Email = email;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR Preferences: " + ex.Message);
                Debug.WriteLine("ERROR Preferences: " + ex.ToString());
            }
        }



        public async Task Login()
        {

            if (await _gestionUsuariosService.Login(GuardarSesionModel))
            {
                // *** NUEVO: Actualizar usuario actual en el servicio de autorización ***
                await _authService.UpdateCurrentUser(_gestionUsuariosService);

                var usuarioId = _authService.UsuarioActual?.Id ?? Preferences.Get("userId", string.Empty);
                if (!string.IsNullOrWhiteSpace(usuarioId))
                {
                    await _oneSignalMauiService.LoginAsync(usuarioId);
                    await _oneSignalMauiService.RequestPermissionAsync();
                    await _notificationDeviceService.RegistrarDispositivoAsync(usuarioId);
                }

                // *** NUEVO: Navegar al Dashboard después del login exitoso ***
                //await Shell.Current.GoToAsync("//DashboardFlyoutItem");
#if !WINDOWS
                var snackbar = Snackbar.Make("Iniciaste Sesion Con Exito!!", actionButtonText: "OK");
                snackbar.VisualOptions.TextColor = Colors.White;
                snackbar.VisualOptions.ActionButtonTextColor = Colors.White;
                snackbar.VisualOptions.BackgroundColor = Colors.Green;
                await snackbar.Show();
#else
                await Application.Current.MainPage.DisplayAlert("Estatus", "Iniciaste Sesion Con Exito!!", "OK");
#endif
            }
            else
            {
#if !WINDOWS

                var snackbar = Snackbar.Make("Por favor verifica tus credenciales", actionButtonText: "OK");
                snackbar.VisualOptions.TextColor = Colors.White;
                snackbar.VisualOptions.ActionButtonTextColor = Colors.White;
                snackbar.VisualOptions.BackgroundColor = Colors.Red;
                await snackbar.Show();
#else
                await Application.Current.MainPage.DisplayAlert("Estatus", "Por favor, verifica tus credenciales!", "OK");
#endif

            }
        }
    }
}
