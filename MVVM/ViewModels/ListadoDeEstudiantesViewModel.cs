using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acento.MVVM.Models.Estudents;
using PropertyChanged;

namespace Acento.MVVM.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class ListadoDeEstudiantesViewModel
    {
        public List<Student> Estudiantes { get; set; }


        public ListadoDeEstudiantesViewModel()
        {
            Estudiantes = new List<Student>()
        {
            new Student { Name = "Federico Garcia", Id = 20, PhoneNumber = "123456789" },
            new Student { Name = "Aurorita", Id = 22, PhoneNumber = "987654321" },
            new Student { Name = "Luis Martinez", Id = 21, PhoneNumber = "456123789" },
            new Student { Name = "María Azuleña", Id = 23, PhoneNumber = "789456123" }
        };
        }
    }
}
