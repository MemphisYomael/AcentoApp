using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyStudentsApp.Services;
using MyStudentsApp.Shared.DTOShared;
using PropertyChanged;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System.Windows.Input;

namespace MyStudentsApp.MVVM.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class CrearProfesorViewModel
    {
        public ProfesorCrearDTO Profesor { get; set; } = new ProfesorCrearDTO();

        private readonly IGestionUsuarioServiceApp _gestionUsuarioService;

        public ICommand CrearProfesor { get; }

        public CrearProfesorViewModel(IGestionUsuarioServiceApp gestionUsuarioService)
        {
            _gestionUsuarioService = gestionUsuarioService;

            // Initialize the ICommand property in the constructor to avoid referencing non-static methods in field initializers
            CrearProfesor = new Command(async () =>
            {
                try
                {
                    await CrearProfesorAsync();
                }
                catch (Exception ex)
                {
                    // Manejo de excepciones, por ejemplo, mostrar un mensaje al usuario
                    Console.WriteLine($"Error al crear profesor: {ex.Message}");
                }
            });
        }

        public async Task CrearProfesorAsync()
        {
            try
            {
                if (Profesor == null)
                {
                    return;
                }
                var response = await _gestionUsuarioService.CrearProfesorAsync(Profesor);
                if (response == null)
                {
#if !WINDOWS
                    var snackbar = Snackbar.Make(
                        "Error al Crear el profesor",
                        action: async () => await App.Current.Windows[0].Page.DisplayAlert("Error", "Por favor, intentalo de nuevo, trata de llenar todos los campos, o verifica tu conexion a internet.", "Ok"),
                        actionButtonText: "Click",
                        duration: TimeSpan.FromSeconds(3),
                        visualOptions: new SnackbarOptions
                        {
                            BackgroundColor = Colors.DarkRed,
                            TextColor = Colors.White,
                            ActionButtonTextColor = Colors.Yellow,
                            CornerRadius = 10,
                        });
                    await snackbar.Show();
#else
                    await App.Current.Windows[0].Page.DisplayAlert("Error", "Por favor, intentalo de nuevo, trata de llenar todos los campos, o verifica tu conexion a internet.", "Ok");
#endif

                }
                else
                {
#if !WINDOWS
                    var snackbar = Snackbar.Make(
                        "Profesor Creado Correctamente",
                        action: async () => await App.Current.Windows[0].Page.DisplayAlert("Profesor Creado Correctamente", "Puedes irte o crear mas profesores!!!", "De Acuerdo!"),
                        actionButtonText: "Click",
                        duration: TimeSpan.FromSeconds(3),
                        visualOptions: new SnackbarOptions
                        {
                            BackgroundColor = Colors.Green,
                            TextColor = Colors.White,
                            ActionButtonTextColor = Colors.Yellow,
                            CornerRadius = 10,
                        });
                    await snackbar.Show();
#else
                    await App.Current.Windows[0].Page.DisplayAlert("Profesor Creado Correctamente", "Puedes irte o crear mas profesores!!!", "De Acuerdo!");
#endif
                }
            }
            catch (Exception ex)
            {
#if !WINDOWS
                var snackbar = Snackbar.Make(
                    "Error al Crear el estudiante",
                    action: async () => await App.Current.Windows[0].Page.DisplayAlert("Error", "Por favor, intentalo de nuevo, trata de llenar todos los campos, o verifica tu conexion a internet.", "Ok"),
                    actionButtonText: "Click",
                    duration: TimeSpan.FromSeconds(3),
                    visualOptions: new SnackbarOptions
                    {
                        BackgroundColor = Colors.DarkRed,
                        TextColor = Colors.White,
                        ActionButtonTextColor = Colors.Yellow,
                        CornerRadius = 10,
                    });

                await snackbar.Show();
#else
                await App.Current.Windows[0].Page.DisplayAlert("Estudiante Creado Correctamente", "Puedes irte o crear mas estudiantes!!!", "De Acuerdo!");
#endif
            }
        }
    }
}
