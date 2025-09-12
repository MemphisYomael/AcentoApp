using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStudentsApp.Shared.DTOShared
{
    public class UsuarioEspecialRequestDTO
    {
        #region usuario
        public bool isStudent { get; set; }
        public string? contrasena { get; set; }
        public string? Email { get; set; }
        public bool isAdministrativo { get; set; }
        public string? UserName { get; set; }
        #endregion

        #region escuela
        public string? nombre { get; set; }
        public string? direccion { get; set; }
        public string? numero { get; set; }
        #endregion
    }
}
