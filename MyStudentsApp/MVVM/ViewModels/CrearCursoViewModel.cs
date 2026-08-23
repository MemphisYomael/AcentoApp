using System.Windows.Input;
using MyStudentsApp.Services;
using MyStudentsApp.Shared.DTOShared;
using PropertyChanged;

namespace MyStudentsApp.MVVM.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class CrearCursoViewModel
    {
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        
        private readonly IGestionUsuarioServiceApp _service;
        
        public ICommand GuardarCommand { get; set; }
        
        public CrearCursoViewModel(IGestionUsuarioServiceApp service)
        {
            _service = service;
            GuardarCommand = new Command(async () => await Guardar());
        }
        
        private async Task Guardar()
        {
            if (string.IsNullOrWhiteSpace(Nombre))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "El nombre es obligatorio", "OK");
                return;
            }
            
            try
            {
                // El API obtiene el schoolId automáticamente del usuario logueado
                var cursoDto = new CursoRequestDTO
                {
                    nombre = Nombre,
                    Description = Descripcion,
                    schoolId = 1 // El API lo reemplazará con el schoolId correcto
                };
                
                var resultado = await _service.CrearCursoAsync(cursoDto);
                
                if (resultado != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Éxito", "Curso creado correctamente", "OK");
                    Nombre = string.Empty;
                    Descripcion = string.Empty;
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "No se pudo crear el curso", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Error: {ex.Message}", "OK");
            }
        }
    }
}
