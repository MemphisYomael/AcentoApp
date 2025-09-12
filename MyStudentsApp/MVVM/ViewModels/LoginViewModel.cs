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
using MyStudentsApp.Shared.DTOShared;
using PropertyChanged;

namespace MyStudentsApp.MVVM.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class LoginViewModel
    {
        private readonly IGestionUsuarioServiceApp _gestionUsuariosService;
        public LoginRequestDTO GuardarSesionModel { get; set; } = new();

        public ICommand loginCommand { get; set; }

        public LoginViewModel(IGestionUsuarioServiceApp gestion)    
        {
            loginCommand = new Command(async () =>
            {
                await Login();
            });
            _gestionUsuariosService = gestion;


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
await Application.Current.MainPage.DisplayAlert("Estatus", "Por favor, refiva tus credenciales!", "OK");

#endif

            }
        }
    }
}
