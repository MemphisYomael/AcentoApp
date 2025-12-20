using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MyStudentsApp.Services;
using MyStudentsApp.Shared.DTOShared;
using PropertyChanged;

namespace MyStudentsApp.MVVM.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class ListadoDeEstudiantesViewModel
    {
        public string titulo { get; set; }
        public List<EstudianteResponseDTO> Estudiantes { get; set; } = new List<EstudianteResponseDTO>();
        public List<EstudianteResponseDTO> EstudiantesFiltrados { get; set; } = new List<EstudianteResponseDTO>();
        private readonly IGestionUsuarioServiceApp _gestionUsuarioService;
        public ICommand buscar { get; set; }

        public ListadoDeEstudiantesViewModel(IGestionUsuarioServiceApp gestionUsuarioService)
        {
            
            _gestionUsuarioService = gestionUsuarioService;

        }

        public async Task ObtenerEstudiantes()
        {
                    var estu = await _gestionUsuarioService.ObtenerEstudiantesAsync();
                    Estudiantes = estu ?? new List<EstudianteResponseDTO>
            {
                new EstudianteResponseDTO
                {
                   apellidos = "Pérez",
                   contrasena = "123456",
                   cumpleanos = new DateTime(2000, 1, 1),
                   Email = "",
                   estudianteId = 1,
                   nombres = "Juan",
                   PhoneNumber = "1234567890",
                   schoolId = 1,
                   usuarioId = "user1",
                   usuarioValidado = true,
                   curso = "10th Grade"
                },
                new EstudianteResponseDTO
                {
                   apellidos = "",
                   contrasena = "123456",
                   cumpleanos = new DateTime(2000, 1, 1),
                   Email = "",
                   estudianteId = 1,
                   nombres = "Aurorita",
                   PhoneNumber = "1234567890",
                   schoolId = 1,
                   usuarioId = "user1",
                   usuarioValidado = true,
                   curso = "10th Grade"
                },
                new EstudianteResponseDTO
                {
                   apellidos = "Mercedez",
                   contrasena = "123456",
                   cumpleanos = new DateTime(2000, 1, 1),
                   Email = "",
                   estudianteId = 1,
                   nombres = "Mario",
                   PhoneNumber = "1234567890",
                   schoolId = 1,
                   usuarioId = "user1",
                   usuarioValidado = true,
                   curso = "10th Grade"
                },

                new EstudianteResponseDTO
                {
                   apellidos = "Pérez",
                   contrasena = "123456",
                   cumpleanos = new DateTime(2000, 1, 1),
                   Email = "",
                   estudianteId = 1,
                   nombres = "Juan",
                   PhoneNumber = "1234567890",
                   schoolId = 1,
                   usuarioId = "user1",
                   usuarioValidado = true,
                   curso = "10th Grade"
                },
                new EstudianteResponseDTO
                {
                   apellidos = "",
                   contrasena = "123456",
                   cumpleanos = new DateTime(2000, 1, 1),
                   Email = "",
                   estudianteId = 1,
                   nombres = "Aurorita",
                   PhoneNumber = "1234567890",
                   schoolId = 1,
                   usuarioId = "user1",
                   usuarioValidado = true,
                   curso = "10th Grade"
                },
                new EstudianteResponseDTO
                {
                   apellidos = "Mercedez",
                   contrasena = "123456",
                   cumpleanos = new DateTime(2000, 1, 1),
                   Email = "",
                   estudianteId = 1,
                   nombres = "Mario",
                   PhoneNumber = "1234567890",
                   schoolId = 1,
                   usuarioId = "user1",
                   usuarioValidado = true,
                   curso = "10th Grade"
                },
new EstudianteResponseDTO
                {
                   apellidos = "Pérez",
                   contrasena = "123456",
                   cumpleanos = new DateTime(2000, 1, 1),
                   Email = "",
                   estudianteId = 1,
                   nombres = "Juan",
                   PhoneNumber = "1234567890",
                   schoolId = 1,
                   usuarioId = "user1",
                   usuarioValidado = true,
                   curso = "10th Grade"
                },
                new EstudianteResponseDTO
                {
                   apellidos = "",
                   contrasena = "123456",
                   cumpleanos = new DateTime(2000, 1, 1),
                   Email = "",
                   estudianteId = 1,
                   nombres = "Aurorita",
                   PhoneNumber = "1234567890",
                   schoolId = 1,
                   usuarioId = "user1",
                   usuarioValidado = true,
                   curso = "10th Grade"
                },
                new EstudianteResponseDTO
                {
                   apellidos = "Mercedez",
                   contrasena = "123456",
                   cumpleanos = new DateTime(2000, 1, 1),
                   Email = "",
                   estudianteId = 1,
                   nombres = "Mario",
                   PhoneNumber = "1234567890",
                   schoolId = 1,
                   usuarioId = "user1",
                   usuarioValidado = true,
                   curso = "10th Grade"
                },
new EstudianteResponseDTO
                {
                   apellidos = "Pérez",
                   contrasena = "123456",
                   cumpleanos = new DateTime(2000, 1, 1),
                   Email = "",
                   estudianteId = 1,
                   nombres = "Juan",
                   PhoneNumber = "1234567890",
                   schoolId = 1,
                   usuarioId = "user1",
                   usuarioValidado = true,
                   curso = "10th Grade"
                },
                new EstudianteResponseDTO
                {
                   apellidos = "",
                   contrasena = "123456",
                   cumpleanos = new DateTime(2000, 1, 1),
                   Email = "",
                   estudianteId = 1,
                   nombres = "Aurorita",
                   PhoneNumber = "1234567890",
                   schoolId = 1,
                   usuarioId = "user1",
                   usuarioValidado = true,
                   curso = "10th Grade"
                },
                new EstudianteResponseDTO
                {
                   apellidos = "Mercedez",
                   contrasena = "123456",
                   cumpleanos = new DateTime(2000, 1, 1),
                   Email = "",
                   estudianteId = 1,
                   nombres = "Mario",
                   PhoneNumber = "1234567890",
                   schoolId = 1,
                   usuarioId = "user1",
                   usuarioValidado = true,
                   curso = "10th Grade"
                },

            };
                    EstudiantesFiltrados = Estudiantes ?? new List<EstudianteResponseDTO>
            {
                new EstudianteResponseDTO
                {
                   apellidos = "Pérez",
                   contrasena = "123456",
                   cumpleanos = new DateTime(2000, 1, 1),
                   Email = "",
                   estudianteId = 1,
                   nombres = "Juan",
                   PhoneNumber = "1234567890",
                   schoolId = 1,
                   usuarioId = "user1",
                   usuarioValidado = true,
                   curso = "10th Grade"
                },
                new EstudianteResponseDTO
                {
                   apellidos = "",
                   contrasena = "123456",
                   cumpleanos = new DateTime(2000, 1, 1),
                   Email = "",
                   estudianteId = 1,
                   nombres = "Aurorita",
                   PhoneNumber = "1234567890",
                   schoolId = 1,
                   usuarioId = "user1",
                   usuarioValidado = true,
                   curso = "10th Grade"
                },
                new EstudianteResponseDTO
                {
                   apellidos = "Mercedez",
                   contrasena = "123456",
                   cumpleanos = new DateTime(2000, 1, 1),
                   Email = "",
                   estudianteId = 1,
                   nombres = "Mario",
                   PhoneNumber = "1234567890",
                   schoolId = 1,
                   usuarioId = "user1",
                   usuarioValidado = true,
                   curso = "10th Grade"
                },

                new EstudianteResponseDTO
                {
                   apellidos = "Pérez",
                   contrasena = "123456",
                   cumpleanos = new DateTime(2000, 1, 1),
                   Email = "",
                   estudianteId = 1,
                   nombres = "Juan",
                   PhoneNumber = "1234567890",
                   schoolId = 1,
                   usuarioId = "user1",
                   usuarioValidado = true,
                   curso = "10th Grade"
                },
                new EstudianteResponseDTO
                {
                   apellidos = "",
                   contrasena = "123456",
                   cumpleanos = new DateTime(2000, 1, 1),
                   Email = "",
                   estudianteId = 1,
                   nombres = "Aurorita",
                   PhoneNumber = "1234567890",
                   schoolId = 1,
                   usuarioId = "user1",
                   usuarioValidado = true,
                   curso = "10th Grade"
                },
                new EstudianteResponseDTO
                {
                   apellidos = "Mercedez",
                   contrasena = "123456",
                   cumpleanos = new DateTime(2000, 1, 1),
                   Email = "",
                   estudianteId = 1,
                   nombres = "Mario",
                   PhoneNumber = "1234567890",
                   schoolId = 1,
                   usuarioId = "user1",
                   usuarioValidado = true,
                   curso = "10th Grade"
                },
new EstudianteResponseDTO
                {
                   apellidos = "Pérez",
                   contrasena = "123456",
                   cumpleanos = new DateTime(2000, 1, 1),
                   Email = "",
                   estudianteId = 1,
                   nombres = "Memphis Yomael",
                   PhoneNumber = "1234567890",
                   schoolId = 1,
                   usuarioId = "user1",
                   usuarioValidado = true,
                   curso = "10th Grade"
                },
                new EstudianteResponseDTO
                {
                   apellidos = "",
                   contrasena = "123456",
                   cumpleanos = new DateTime(2000, 1, 1),
                   Email = "",
                   estudianteId = 1,
                   nombres = "Aurorita",
                   PhoneNumber = "1234567890",
                   schoolId = 1,
                   usuarioId = "user1",
                   usuarioValidado = true,
                   curso = "10th Grade"
                },
                new EstudianteResponseDTO
                {
                   apellidos = "Mercedez",
                   contrasena = "123456",
                   cumpleanos = new DateTime(2000, 1, 1),
                   Email = "",
                   estudianteId = 1,
                   nombres = "Mario",
                   PhoneNumber = "1234567890",
                   schoolId = 1,
                   usuarioId = "user1",
                   usuarioValidado = true,
                   curso = "10th Grade"
                },
new EstudianteResponseDTO
                {
                   apellidos = "Pérez",
                   contrasena = "123456",
                   cumpleanos = new DateTime(2000, 1, 1),
                   Email = "",
                   estudianteId = 1,
                   nombres = "Juan",
                   PhoneNumber = "1234567890",
                   schoolId = 1,
                   usuarioId = "user1",
                   usuarioValidado = true,
                   curso = "10th Grade"
                },
                new EstudianteResponseDTO
                {
                   apellidos = "",
                   contrasena = "123456",
                   cumpleanos = new DateTime(2000, 1, 1),
                   Email = "",
                   estudianteId = 1,
                   nombres = "Aurorita",
                   PhoneNumber = "1234567890",
                   schoolId = 1,
                   usuarioId = "user1",
                   usuarioValidado = true,
                   curso = "10th Grade"
                },
                new EstudianteResponseDTO
                {
                   apellidos = "Mercedez",
                   contrasena = "123456",
                   cumpleanos = new DateTime(2000, 1, 1),
                   Email = "",
                   estudianteId = 1,
                   nombres = "Mario",
                   PhoneNumber = "1234567890",
                   schoolId = 1,
                   usuarioId = "user1",
                   usuarioValidado = true,
                   curso = "10th Grade"
                },

            };
           
        }
    }
}
