using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyStudentsApp.DbContext
{
    public class GuardarSesionModel
    {
        public string? token { get; set; }
        public string? userId { get; set; }
        public string? userName { get; set; }
        public string? password { get; set; }

    }
}
