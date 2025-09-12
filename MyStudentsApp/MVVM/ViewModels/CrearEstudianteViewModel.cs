using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using MyStudentsApp.Services;
using MyStudentsApp.Shared.DTOShared;
using PropertyChanged;

namespace MyStudentsApp.MVVM.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class CrearEstudianteViewModel
    {
        public EstudianteCreacionDTO EstudianteCreacionDTO { get; set; }
        public ICommand CrearEstudianteCommand { get; set; }
        public IGestionUsuarioServiceApp _GestionUsuarioService { get; set; }

        public CrearEstudianteViewModel(IGestionUsuarioServiceApp gestionUsuarioService)
        {
            _GestionUsuarioService = gestionUsuarioService;
            EstudianteCreacionDTO = new EstudianteCreacionDTO();
            CrearEstudianteCommand = new Command(() =>
            {
                try
                {
                    CrearEstudiante();
                }
                catch (Exception ex)
                {
                    // Manejo de excepciones, por ejemplo, mostrar un mensaje al usuario
                    Console.WriteLine($"Error al crear estudiante: {ex.Message}");
                }
            });
        }

        public void CrearEstudiante()
        {
            if (EstudianteCreacionDTO.nombres == null)
            {
                throw new ArgumentNullException(nameof(EstudianteCreacionDTO), "El DTO de creación de estudiante no puede ser nulo.");
            }

            if (EstudianteCreacionDTO.apellidos == null)
            {
                throw new ArgumentNullException(nameof(EstudianteCreacionDTO), "El DTO de creación de estudiante no puede ser nulo.");
            }
            if (EstudianteCreacionDTO.contrasena == null)
            {
                throw new ArgumentNullException(nameof(EstudianteCreacionDTO), "El DTO de creación de estudiante no puede ser nulo.");
            }
            if (EstudianteCreacionDTO.Email == null)
            {
                throw new ArgumentNullException(nameof(EstudianteCreacionDTO), "El DTO de creación de estudiante no puede ser nulo.");
            }
            if(EstudianteCreacionDTO.PhoneNumber == null)
            {
                throw new ArgumentNullException(nameof(EstudianteCreacionDTO), "El DTO de creación de estudiante no puede ser nulo.");
            }

            _GestionUsuarioService.CrearEstudianteAsync(EstudianteCreacionDTO)
                .ContinueWith(async (task) =>
                {
                    if (task.Result == null)
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
await MainThread.InvokeOnMainThreadAsync(async () =>
{
                        await App.Current.MainPage.DisplayAlert("Estudiante Creado Correctamente", "Puedes irte o crear mas estudiantes!!!", "De Acuerdo!");
                        });
#endif

                        // Lógica adicional después de crear el estudiante, como navegar a otra vista o mostrar un mensaje de éxito
                    }
                    else
                    {
#if !WINDOWS
                        var snackbar = Snackbar.Make(
                        "Estudiante Creado Correctamente",
                        action: async () => await Application.Current.Windows[0].Page.DisplayAlert("Elige que quieres hacer ahora", "Puedes irte o crear mas estudiantes!!!", "De Acuerdo!"),
                        actionButtonText: "Click",
                        duration: TimeSpan.FromSeconds(3),
                        visualOptions: new SnackbarOptions
                        {
                            BackgroundColor = Colors.DarkSlateBlue,
                            TextColor = Colors.White,
                            ActionButtonTextColor = Colors.Yellow,
                            CornerRadius = 10,
                        });

                        await snackbar.Show();
#else
await MainThread.InvokeOnMainThreadAsync(async () =>
{
                            await App.Current.MainPage.DisplayAlert("Estudiante Creado Correctamente", "Puedes irte o crear mas estudiantes!!!", "De Acuerdo!");
                            });
#endif
                        // Lógica adicional después de crear el estudiante, como navegar a otra vista o mostrar un mensaje de éxito
                    }
                });
        }
    }
}
