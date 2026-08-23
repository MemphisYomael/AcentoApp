using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MyStudentsApp.Services;
using MyStudentsApp.Shared.DTOShared;
using PropertyChanged;

namespace MyStudentsApp.MVVM.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class ListadoDeProfesoresViewModel
    {
        public string titulo { get; set; }
        public List<ProfesorResponseDTO> Profesores { get; set; } = new List<ProfesorResponseDTO>();
        public List<ProfesorResponseDTO> ProfesoresFiltrados { get; set; } = new List<ProfesorResponseDTO>();
        private readonly IGestionUsuarioServiceApp _gestionUsuarioService;
        public ICommand buscar { get; set; }

        public ListadoDeProfesoresViewModel(IGestionUsuarioServiceApp gestionUsuarioService)
        {
            _gestionUsuarioService = gestionUsuarioService;
        }

        public async Task ObtenerProfesores()
        {
            var profesores = await _gestionUsuarioService.ObtenerProfesoresDeEstudianteAsync();
            Profesores = profesores ?? new List<ProfesorResponseDTO>();
            ProfesoresFiltrados = Profesores;
        }
    }
}
