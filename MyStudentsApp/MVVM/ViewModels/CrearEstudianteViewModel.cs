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
            CrearEstudianteCommand = new Command(async () => await CrearEstudiante());
        }

        public async Task CrearEstudiante()
        {
            if (string.IsNullOrWhiteSpace(EstudianteCreacionDTO.nombres))
            {
                await MostrarError("Debe indicar los nombres del estudiante.");
                return;
            }

            if (string.IsNullOrWhiteSpace(EstudianteCreacionDTO.apellidos))
            {
                await MostrarError("Debe indicar los apellidos del estudiante.");
                return;
            }
            if (string.IsNullOrWhiteSpace(EstudianteCreacionDTO.contrasena))
            {
                await MostrarError("Debe indicar una contraseña para el estudiante.");
                return;
            }
            if (string.IsNullOrWhiteSpace(EstudianteCreacionDTO.Email))
            {
                await MostrarError("Debe indicar el correo del estudiante.");
                return;
            }
            if (string.IsNullOrWhiteSpace(EstudianteCreacionDTO.PhoneNumber))
            {
                await MostrarError("Debe indicar el teléfono del estudiante.");
                return;
            }

            try
            {
                var estudianteCreado = await _GestionUsuarioService.CrearEstudianteAsync(EstudianteCreacionDTO);
                if (estudianteCreado == null)
                {
                    await MostrarError("No se logró crear el estudiante. Verifica los datos, la conexión y que el usuario tenga una escuela asignada.");
                    return;
                }

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
                EstudianteCreacionDTO = new EstudianteCreacionDTO();
            }
            catch (Exception ex)
            {
                await MostrarError($"Error al crear estudiante: {ex.Message}");
            }
        }

        private static async Task MostrarError(string mensaje)
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await App.Current.MainPage.DisplayAlert("Error", mensaje, "Ok");
            });
        }
    }
}
